using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private int _heallValue;
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_Controler>().RestoreHP(_heallValue);
        Destroy(gameObject);
    }
}
