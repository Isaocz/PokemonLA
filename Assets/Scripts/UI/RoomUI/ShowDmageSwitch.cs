using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDmageSwitch : UISwitch
{

    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("ShowDamage"));
    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("ShowDamage", Index);
        InitializePlayerSetting.GlobalPlayerSetting.isShowDamage = ((PlayerPrefs.GetInt("ShowDamage")) != 0);
    }
}
