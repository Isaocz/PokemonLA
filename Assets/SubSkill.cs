using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill : MonoBehaviour
{

    //声明玩家对象，
    public PlayerControler player;
    //声明技能存在的时间
    public float ExistenceTime;
    //声明物理威力值
    public float Damage;
    //声明特攻威力值
    public float SpDamage;
    //声明动画管理者
    public Animator animator;
    //射弹类技能的最大距离
    public float MaxRange;

    //声明技能的属性
    public int SkillType;

    //一个布尔值表示攻击是否已发生，用于非多段伤害
    bool isHitDone = false;


    //声明2个变量，表示技能的冷却时间，以及技能可以击退敌人的距离
    public float KOPoint;
    public float ColdDown;

    //代表会心一击等级的变量
    public int CTLevel;

    public Skill MainSkill;
    public SubSkill subskill;





    //技能的Tag
    public Skill.SkillTagEnum[] SkillTag;
    //Tag1:接触类 Tag2:非接触类 Tag3:爪类 Tag4:牙类 Tag5:声音类

    //表示技能生成时是否生成于玩家所面对方向，如为Fales生成在玩家所面对的方向，如为true生成在玩家位置（多用于自我buff类技能）
    public bool isNotDirection;
    //对于一个有方向的技能（isNotDirection == False），生成时距离玩家的距离有多远。
    public float DirctionDistance;

    //表示技能是否会随着玩家移动 只对于isNotDirection == true的技能生效
    public bool isNotMoveWithPlayer;

    //表示技能是否是多端攻击
    public bool isMultipleDamage;
    //对于一个多段攻击技能，表示多段之间的冷却时间
    public float MultipleDamageCDTime;

    //表示技能生成是否需要抬手，比如位移类技能需要在摁下摁键的那一刻开始位移，而射弹类节能会有一个抬手前摇
    public bool isImmediately;


    //用于多端攻击的构造体
    struct EmptyList
    {


        public EmptyList(Empty target, bool v1, float v2) : this()
        {
            Target = target;
            isMultipleDamageColdDown = v1;
            MultipleDamageColdDownTimer = v2;
        }

        public Empty Target;
        public bool isMultipleDamageColdDown { get; set; }
        public float MultipleDamageColdDownTimer { get; set; }

    }
    List<EmptyList> TargetList = new List<EmptyList> { };







    //引用于所有技能的Update函数，当存在时间耗尽时技能消失
    public void StartExistenceTimer()
    {
        ExistenceTime -= Time.deltaTime;

        //多段攻击开始冷却之后开始计时
        if (isMultipleDamage)
        {
            RestoreTargetListCD();
        }

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


    void RestoreTargetListCD()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            EmptyList CDCell = TargetList[i];
            if (CDCell.isMultipleDamageColdDown)
            {
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= MultipleDamageCDTime) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }



    //对敌人target造成伤害和击退
    public void HitAndKo(Empty target)
    {
        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        if (isMultipleDamage)
        {
            bool isTargetExitInList = false;
            if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); break; }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown))
        {
            if (Damage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                else
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage*1.5f, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }


            }
            else if (SpDamage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage, 0, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                else
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage*1.5f, 0, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }

            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (isMultipleDamage)
            {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            if (player.playerData.IsPassiveGetList[26] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.6f)
            {
                if (SkillTag != null)
                {
                    foreach (Skill.SkillTagEnum i in SkillTag)
                    {
                        if (i == Skill.SkillTagEnum.接触类) { target.EmptyToxicDone(1 , 30); }
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
