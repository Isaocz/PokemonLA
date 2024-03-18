using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTypeSwitch : UISwitch
{


    CameraChangeBG MainCamera;

    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("BackGroundIndex"));
       
    }

    public override void SetSwitch(int Index)
    {
        if (MainCamera == null) { MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraChangeBG>(); }
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("BackGroundIndex", Index);
        InitializePlayerSetting.GlobalPlayerSetting.BGIndex = PlayerPrefs.GetInt("BackGroundIndex");
        MainCamera.ChangeBG(Index);
    }
}
