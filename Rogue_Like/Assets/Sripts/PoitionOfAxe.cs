using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoitionOfAxe : MonoBehaviour
{
    [SerializeField] private GameObject _axe;
    [SerializeField] private Transform _startTrowAxePoint;
    [SerializeField] private float _speedAxe;

    private void OnTriggerEnter2D(Collider2D info)
    {
        GameObject axe = Instantiate(_axe, _startTrowAxePoint.position, Quaternion.identity);
        axe.GetComponent<Rigidbody2D>().velocity = transform.right * _speedAxe;
        Destroy(gameObject);
    }
}
