using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : Skill
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
            HitAndKo(target);
            if (SkillFrom == 2 && target.TypeDef[17] >= 0) { target.TypeDef[17]--; }
            //if (animator != null) { animator.SetTrigger("Hit"); }
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.7f)
            {
                target.Fear(2.5f, 1);
            }
        }
    }
}
