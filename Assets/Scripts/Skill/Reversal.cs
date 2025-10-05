using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversal : Skill
{
    bool IsTriggerDone = false;

    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject PS = transform.GetChild(0).gameObject;
        PS.transform.parent = player.transform;
        PS.transform.localPosition = Vector3.zero;
        PS.transform.localScale = new Vector3(1,1,1);
        PS.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        if (!IsTriggerDone) { StartExistenceTimer(); }
    }

    void FlailDamage(float PlayerHPPer)
    {
        if (PlayerHPPer >= 0.6875f) { Damage = 20; }
        else if (PlayerHPPer >= 0.3542f && PlayerHPPer < 0.6875f) { Damage = 40; if (SkillFrom == 2) { CTLevel++; } }
        else if (PlayerHPPer >= 0.2083f && PlayerHPPer < 0.3542f) { Damage = 80; if (SkillFrom == 2) { CTLevel++; CTDamage++; } }
        else if (PlayerHPPer >= 0.1042f && PlayerHPPer < 0.2083f) { Damage = 100; if (SkillFrom == 2) { CTLevel++; CTDamage++; } }
        else if (PlayerHPPer >= 0.0417f && PlayerHPPer < 0.1042f) { Damage = 150; if (SkillFrom == 2) { CTLevel += 2; CTDamage++; } }
        else if (PlayerHPPer < 0.0417f) { Damage = 200; if (SkillFrom == 2) { CTLevel += 2; CTDamage += 2; } }
    }

    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            IsTriggerDone = true;
            Empty target = other.GetComponent<Empty>();
            FlailDamage(((float)(player.Hp)) / ((float)(player.maxHp)));
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            gameObject.transform.position = other.transform.position;
        }
    }
}
