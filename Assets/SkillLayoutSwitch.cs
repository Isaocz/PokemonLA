using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLayoutSwitch : UISwitch
{

    CameraChangeBG MainCamera;

    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("SkillButtonLayout"));

    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("SkillButtonLayout", Index);
        InitializePlayerSetting.GlobalPlayerSetting.SkillButtonLayout = PlayerPrefs.GetInt("SkillButtonLayout");
        AndroidSkillButton.androidSkillButton.SetButtonPos(Index);
    }
}
