using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryCutter : Skill
{

    public SubFuryCutter sub2;
    public SubFuryCutter sub3;



    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);

        Invoke( "Improve" , 0.1f);
    }

    void Update()
    {
        StartExistenceTimer();
    }

    void Improve()
    {
        if (SkillFrom == 2)
        {

            switch (Count)
            {
                case 0:
                    DrainBounsPer += 0.0f;
                    break;
                case 2:
                    DrainBounsPer += 0.1f;
                    break;
                case 4:
                    DrainBounsPer += 0.2f;
                    break;
            }
        }
    }

    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {

            Empty target = other.GetComponent<Empty>();
            if (target != null) {

                int BeforeHP = target.EmptyHp;
                HitAndKo(target);
                int AfterHP = target.EmptyHp;
                if (SkillFrom == 2) { Drain(BeforeHP , AfterHP , DrainBounsPer); }
                if (Count == 0) {
                    player.AddASubSkill(sub2);
                }
                else
                {
                    player.AddASubSkill(sub3);
                }
            }
        }
    }
}
