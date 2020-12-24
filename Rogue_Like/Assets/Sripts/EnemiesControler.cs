using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesControler : MonoBehaviour
{
    [SerializeField] private int _hp;

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
