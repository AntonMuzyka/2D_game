using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeProjecktTitle : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D info)
    {
        EnemiesControler enemies = info.GetComponent<EnemiesControler>();
        if (enemies != null)
            enemies.TakeDamage(_damage);
        Destroy(gameObject, 3.0f);
    }
}
