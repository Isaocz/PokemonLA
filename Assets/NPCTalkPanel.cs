using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkPanel : MonoBehaviour
{
    protected Text TalkInformation;
    public bool isTalkPuse;
    public int TalkIndex;
    protected string[] TalkTextList;
    public PlayerControler player;
    protected NPC ParentNPC;

    // Start is called before the first frame update
    protected void NPCTPAwake()
    {
        TalkInformation = transform.GetChild(0).GetComponent<Text>();
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex];
        ParentNPC = transform.parent.parent.GetComponent<NPC>();
    }

    protected void NPCTPContinue()
    {
        if (!isTalkPuse) {
            TalkIndex++;
            if (TalkIndex < TalkTextList.Length)
            {
                TalkInformation.text = TalkTextList[TalkIndex];
            }
            else
            {
                TalkIndex = 0;
                TalkInformation.text = TalkTextList[0];
                gameObject.SetActive(false);
            }
        }
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
}
