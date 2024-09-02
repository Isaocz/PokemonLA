using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEjectButton : MonoBehaviour
{

    bool isPanelActive = false;
    PlayerControler Player;
    Vector3 PlayerPosition;
    float Timer;

    public GameObject EBPanel;
    public Button EBButton;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerControler>();
        PlayerPosition = Player.transform.position;
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null) { Player = GameObject.FindObjectOfType<PlayerControler>(); }
        if (!isPanelActive)
        {
            if (PlayerPosition == Player.transform.position && !Player.isInZ && !Player.CanNotUseSpaceItem) { Timer += Time.deltaTime; }
            else { 
                Timer = 0; PlayerPosition = Player.transform.position; isPanelActive = false;
                EBPanel.gameObject.SetActive(false);
                EBButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (PlayerPosition != Player.transform.position || Player.isInZ || Player.CanNotUseSpaceItem) { 
                Timer = 0; PlayerPosition = Player.transform.position; isPanelActive = false;
                EBPanel.gameObject.SetActive(false);
                EBButton.gameObject.SetActive(false);
            }
        }

        if (Timer >= 8.0f && MapCreater.StaticMap.RRoom[Player.NowRoom].isClear <= 0)
        {
            isPanelActive = true;
            EBPanel.gameObject.SetActive(true);
            EBButton.gameObject.SetActive(true);
        }

    }

    public void Eject()
    {
        Player.TP(Player.NowRoom);
        Timer = 0; PlayerPosition = Player.transform.position; isPanelActive = false;
        EBPanel.gameObject.SetActive(false);
        EBButton.gameObject.SetActive(false);

    }
}
