using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuControler : MonoBehaviour
{
    protected ServiceManadger _serviceManager;

    [SerializeField] protected GameObject _menu;

    [Header ("MainButtons")]
    [SerializeField] protected Button _play;
    [SerializeField] protected Button _settings;
    [SerializeField] protected Button _exit;

    protected virtual void Start()
    {
        _serviceManager = ServiceManadger.Instanse;
        _exit.onClick.AddListener(_serviceManager.Quit);
    }

    protected virtual void OnDestroy() 
    {
        _exit.onClick.RemoveListener(_serviceManager.Quit);
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void ChangeMenuStatus()
    {
        _menu.SetActive(!_menu.activeInHierarchy);
    }
}
