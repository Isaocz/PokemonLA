using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haze : Skill
{

    CircleCollider2D HazePlusCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        HazePlusCollider2D = GetComponent<CircleCollider2D>();
        player.playerData.RestoreJORSata();
        player.ReFreshAbllityPoint();
    }
    private void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 3)
        {
            HazePlusCollider2D.radius -= Time.deltaTime * 1.5f;
        }
        if (ExistenceTime >= 14)
        {
            HazePlusCollider2D.radius += Time.deltaTime * 2f;
        }
    }



    List<Empty> HazePlusList = new List<Empty> { };
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty Target = other.GetComponent<Empty>();
            if (Target != null) {
                if (Target.AtkUpLevel != 0) { Target.AtkChange(-Target.AtkUpLevel , 0); }
                if (Target.DefUpLevel != 0) { Target.DefChange(-Target.DefUpLevel , 0); }
                if (Target.SpAUpLevel != 0) { Target.SpAChange(-Target.SpAUpLevel , 0); }
                if (Target.SpDUpLevel != 0) { Target.SpDChange(-Target.SpDUpLevel , 0); }
                Target.SpeedAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.SpeedEmptyPoint);
                Target.SpeedRemove01(0); 


                if (SkillFrom == 2)
                {
                    if (!HazePlusList.Contains(Target))
                    {
                        Target.Cold(15);
                    }
                    HazePlusList.Add(Target);
                }
            }
        }
    }
}
