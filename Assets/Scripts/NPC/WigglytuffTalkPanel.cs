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
            "哈喽哈喽！这里是实惠培育屋咪！\n我是这里的负责人胖可丁儿老师。",
            "哦哦，原来你就是" + GetPlayerParentName() + "女士的孩子咪\n今天早上我请她来帮忙,她说你正好会路过这里\n拜托你来帮我一下咪！",
            "其实呢。。。\n最近培育屋里有个小朋友很不听话咪。。。",
            FriendsBall.passiveList.transform.GetChild(BabyIndex).GetComponent<BabyPassiveItem>().BabyDescribe,
            "所以呢，想请你带那孩子一起去冒险，\n帮助那孩子成长的同时那孩子也会帮助你哦！",
            "要决定带那孩子一起走吗，\n一起的话为了安全起见你需要先给我五块钱的押金咪。。。"

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
                        "你回来了咪，\n能请你带上那孩子吗。。。",
                        "这样啊，如果改变主意的话随时回来找我咪。。。"
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
                        "好耶！\n那么一路顺风咪！"
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
