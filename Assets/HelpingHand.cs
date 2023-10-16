using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpingHand : Skill
{

    public SubHelpingHand subHelpingHand;

    // Start is called before the first frame update
    void Start()
    {
        player.AddASubSkill(subHelpingHand);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
