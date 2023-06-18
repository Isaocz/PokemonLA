using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    GameObject ZBotton;
    Animator animator;
    public PlayerControler playerControler;
    protected NPCTalkPanel TalkPanel;
    bool isInTrriger;
    bool isTalked;

    protected void NPCStart()
    {
        ZBotton = gameObject.transform.GetChild(3).gameObject;
        TalkPanel = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<NPCTalkPanel>();
        animator = GetComponent<Animator>();
    }

    protected void NPCOnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            playerControler = other.GetComponent<PlayerControler>();
            ZBotton.SetActive(true);
            isInTrriger = true;
        }
    }

    protected void NPCOnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            ZBotton.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
        }
    }

    protected void NPCUpdate()
    {
        if (isInTrriger && Input.GetKeyDown(KeyCode.Z))
        {
            isTalked = true;
            TalkPanel.gameObject.SetActive(true);
            TalkPanel.player = playerControler;
        }
    }
}
