using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : Skill
{
    public SubDetect sub;

    // Start is called before the first frame update
    void Start()
    {
        player.isCanNotMove = true;
        player.isInvincibleAlways = true;
        if (SkillFrom == 2) { player.AddASubSkill(sub); }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }
}
