using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blissey : MonoBehaviour
{
    GameObject ZBotton;
    BlisseyTalkUI TalkPanel;
    Text TalkInformation;
    Animator animator;
    public PlayerControler playerControler;
    bool isInTrriger;
    bool isHi;
    bool isTalked;
    public bool isSleep;

    private void Start()
    {
        ZBotton = gameObject.transform.GetChild(3).gameObject;
        TalkPanel = gameObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<BlisseyTalkUI>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            playerControler = other.GetComponent<PlayerControler>();
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
            TalkPanel.PlayerExit();
            GoodBye();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTrriger && Input.GetKeyDown(KeyCode.Z) && !isHi && !isSleep)
        {
            animator.ResetTrigger("TalkEnd");
            animator.SetTrigger("Hi"); 
            isHi = true;
            isTalked = true;
            TalkPanel.gameObject.SetActive(true);
        }
        else if(isSleep && isInTrriger && Input.GetKeyDown(KeyCode.Z) && !isHi)
        {
            TalkPanel.SleepTalk();
        }
        if(isTalked && !transform.parent.GetComponent<Room>().isInThisRoom)
        {
            isSleep = true;
            isTalked = false;
            animator.SetTrigger("Sleep");
        }
    }
    


    void Sleep()
    {
        animator.SetTrigger("Sleep");
        isSleep = true;
    }

    public void GoodBye()
    {
        if (isHi)
        {
            animator.SetTrigger("TalkEnd");
        }
        isHi = false;
    }
}
