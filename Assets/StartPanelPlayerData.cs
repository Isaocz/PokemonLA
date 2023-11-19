using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelPlayerData : MonoBehaviour
{

    public static StartPanelPlayerData PlayerData;
    public PlayerControler Player;
    public int PlayerAbilityIndex;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        PlayerData = this;
    }
}
