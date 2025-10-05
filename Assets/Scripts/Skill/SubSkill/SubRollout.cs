using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRollout : SubSkill
{
    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillIndex == 459 || MainSkill.SkillIndex == 460)
        {
            MainSkill.GetComponent<Rollout>().RolloutCount = Count;
        }
        player.RemoveASubSkill(subskill);
        Destroy(gameObject);
    }
}
