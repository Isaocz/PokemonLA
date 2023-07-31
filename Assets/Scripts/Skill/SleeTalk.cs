using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeTalk : Skill
{
    Skill SleepTalkSkill;

    // Start is called before the first frame update
    void Start()
    {
        RandomOutPutASkill();
        Debug.Log(SleepTalkSkill);

        Skill skillObj = null;
        if (!SleepTalkSkill.isNotDirection)
        {
            if (player.look.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(SleepTalkSkill, new Vector2(player.transform.position.x , player.transform.position.y) + (Vector2.up * 0.4f) + (player.look * SleepTalkSkill.DirctionDistance), Quaternion.Euler(0, 0, 0), SleepTalkSkill.isNotMoveWithPlayer ? null : player.transform);
            }
            else if (player.look.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(SleepTalkSkill, new Vector2(player.transform.position.x, player.transform.position.y) + (Vector2.up * 0.4f) + (player.look * SleepTalkSkill.DirctionDistance), Quaternion.Euler(0, 0, 180), SleepTalkSkill.isNotMoveWithPlayer ? null : player.transform);
            }
            else if (player.look.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(SleepTalkSkill, new Vector2(player.transform.position.x, player.transform.position.y) + (Vector2.up * 0.4f) + (player.look * SleepTalkSkill.DirctionDistance), Quaternion.Euler(0, 0, 90), SleepTalkSkill.isNotMoveWithPlayer ? null : player.transform);
            }
            else if (player.look.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(SleepTalkSkill, new Vector2(player.transform.position.x, player.transform.position.y) + (Vector2.up * 0.4f) + (player.look * SleepTalkSkill.DirctionDistance), Quaternion.Euler(0, 0, 270), SleepTalkSkill.isNotMoveWithPlayer ? null : player.transform);
            }
        }
        else
        {
            skillObj = Instantiate(SleepTalkSkill, new Vector2(player.transform.position.x, player.transform.position.y), Quaternion.identity, SleepTalkSkill.isNotMoveWithPlayer ? null : player.transform);
        }
        player.playerSubSkillList.CallSubSkill(SleepTalkSkill);
        skillObj.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    void RandomOutPutASkill()
    {
        int i = Random.Range(0,4);
        if (i == 0) { SleepTalkSkill = player.Skill01; }
        if (i == 1) { SleepTalkSkill = player.Skill02; }
        if (i == 2) { SleepTalkSkill = player.Skill03; }
        if (i == 3) { SleepTalkSkill = player.Skill04; }
        while (SleepTalkSkill.SkillIndex == 55 || SleepTalkSkill.SkillIndex == 56 || (SkillFrom == 2 && (SleepTalkSkill.SkillIndex == 51 || SleepTalkSkill.SkillIndex == 52)) )
        {
            i = Random.Range(0, 4);
            if (i == 0) { SleepTalkSkill = player.Skill01; }
            if (i == 1) { SleepTalkSkill = player.Skill02; }
            if (i == 2) { SleepTalkSkill = player.Skill03; }
            if (i == 3) { SleepTalkSkill = player.Skill04; }
        }
    }
}
