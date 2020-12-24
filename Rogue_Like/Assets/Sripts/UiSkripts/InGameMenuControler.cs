using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuControler : BaseGameMenuControler
{    

    [SerializeField] private Button _restart;
    [SerializeField] private Button _backToMenu;
   
    protected override void Start()
    {
        base.Start();
        _play.onClick.AddListener(ChangeMenuStatus);
        _restart.onClick.AddListener(_serviceManager.Restart);
        _backToMenu.onClick.AddListener(GoToMainMenu);
    }

    protected override void OnDestroy()
    {
        _play.onClick.RemoveListener(ChangeMenuStatus);
        _restart.onClick.RemoveListener(_serviceManager.Restart);
        _backToMenu.onClick.RemoveListener(GoToMainMenu);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ChangeMenuStatus();
        }
    }

    protected override void ChangeMenuStatus()
    {
        base.ChangeMenuStatus();
        Time.timeScale = _menu.activeInHierarchy ? 0 : 1;
    }

    public void GoToMainMenu()
    {
        ServiceManadger.Instanse.ChangeLvl((int)Scenes.MainMenu);
    }
}
