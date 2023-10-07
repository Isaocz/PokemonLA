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
                Target.AtkAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.AtkEmptyPoint);
                Target.SpAAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.SpAEmptyPoint);
                Target.DefAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.DefEmptyPoint);
                Target.SpdAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.SpdEmptyPoint);
                Target.SpeedAbilityPoint = Target.AbilityForLevel(Target.Emptylevel, Target.SpeedEmptyPoint);

                Target.AtkDownRemove(); Target.AtkUpRemove();
                Target.DefDownRemove(); Target.DefUpRemove();
                Target.SpADownRemove(); Target.SpAUpRemove();
                Target.SpDDownRemove(); Target.SpDUpRemove();
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
