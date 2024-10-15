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


    [Header("0生气Angry 1哭泣Crying 2坚定Determined 3晕倒Dizzy 4开心Happy 5受到启发Inspired  \n6欢庆Joyous 7普通Normal 8痛苦Pain 9伤心Sad 10震惊Shouting 11叹气Sigh  \n12特殊1Special1 13特殊2Special2 14目瞪口呆Stunned 15惊讶Surprised 16泪眼汪汪TearyEyed 17担心Worried \n18睡觉Sleep")]
    /// <summary>
    /// 角色表情
    /// </summary>
    public Sprite[] NPCFaceList = new Sprite[19];


    /// <summary>
    /// 输出角色表情
    /// </summary>
    public Sprite NPCFace(DialogString.Face face)
    {
        Sprite Output = NPCFaceList[(int)face];
        if (Output == null && NPCFaceList[(int)DialogString.Face.Normal] != null) { Output = NPCFaceList[(int)DialogString.Face.Normal]; }
        return Output;
    }


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
