using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucarioTalkPanel : TownNPCTalkPanel
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

}
