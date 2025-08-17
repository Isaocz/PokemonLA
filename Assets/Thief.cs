using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Skill
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
            if (target != null)
            {
                if (target.DropItem != null)
                {
                    
                    if ((target.EmptyBossLevel == Empty.emptyBossLevel.Boss || target.EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
                    {
                        if (SkillFrom == 2) { Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp / 4, PokemonType.TypeEnum.IgnoreType); }
                        Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                    }
                    else
                    {
                        if (target.IsHaveDropItem)
                        {
                            if (SkillFrom == 2) { Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp / 4, PokemonType.TypeEnum.IgnoreType); }
                            target.EmptyDrop();
                        }
                        else
                        {
                            if (SkillFrom == 2) { Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 8, 0, 0, PokemonType.TypeEnum.IgnoreType); player.KnockOutDirection = Vector2.zero; player.KnockOutPoint = 0; }
                        }
                    }
                    target.DropItem = null;
                }
                else
                {
                    if (SkillFrom == 2) { Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 8, 0, 0, PokemonType.TypeEnum.IgnoreType); player.KnockOutDirection = Vector2.zero;player.KnockOutPoint = 0; }
                }
                HitAndKo(target);
            }
            if (animator != null) { animator.SetTrigger("Hit"); }
        }
    }
}
