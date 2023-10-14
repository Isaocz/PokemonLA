using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenHeadbutt : Skill
{

    public SubZenHeadbutt SubZH;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (SkillFrom == 2) { player.AddASubSkill(SubZH); }
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
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.8f)
            {
                target.Fear(2.5f, 1);
            }
        }

    }
}
