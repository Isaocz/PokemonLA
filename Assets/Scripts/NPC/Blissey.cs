using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blissey : MonoBehaviour
{
    GameObject ZBottonObj;
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
        ZBottonObj = gameObject.transform.GetChild(3).gameObject;
        TalkPanel = gameObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<BlisseyTalkUI>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("xxxxxx");
        if (other.tag == ("Player"))
        {
            if (other.GetComponent<PlayerControler>() != null)
            {
                playerControler = other.GetComponent<PlayerControler>();
            }
            ZBottonObj.SetActive(true);
            isInTrriger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger && other.GetComponent<PlayerControler>() != null)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            GoodBye();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( (isInTrriger || ZBottonObj.activeInHierarchy) && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            CloseButton();
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown && !isHi && !isSleep)
        {
            playerControler.CanNotUseSpaceItem = true;
            animator.ResetTrigger("TalkEnd");
            animator.SetTrigger("Hi"); 
            isHi = true;
            isTalked = true;
            TalkPanel.gameObject.SetActive(true);
        }
        else if(isSleep && isInTrriger && ZButton.Z.IsZButtonDown && !isHi)
        {
            playerControler.CanNotUseSpaceItem = true;
            TalkPanel.SleepTalk();
        }
        if(isTalked && !transform.parent.GetComponent<Room>().isInThisRoom)
        {
            isSleep = true;
            isTalked = false;
            animator.SetTrigger("Sleep");
        }
    }
    
    public void CloseButton()
    {
        ZBottonObj.SetActive(false);
        isInTrriger = false;
        TalkPanel.PlayerExit();
        playerControler.CanNotUseSpaceItem = false;
        GoodBye();
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



    public void BlisseyAwake()
    {
        isHi = false;
        isTalked = false;
        isSleep = false;
        animator.SetTrigger("Awake");
    }
}
