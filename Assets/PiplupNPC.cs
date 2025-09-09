using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiplupNPC : TownNPC
{

    void Start()
    {
        //��ʼ��״̬
        NPCStart();

        //��������TODO��ʼ��״̬��location��npcState

        //��ʼ������
        SetDirector(new Vector2(0,-1));

        //�����ٶȷ�����Я��
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
