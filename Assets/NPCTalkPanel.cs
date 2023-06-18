using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkPanel : MonoBehaviour
{
    Text TalkInformation;
    public int TalkIndex;
    protected string[] TalkTextList;
    public PlayerControler player;

    // Start is called before the first frame update
    protected void NPCTPAwake()
    {
        TalkInformation = transform.GetChild(0).GetComponent<Text>();
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
}
