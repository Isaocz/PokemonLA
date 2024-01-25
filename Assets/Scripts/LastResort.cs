using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastResort : Skill
{


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
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
            if (target != null ) {
                HitAndKo(target);
                if (animator != null) { animator.SetTrigger("Hit"); }
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).position = target.transform.position;
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                transform.GetChild(0).parent = null;
            }
        }

    }
}
