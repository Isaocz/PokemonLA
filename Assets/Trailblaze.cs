using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailblaze : Skill
{
    //��ʼ�����������ײ����ɵ��˺�
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


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
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
