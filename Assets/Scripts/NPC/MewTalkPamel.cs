using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewTalkPamel : NPCTalkPanel
{
    bool isTalked;

    // Start is called before the first frame update
    private void Awake()
    {
        TalkTextList = new string[] { "Miu! �����������Ȼ��ץ���ң�","��С���ң��һ�ö������ļ����أ�" ,"ʲô������Ҫѧϰ��Ȥ����ʽ��\n�ٺ٣����Ҿͽ������!","����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" };
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.isMewBeTalk) 
        {
            TalkTextList = new string[] { "Miu! �ֱ���ץ���ˣ�", "��ô������������϶�û���һ����ʽ�࣡", "��Ҫ����ѧϰ��ʽ��\n�ٺ٣����Ҿͽ������!", "����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" };
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
        TalkTextList = new string[] { "�����������\n��Ҫѧϰ������", "����ѧϰ����\n��ʱ����Ŷ,�������һ���棡" };
        isTalkPuse = false;
        TalkIndex = 1;
        TalkInformation.text = TalkTextList[TalkIndex];
    }

}
