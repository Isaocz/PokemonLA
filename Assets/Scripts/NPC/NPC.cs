using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// NPC的父类
/// </summary>
public class NPC<T> : MonoBehaviour
{



    public Animator animator;
    public GameObject playerControler;
    public NPCTalkPanel TalkPanel;
    protected GameObject ZBottonObj;
    protected bool isInTrriger;

    Type t;

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


    public virtual void CloseButton()
    {
        if (t is null)
        {
            GetT();
        }
        ZBottonObj.SetActive(false);
        isInTrriger = false;
        TalkPanel.PlayerExit();
        if (t == typeof(GameNPC)) { playerControler.GetComponent<PlayerControler>().CanNotUseSpaceItem = false; }
        
        
    }

    protected void NPCOnTriggerExit2D(Collider2D other)
    {
        if (t is null)
        {
            GetT();
        }
        if (other.tag == ("Player") && other.GetComponent<T>() != null)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            if (t == typeof(GameNPC)) { playerControler.GetComponent<PlayerControler>().CanNotUseSpaceItem = false; }

        }
    }

    protected void NPCOnTriggerStay2D(Collider2D other)
    {
        if (t is null)
        {
            GetT();
        }
        if (other.tag == ("Player"))
        {
            playerControler = other.gameObject;
            ZBottonObj.SetActive(true);
            isInTrriger = true;
        }
    }

    protected void NPCStart()
    {
        GetT();
        ZBottonObj = gameObject.transform.GetChild(3).gameObject;
        //TalkPanel = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<GameNPCTalkPanel>();
        if (t == typeof(GameNPC)) { TalkPanel = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<GameNPCTalkPanel>(); }
        else if (t == typeof(TownNPC)) { TalkPanel = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TownNPCTalkPanel>(); }
        Debug.Log(TalkPanel);
        animator = GetComponent<Animator>();

        FindPlayer();

    }

    protected void NPCUpdate()
    {
        if (t is null)
        {
            GetT();
        }
        if (playerControler == null)
        {
            FindPlayer();


        }
        if (isInTrriger && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            PlayerSpaceItem(false);
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown && !TalkPanel.isTalkPuse)
        {
            Debug.Log(t == typeof(GameNPC));
            Debug.Log(t.Name);
            if (t == typeof(GameNPC)) { TalkPanel.GetComponent<GameNPCTalkPanel>().player = playerControler.GetComponent<PlayerControler>(); }
            else if (t == typeof(TownNPC)) { TalkPanel.GetComponent<TownNPCTalkPanel>().player = playerControler.GetComponent<PlayerPokemon>(); }
            TalkPanel.gameObject.SetActive(true);
            PlayerSpaceItem(true);
        }
        if (!isInTrriger && TalkPanel.enabled == true)
        {
            TalkPanel.gameObject.SetActive(false);
        }
    }

    protected void PlayerisinTrigger()
    {
        if (isInTrriger && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            CloseButton();
        }
    }



    /// <summary>
    /// 获取该实例为player或者townplayer
    /// </summary>

    public void GetT()
    {
        Type type = this.GetType();
        Type baseType = type.BaseType;
        t = baseType;

    }



    /// <summary>
    /// 搜索对应的player
    /// </summary>
    public void FindPlayer()
    {
        if (t == typeof(GameNPC)) { playerControler = GameObject.FindObjectOfType<PlayerControler>().gameObject; }
        else if (t == typeof(TownNPC)) { playerControler = GameObject.FindObjectOfType<PlayerPokemon>().gameObject; }
    }

    public void PlayerSpaceItem(bool b)
    {
        if (t == typeof(GameNPC)) { playerControler.GetComponent<PlayerControler>().CanNotUseSpaceItem = b ; }
    }
}