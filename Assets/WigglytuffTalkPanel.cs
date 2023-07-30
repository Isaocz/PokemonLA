using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigglytuffTalkPanel : NPCTalkPanel
{
    bool isTalked;
    public PokemonBall FriendsBall;
    int BabyIndex;
    bool isTakeBaby;




    // Start is called before the first frame update
    private void Awake()
    {


        BabyIndex = Random.Range(0, FriendsBall.passiveList.SpritesList.Length);

        TalkTextList = new string[] {
            "��ඹ�ඣ�������ʵ���������䣡\n��������ĸ������ֿɶ�����ʦ��",
            "ŶŶ��ԭ�������" + GetPlayerParentName() + "Ůʿ�ĺ�����\n������������������æ,��˵�����û�·������\n������������һ���䣡",
            "��ʵ�ء�����\n������������и�С���Ѻܲ������䡣����",
            FriendsBall.passiveList.transform.GetChild(BabyIndex).GetComponent<BabyPassiveItem>().BabyDescribe,
            "�����أ���������Ǻ���һ��ȥð�գ�\n�����Ǻ��ӳɳ���ͬʱ�Ǻ���Ҳ�������Ŷ��",
            "Ҫ�������Ǻ���һ������\nһ��Ļ�Ϊ�˰�ȫ�������Ҫ�ȸ������Ǯ��Ѻ���䡣����"

        };
        NPCTPAwake();
    }

    private void OnEnable()
    {
        if (!isTalked) {
            ParentNPC.animator.SetTrigger("TalkStart");
            ParentNPC.animator.SetTrigger("Happy");
            ParentNPC.animator.ResetTrigger("TalkOver");
            ParentNPC.animator.ResetTrigger("Sad");
            ParentNPC.animator.ResetTrigger("Jump");
        }
        else
        {
            if (!isTakeBaby)
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(true);
                isTalkPuse = true;
                ParentNPC.animator.SetTrigger("TalkStart");
                ParentNPC.animator.ResetTrigger("Happy");
                ParentNPC.animator.ResetTrigger("TalkOver");
                ParentNPC.animator.SetTrigger("Sad");
                ParentNPC.animator.ResetTrigger("Jump");
            }
            else
            {
                ParentNPC.animator.ResetTrigger("TalkStart");
                ParentNPC.animator.ResetTrigger("Happy");
                ParentNPC.animator.ResetTrigger("TalkOver");
                ParentNPC.animator.ResetTrigger("Sad");
                ParentNPC.animator.SetTrigger("Jump");
            }
        }
    }

    string GetPlayerParentName()
    {
        PlayerControler p = player;
        while (p.EvolutionPlayer != null)
        {
            p = player.EvolutionPlayer;
        }
        string ParentName = p.PlayerNameChinese;
        return ParentName;
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isTalked)
            {

                NPCTPContinue();
                if (TalkIndex == 2)
                {
                    ParentNPC.animator.SetTrigger("Sad");
                    ParentNPC.animator.ResetTrigger("Happy");
                }
                if (TalkIndex == 5)
                {
                    isTalkPuse = true;
                    isTalked = true;
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(3).gameObject.SetActive(true);
                    TalkTextList = new string[]{
                        "��������䣬\n����������Ǻ����𡣡���",
                        "������������ı�����Ļ���ʱ���������䡣����"
                    };
                }
            }
            else
            {
                NPCTPContinue();
                if (!isTakeBaby && TalkIndex == 1)
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                    transform.GetChild(3).gameObject.SetActive(false);
                }
            }
        }
    }


    public void TalkBaby()
    {
        if (player.Money >= 5) {
            player.ChangeMoney(-5);
            isTalkPuse = false;
            isTakeBaby = true;
            TalkTextList = new string[]{
                        "��Ү��\n��ôһ·˳���䣡"
                    };
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0];
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            Instantiate(FriendsBall, ParentNPC.transform.position + Vector3.down + Vector3.right * 3, Quaternion.identity).PassiveDropIndex = BabyIndex;
            ParentNPC.animator.ResetTrigger("Sad");
            ParentNPC.animator.SetTrigger("TalkOver");
            ParentNPC.animator.SetTrigger("Jump");
            //ParentNPC.animator.SetTrigger("TalkStart");
            //ParentNPC.animator.SetTrigger("Happy");
            ParentNPC.animator.ResetTrigger("Sad");
        }
    }

    public void NotTalkBaby()
    {
        isTalkPuse = false;
        TalkIndex = 1;
        TalkInformation.text = TalkTextList[1];
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }



}
