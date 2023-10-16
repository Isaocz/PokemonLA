using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTripleAxel : SubSkill
{

    public int StateNow;

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillIndex == 273 || MainSkill.SkillIndex == 274)
        {
            MainSkill.Damage *= StateNow;
            MainSkill.GetComponent<TripleAxel>().KickCount = StateNow;
        }
        player.RemoveASubSkill(subskill);
        Destroy(gameObject);
    }
}
