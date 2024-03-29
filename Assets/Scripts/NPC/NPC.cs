using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    protected GameObject ZBotton;
    public Animator animator;
    public PlayerControler playerControler;
    public NPCTalkPanel TalkPanel;
    protected bool isInTrriger;

    protected void NPCStart()
    {
        ZBotton = gameObject.transform.GetChild(3).gameObject;
        TalkPanel = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<NPCTalkPanel>();
        animator = GetComponent<Animator>();
        playerControler = GameObject.FindObjectOfType<PlayerControler>();

    }

    protected void NPCOnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            if (other.GetComponent<PlayerControler>() != null) {
                playerControler = other.GetComponent<PlayerControler>();
            }
            ZBotton.SetActive(true);
            isInTrriger = true;
        }
    }

    protected void NPCOnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
        {
            ZBotton.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            playerControler.CanNotUseSpaceItem = false;
        }
    }

    protected void NPCUpdate()
    {
        if (playerControler == null)
        {
            playerControler = GameObject.FindObjectOfType<PlayerControler>();
        }
        if (isInTrriger && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            ZBotton.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            playerControler.CanNotUseSpaceItem = false;
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown && !TalkPanel.isTalkPuse)
        {
            TalkPanel.player = playerControler;
            TalkPanel.gameObject.SetActive(true);
            playerControler.CanNotUseSpaceItem = true;
        }
    }
}
