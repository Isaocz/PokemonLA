using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeowthTalkUI : MonoBehaviour
{
    TextMeshProUGUI TalkInformation;
    GameObject ShopPanel;
    GameObject ShadowPanel;
    Meowth ParentMeowth;
    public int TalkIndex;
    DialogString[] TalkTextList;

    public Image Headicon;


    // Start is called before the first frame update
    void Start()
    {
        ShadowPanel = transform.parent.GetChild(0).gameObject;
        ParentMeowth = transform.parent.parent.GetComponent<Meowth>();
        TalkInformation = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ShopPanel = transform.parent.GetChild(2).gameObject;
        TalkTextList = new DialogString[]{

            new DialogString("欢迎光临友好商店喵！\n给猫金币喵！"  , DialogString.Face.Happy),
            new DialogString("请问您需要什么喵?", DialogString.Face.Normal),
            new DialogString("就是这样!\n谢谢惠顾喵！" , DialogString.Face.Happy),
        };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        Headicon.sprite = ParentMeowth.NPCFace(TalkTextList[TalkIndex].DialogueFace);
    }

    public void ZButtonDown()
    {
        ZButton.Z.IsZButtonDown = true;
    }
    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            ParentMeowth.playerControler.CanNotUseSpaceItem = false;
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0].DialogueString;
            Headicon.sprite = ParentMeowth.NPCFace(TalkTextList[0].DialogueFace);
            gameObject.SetActive(false);
        }
    }

    void TalkContinue()
    {
        if (TalkIndex == 1){
            ShadowPanel.SetActive(true);
            ShopPanel.SetActive(true);
            gameObject.SetActive(false);
        }else if (TalkIndex == 2)
        {
            TalkIndex = 0; TalkInformation.text = TalkTextList[0].DialogueString;
            Headicon.sprite = ParentMeowth.NPCFace(TalkTextList[0].DialogueFace);
            gameObject.SetActive(false);
            ParentMeowth.playerControler.CanNotUseSpaceItem = false;
            ParentMeowth.GoodBye();
        }
        TalkIndex += 1;
        if(TalkIndex >= 3) { 
            TalkIndex = 0; 
            TalkInformation.text = TalkTextList[0].DialogueString;
            Headicon.sprite = ParentMeowth.NPCFace(TalkTextList[0].DialogueFace);
        }
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        Headicon.sprite = ParentMeowth.NPCFace(TalkTextList[TalkIndex].DialogueFace);
    }

    // Update is called once per frame
    void Update()
    {
        if (ZButton.Z.IsZButtonDown)
        {
            TalkContinue();
        }
    }
}
