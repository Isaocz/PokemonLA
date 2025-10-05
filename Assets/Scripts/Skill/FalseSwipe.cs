using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseSwipe : Skill
{
    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            if (target != null) {
                int BeforeHP = target.EmptyHp;
                target.IsBeFalseSwipe = true;
                HitAndKo(target);
                target.IsBeFalseSwipe = false;
                if (animator != null) { animator.SetTrigger("Hit"); }
                if (SkillFrom == 2 && BeforeHP != 1 && target.EmptyHp == 1)
                {
                    player.ChangeHPW(target.HWP);
                    player.ReFreshAbllityPoint();
                }
            }
        }
    }
}
