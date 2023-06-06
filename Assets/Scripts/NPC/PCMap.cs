using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMap : MonoBehaviour
{

    GameObject ZBotton;
    bool isInTrriger;
    public PlayerControler player;
    public PCMapTalkPanel TalkPanel;
    // Start is called before the first frame update
    void Start()
    {
        ZBotton = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTrriger && Input.GetKeyDown(KeyCode.Z))
        {
            TalkPanel.gameObject.SetActive(true);
            if (TalkPanel.TalkIndex != 0 && TalkPanel.TalkIndex != -1)
            {
                TalkPanel.PlayerExit();
            }if(TalkPanel.TalkIndex == -1)
            {
                TalkPanel.TalkIndex = 3;
                UiMiniMap.Instance.SeeMapJustOneRoom();
            }
        }
        if (!isInTrriger)
        {
            TalkPanel.PlayerExit();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            player = other.GetComponent<PlayerControler>();
            ZBotton.SetActive(true);
            isInTrriger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger)
        {
            ZBotton.SetActive(false);
            isInTrriger = false;
        }
    }

}
