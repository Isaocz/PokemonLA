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
        TalkTextList = new string[] { "��ӭ�������������ģ�����Ϣһ�°ɣ�", "��ô�ͽ����Ұ�!���Ե�һ�¡�", "���Ե�һ�¡�����", "��������Ӧ���ָ������ˣ�\n�ڴ������´ι���!","Ҳ����̫���ˡ�����\n�Ҹ���С��˯���ˡ�����" };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex];
    }
    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            ParentBlissey.playerControler.CanNotUseSpaceItem = false;
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0];
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
            TalkInformation.text = TalkTextList[0];
            ParentBlissey.GoodBye();
            gameObject.SetActive(false);
            ParentBlissey.playerControler.CanNotUseSpaceItem = false;
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
        Invoke("HealEnd", 6.2f);
    }

    void Heal()
    {
        //ParentBlissey.playerControler.ChangeHp(ParentBlissey.playerControler.maxHp,0,0);
        Pokemon.PokemonHpChange(null, ParentBlissey.playerControler.gameObject, 0, 0, ParentBlissey.playerControler.maxHp, Type.TypeEnum.IgnoreType);
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
        TalkInformation.text = TalkTextList[4];
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
