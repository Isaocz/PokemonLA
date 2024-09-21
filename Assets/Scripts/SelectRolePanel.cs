using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRolePanel : MonoBehaviour
{

    public List<PlayerControler> PlayerList = new List<PlayerControler> { };
    public Button GameStartButton;
    StartPanelPlayerData pData;

    List<Toggle> TogglesList = new List<Toggle> { };


    // Start is called before the first frame update
    void Start()
    {
        pData = StartPanelPlayerData.PlayerData;
        GameStartButton.interactable = false;


        for (int i = 0; i < transform.GetChild(2).GetChild(0).GetChild(0).childCount; i++)
        {
            Toggle t = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetComponent<Toggle>();
            if (t != null ) { TogglesList.Add(t);  }
        }
    }

    void CancelSelect(int Index)
    {
        pData.Player = null;
        pData.PlayerAbilityIndex = -1;
        GameStartButton.interactable = false;


        TogglesList[Index].transform.GetChild(13).GetComponent<Toggle>().interactable = false;
        TogglesList[Index].transform.GetChild(13).GetComponent<Toggle>().isOn = false;
        TogglesList[Index].transform.GetChild(14).GetComponent<Toggle>().interactable = false;
        TogglesList[Index].transform.GetChild(14).GetComponent<Toggle>().isOn = false;
        TogglesList[Index].transform.GetChild(15).GetComponent<Toggle>().interactable = false;
        TogglesList[Index].transform.GetChild(15).GetComponent<Toggle>().isOn = false;
    }

    public void SelectPlayer( int Index )
    {
        Debug.Log(Index); Debug.Log(TogglesList[Index].isOn);
        if (!TogglesList[Index].isOn) { CancelSelect(Index); }
        else
        {
            for (int i = 0; i < TogglesList.Count; i++)
            {
                if (i != Index) { TogglesList[i].isOn = false; }
            }
            TogglesList[Index].transform.GetChild(13).GetComponent<Toggle>().interactable = true;
            TogglesList[Index].transform.GetChild(13).GetComponent<Toggle>().isOn = true;
            TogglesList[Index].transform.GetChild(14).GetComponent<Toggle>().interactable = true;
            TogglesList[Index].transform.GetChild(15).GetComponent<Toggle>().interactable = true;
            pData.Player = PlayerList[Index];
            GameStartButton.interactable = true;
        }

    }
}
