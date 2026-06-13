using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerHaloSwitch : UISwitch
{
    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("Highlight"));
    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("Highlight", Index);
        InitializePlayerSetting.GlobalPlayerSetting.isHighlight = (PlayerPrefs.GetInt("Highlight")!=0);


        PlayerControler player = GameObject.FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            player.SetHighLightHalo();
        }
    }
}