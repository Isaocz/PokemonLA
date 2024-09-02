using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBounsSwitch : UISwitch
{
    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("ShowBouns"));
    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("ShowBouns", Index);
        InitializePlayerSetting.GlobalPlayerSetting.isShowBouns = PlayerPrefs.GetInt("ShowBouns");


        PlayerControler player = GameObject.FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            player.RefreshAbllityUI();
        }
    }
}
