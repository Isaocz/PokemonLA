using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //声明玩家对象，
    public PlayerControler player;
    //声明技能存在的时间
    public float ExistenceTime;
    //声明物理威力值
    public float Damage;
    //声明特攻威力值
    public float SpDamage;
    //声明是否为威力可变化技能
    public bool IsDamageChangeable;
    //声明动画管理者
    public Animator animator;
    //射弹类技能的最大距离
    public float MaxRange;

    //声明技能的属性
    public int SkillType;
    //技能的英文名，中文名，技能描述
    public string SkillName;
    public string SkillChineseName;
    public string SkillDiscribe;

    //一个布尔值表示攻击是否已发生，用于非多段伤害
    bool isHitDone = false;


    //声明2个变量，表示技能的冷却时间，以及技能可以击退敌人的距离
    public float KOPoint;
    public float ColdDown;

    //技能的Tag
    public int[] SkillTag;
    //Tag1:接触类 Tag2:非接触类 Tag3:爪类 Tag4:牙类 Tag5:声音类

    //表示技能生成时是否生成于玩家所面对方向，如为Fales生成在玩家所面对的方向，如为False生成在玩家位置（多用于自我buff类技能）
    public bool isNotDirection;

    //表示技能是否会随着玩家移动 只对于isNotDirection == true的技能生效
    public bool isNotMoveWithPlayer;

    //表示技能是否是多端攻击
    public bool isMultipleDamage;

    //表示技能生成是否需要抬手，比如位移类技能需要在摁下摁键的那一刻开始位移，而射弹类节能会有一个抬手前摇
    public bool isImmediately;


    //引用于所有技能的Update函数，当存在时间耗尽时技能消失
    public void StartExistenceTimer()
    {
        ExistenceTime -= Time.deltaTime;
        if (ExistenceTime <= 0)
        {
            DestroySelf();
        }
    }

    //摧毁技能的函数，因为有时会在动画中调用，所以独立出来
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    //对敌人target造成伤害和击退
    public void HitAndKo(Empty target)
    {
        
        if (isMultipleDamage || !isHitDone) {
            if(Damage == 0)
            {
                if(Random.Range(0.0f , 1.0f ) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                else
                {
                    target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                

            }
            else if(SpDamage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                else
                {
                    target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                
            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (player.playerData.IsPassiveGetList[26] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.6f)
            {
                if (SkillTag != null)
                {
                    foreach (int i in SkillTag)
                    {
                        if (i == 1) { target.EmptyToxicDone(1); }
                    }
                }
            }
            if (player.playerData.IsPassiveGetList[25] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
            {
                target.Fear(3.0f, 1);
            }
        }

    }

}
