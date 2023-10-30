using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychUp : Skill
{
    int AtkBouns;
    int DefBouns;
    int SpABouns;
    int SpDBouns;


    // Start is called before the first frame update
    void Start()
    {
        GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
        for (int i = 0; i < EmptyParent.transform.childCount; i++)
        {
            Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
            if (e != null)
            {
                AtkBouns += e.AtkUpLevel;
                DefBouns += e.DefUpLevel;
                SpABouns += e.SpAUpLevel;
                SpDBouns += e.SpDUpLevel;
            }
        }

        player.playerData.AtkBounsJustOneRoom = AtkBouns * ((SkillFrom == 2) ? 2 : 1);
        player.playerData.DefBounsJustOneRoom = DefBouns * ((SkillFrom == 2) ? 2 : 1);
        player.playerData.SpABounsJustOneRoom = SpABouns * ((SkillFrom == 2) ? 2 : 1);
        player.playerData.SpDBounsJustOneRoom = SpDBouns * ((SkillFrom == 2) ? 2 : 1);
        player.ReFreshAbllityPoint();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
