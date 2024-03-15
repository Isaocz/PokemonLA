using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //技能的序号
    public int SkillIndex;
    //声明玩家对象，
    public PlayerControler player;
    //如果该技能为跟随宝宝释放，使用宝宝
    public Baby baby;
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
    //精通技能的描述
    public string PlusSkillDiscribe;
    //该技能的精通技能
    public Skill PlusSkill;
    //对于已精通的技能，代表了精通前的版本；
    public Skill MinusSkill;

    //一个布尔值表示攻击是否已发生，用于非多段伤害
    protected bool isHitDone = false;


    //声明2个变量，表示技能的冷却时间，以及技能可以击退敌人的距离
    public float KOPoint;
    public float ColdDown;

    //代表会心一击等级的变量
    public int CTLevel;
    public int CTDamage;
    //表示该技能是否经过PPUP
    public bool isPPUP
    {
        //Debug.Log(isppp);
        get { return isppp; }
        set { isppp = value; }
    }
    bool isppp ;

    
    //技能的品质等级
    public int SkillQualityLevel;
    //表示技能的来源 0表示由学习获得 1表示由技能学习机获得 2表示精通技能
    public int SkillFrom;

    


    //技能的Tag
    public Skill.SkillTagEnum[] SkillTag;
    //Tag1:接触类 Tag2:非接触类 Tag3:爪类 Tag4:牙类 Tag5:声音类
    public enum SkillTagEnum
    {
        接触类,非接触类,爪类,牙类,声音类,连续多次使用类,恢复HP类,吸取HP类,降低使用者能力类,反作用力伤害类,爆炸类,
        拳类,波动和波导类,粉末类,球和弹类,心灵攻击类,跳舞类,风类,切割类,天气类,场地类,防住类,压击类,绑紧类,雾类,脚踢类,激光类,尾巴类,带有跟踪效果或类似效果的技能,
    }

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
    protected struct EmptyList
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
    protected List<EmptyList> TargetList = new List<EmptyList> { };


    void ResetPlayer()
    {
        if(player == null && baby == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }

    

    //生成技能的暴击特效
    protected void GetCTEffect(Empty target)
    {
        //获取暴击动画特效
        GameObject CTEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(0);
        //实例化
        Instantiate(CTEffect, target.transform.position + Vector3.right*Random.Range(-0.5f,0.5f) + Vector3.up * Random.Range(0.0f, 0.8f), Quaternion.identity , target.transform).SetActive(true);
    }



    private void Awake()
    {
        Invoke("SkillStart", 0.05f);
    }

    void SkillStart()
    {
        Debug.Log(111);
        if (player.isInSuperPsychicTerrain)
        {
            bool isTraceSkill = false;
            for (int i = 0; i < SkillTag.Length; i++)
            {
                if (SkillTag[i] == SkillTagEnum.带有跟踪效果或类似效果的技能) { isTraceSkill = true; }
            }
            if (isTraceSkill)
            {
                TraceEffect TE = null;
                foreach (Transform t in transform)
                {
                    if (t.GetComponent<TraceEffect>())
                    {
                        TE = t.GetComponent<TraceEffect>();
                        break;
                    }
                }
                if (TE == null) { TE = GetComponent<TraceEffect>(); }
                if (TE != null)
                {
                    TE.distance += 1.3f;
                }
            }
        }
        if (SkillType == 15 && player.PlayerAbility == PlayerControler.PlayerAbilityList.雪隐)
        {
            player.TriggerSnowCloak();
        }
        if (player.PlayerAbility == PlayerControler.PlayerAbilityList.迟钝)
        {
            bool isTouch = false;
            foreach (Skill.SkillTagEnum i in SkillTag){  if (i == Skill.SkillTagEnum.接触类) { isTouch = true; break; } }
            if (isTouch) { player.TriggerOblivious(); }
        }


        //声音类技能
        if (_mTool.ContainsSkillTag(SkillTag, SkillTagEnum.声音类)){ 

            //道具065 胖丁麦克风
            if (player.playerData.IsPassiveGetList[65])
            {
                Instantiate(PassiveItemGameObjList.ObjList.List[19], player.transform.position, Quaternion.identity, player.transform);
            }

            //道具073 闪焰高歌
            if (player.playerData.IsPassiveGetList[73])
            {
                TorchSongFire f01 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                TorchSongFire f02 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                TorchSongFire f03 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                f01.LaunchNotForce((Vector3)player.look.normalized, 3.5f);
                f02.LaunchNotForce((Quaternion.AngleAxis(30, Vector3.forward) * (Vector3)player.look).normalized, 3.5f);
                f03.LaunchNotForce((Quaternion.AngleAxis(-30, Vector3.forward) * (Vector3)player.look).normalized, 3.5f);
                f01.player = player;
                f02.player = player;
                f03.player = player;
            }

            //道具117 爽喉喷雾
            if (player.playerData.IsPassiveGetList[117])
            {
                player.playerData.PassiveItemThroatSpray();
            }
        }

        //声音类技能
        if (_mTool.ContainsSkillTag(SkillTag, SkillTagEnum.接触类))
        {
            //道具138 锐利之爪
            if (player.playerData.IsPassiveGetList[138])
            {
                CTDamage++;CTLevel++;
            }
        }

        if (SkillType == (int)Type.TypeEnum.Water )
        {
            //道具074 润水发型
            if (player.playerData.IsPassiveGetList[74])
            {
                player.MinusSkillCDTime(1 , 0.1f , false);
                player.MinusSkillCDTime(2 , 0.1f , false);
                player.MinusSkillCDTime(3 , 0.1f , false);
                player.MinusSkillCDTime(4 , 0.1f , false);
            }
        }

        //道具078 贝壳刃
        if (player.playerData.IsPassiveGetList[78])
        {
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.85f)
            {
                ScalchopPro s01 = Instantiate(PassiveItemGameObjList.ObjList.List[23], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<ScalchopPro>();
                s01.player = player;
                s01.LaunchNotForce(player.look , 9.0f);
                s01.transform.rotation = Quaternion.Euler( 0 , 0 , _mTool.Angle_360Y(player.look , Vector2.right) );
            }
        }

        //道具091 飞水手里剑
        if (player.playerData.IsPassiveGetList[91])
        {
            ScalchopPro s01 = Instantiate(PassiveItemGameObjList.ObjList.List[24], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<ScalchopPro>();
            s01.player = player;
            s01.LaunchNotForce(-player.look, 9.0f);
        }

        //道具093 魔幻假面
        if (player.playerData.IsPassiveGetList[93])
        {
            player.playerData.MasqueradeChangeType((Type.TypeEnum)SkillType);
        }

        //道具096 冷静头脑
        if (SkillType == 15 && player.playerData.IsPassiveGetList[96])
        {
            player.playerData.PassiveItemClamMind();
        }

        //道具103 冰萝卜
        if (SkillType == 15 && player.playerData.IsPassiveGetList[103])
        {
            Damage *= 1.35f;SpDamage *= 1.35f;
        }

        //道具104 黑萝卜
        if (SkillType == 8 && player.playerData.IsPassiveGetList[104])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;

        }
        //道具106 恶之挂轴
        if (SkillType == 17 && player.playerData.IsPassiveGetList[106])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;
        }

        //道具107 水之挂轴
        if (SkillType == 11 && player.playerData.IsPassiveGetList[107])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;
        }

        //道具112 达人带
        if (player.playerData.IsPassiveGetList[112])
        {
            CTDamage++;
        }

        //道具137 焦点镜
        if (player.playerData.IsPassiveGetList[137])
        {
            CTLevel++;
        }

        //道具114 节拍器
        if (player.playerData.IsPassiveGetList[114])
        {
            if (SkillIndex == player.playerData.MetronomeSkillIndex) { player.playerData.MetronomeCount++; }
            else { player.playerData.MetronomeCount = 0; player.playerData.MetronomeSkillIndex = SkillIndex; }
            Damage *= Mathf.Clamp(Mathf.Pow(1.2f, player.playerData.MetronomeCount) , 1.0f , 2.0f);
            SpDamage *= Mathf.Clamp(Mathf.Pow(1.2f, player.playerData.MetronomeCount), 1.0f, 2.0f);
        }

        //特性12:激流
        if (player.PlayerAbility == PlayerControler.PlayerAbilityList.激流 && SkillType == (int)Type.TypeEnum.Water && player.Hp < player.maxHp / 3.0f) { 
            Damage *= 1.5f; 
            SpDamage *= 1.5f; 
        }
    }

    //引用于所有技能的Update函数，当存在时间耗尽时技能消失
    public void StartExistenceTimer()
    {
        ResetPlayer();
        ExistenceTime -= Time.deltaTime;
        //if (!isStartMDone) { SkillStart(); }

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
            if (CDCell.isMultipleDamageColdDown) { 
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= MultipleDamageCDTime) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }


    /// <summary>
    /// 对敌人target造成伤害和击退
    /// </summary>
    /// <param name="target"></param>
    public virtual void HitAndKo(Empty target)
    {
        BeforeHitEvent(target);

        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        if (isMultipleDamage)
        {
            bool isTargetExitInList = false;
            if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i ; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break;   }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown)) {

            int BeforeHP = target.EmptyHp;
            if (Damage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                if (player != null) {
                    int EmptyBeforeHP = target.EmptyHp;
                    if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                    {
                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, (Type.TypeEnum)SkillType);
                    }
                    else
                    {
                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage * 1.5f * (Mathf.Pow(1.2f, CTDamage)) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1 * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, (Type.TypeEnum)SkillType, true);
                        GetCTEffect(target);
                    }
                    if (target.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, SkillTagEnum.接触类)) { Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHP - target.EmptyHp) / 4, 1, 10000), 0, 0, Type.TypeEnum.IgnoreType); }
                }
                else if (baby != null)
                {
                    Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, 0, SpDamage * 1.5f, 0, (Type.TypeEnum)SkillType);
                    Debug.Log(baby);
                }

            }
            else if(SpDamage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

                if (player != null)
                {
                    int EmptyBeforeHP = target.EmptyHp;
                    if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                    {

                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, 0, (Type.TypeEnum)SkillType);
                        //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);

                    }
                    else
                    {

                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage * 1.5f * (Mathf.Pow(1.2f, CTDamage) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, 0, (Type.TypeEnum)SkillType, true);
                        GetCTEffect(target);
                        //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? (( target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1 ) : 1)) ) + 2, 0, SkillType);

                    }
                    if (target.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, SkillTagEnum.接触类)) { Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHP - target.EmptyHp) / 4 , 1 , 10000), 0, 0, Type.TypeEnum.IgnoreType); }
                }
                else if (baby != null)
                {
                    Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, Damage, 0, 0, (Type.TypeEnum)SkillType);
                    Debug.Log(baby);
                }
            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (isMultipleDamage) {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            HitEvent(target);


            //道具136 贝壳铃
            if (player.playerData.IsPassiveGetList[136])
            {
                Drain(BeforeHP , target.EmptyHp , 0.1f);
            }

            //招式378 羽栖+
            {
                if (player.Skill01 != null && player.Skill01.SkillIndex == 378 ) { player.MinusSkillCDTime(1,KOPoint,true); }
                if (player.Skill02 != null && player.Skill02.SkillIndex == 378 ) { player.MinusSkillCDTime(2,KOPoint,true); }
                if (player.Skill03 != null && player.Skill03.SkillIndex == 378 ) { player.MinusSkillCDTime(3,KOPoint,true); }
                if (player.Skill04 != null && player.Skill04.SkillIndex == 378 ) { player.MinusSkillCDTime(4,KOPoint,true); }
            }

        }

    }


    /// <summary>
    /// 造成伤害时发生的事件
    /// </summary>
    /// <param name="target"></param>
    public void HitEvent(Empty target)
    {
        if (player != null)
        {
            //是否是接触类技能
            bool isTouch = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.接触类) { isTouch = true; break; }
                }
            }

            //是否是牙类技能
            bool isBite = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.牙类) { isBite = true; break; }
                }
            }

            //是否是爪类技能
            bool isClaw = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.爪类) { isClaw = true; break; }
                }
            }



            //接触技能触发的事件
            if (isTouch) {

                //道具026 毒手
                if (player.playerData.IsPassiveGetList[26])
                {
                    target.EmptyToxicDone(1, 30, 0.4f + (float)player.LuckPoint / 10);
                }

                //道具059 金假牙
                if (player.playerData.IsPassiveGetList[59] && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30) > 0.8f)
                {
                    Instantiate(PassiveItemGameObjList.ObjList.List[18] , transform.position , Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                }

                //特性 迷人之躯
                if (player.PlayerAbility == PlayerControler.PlayerAbilityList.迷人之躯 && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30) > 0.9f)
                {
                    target.EmptyInfatuation(15, 0.5f);
                }

            }

            //牙技能触发的事件
            if (isBite)
            {
                //道具63 锐利之牙
                if (player.playerData.IsPassiveGetList[63] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 10 > 0.4f)
                {
                    target.Fear(3.0f, 1);
                }
            }

            //爪技能触发的事件
            if (isClaw)
            {
                //道具63 锐利之牙
                if (player.playerData.IsPassiveGetList[63] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 10 > 0.4f)
                {
                    target.Fear(3.0f, 1);
                }
            }

            //所有技能触发的事件
            {
                //道具025 王者之证
                if (player.playerData.IsPassiveGetList[25] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
                {
                    target.Fear(3.0f, 1);
                }

                //道具097 毒锁链
                if (player.playerData.IsPassiveGetList[97])
                {
                    target.EmptyToxicDone(1, 30, 0.2f + (float)player.LuckPoint / 30);
                }

            }


        }
    }


    /// <summary>
    /// 造成伤害之前发生的事件
    /// </summary>
    /// <param name="target"></param>
    public void BeforeHitEvent(Empty target)
    {
        //道具101 博士的面具
        if (player.playerData.IsPassiveGetList[101] && target.isBlindDone)
        {
            Damage *= 1.5f;
            SpDamage *= 1.5f;
        }
    }


    //=================================多次攻击的次数判定===========================================
   /// <summary>
   /// 如岩石暴击，冰锥等次数为2-5次的攻击，使用次方法输出次数
   /// </summary>
   /// <returns></returns>
    protected int Count2_5()
    {
        float p = Random.Range(0.0f, 1.0f)+((float)player.LuckPoint/30);
        //道具119 机变骰子
        if (player.playerData.IsPassiveGetList[119]) {
            if (p >= 0 && p <= 0.4f)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        else
        {
            if (p >= 0 && p <= 0.35f)
            {
                return 2;
            }
            else if (p >= 0.35f && p <= 0.70f)
            {
                return 3;
            }
            else if (p >= 0.70f && p <= 0.85f)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }

    protected int Count1_3()
    {
        float p = Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30);

        //道具119 机变骰子
        if (player.playerData.IsPassiveGetList[119])
        {
            return 3;
        }
        else
        {
            if (p >= 0 && p <= 0.65f)
            {
                return 1;
            }
            else if (p >= 0.65f && p <= 0.90f)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    //=================================多次攻击的次数判定===========================================





    //==========================有关技能的条件判定=================================

    public bool useSkillConditions(PlayerControler player)
    {
        //为梦话或者打鼾
        if( SkillIndex == 53 || SkillIndex == 54 || SkillIndex == 55 || SkillIndex == 56)
        {
            return SleepCanUseSkill(player);
        }
        //为投掷
        else if (SkillIndex == 303)
        {

            if (player.spaceItem == null) { return false; }
            else { return true; }
        }
        //为投掷精通
        else if (SkillIndex == 304)
        {
            if (Mathf.Ceil(12 + player.maxHp * 0.06f) > player.Hp - 1) { return false; }
            else { return true; }
        }
        //为珍藏或者珍藏精通
        else if (SkillIndex == 317 || SkillIndex == 318)
        {
            if (((player.Skill01 == null) || (player.Skill01 != null && player.Skill01.SkillIndex != 317 && player.Skill01.SkillIndex != 318 && player.isSkill01CD) || (player.Skill01 != null && ( player.Skill01.SkillIndex == 317 || player.Skill01.SkillIndex == 318 ))) &&
                ((player.Skill02 == null) || (player.Skill02 != null && player.Skill02.SkillIndex != 317 && player.Skill02.SkillIndex != 318 && player.isSkill02CD) || (player.Skill02 != null && (player.Skill02.SkillIndex == 317 || player.Skill02.SkillIndex == 318))) &&
                ((player.Skill03 == null) || (player.Skill03 != null && player.Skill03.SkillIndex != 317 && player.Skill03.SkillIndex != 318 && player.isSkill03CD) || (player.Skill03 != null && (player.Skill03.SkillIndex == 317 || player.Skill03.SkillIndex == 318))) &&
                ((player.Skill04 == null) || (player.Skill04 != null && player.Skill04.SkillIndex != 317 && player.Skill04.SkillIndex != 318 && player.isSkill04CD) || (player.Skill04 != null && (player.Skill04.SkillIndex == 317 || player.Skill04.SkillIndex == 318))) 
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //为其他技能
        else
        {
            return NormalSkill(player);
        }
    }


    protected static bool NormalSkill(PlayerControler player)
    {
        if (player.isSleepDone) { return false; }
        else { return true; }
    }

    protected static bool SleepCanUseSkill(PlayerControler player)
    {
        if (player.isSleepDone) { return true; }
        else { return false; }
    }

    //==========================有关技能的条件判定=================================



    //==================================百分比吸血========================================
    /// <summary>
    /// 吸血百分比的补正
    /// </summary>
    public float DrainBounsPer;
    public void Drain(int TargetHpBefore , int TargetHpAfter , float DrainPer)
    {
        if (TargetHpAfter < TargetHpBefore)
        {
            Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, Mathf.Clamp((int)((float)(TargetHpBefore - TargetHpAfter) * (DrainPer + DrainBounsPer)), 1, 100), Type.TypeEnum.IgnoreType);
        }
    }

    //==============================================================================


}
