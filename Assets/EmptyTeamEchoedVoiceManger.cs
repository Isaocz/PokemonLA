using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTeamEchoedVoiceManger : EmptyTeamManger
{
    /// <summary>
    /// 回声的强度等级
    /// </summary>
    int EchoedVoiceLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        EmptyTeamStart();
    }

    private void FixedUpdate()
    {
        EmptyTeamFixedUpdate();
    }

    protected override bool EmptyTeamEnqueueCondition(Empty empty)
    {
        bool output = false;
        if (empty.isUseEchoedVoice ) { output = true; }
        else { output = false; }
        return (base.EmptyTeamEnqueueCondition(empty)) && output;
    }

    protected override bool EmptyTeamActCondition(Empty empty)
    {
        return (base.EmptyTeamActCondition(empty)) && (empty.isEchoedVoiceisReady());
    }

    protected override void TeamEmptyAct(Empty empty)
    {
        base.TeamEmptyAct(empty);
        empty.UseEchoedVoice(EchoedVoiceLevel);
        EchoedVoiceLevel += 1;//回声等级上升
        if (EchoedVoiceLevel >= 5) { EchoedVoiceLevel = 0; }
    }

}
