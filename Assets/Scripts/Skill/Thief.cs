using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Skill
{
    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
