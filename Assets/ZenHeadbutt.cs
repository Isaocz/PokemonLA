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
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.8f)
            {
                target.Fear(2.5f, 1);
            }
        }

    }
}
