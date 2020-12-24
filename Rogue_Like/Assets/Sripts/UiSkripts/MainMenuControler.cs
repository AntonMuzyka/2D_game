using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControler : BaseGameMenuControler
{
    protected override void Start()
    {
        base.Start();
        _play.onClick.AddListener(GoToFirstLvl);
    }

    protected override void OnDestroy()
    {
        _play.onClick.RemoveListener(ChangeMenuStatus);
    }

    protected override void ChangeMenuStatus()
    {
        base.ChangeMenuStatus();
        Time.timeScale = _menu.activeInHierarchy ? 0 : 1;
    }

    public void GoToFirstLvl()
    {
        ServiceManadger.Instanse.ChangeLvl((int)Scenes.First);
    }
}
