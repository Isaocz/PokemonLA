using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiplupNPC : TownNPC
{

    void Start()
    {
        //初始化状态
        NPCStart();

        //■■！！TODO初始化状态机location，npcState
        Initialize();

        //初始化方向
        SetDirector(new Vector2(0,-1));

        //开启速度方向检测携程
        StartCoroutine(CheckLook()) ;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerisinTrigger();

        if (TalkPanel.gameObject.activeSelf == false)
        {

            NPCUpdate();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        NPCOnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NPCOnTriggerExit2D(other);
    }
}
