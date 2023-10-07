using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailblaze : Skill
{
    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (player.InGressCount != 0)
        {
            if (SkillFrom != 2) {
                if (player.Skill01 != null && player.Skill01.SkillIndex == SkillIndex)
                {
                    player.MinusSkillCDTime(1, 0.2f, false);
                }
                if (player.Skill02 != null && player.Skill02.SkillIndex == SkillIndex)
                {
                    player.MinusSkillCDTime(2, 0.2f, false);
                }
                if (player.Skill03 != null && player.Skill03.SkillIndex == SkillIndex)
                {
                    player.MinusSkillCDTime(3, 0.2f, false);
                }
                if (player.Skill04 != null && player.Skill04.SkillIndex == SkillIndex)
                {
                    player.MinusSkillCDTime(4, 0.2f, false);
                }
            }
            else
            {
                if (player.Skill01 != null)
                {
                    player.MinusSkillCDTime(1, 0.2f, false);
                }
                if (player.Skill02 != null)
                {
                    player.MinusSkillCDTime(2, 0.2f, false);
                }
                if (player.Skill03 != null)
                {
                    player.MinusSkillCDTime(3, 0.2f, false);
                }
                if (player.Skill04 != null)
                {
                    player.MinusSkillCDTime(4, 0.2f, false);
                }
            }
        }
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
        }
        
    }
}
