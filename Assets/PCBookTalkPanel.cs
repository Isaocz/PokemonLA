using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCBookTalkPanel : NPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;
    SkillShopStrengthenPanel StrengthenPanel;

    private void Awake()
    {
        TalkTextList = new string[] { Book() + _mTool.Tips[Random.Range(0, _mTool.Tips.Length)] + "��" };
        NPCTPAwake();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TalkTextList = new string[] { Book() + _mTool.Tips[Random.Range(0, _mTool.Tips.Length)] + "��" };
            TalkInformation.text = TalkTextList[0];
            TalkIndex = 0;
            gameObject.SetActive(false);
            
        }

    }

    string Book()
    {
        string OutPut = "��������Сð�ա���˵��";
        switch (Random.Range(0,7))
        {
            case 0:
                OutPut = "��������Сð�ա���˵��";
                break;
            case 1:
                OutPut = "�����ð�ռҡ���˵��";
                break;
            case 2:
                OutPut = "��ð�չ��»ᡷ��˵��";
                break;
            case 3:
                OutPut = "���ܿ�ð�ա���˵��";
                break;
            case 4:
                OutPut = "���¿�OH����ð���Ļ�����˵��";
                break;
            case 5:
                OutPut = "����֮�顷��˵��";
                break;
            case 6:
                OutPut = "��װð�ա���˵��";
                break;
        }
        return OutPut;
    }
}
