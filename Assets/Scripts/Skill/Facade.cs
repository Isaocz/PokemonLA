using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : Skill
{
    bool IsTriggerDone = false;
    public GameObject FlailWater;

    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        if(SkillFrom == 2)
        {
            if (player.isSleepStart || player.isToxicStart || player.isParalysisStart || (player.isBurnStart && !player.isBurnDone))
            {
                Damage *= 2;
            }
            if (player.isBurnDone)
            {
                Damage *= 4;
            }
        }
        else
        {
            if(player.isToxicDone || player.isParalysisDone)
            {
                Damage *= 2;
            }
            if (player.isBurnDone)
            {
                Damage *= 4;
            }
        }
        animator = GetComponent<Animator>();
        Instantiate(FlailWater, player.transform.position + Vector3.up * 0.5f, Quaternion.identity, player.transform);
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        if (!IsTriggerDone) { StartExistenceTimer(); }
    }


    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            IsTriggerDone = true;
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            gameObject.transform.position = other.transform.position;
        }
    }

}
