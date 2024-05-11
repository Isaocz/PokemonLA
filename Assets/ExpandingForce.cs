using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingForce : Skill
{
    Vector3 Direction;

    public bool isChild;

    // Start is called before the first frame update
    void Start()
    {
        Direction = transform.rotation * Vector2.right;
        transform.rotation = Quaternion.Euler(0,0,0);

        if (player.isInPsychicTerrain || player.isInSuperPsychicTerrain) {
            if (!isChild) {
                GameObject efile = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
                foreach (Transform e in efile.transform)
                {
                    if (e.GetComponent<Empty>() != null)
                    {
                        Instantiate(gameObject, e.position, Quaternion.identity).GetComponent<ExpandingForce>().isChild = true;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Empty enemy = collision.GetComponent<Empty>();
        if (enemy != null)
        {
            HitAndKo(enemy);

            if (SkillFrom == 2) {
                if (player.Skill01 && player.Skill01.SkillType == 14) { player.MinusSkillCDTime( 1 , 0.08f , false); }
                if (player.Skill02 && player.Skill02.SkillType == 14) { player.MinusSkillCDTime( 2 , 0.08f , false); }
                if (player.Skill03 && player.Skill03.SkillType == 14) { player.MinusSkillCDTime( 3 , 0.08f , false); }
                if (player.Skill04 && player.Skill04.SkillType == 14) { player.MinusSkillCDTime( 4 , 0.08f , false); }

                        
            }
        }
    }
}
