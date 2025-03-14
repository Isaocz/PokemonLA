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


    [Header("0����Angry 1����Crying 2�ᶨDetermined 3�ε�Dizzy 4����Happy 5�ܵ�����Inspired  \n6����Joyous 7��ͨNormal 8ʹ��Pain 9����Sad 10��Shouting 11̾��Sigh  \n12����1Special1 13����2Special2 14Ŀ�ɿڴ�Stunned 15����Surprised 16��������TearyEyed 17����Worried \n18˯��Sleep")]
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public Sprite[] NPCFaceList = new Sprite[19];


    /// <summary>
    /// �����ɫ����
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
        if (isInTrriger && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            playerControler.CanNotUseSpaceItem = false;
            isHi = false;
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown && !isHi)
        {
            animator.SetTrigger("Hi"); isHi = true;
            TalkPanel.gameObject.SetActive(true);
            playerControler.CanNotUseSpaceItem = true;
        }
    }

    public void CloseButton()
    {
        ZBottonObj.SetActive(false);
        isInTrriger = false;
        TalkPanel.PlayerExit();
        isHi = false;
        playerControler.CanNotUseSpaceItem = false;
    }

    public void GoodBye()
    {
        animator.SetTrigger("Hi"); isHi = false;
    }
}
