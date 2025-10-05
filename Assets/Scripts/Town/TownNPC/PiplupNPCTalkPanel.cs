using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiplupNPCTalkPanel : TownNPCTalkPanel
{

    private void OnEnable()
    {
        NPCTPAwake();
    }

    void Update()
    {
        if (ZButton.Z.IsZButtonDown)
        {
            NPCTPContinue();


        }

    }

    public void GiveUpProject()
    {
        SaveLoader.saveLoader.saveData.TownNPCDialogState.isStartAProject = false;
        isTalkPuse = false;
        //CurrentDialogNode = CurrentDialogNode.ChildNode[0];
        //SetText();
    }

    public void StartProject()
    {
        SaveLoader.saveLoader.saveData.TownNPCDialogState.isStartAProject = true;
        isTalkPuse = false;
        //CurrentDialogNode = CurrentDialogNode.ChildNode[0];
        //SetText();
    }
}
