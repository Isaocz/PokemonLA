using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHardworkSwitch : UISwitch
{

    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("ShowHardworking"));
    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("ShowHardworking", Index);
        InitializePlayerSetting.GlobalPlayerSetting.isShowHardworking = ((PlayerPrefs.GetInt("ShowHardworking")) != 0);
    }
}
