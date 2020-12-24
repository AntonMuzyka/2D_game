using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controler : MonoBehaviour
{
    private ServiceManadger _serviceManadger;
    [SerializeField] private int _maxHP;
    private int _currentHp;

    [SerializeField] Slider _hpSlider;

    Movement_Controller _movementController;
    Vector2 _startPosition;

    private bool _canByDamaged = true;

    void Start()
    {
        _movementController = GetComponent<Movement_Controller>();
        _movementController.OnGetHurt += OnGetHurt;
        _currentHp = _maxHP;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.value = _maxHP;
        _startPosition = transform.position;
        _serviceManadger = ServiceManadger.Instanse;
    }

    public void TakeDamage(int Damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {
        if (!_canByDamaged)
            return;

        _currentHp -= Damage;
        if (_currentHp <= 0)
        {
            OnDeath();
        }

        switch (type)
        {
            case DamageType.Casual:
                _movementController.GetHurt(enemy.position);
                break;
            case DamageType.Projectile:
                break;
            default:
                break;
        }
        _hpSlider.value = _currentHp;
    }

    private void OnGetHurt(bool canByDamaged)
    {
        _canByDamaged = canByDamaged;
    }

    public void RestoreHP(int hp)
    {
        _currentHp += hp;
        if (_currentHp > _maxHP)
        {
            _currentHp = _maxHP;
        }
        _hpSlider.value = _currentHp;
    }

    public void OnDeath()
    {
        //transform.position = _startPosition;
        _serviceManadger.Restart();
    }
}
