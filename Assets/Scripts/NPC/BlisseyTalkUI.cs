using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlisseyTalkUI : MonoBehaviour
{
    bool isHeal;
    bool isSleepTalk;
    Text TalkInformation;
    Blissey ParentBlissey;
    public int TalkIndex;
    string[] TalkTextList;

    // Start is called before the first frame update
    void Awake()
    {
        ParentBlissey = transform.parent.parent.GetComponent<Blissey>();
        TalkInformation = transform.GetChild(0).GetComponent<Text>();
        TalkTextList = new string[] { "欢迎来到宝可梦中心，请休息一下吧！", "那么就交给我吧!请稍等一下。", "请稍等一下。。。", "看起来你应经恢复精神了，\n期待您的下次光临!","也许是太累了。。。\n幸福蛋小姐睡着了。。。" };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex];
    }
    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0];
            gameObject.SetActive(false);
        }
        
    }


    void TalkContinue()
    {
        if (TalkIndex == 1)
        {
            CallTPMask();
        }
        else if (TalkIndex == 3)
        {
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0];
            ParentBlissey.GoodBye();
            gameObject.SetActive(false);
        }
        else
        {
            TalkIndex += 1;
            if (TalkIndex >= 4) { TalkIndex = 0; TalkInformation.text = TalkTextList[0]; }
            TalkInformation.text = TalkTextList[TalkIndex];
        }
    }

    void CallTPMask()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 5f;
        TalkIndex += 1;
        TalkInformation.text = TalkTextList[TalkIndex];
        isHeal = true;
        ParentBlissey.playerControler.isTP = true;
        BackGroundMusic.StaticBGM.BGM.volume /= 5;
        ParentBlissey.GetComponent<AudioSource>().Play();
        Invoke("Heal",2f);
        Invoke("HealEnd", 4.2f);
    }

    void Heal()
    {
        ParentBlissey.playerControler.ChangeHp(ParentBlissey.playerControler.maxHp,0,0);
        ParentBlissey.playerControler.BurnRemove();
        ParentBlissey.playerControler.ParalysisRemove();
        ParentBlissey.playerControler.SleepRemove();
        ParentBlissey.playerControler.ToxicRemove();
    }

    void HealEnd()
    {
        ParentBlissey.playerControler.isTP = false;
        isHeal = false;
        BackGroundMusic.StaticBGM.BGM.volume *= 3;
    }

    public void SleepTalk()
    {
        TalkIndex = 4;
        gameObject.SetActive(true);
        TalkInformation.text = TalkTextList[4];
    }

    void SleepTalkEnd()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isHeal && TalkIndex != 4)
        {
            TalkContinue();
        }
        if (Input.GetKeyDown(KeyCode.Z) && TalkIndex == 4)
        {
            SleepTalkEnd();
        }
    }
}
