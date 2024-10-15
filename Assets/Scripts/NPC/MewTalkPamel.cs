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
            new DialogString("Miu! �����������Ȼ��ץ���ң�" , DialogString.Face.Shouting)  ,
            new DialogString("��С���ң��һ�ö������ļ����أ�" , DialogString.Face.Determined)  ,
            new DialogString("ʲô������Ҫѧϰ��Ȥ����ʽ��\n�ٺ٣����Ҿͽ�����ɣ�" , DialogString.Face.Happy)  ,
            new DialogString("����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" , DialogString.Face.Joyous)  ,
        };
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.isMewBeTalk) 
        {
            TalkTextList = new DialogString[] {
                new DialogString("Miu! �ֱ���ץ���ˣ�" , DialogString.Face.Shouting)  ,
                new DialogString("��ô������������϶�û���һ����ʽ�࣡" , DialogString.Face.Determined)  ,
                new DialogString("��Ҫ����ѧϰ��ʽ��\n�ٺ٣����Ҿͽ������!" , DialogString.Face.Happy)  ,
                new DialogString("����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" , DialogString.Face.Joyous)  ,
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
            new DialogString("�����������\n��Ҫѧϰ������" , DialogString.Face.Inspired),
            new DialogString("����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" , DialogString.Face.Joyous),  };
        isTalkPuse = false;
        TalkIndex = 1;
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        HeadIconImage.sprite = ParentNPC.NPCFace(TalkTextList[TalkIndex].DialogueFace);
    }

}
