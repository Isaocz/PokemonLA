using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreak : Skill
{
    bool isR;

    // Start is called before the first frame update
    void Start()
    {
        if ( transform.rotation.eulerAngles.z == 180 || transform.rotation.eulerAngles.z == -180) { transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true; }
        transform.rotation = Quaternion.Euler(0,0,0);

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {

                if (target != null)
                {
                    if (target.AtkUpLevel != 0) { target.AtkChange(-target.AtkUpLevel, 0); target.ResetAtk(); isR = true; }
                    if (target.DefUpLevel != 0) { target.DefChange(-target.DefUpLevel, 0); target.ResetDef(); isR = true; }
                    if (target.SpAUpLevel != 0) { target.SpAChange(-target.SpAUpLevel, 0); target.ResetSpA(); isR = true; }
                    if (target.SpDUpLevel != 0) { target.SpDChange(-target.SpDUpLevel, 0); target.ResetSpD(); isR = true; }
                }
                if (SkillFrom == 2 && isR)
                {
                    CTLevel = 5;
                }
                HitAndKo(target);
            }
            
        }
    }

}
