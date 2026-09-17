using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewTalkPamel : GameNPCTalkPanel
{
    bool isTalked;

    // Start is called before the first frame update
    private void Awake()
    {
        TalkTextList = new DialogString[] {
            new DialogString("Miu! 你好厉害！居然能抓到我！" , DialogString.Face.Shouting)  ,
            new DialogString("别小看我，我会好多厉害的技能呢！" , DialogString.Face.Determined)  ,
            new DialogString("什么，你想要学习有趣的招式吗？\n嘿嘿，那我就交给你吧！" , DialogString.Face.Happy)  ,
            new DialogString("不想学习了吗？\n随时回来哦,还想和你一起玩！" , DialogString.Face.Joyous)  ,
        };
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.isMewBeTalk) 
        {
            TalkTextList = new DialogString[] {
                new DialogString("Miu! 又被你抓到了！" , DialogString.Face.Shouting)  ,
                new DialogString("你好聪明啊，不过肯定没有我会的招式多！" , DialogString.Face.Determined)  ,
                new DialogString("想要继续学习招式吗\n嘿嘿，那我就交给你吧!" , DialogString.Face.Happy)  ,
                new DialogString("不想学习了吗？\n随时回来哦,还想和你一起玩！" , DialogString.Face.Joyous)  ,
            };
        }
        NPCTPAwake();
        if (FloorNum.GlobalFloorNum != null && !FloorNum.GlobalFloorNum.isMewBeTalk)
        {
            FloorNum.GlobalFloorNum.isMewBeTalk = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ZButton.Z.IsZButtonDown)
        {
            if (!isTalked) {
                if (TalkIndex == 2 && isTalkPuse)
                {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
                NPCTPContinue();
                if (TalkIndex == 2 && !isTalkPuse)
                {
                    isTalkPuse = true;
                }
            }
            else
            {
                if (TalkIndex == 0)
                {
                    isTalkPuse = true;
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    NPCTPContinue();
                }
            }
        }

    }

    public void Talked()
    {
        isTalked = true;
        TalkTextList = new DialogString[] { 
            new DialogString("啊！你回来了\n还要学习技能吗？" , DialogString.Face.Inspired),
            new DialogString("不想学习了吗？\n随时回来哦,还想和你一起玩！" , DialogString.Face.Joyous),  };
        isTalkPuse = false;
        TalkIndex = 1;
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        HeadIconImage.sprite = ParentNPC.NPCFace(TalkTextList[TalkIndex].DialogueFace);
    }

}
