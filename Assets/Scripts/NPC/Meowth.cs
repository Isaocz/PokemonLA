using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meowth : MonoBehaviour
{
    GameObject ZBottonObj;
    MeowthTalkUI TalkPanel;
    Text TalkInformation;
    Animator animator;
    public PlayerControler playerControler;
    bool isInTrriger;
    bool isHi;
    public Vector3 GoodInstancePlace;

    private void Start()
    {
        ZBottonObj = gameObject.transform.GetChild(3).gameObject;
        TalkPanel =  gameObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<MeowthTalkUI>();
        animator = GetComponent<Animator>();
        GoodInstancePlace = new Vector3(-1, -2.5f, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == ("Player"))
        {
            if (other.GetComponent<PlayerControler>() != null) {
                playerControler = other.GetComponent<PlayerControler>();
            }
            ZBottonObj.SetActive(true);
            isInTrriger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            isHi = false;
            playerControler.CanNotUseSpaceItem = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isInTrriger && ZButton.Z.IsZButtonDown && !isHi)
        {
            animator.SetTrigger("Hi"); isHi = true;
            TalkPanel.gameObject.SetActive(true);
            playerControler.CanNotUseSpaceItem = true;
        }
    }

    public void GoodBye()
    {
        animator.SetTrigger("Hi"); isHi = false;
    }
}
