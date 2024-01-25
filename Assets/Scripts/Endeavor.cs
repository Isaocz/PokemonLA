using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endeavor : Skill
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
                HitAndKo(target);
                if (animator != null) { animator.SetTrigger("Hit"); }
                if (!target.isBoos && target.EmptyHp > player.Hp)
                {
                    Pokemon.PokemonHpChange(null, target.gameObject, target.EmptyHp - player.Hp, 0, 0, Type.TypeEnum.IgnoreType);
                }
            }
        }
    }
}
