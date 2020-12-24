using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_Controller : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };
    [SerializeField] Rigidbody2D _playerRB;
    [SerializeField] Animator _playerAnimator;

    [Header("Horisontal movement")]
    [SerializeField] float _Speed;
    bool _faceRight = true;

    private bool _canMove = true;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 250.0f;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _WhatIsGround;
    bool _grounded;
    int _countJump;

    [Header ("Rolling")]
    [SerializeField] Transform _groundCheckHead;       
    [SerializeField] Collider2D _head;
    [SerializeField] Collider2D _body;   
    bool _canStand;
    float _rollDistance = 0;

    [Header("Cast")]
    [SerializeField] private GameObject _knife;
    [SerializeField] private Transform _startTrowKnifePoint;
    [SerializeField] private float _speedKnife;
    private bool _isCast;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemies;
    private bool _isStriking;
    private bool _isAirAtk;

    [SerializeField] private float _pushForce;
    private float _lastHurtTime;


    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _radius);
        Gizmos.DrawWireSphere(_groundCheckHead.position, _radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_strikePoint.position, _attackRange);
    }

    void flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }
    
    public void Move(float _move, bool _jump, bool _rolling, bool _sit)
    {
        if (!_canMove)
            return;

        #region movement
        _playerRB.velocity = new Vector2(_Speed * _move, _playerRB.velocity.y);

        if (_move > 0 && !_faceRight)
        {
            flip();
        }
        else if (_move < 0 && _faceRight)
        {
            flip();
        }
        #endregion

        #region jump
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _WhatIsGround);
        if (_jump && _grounded)
        {
            _countJump = 1;
            _playerRB.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }
        else if (_jump && _countJump > 0)
        {
            _countJump--;
            _playerRB.AddForce(Vector2.zero);
            _playerRB.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }
        #endregion

        #region rolling 
        //Не робить як я хотів
        _canStand = Physics2D.OverlapCircle(_groundCheckHead.position, _radius, _WhatIsGround);
        if (_rolling && _faceRight && _rollDistance < 100.0f) 
        {
            _rolling = true;
            _playerRB.velocity = new Vector2(_Speed, _playerRB.velocity.y);
            //_playerRB.velocity = transform.right * _Speed;
            _head.enabled = false;
            _body.enabled = false;
        }
        else if (_rolling && !_faceRight)
        {
            _rolling = true;
            _playerRB.velocity = new Vector2(-_Speed, _playerRB.velocity.y);
            _head.enabled = false;
            _body.enabled = false;
        }
        else if (!_rolling && !_canStand)
        {
            _head.enabled = true;
            _body.enabled = true;
        }
        #endregion

        #region sit
        _canStand = Physics2D.OverlapCircle(_groundCheckHead.position, _radius, _WhatIsGround);
        if (_sit)
        {
            _head.enabled = false;
            _body.enabled = false;
        }
        else if (!_sit && !_canStand)
        {
            _head.enabled = true;
            _body.enabled = true;
        }
        #endregion

        #region Animation
        _playerAnimator.SetBool("Jump", !_grounded);
        _playerAnimator.SetBool("Sit", !_head.enabled);
        _playerAnimator.SetFloat("Speed", Mathf.Abs(_move));
        #endregion
    }

    public void GetHurt(Vector2 position)
    {
        _canMove = false;
        OnGetHurt(false);
        Vector2 PushDirection = new Vector2();
        PushDirection.x = position.x > transform.position.x ? -1 : 1;
        PushDirection.y = 1;
        _playerAnimator.SetBool("Hit", true);
        _playerRB.AddForce(PushDirection * _pushForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _WhatIsGround);

        if (_playerAnimator.GetBool("Hit") && _grounded && Time.time > 0.5f)
            EndHurt();
    }

    private void EndHurt()
    {
        OnGetHurt(true);
        _canMove = true;
        _playerAnimator.SetBool("Hit", false);
    }

    public void StartAirAtk()
    {
        if (_isAirAtk)
            return;
        _playerAnimator.SetBool("AirAtk", true);
        _isAirAtk = true;
    }

    private void AirAtk()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _attackRange, _enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemiesControler enemy = enemies[i].GetComponent<EnemiesControler>();
            enemy.TakeDamage(_damage);
        }
    }

    private void EndAirAtk()
    {
        _isAirAtk = false;
        _playerAnimator.SetBool("AirAtk", false);
    }

    public void StartRoll()
    {
        _playerAnimator.SetBool("Roll", _grounded && !_canStand);
    }
    public void StopRoling()
    {
        _playerRB.velocity = Vector2.zero;
        _playerAnimator.SetBool("Roll", false);
    }

    public void StartCast()
    {
        if (_isCast)
            return;
        _isCast = true;
        _playerAnimator.SetBool("Cast", true);
    }

    private void CastKnife()
    {
        GameObject knife = Instantiate(_knife, _startTrowKnifePoint.position, Quaternion.identity);
        knife.GetComponent<Rigidbody2D>().velocity = transform.right * _speedKnife;
        knife.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(knife, 1.5f);
    }

    private void EndCast()
    {
        _isCast = false;
        _playerAnimator.SetBool("Cast", false);
    }

    public void StartStrike()
    {
        if (_isStriking)
            return;
        _playerAnimator.SetBool("Strike", true);
        _isStriking = true;
    }

    private void Strike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _attackRange, _enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemiesControler enemy = enemies[i].GetComponent<EnemiesControler>();
            enemy.TakeDamage(_damage);
        }
    }

    private void EndStrike()
    {
        _isStriking = false;
        _playerAnimator.SetBool("Strike", false);
        _canMove = true;
    }
}
