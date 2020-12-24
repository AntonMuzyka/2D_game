using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonController : EnemyControllerBase
{
    [SerializeField] private float _idleTime;

    private bool _isAttackRange;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private float _powerStrikeRange;
    [SerializeField] private LayerMask _enemies;
    protected bool _attacking;



    private float _currentStrikeRange;
    private bool _inRage;
    private EnemyState _stateOnHold;
 
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (_currentState)
        {
            case EnemyState.Idle:
                _enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Walk:
                _startPoint = transform.position;
                break;
            case EnemyState.Strike:
                _attacking = true;
                _currentStrikeRange = state == EnemyState.Strike ? _strikeRange: _powerStrikeRange;
                _enemyRb.velocity = Vector2.zero;
                if(!CanAtk())
                {
                    _stateOnHold = state;
                    _enemyAnimator.SetBool(_currentState.ToString(), false);
                    state = EnemyState.Walk;
                }
                break;
        }
    }

    protected void FixedUpdate()
    {
        base.IsGroundEnding();
        if (IsGroundEnding())
            Flip();

        if (_currentState == EnemyState.Walk)
            Move();

        if (CanAtk())
            ChangeState(_stateOnHold);
    }

    private bool CanAtk()
    {
        return Vector2.Distance(transform.position, _player.transform.position) < _currentStrikeRange;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_strikePoint.position, new Vector3(_strikeRange, _strikeRange, 0));
    }

    protected void Strike()
    {
        Collider2D player = Physics2D.OverlapBox(_strikePoint.position, new Vector2(_strikeRange, _strikeRange), 0, _enemies);
        if (player != null)
        {
            Player_Controler playerControler = player.GetComponent<Player_Controler>();
            int damage = _inRage ? _damage * 2 : _damage;
            if (playerControler != null)
                playerControler.TakeDamage(_damage);
        }
    }



    protected virtual void DoStateAction()
    {
        switch (_currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
        }
    }

    protected virtual void EndState()
    {
        if (_currentState == EnemyState.Strike)
            _attacking = false;

    }

}
