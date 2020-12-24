using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorisontalMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    private Vector2 _startPoint;
    private int _direction = 1;

    void Start()
    {
        _startPoint = transform.position;
    }

    void Update()
    {
        if (transform.position.x - _startPoint.x > _range && _direction > 0)
        {
            _direction *= -1;
        }
        else if (_startPoint.x - transform.position.x > _range && _direction < 0)
        {
            _direction *= -1;
        }

        transform.Translate(_speed * _direction * Time.deltaTime, 0 , 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_range * 2, 0.5f, 0));
    }
}
