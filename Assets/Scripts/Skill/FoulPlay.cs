using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulPlay : Skill
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

                if (SkillFrom == 2) {
                    if(target.AtkAbilityPoint >= player.AtkAbilityPoint)
                    {
                        Damage += 25;
                        Pokemon.PokemonHpChange(null , player.gameObject ,player.maxHp/16 , 0,0,PokemonType.TypeEnum.IgnoreType );
                        player.KnockOutDirection = Vector2.zero;
                        player.KnockOutPoint = 0;
                    }
                    else
                    {
                        Pokemon.PokemonHpChange(null, player.gameObject,0 , 0, player.maxHp / 16, PokemonType.TypeEnum.IgnoreType);
                    }
                }
                HitAndKo(target);
            }
            if (animator != null) { animator.SetTrigger("Hit"); }
        }
    }
}
