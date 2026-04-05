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
    public int CTDamage;

    public Skill MainSkill;
    public SubSkill subskill;

    public bool isPlusSkill;






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


    // 攻击种类（扑击等）
    public Pokemon.SpecialAttackTypes AttackType;


    //用于多端攻击的构造体
    struct EmptyList
    {


        public EmptyList(GameObject target, bool v1, float v2) : this()
        {
            Target = target;
            isMultipleDamageColdDown = v1;
            MultipleDamageColdDownTimer = v2;
        }

        public GameObject Target;
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
    public void HitAndKo(GameObject target)
    {
        Empty emptyTarget = target.GetComponent<Empty>();
        if (emptyTarget != null)
        {
            EmptyList TCEell = new EmptyList(emptyTarget.gameObject, false, 0.0f);
            int ListIndex = 0;


            if (isMultipleDamage)
            {
                bool isTargetExitInList = false;
                if (TargetList.Count == 0) { TargetList.Add(new EmptyList(emptyTarget.gameObject, false, 0.0f)); }
                for (int i = 0; i < TargetList.Count; i++)
                {
                    if (TargetList[i].Target == emptyTarget.gameObject) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; /*Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown)*/; break; }
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
                    float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                    if (player != null)
                    {
                        int EmptyBeforeHPandSHIELD = emptyTarget.EmptyHp + emptyTarget.EmptyShield;
                        int EmptyBeforeHP = emptyTarget.EmptyHp;
                        if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                        {
                            if (AttackType == Pokemon.SpecialAttackTypes.None)
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, 0, SpDamage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, (PokemonType.TypeEnum)SkillType);
                            }
                            else
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, 0, SpDamage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, (PokemonType.TypeEnum)SkillType, AttackType);

                            }
                        }
                        else
                        {
                            if (AttackType == Pokemon.SpecialAttackTypes.None)
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, 0, SpDamage * 1.5f * (Mathf.Pow(1.2f, CTDamage)) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1 * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, (PokemonType.TypeEnum)SkillType, true);
                            }
                            else
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, 0, SpDamage * 1.5f * (Mathf.Pow(1.2f, CTDamage)) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1 * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, (PokemonType.TypeEnum)SkillType, AttackType, true);

                            }
                            Skill.GetCTEffect(emptyTarget);
                        }
                        //粗糙皮肤
                        if (emptyTarget.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, Skill.SkillTagEnum.接触类))
                        {
                            Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHPandSHIELD - (emptyTarget.EmptyHp + emptyTarget.EmptyShield)) / 4, 1, 10000), 0, 0, PokemonType.TypeEnum.IgnoreType);
                        }
                        //冰冻之躯
                        if (emptyTarget.Abillity == Empty.EmptyAbillity.IceBody && _mTool.ContainsSkillTag(SkillTag, Skill.SkillTagEnum.接触类))
                        {
                            player.PlayerFrozenFloatPlus(0.25f, 2.0f);
                        }
                    }
                }
                else if (SpDamage == 0)
                {
                    float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

                    if (player != null)
                    {
                        int EmptyBeforeHPandSHIELD = emptyTarget.EmptyHp + emptyTarget.EmptyShield;
                        int EmptyBeforeHP = emptyTarget.EmptyHp;
                        if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                        {

                            if (AttackType == Pokemon.SpecialAttackTypes.None)
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, Damage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, 0, (PokemonType.TypeEnum)SkillType);
                            }
                            else
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, Damage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, 0, (PokemonType.TypeEnum)SkillType, AttackType);

                            }
                            //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);

                        }
                        else
                        {
                            if (AttackType == Pokemon.SpecialAttackTypes.None)
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, Damage * 1.5f * (Mathf.Pow(1.2f, CTDamage) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, 0, (PokemonType.TypeEnum)SkillType, true);
                            }
                            else
                            {
                                Pokemon.PokemonHpChange(player.gameObject, emptyTarget.gameObject, Damage * 1.5f * (Mathf.Pow(1.2f, CTDamage) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, 0, (PokemonType.TypeEnum)SkillType, AttackType, true);

                            }
                            Skill.GetCTEffect(emptyTarget);
                            //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? (( target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1 ) : 1)) ) + 2, 0, SkillType);

                        }
                        //粗糙皮肤
                        if (emptyTarget.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, Skill.SkillTagEnum.接触类))
                        {
                            Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHPandSHIELD - (emptyTarget.EmptyHp + emptyTarget.EmptyShield)) / 4, 1, 10000), 0, 0, PokemonType.TypeEnum.IgnoreType);
                        }
                        //冰冻之躯
                        if (emptyTarget.Abillity == Empty.EmptyAbillity.IceBody && _mTool.ContainsSkillTag(SkillTag, Skill.SkillTagEnum.接触类))
                        {
                            player.PlayerFrozenFloatPlus(0.25f, 2.0f);
                        }
                    }
                }
                emptyTarget.EmptyKnockOut(KOPoint);
                isHitDone = true;
                if (isMultipleDamage)
                {
                    TCEell.isMultipleDamageColdDown = true;
                    TargetList[ListIndex] = TCEell;
                }
            }
        }
        else
        {

            EmptyList TCEell = new EmptyList(target.gameObject, false, 0.0f);
            int ListIndex = 0;

            if (isMultipleDamage)
            {
                bool isTargetExitInList = false;
                if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target.gameObject, false, 0.0f)); }
                for (int i = 0; i < TargetList.Count; i++)
                {
                    if (TargetList[i].Target == target.gameObject) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break; }
                }
                if (!isTargetExitInList)
                {
                    TargetList.Add(TCEell);
                }
            }
            if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown))
            {
                Pokemon.PokemonHpChange(null, target.gameObject, Damage, SpDamage, 0, (PokemonType.TypeEnum)SkillType);
                isHitDone = true;
                if (isMultipleDamage)
                {
                    TCEell.isMultipleDamageColdDown = true;
                    TargetList[ListIndex] = TCEell;
                }

            }
        }
    }
}
