using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlisseyTalkUI : MonoBehaviour
{
    bool isHeal;
    bool isSleepTalk;
    TextMeshProUGUI TalkInformation;
    Blissey ParentBlissey;
    public int TalkIndex;

    public Image Headicon;
    DialogString[] TalkTextList;

    // Start is called before the first frame update
    void Awake()
    {
        ParentBlissey = transform.parent.parent.GetComponent<Blissey>();
        TalkInformation = transform.GetChild(0).GetComponent<TextMeshProUGUI>();


        TalkTextList = new DialogString[] {
            
            new DialogString("欢迎来到宝可梦中心，请休息一下吧！" , DialogString.Face.Happy),
            new DialogString("那么就交给我吧!请稍等一下。" , DialogString.Face.Normal),
            new DialogString("请稍等一下。。。" , DialogString.Face.Normal),
            new DialogString("看起来你应经恢复精神了，\n期待您的下次光临！" , DialogString.Face.Joyous),
            new DialogString("也许是太累了。。。\n幸福蛋小姐睡着了。。。" , DialogString.Face.Sleep),
        };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[TalkIndex].DialogueFace); 


    }
    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            ParentBlissey.playerControler.CanNotUseSpaceItem = false;
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0].DialogueString;
            Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[0].DialogueFace);


            gameObject.SetActive(false);
        }
        
    }

    public void ZButtonDown()
    {
        ZButton.Z.IsZButtonDown = true;
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
            TalkInformation.text = TalkTextList[0].DialogueString;
            Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[0].DialogueFace);
            ParentBlissey.GoodBye();
            gameObject.SetActive(false);
            ParentBlissey.playerControler.CanNotUseSpaceItem = false;
        }
        else
        {
            TalkIndex += 1;
            if (TalkIndex >= 4) { TalkIndex = 0; TalkInformation.text = TalkTextList[0].DialogueString; Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[0].DialogueFace); }
            TalkInformation.text = TalkTextList[TalkIndex].DialogueString; Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[TalkIndex].DialogueFace);
        }
    }

    void CallTPMask()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 5f;
        TalkIndex += 1;
        TalkInformation.text = TalkTextList[TalkIndex].DialogueString;
        Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[TalkIndex].DialogueFace);
        isHeal = true;
        ParentBlissey.playerControler.isTP = true;
        BackGroundMusic.StaticBGM.BGM.volume /= 5;
        ParentBlissey.GetComponent<AudioSource>().Play();
        Invoke("Heal",2f);
        Invoke("HealEnd", 6.2f);
    }

    void Heal()
    {
        //ParentBlissey.playerControler.ChangeHp(ParentBlissey.playerControler.maxHp,0,0);
        Pokemon.PokemonHpChange(null, ParentBlissey.playerControler.gameObject, 0, 0, ParentBlissey.playerControler.maxHp, PokemonType.TypeEnum.IgnoreType);
        ParentBlissey.playerControler.BurnRemove();
        ParentBlissey.playerControler.ParalysisRemove();
        ParentBlissey.playerControler.SleepRemove();
        ParentBlissey.playerControler.ToxicRemove();
        ParentBlissey.playerControler.PlayerFrozenRemove();
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
        TalkInformation.text = TalkTextList[4].DialogueString;
        Headicon.sprite = ParentBlissey.NPCFace(TalkTextList[4].DialogueFace);
    }

    void SleepTalkEnd()
    {
        gameObject.SetActive(false);
        ParentBlissey.playerControler.CanNotUseSpaceItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ZButton.Z.IsZButtonDown && !isHeal && TalkIndex != 4)
        {
            TalkContinue();
        }
        if (ZButton.Z.IsZButtonDown && TalkIndex == 4)
        {
            SleepTalkEnd();
        }
    }
}
