using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class GameNPCTalkPanel : NPCTalkPanel
{

    public PlayerControler player;
    public int TalkIndex;
    protected DialogString[] TalkTextList = new DialogString[] { };


    public override void PlayerExit()
    {
        if (TalkInformation != null)
        {
            TalkIndex = 0;
            ChangeTalkandFace(0);


            gameObject.SetActive(false);
            ParentNPC.PlayerSpaceItem(false);
        }

    }

    // Start is called before the first frame update
    protected void NPCTPAwake()
    {
        TalkInformation = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TalkIndex = 0;
        ParentNPC = transform.parent.parent.GetComponent<GameNPC>();
        
        ChangeTalkandFace(TalkIndex);


        ParentNPC.PlayerSpaceItem(true);

    }

    protected void ChangeTalkandFace(int Index)
    {
        if (Index < TalkTextList.Length)
        {
            TalkInformation.text = TalkTextList[Index].DialogueString;
            if (HeadIconImage != null)
            {
                HeadIconImage.sprite = ParentNPC.NPCFace(TalkTextList[Index].DialogueFace);
            }
        }
    }


    protected void NPCTPContinue()
    {
        if (!isTalkPuse) {
            TalkIndex++;
            if (TalkIndex < TalkTextList.Length)
            {
                ChangeTalkandFace(TalkIndex);

            }
            else
            {
                TalkIndex = 0;
                ChangeTalkandFace(0);

                gameObject.SetActive(false);
                ParentNPC.PlayerSpaceItem(false);
            }
        }
    }
}
