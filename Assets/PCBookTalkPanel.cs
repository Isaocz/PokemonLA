using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCBookTalkPanel : GameNPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;
    SkillShopStrengthenPanel StrengthenPanel;

    private void Awake()
    {
        TalkTextList = new DialogString[] { new DialogString(Book() + _mTool.Tips[Random.Range(0, _mTool.Tips.Length)] + "。", DialogString.Face.Normal) };
        NPCTPAwake();
    }
    void Update()
    {
        if (ZButton.Z.IsZButtonDown)
        {
            TalkTextList = new DialogString[] { new DialogString(Book() + _mTool.Tips[Random.Range(0, _mTool.Tips.Length)] + "。" , DialogString.Face.Normal) };
            TalkInformation.text = TalkTextList[0].DialogueString;
            TalkIndex = 0;
            gameObject.SetActive(false);
            
        }

    }

    string Book()
    {
        string OutPut = "《街区的小冒险》上说：";
        switch (Random.Range(0,7))
        {
            case 0:
                OutPut = "《街区的小冒险》上说：";
                break;
            case 1:
                OutPut = "《大大冒险家》上说：";
                break;
            case 2:
                OutPut = "《冒险故事会》上说：";
                break;
            case 3:
                OutPut = "《周刊冒险》上说：";
                break;
            case 4:
                OutPut = "《月刊OH！超冒险文化》上说：";
                break;
            case 5:
                OutPut = "《猪之书》上说：";
                break;
            case 6:
                OutPut = "《装冒险》上说：";
                break;
        }
        return OutPut;
    }
}
