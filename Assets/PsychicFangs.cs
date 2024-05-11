using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicFangs : Skill
{

    // Start is called before the first frame update
    void Start()
    {
        if (transform.rotation.eulerAngles.z == 0) { transform.position += Vector3.right * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 90) { transform.position += Vector3.up * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 180) { transform.position += Vector3.left * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 270) { transform.position += Vector3.down * 0.5f; }
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Update()
    {
        StartExistenceTimer();
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                if (SkillFrom == 2) { target.TypeDef[14]--; }
                HitAndKo(target);
                if (target.AtkUpLevel > 0) { target.AtkChange(-target.AtkUpLevel, 0); target.ResetAtk(); }
                if (target.DefUpLevel > 0) { target.DefChange(-target.DefUpLevel, 0); target.ResetDef();  }
                if (target.SpAUpLevel > 0) { target.SpAChange(-target.SpAUpLevel, 0); target.ResetSpA(); }
                if (target.SpDUpLevel > 0) { target.SpDChange(-target.SpDUpLevel, 0); target.ResetSpD();  }
            }
        }
    }
}
