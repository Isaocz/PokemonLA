using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCcomputerTalkPanle : MonoBehaviour
{
    Text TalkInformation;
    PCcomputer ParentPCComputer;
    public int TalkIndex;
    string[] TalkTextList;
    bool isGetGift;
    public PokemonBall pokemonBall;

    // Start is called before the first frame update
    void Start()
    {
        ParentPCComputer = transform.parent.parent.GetComponent<PCcomputer>();
        TalkInformation = transform.GetChild(0).GetComponent<Text>();
        TalkTextList = new string[] { "检查一下信息吧。。。", "受到了妈妈寄来的礼物！", "谢谢妈妈！希望她身体健康！", "没有什么邮件。。。" };
        if (!ParentPCComputer.player.playerData.IsPassiveGetList[29]) {  TalkIndex = 0;  }
        else {     TalkIndex = 3;   }
        TalkInformation.text = TalkTextList[TalkIndex];
    }

    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            if (!isGetGift)
            {
                TalkIndex = 0;
                TalkInformation.text = TalkTextList[0];
                gameObject.SetActive(false);
            }
            else
            {
                TalkIndex = 3;
                TalkInformation.text = TalkTextList[3];
                gameObject.SetActive(false);
            }
        }

    }


    public void TalkContinue()
    {
        if (TalkIndex == 3)
        {
            gameObject.SetActive(false);
        }
        else if (TalkIndex == 2)
        {
            PokemonBall Gift = Instantiate(pokemonBall, ParentPCComputer.transform.position + Vector3.down * 3.5f , Quaternion.identity , ParentPCComputer.transform);
            Gift.PassiveDropPer = 1;
            isGetGift = true;
            TalkIndex = 3;
            TalkInformation.text = TalkTextList[3];
            gameObject.SetActive(false);
        }
        else
        {
            TalkIndex += 1;
            if (TalkIndex >= 4) { TalkIndex = 0; TalkInformation.text = TalkTextList[0]; }
            TalkInformation.text = TalkTextList[TalkIndex];
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TalkContinue();
        }
    }

}
