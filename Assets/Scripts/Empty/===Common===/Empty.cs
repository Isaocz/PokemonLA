using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class EnumMultiAttribute : PropertyAttribute { }
public class Empty : Pokemon
{

    /// <summary>
    /// 敌人识别码
    /// </summary>
    public string EmptyCD;

    /// <summary>
    /// 切换房间时保存的等级
    /// </summary>
    public int SaveLevel
    {
        get { return saveLevel; }
        set { saveLevel = value; }
    }
    int saveLevel = -1;

    /// <summary>
    /// 切换房间时保存的血量
    /// </summary>
    public int SaveHp
    {
        get { return saveHp; }
        set { saveHp = value; }
    }
    int saveHp = -1;

    /// <summary>
    /// 切换房间时保存的 护盾
    /// </summary>
    public int SaveShield
    {
        get { return saveShield; }
        set { saveShield = value; }
    }
    int saveShield = -1;


    //攻击到玩家时造成的击退值声明4个变量，一个代表对玩家造成的伤害，一个代表击退值，一个表示移动的距离,一个表示移动速度，一个表示初始血量
    public float Knock = 5f;
    //敌人的当前血量和最大血量
    [Tooltip("当前HP")]
    public int EmptyHp;
    public int maxHP;
    [Tooltip("当前护盾")]
    public int EmptyShield;
    //敌人的等级
    [Tooltip("等级")]
    public int Emptylevel;
    [Tooltip("敌人是否正处于出生动画中，如果正在出生不会行动")]
    public bool isBorn;
    [Tooltip("ui：血条")]
    public EmptyHpBar uIHealth;

    //声明两个整形变量，表示敌人的两个属性
    [Header("属性")]
    [EnumMultiAttribute]
    public PokemonType.TypeEnum EmptyType01;
    //public int EmptyType01;
    public PokemonType.TypeEnum EmptyType02;

    [Header("种族值")]
    //声明六个整形数据，表示角色的六项种族值,以及六项当前能力值,MaxLevel表示该敌人可以到达的最高等级
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int DefEmptyPoint;
    public int SpAEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;
    public int MaxLevel;


    public enum EmptyAbillity
    {
        None,
        RoughSkin,//粗糙皮肤
        Levitate,//漂浮
        IceBody,//冰冻之躯（接触后冰冻）
    }
    public EmptyAbillity Abillity;

    //能力值
    public float AtkAbilityPoint { get { return AtkAbility; } set { AtkAbility = value; } }
    float AtkAbility;
    public float SpAAbilityPoint { get { return SpAAbility; } set { SpAAbility = value; } }
    float SpAAbility;
    public float DefAbilityPoint { get { return DefAbility; } set { DefAbility = value; } }
    float DefAbility;
    public float SpdAbilityPoint { get { return SpDAbility; } set { SpDAbility = value; } }
    float SpDAbility;
    public float SpeedAbilityPoint { get { return SpeedAbility; } set { SpeedAbility = value; } }
    float SpeedAbility;

    public bool isCanHitAnimation { get { return iscanHitAnimation; } set { iscanHitAnimation = value; } }bool iscanHitAnimation;


    /// <summary>
    /// 敌人对于某一属性的抗性，关于属性的Index请参考Type.cs
    /// </summary>
    public int[] TypeDef = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



    //敌人摧毁时的时间
    public delegate void EmptyEvent();
    public EmptyEvent DestoryEvent;

    //声明2个布尔值，表示目标对象是否死亡,表示目标是否被攻击
    public bool isDie = false;
    public bool isHit = false;
    //无敌状态
    private bool isInvincible = false;
    public bool Invincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }
    //一个击退计时器，一个被击退值
    float KOTimer = 0;
    float KOPoint;


    //声明一个变量，表示该目标被击倒后获得的经验，Exp根据BaseExp换算而来
    public int Exp;
    public int BaseExp;
    /// <summary>
    /// 二维变量分别为获得努力值的种类和点数(数值参考PlayerControler.ChangeHPW())
    /// </summary>
    public Vector2Int HWP;


    /// <summary>
    /// 敌人受到伤害的最大比例（比如返回值为3.0f ， 则该敌人每次最多受到最大生命值三分之1的伤害）
    /// </summary>
    /// <returns></returns>
    int MaxDmagePer()
    {
        if (EmptyBossLevel == emptyBossLevel.Boss) { return 6; }
        if (EmptyBossLevel == emptyBossLevel.MiniBoss) { return 3; }
        return 1;
    }


    //获取玩家对象
    public PlayerControler player;

    //声明一个刚体变量 一个动画管理者变量(动画管理者已经转到父类Pokeomn)
    public new Rigidbody2D rigidbody2D;


    //表示异常状态的各种变量
    /// <summary>
    /// 敌人是否陷入沉默状态
    /// </summary>
    public bool isSilence = false;



    /// <summary>
    /// 敌人boss等级 分为普通敌人 小boss 大boss 最终boss
    /// </summary>
    public enum emptyBossLevel
    {
        NormalEmpty,
        MiniBoss,
        Boss,
        EndBoss
    }
    public emptyBossLevel EmptyBossLevel;


    /// <summary>
    /// 敌人被打死后掉落的道具
    /// </summary>
    public GameObject DropItem;

    /// <summary>
    /// 敌人受到的伤害显示
    /// </summary>
    public GameObject FloatingDmg;

    /*
    //表示敌人是否处于被白雾【精通】击中
    public bool isMistPlus;
    */

    /// <summary>
    /// 表示敌人是否进入被替身吸引状态
    /// </summary>
    public bool isSubsititue
    {
        get { return issubstitute; }
        set { issubstitute = value; }
    }
    bool issubstitute;
    /// <summary>
    /// 表示敌人被替身吸引时的目标替身
    /// </summary>
    public GameObject SubsititueTarget
    {
        get { return subsititueTarget; }
        set { subsititueTarget = value; }
    }
    GameObject subsititueTarget;

    [Tooltip("非冰系敌人是否抵抗冰雹")]
    public bool isHailDef;
    [Tooltip("非地岩钢系敌人是否抵抗沙暴")]
    public bool isSandStormDef;


    public bool isShadow;


    public Room ParentPokemonRoom
    {
        get { return parentRoom; }
        set { parentRoom = value; }
    }
    Room parentRoom;


    //当敌人身体有多个部分时（如三地鼠的多个地鼠 ， 大岩蛇的多个体节） ， 把每一个身体部分放入该List；
    public List<SubEmptyBody> SubEmptyBodyList
    {
        get { return subEmptyBodyList; }
        set { subEmptyBodyList = value; }
    }
    List<SubEmptyBody> subEmptyBodyList = new List<SubEmptyBody> { };


    /// <summary>
    /// 仅对于有多体节的敌人使用，为了防止多体节被多次扣血，在短时间内让敌人无敌。
    /// </summary>
    public bool isSubBodyEmptyInvincible
    {
        get { return issubBodyEmptyInvincible; }
        set { issubBodyEmptyInvincible = value; }
    }
    bool issubBodyEmptyInvincible;


    /// <summary>
    /// 敌人是否持有道具
    /// </summary>
    public bool IsHaveDropItem { get { return isHaveDropItem; } set { isHaveDropItem = value; } }
    bool isHaveDropItem;


    //Boss的防御和攻击加成。
    protected float BossDefBouns = 1.5f;
    protected float BossAtkBouns = 0.7f;



    /// <summary>
    /// 敌人死亡时是否可以触发亡语效果，如在死亡时冰冻，畏缩则不可触发；
    /// </summary>
    public bool IsDeadrattle
    {
        get { return (!isEmptyFrozenDone) && (!isFearDone) && (!isBlindDone); }
    }



    /// <summary>
    /// 敌人是否被伤害过
    /// </summary>
    public bool isHurt { get { return ishurt; } set { ishurt = value; } }
    bool ishurt;

































































    //=============================初始化敌人数据================================

    //敌人最初的未经改变的速度
    float FirstSpeed;
    public void ResetSpeed()
    {
        speed = FirstSpeed;
    }

    /// <summary>
    /// ---start中调用---，根据玩家等级动态设定敌人等级，
    /// </summary>
    /// <param name="PlayerLevel"></param>
    /// <param name="MaxLevel"></param>
    /// <returns></returns>
    protected int SetLevel(int PlayerLevel,int MaxLevel)
    {
        StartCoroutine(SetInvincible(1.2f));
        if (transform.parent.parent.GetComponent<Room>() != null) { ParentPokemonRoom = transform.parent.parent.GetComponent<Room>(); }
        FirstSpeed = speed;
        int OutPut;
        if (!(EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss))
        {
            OutPut = Mathf.Clamp( PlayerLevel + (player.playerData.IsPassiveGetList[29] ? 5 : 0) + (Random.Range(-2, 2)  ) , 1  , 100 );
            if (FloorNum.GlobalFloorNum != null)
            {
                OutPut = Mathf.Clamp(OutPut + FloorNum.GlobalFloorNum.EmptyLevelAlpha[FloorNum.GlobalFloorNum.FloorNumber], 1, 100);
                if (FloorNum.GlobalFloorNum.FloorNumber == 0)
                {
                    if (Mathf.Abs(transform.parent.position.x / 30) + Mathf.Abs(transform.parent.position.y / 24) < 3)
                    {
                        OutPut = Random.Range(1, 3);
                    }
                }
            }
            if (OutPut > MaxLevel) { OutPut = MaxLevel; }
        }
        else
        {
            OutPut = Mathf.Clamp(player.Level + (player.playerData.IsPassiveGetList[29] ? 5 : 0) + (Random.Range(-2, 2) ), (player.playerData.IsPassiveGetList[29] ? 10 : 5), 100);
        }
        //读取继承数据
        if (saveLevel != -1) { OutPut = saveLevel;  }
        return OutPut;
    }

    /// <summary>
    /// 确定敌人是否有道具
    /// </summary>
    void SetHaveDropItem()
    {
        if (!(EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss))
        {
            //携带探宝器更容易携带道具
            if (!player.playerData.IsPassiveGetList[61]) {
                isHaveDropItem = Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 100 > 0.93f;
            }
            else
            {
                isHaveDropItem = Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.85f;
                if (isHaveDropItem) { playerUIState.StatePlus(13); }
            }
        }
        else
        {
            isHaveDropItem = true;
        }
    }

    /// <summary>
    /// ---start中调用---，根据种族值初始化敌人血量
    /// </summary>
    /// <param name="level"></param>
    protected void EmptyHpForLevel(int level)
    {
        float BossBonus = 1.7f;
        float MiniBossBonus = 1.35f;
        if (FloorNum.GlobalFloorNum != null)
        {
            BossBonus = FloorNum.GlobalFloorNum.FloorBossHPBonus[FloorNum.GlobalFloorNum.FloorNumber];
        }
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f)) * ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? BossBonus : 1) * ((EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? MiniBossBonus : 1));
        maxHP = EmptyHp;
        SetHaveDropItem();
    }

    /// <summary>
    /// ---start中调用---，根据种族值初始化敌人其他能力值
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    public int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }



    /// <summary>
    /// 开始事件后的事件
    /// </summary>
    public virtual void StartOverEvent()
    {
        //GetShield((int)(maxHP / 3.0f));
        //添加敌人至房间敌人列表
        ParentPokemonRoom.AddEmptyList(this);

        //如果存在继承数据 继承
        if (saveHp != -1 && saveShield != -1)
        {
            if (saveHp < maxHP)
            {
                EmptyHp = saveHp;
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
            }
            if (saveShield > EmptyShield)
            {
                GetShield(saveShield - EmptyShield);
            }
        }
    }


    /// <summary>
    /// 存储预载数据
    /// </summary>
    /// <param name="e"></param>
    public void StoreSaveData( Empty e)
    {
        SaveLevel = e.Emptylevel;
        SaveHp = e.EmptyHp;
        SaveShield = e.EmptyShield;
    }


    //=============================初始化敌人数据================================










    //====================敌人血量改变======================

    /// <summary>
    /// 敌人是否被刀背打击中
    /// </summary>
    public bool IsBeFalseSwipe { get { return isBeFalseSwipe; } set { isBeFalseSwipe = value; } }
    bool isBeFalseSwipe;

    /// <summary>
    ///     //声明一个函数，改变敌人对象的血量
    /// </summary>
    /// <param name="Dmage">物攻伤害</param>
    /// <param name="SpDmage">特攻伤害</param>
    /// <param name="SkillType">伤害属性（数字参考Type.cs）</param>
    public void EmptyHpChange(float  Dmage , float SpDmage , int SkillType, bool Crit = false)
    {
        if (isShadow && Dmage + SpDmage >= 0)
        {
            animator.SetTrigger("ShadowOver");
        }
        else
        {
            //无敌
            if (isInvincible)
            {
                return;
            }

            PokemonType.TypeEnum enumVaue = (PokemonType.TypeEnum)SkillType;

            //Debug.Log(name);
            //Debug.Log(player);
            //Debug.Log(player.playerData);
            //Debug.Log(player.playerData.IsPassiveGetList[118]);

            Dmage = Dmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));
            SpDmage = SpDmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));

            float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);

            //发生血量改变前的血量
            int BeforeHP = EmptyHp;
            //发生血量改变前的护盾
            int BeforeShield = EmptyShield;

            //伤害
            if (Dmage + SpDmage >= 0)
            {
                int allDmg = 0;
                //正常伤害
                if (SkillType != 19)
                {
                    allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (PokemonType.TYPE[SkillType][(int)EmptyType01]) * PokemonType.TYPE[SkillType][(int)EmptyType02]), 1, ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? (maxHP / MaxDmagePer()) : 100000));
                    //标记为受伤状态
                    if (allDmg > 0 && !ishurt) { ishurt = true; }
                    //非超能场地
                    if (!isInPsychicTerrain) 
                    {
                        EmptyBeingHurt(allDmg);
                    }
                    //超能场地免疫低伤害
                    else
                    {
                        if(Mathf.Abs((int)allDmg) > (int)(maxHP / 16))
                        {
                            EmptyBeingHurt(allDmg);
                        }
                    }
                }
                //无属性伤害
                else
                {
                    allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? (maxHP / MaxDmagePer()) : 100000));
                    //标记为受伤状态
                    if (allDmg > 0 && !ishurt) { ishurt = true; }
                    //非超能场地
                    if (!isInPsychicTerrain) 
                    {
                        EmptyBeingHurt(allDmg);
                    }
                    //超能场地免疫低伤害
                    else
                    {
                        if (Mathf.Abs((int)allDmg) > (int)(maxHP / 16))
                        {
                            EmptyBeingHurt(allDmg);
                        }
                    }
                }
                //显示伤害数字
                if (InitializePlayerSetting.GlobalPlayerSetting.isShowDamage && GetComponent<SubEmptyBody>() == null && FloatingDmg)
                {
                    GameObject fd = Instantiate(FloatingDmg, transform.position + Vector3.right * Random.Range(-0.8f, 0.8f), Quaternion.identity) as GameObject;
                    if (Dmage > SpDmage)
                    {
                        fd.transform.GetComponent<damageShow>().SetText(allDmg, Crit, false, false);
                    }
                    else
                    {
                        fd.transform.GetComponent<damageShow>().SetText(allDmg, Crit, false, true);
                    }
                }
                EmptySleepRemove();
                HitEvent();
            }
            //回血
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), (IsBeFalseSwipe ? 1 : 0), maxHP);
                if (InitializePlayerSetting.GlobalPlayerSetting.isShowDamage && FloatingDmg)
                {
                    int allRecover = -(int)(Dmage + SpDmage);
                    GameObject fd = Instantiate(FloatingDmg, transform.position + Vector3.right * Random.Range(-0.8f, 0.8f), Quaternion.identity);
                    fd.transform.GetComponent<damageShow>().SetText(allRecover, Crit, true, false);
                }
            }

            //Debug.Log(Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000) + " + " + "Dmage:" + (int)(Dmage + SpDmage));
            Debug.Log(
                "Dmage=" + Dmage + "  " 
                + "SpDmage=" + SpDmage + "  "
                + "typeDef=" + typeDef + "  "
                + "Type.TYPE[SkillType][(int)EmptyType01]=" + PokemonType.TYPE[SkillType][(int)EmptyType01] + "  "
                + "Type.TYPE[SkillType][(int)EmptyType02]=" + PokemonType.TYPE[SkillType][(int)EmptyType02] + "  ");
            //伤害调用UI
            if ((int)Dmage + (int)SpDmage > 0)
            {
                //血量发生改变则调用动画 护盾改变不调用
                if (BeforeHP != EmptyHp) {
                    if (!isCanHitAnimation && animator != null) { animator.SetTrigger("Hit"); }
                }
                //Debug.Log((float)EmptyHp / (float)maxHP + "=" + (float)EmptyHp + "/" + (float)maxHP);
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
                uIHealth.ShieldPer = (float)EmptyShield / (float)maxHP;
                uIHealth.ChangeShieldDown();
            }
            //回血调用UI
            else
            {
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpUp();
            }
            //刀背打
            if (IsBeFalseSwipe)
            {
                IsBeFalseSwipe = false;
            }
        }
    }





    void EmptyBeingHurt(int allDmg)
    {
        //有护盾时扣除护盾
        if (EmptyShield > 0)
        {
            int BeforeShield = EmptyShield;
            EmptyShield = Mathf.Clamp(EmptyShield - allDmg, 0, BeforeShield);
            if (EmptyShield <= 0 && GetComponent<SubEmptyBody>() == null)
            {
                if (EmptyBossLevel == emptyBossLevel.Boss || EmptyBossLevel == emptyBossLevel.EndBoss || EmptyBossLevel == emptyBossLevel.MiniBoss) {  }
                else
                {
                    GameObject ShieldBreakEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(3);
                    GameObject shieldBreakEffect = Instantiate(ShieldBreakEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    shieldBreakEffect.SetActive(true);
                }
                //护盾标志破裂
                //uIHealth.BreakShieldMark();
                ShieldBreakEvent();
            }
        }
        //无护盾时扣除生命值
        else
        {
            EmptyHp = Mathf.Clamp(EmptyHp - allDmg, (IsBeFalseSwipe ? 1 : 0), maxHP);
        }
    }

    public void GetShield(int point)
    {
        //当前护盾为0 且护盾增加值大于0 获得ui护盾标志
        //if (EmptyShield <= 0 && point > 0 )
        //{
        //    uIHealth.GetShieldMark();
        //}

        point = Mathf.Clamp(point, 0, maxHP - EmptyShield);
        EmptyShield += point;
        uIHealth.ShieldPer = (float)EmptyShield / (float)maxHP;
        //Debug.Log(uIHealth.ShieldPer);
        uIHealth.ChangeShieldUp();
        if (subEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < subEmptyBodyList.Count; i++)
            {
                subEmptyBodyList[i].EmptyShield += point;
                subEmptyBodyList[i].NowShield = subEmptyBodyList[i].EmptyShield;
            }
        }
    } 

    /// <summary>
    /// 首次受伤时发生的事件
    /// </summary>
    public void FirstHurtEvent()
    {
        ishurt = true;
        if (subEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < subEmptyBodyList.Count; i++)
            {
                subEmptyBodyList[i].isHurt = true;
            }
        }
    }


    /// <summary>
    /// 破盾事件
    /// </summary>
    public virtual void ShieldBreakEvent()
    {
        //非boss破盾时致盲
        if (EmptyBossLevel != emptyBossLevel.Boss && EmptyBossLevel != emptyBossLevel.EndBoss)
        {
            Blind(1.5f , 10.0f);
        }
    }


    /// <summary>
    /// 受伤发生的事件
    /// </summary>
    public virtual void HitEvent()
    {

    }



    //====================敌人血量改变======================









    //========================敌人被击退相关=========================
    /// <summary>
    /// 声明一个函数，设置敌人被击退的距离
    /// </summary>
    /// <param name="KnockOutPoint"></param>
    public void EmptyKnockOut(float KnockOutPoint)
    {
        //有护盾时不击退
        if (EmptyShield <= 0) {
            isHit = true;
            KOPoint = KnockOutPoint;
        }
    }

    /// <summary>
    /// ---Update中调用---，当敌人被击退时移动敌人
    /// </summary>
    protected void EmptyBeKnock()
    {
        if (!isDie && isHit)
        {
            if (rigidbody2D != null) {
                KOTimer += Time.deltaTime;
                Vector2 position = rigidbody2D.position;
                Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                KODirection.Normalize();
                position.x = position.x - KOPoint * KODirection.x * Time.deltaTime;
                position.y = position.y - KOPoint * KODirection.y * Time.deltaTime;
                rigidbody2D.position = position;
            }
        }
        if (KOTimer >= 0.15f)
        {
            isHit = false;
            KOTimer = 0;
        }
    }

    /// <summary>
    /// ---Update中调用---，当敌人被击退时移动敌人
    /// </summary>
    protected void EmptyBeKnockByTransform()
    {
        if (!isDie && isHit)
        {
            KOTimer += Time.deltaTime;
            Vector2 position = transform.position;
            Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            KODirection.Normalize();
            position.x = position.x - KOPoint * KODirection.x * Time.deltaTime / (Mathf.Pow(rigidbody2D.mass, 0.5f));
            position.y = position.y - KOPoint * KODirection.y * Time.deltaTime / (Mathf.Pow(rigidbody2D.mass, 0.5f));
            transform.position = position;
        }
        if (KOTimer >= 0.15f)
        {
            isHit = false;
            KOTimer = 0;
        }
    }
    //========================敌人被击退相关=========================








    //========================敌人碰撞伤害========================

    /// <summary>
    /// ---OnColliderEnter2D中调用---，对于玩家造成触碰伤害
    /// </summary>
    /// <param name="player"></param>
    public void EmptyTouchHit(GameObject playerObj)
    {
        if (playerObj.layer != 23) {
            //如果触碰到的是玩家，使玩家扣除一点血量
            PlayerControler playerControler = playerObj.gameObject.GetComponent<PlayerControler>();
            PokemonHpChange(this.gameObject, playerObj.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
                playerControler.KnockOutPoint = Knock;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (playerControler.playerData.IsPassiveGetList[115])
                {
                    PokemonHpChange(playerControler.gameObject, this.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
                }
            }
        }
        
    }




    protected bool isInfatuationDmageDone;
    float InfatuationDmageCDTimer;
    /// <summary>
    /// 当地人被魅惑时，调用方法
    /// </summary>
    public void UpdateInfatuationDmageCDTimer()
    {
        if (isInfatuationDmageDone)
        {
            InfatuationDmageCDTimer += Time.deltaTime;
            if(InfatuationDmageCDTimer >= 0.8)
            {
                InfatuationDmageCDTimer = 0; isInfatuationDmageDone = false;
            }
        }
    }

    public void InfatuationEmptyTouchHit(GameObject TargetEmpty)
    {
        if (!isInfatuationDmageDone) {
            //如果触碰到的是玩家，使玩家扣除一点血量
            Empty e = TargetEmpty.gameObject.GetComponent<Empty>();
            if (e != null)
            {
                PokemonHpChange(this.gameObject, e.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
                e.EmptyKnockOut(Knock);
                isInfatuationDmageDone = true;
            }
        }
    }

    //========================敌人碰撞伤害========================










    //=============================死亡事件===========================

    /// <summary>
    /// ---Update中调用---，血量小于等于0时玩家获得经验和努力，敌人进入死亡动画
    /// </summary>
    public void EmptyDie()
    {
        //每帧检测一次，当目标血量小于0时销毁目标
        if (EmptyHp <= 0)
        {
            //isDeadrattle = (!isEmptyFrozenDone) && (!isFearDone) && (!isBlindDone);
            FrozenRemove();
            Destroy(rigidbody2D);
            RemoveChild();
            if (!isDie)
            {
                if (GetComponent<Collider2D>()) { GetComponent<Collider2D>().enabled = false; }
                player.ChangeEx((int)(Exp * ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss ) ? 1.8f : 1.3f)));
                player.ChangeHPW(HWP);

                //给AP
                if (FloorNum.GlobalFloorNum != null && ScoreCounter.Instance != null)
                {
                    ScoreCounter.Instance.EmptyBounsAP += APBounsPoint.EmptyBouns(this , FloorNum.GlobalFloorNum.FloorNumber);
                }

                if (player.playerData.IsPassiveGetList[134] && (EmptyType01 == PokemonType.TypeEnum.Dark || EmptyType02 == PokemonType.TypeEnum.Dark) ) { player.ChangeHPW(HWP); }
                Room r = transform.parent.parent.GetComponent<Room>();
                if (r == null) { r = ParentPokemonRoom; }
                r.isClear -= 1;

                if (DestoryEvent != null) { DestoryEvent(); }
                if (player.playerData.IsPassiveGetList[89] && player.playerData.AttackWeightCount < 6)
                {
                    player.playerData.AttackWeightCount++;
                    player.playerData.AtkBounsJustOneRoom++;
                    player.ReFreshAbllityPoint();
                }
                if (player.playerData.IsPassiveGetList[90] && player.playerData.SpAtkSpecsCount < 6)
                {
                    player.playerData.SpAtkSpecsCount++;
                    player.playerData.SpABounsJustOneRoom++;
                    player.ReFreshAbllityPoint();
                }
                isDie = true;
                //死亡事件
                DieEvent();
            }
            
            animator.SetTrigger("Die");
        }
    }
    /// <summary>
    /// 销毁，仅在死亡动画播放完毕后通过动画调用
    /// </summary>
    public void EmptyDestroy()
    {
        //从房间敌人列表中被移除
        ParentPokemonRoom.RemoveEmptyList(this);
        if (DropItem != null)
        {
            EmptyDrop();
        }
        Destroy(gameObject);
    }
    

    /// <summary>
    /// 延迟销毁敌人
    /// </summary>
    /// <param name="time"></param>
    public void EmptyDelayDestroy(float time)
    {
        //从房间敌人列表中被移除
        ParentPokemonRoom.RemoveEmptyList(this);
        if (DropItem != null)
        {
            EmptyDrop();
        }
        Destroy(gameObject  ,time);
    }
    
    public void EmptyDrop()
    {
        if (IsHaveDropItem) {
            
            if ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
            {
                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, parentRoom.transform.position.x - 12.0f, parentRoom.transform.position.x + 12.0f), Mathf.Clamp(transform.position.y, parentRoom.transform.position.y - 7.0f, parentRoom.transform.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                }
            }
            else
            {

                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, parentRoom.transform.position.x - 12.0f, parentRoom.transform.position.x + 12.0f), Mathf.Clamp(transform.position.y, parentRoom.transform.position.y - 7.0f, parentRoom.transform.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform);
                }

            }
            IsHaveDropItem = false;
            playerUIState.StateDestory(13);
            DropItem = null;
        }
    }

    /// <summary>
    /// 死亡时触发事件
    /// </summary>
    public virtual void DieEvent()
    {
        //从房间敌人列表中被移除
        ParentPokemonRoom.RemoveEmptyList(this);

        // 子敌人相关
        if (ChildrenList.Count != 0)
        {
            ParentDie();
        }
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ChildDie(this);
        }
        if (PartnerEmpty != null)
        {
            DieAsPartner();
        }
    }



    //=============================死亡事件===========================




















    //===============================着迷时搜索其他敌人=================================

    /// <summary>
    /// 对于通过射线检测范围内目标的敌人，在着迷时使用此方法寻找敌人
    /// </summary>
    /// <param name="Range">该敌人的视野范围</param>
    public Transform InfatuationForRangeRayCastEmpty(float Range)
    {
        /**
        //输出的敌人目标
        Empty OutPutEmpty = null;
        //仅当房间敌人数多余1（也就是房间内有除该敌人以外的敌人）时，搜索敌人
        if (transform.parent.childCount >= 1)
        {
            float D = 1000;
            //遍历当前房间内所有敌人
            foreach (Transform e in transform.parent)
            {
                //如果e不是自己,射线检测，如果成功输出e
                if(e.gameObject != this.gameObject)
                {
                    RaycastHit2D SearchTarget = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(e.transform.position.x - transform.position.x, e.transform.position.y - transform.position.y), Range , LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly"));
                    if (SearchTarget.collider != null && SearchTarget.collider.gameObject != gameObject && ( SearchTarget.transform.tag == "Empty" || SearchTarget.transform.tag == "EmptyFly"))
                    {
                        if((transform.position - e.transform.position).magnitude < D)
                        {
                            OutPutEmpty = e.GetComponent<Empty>();
                            D = (transform.position - e.transform.position).magnitude;
                        }
                    }

                }
            }
        }
        return OutPutEmpty;
        **/
        //输出的敌人目标
        Transform OutPutEmptyTransform = null;
        //List<Empty> EmptyCheckList = _mTool.GetAll<Empty>(ParentPokemonRoom.EmptyFile());
        //List<NormalEmptyCloneBody> EmptyCloneCheckList = _mTool.GetAll<NormalEmptyCloneBody>(ParentPokemonRoom.EmptyFile());
        List<Empty> EmptyCheckList = ParentPokemonRoom.GetEmptyList();
        List<NormalEmptyCloneBody> EmptyCloneCheckList = ParentPokemonRoom.GetEmptyCloneList();
        //仅当房间敌人数多余1（也就是房间内有除该敌人以外的敌人）时，搜索敌人
        if (EmptyCheckList.Count + EmptyCloneCheckList.Count >= 1)
        {
            float D = 1000;
            //遍历当前房间内所有敌人
            foreach (Empty e in EmptyCheckList)
            {
                //如果e不是自己,计算距离，如果小于当前距离输出e
                if (e.gameObject != this.gameObject)
                {
                    RaycastHit2D SearchTarget = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(e.transform.position.x - transform.position.x, e.transform.position.y - transform.position.y), Range, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly"));
                    if (SearchTarget.collider != null && SearchTarget.collider.gameObject != gameObject && (SearchTarget.transform.tag == "Empty" || SearchTarget.transform.tag == "EmptyFly"))
                    {
                        if ((transform.position - e.transform.position).magnitude < D)
                        {
                            OutPutEmptyTransform = e.transform;
                            D = (transform.position - e.transform.position).magnitude;
                        }
                    }
                }
            }
            //遍历当前房间内所有敌人 敌人幻影
            foreach (NormalEmptyCloneBody ec in EmptyCloneCheckList)
            {
                //如果e不是自己的分身,计算距离，如果小于当前距离输出e
                if (ec.ParentEmpty != this.gameObject)
                {
                    RaycastHit2D SearchTarget = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(ec.transform.position.x - transform.position.x, ec.transform.position.y - transform.position.y), Range, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly"));
                    if (SearchTarget.collider != null && SearchTarget.collider.gameObject != gameObject && (SearchTarget.transform.tag == "Empty" || SearchTarget.transform.tag == "EmptyFly"))
                    {
                        if ((transform.position - ec.transform.position).magnitude < D)
                        {
                            OutPutEmptyTransform = ec.transform;
                            D = (transform.position - ec.transform.position).magnitude;
                        }
                    }
                }
            }
        }

        //检查目标是敌人还是幻影
        if (isEmptyInfatuationDone)
        {
            if (OutPutEmptyTransform != null)
            {
                Empty OutPutEmpty = OutPutEmptyTransform.GetComponent<Empty>();
                if (OutPutEmpty != null) { InfatuationTargetEmpty = OutPutEmpty; }
                else { InfatuationTargetEmpty = null; }
            }
        }
        else { InfatuationTargetEmpty = null; }


        //返回输出目标
        return OutPutEmptyTransform;
    }



    protected Empty InfatuationTargetEmpty;


    /// <summary>
    /// 对于不通过射线检测的敌人（如耿鬼这种锁定玩家的飞行敌人），在着迷时使用此方法，计算离自己距离最近的敌人
    /// </summary>
    /// <returns></returns>
    public Transform InfatuationForDistanceEmpty()
    {
        /**
        //输出的敌人目标
        Empty OutPutEmpty = null;
        List<Empty> checkList = _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile());
        //_mTool.DebugLogList<Empty>(checkList);
        //仅当房间敌人数多余1（也就是房间内有除该敌人以外的敌人）时，搜索敌人
        if (checkList.Count >= 1)
        {
            float D = 1000;
            //遍历当前房间内所有敌人
            foreach (Empty e in checkList)
            {
                //如果e不是自己,计算距离，如果小于当前距离输出e
                if (e.gameObject != this.gameObject)
                {                
                    if ((transform.position - e.transform.position).magnitude < D)     
                    {
                        OutPutEmpty = e;
                        D = (transform.position - e.transform.position).magnitude; 
                    }
                }
            }
        }
        InfatuationTargetEmpty = OutPutEmpty;
        return OutPutEmpty;
        **/

        //输出的敌人目标
        Transform OutPutEmptyTransform = null;
        //List<Empty> EmptyCheckList = _mTool.GetAll<Empty>(ParentPokemonRoom.EmptyFile());
        //List<NormalEmptyCloneBody> EmptyCloneCheckList = _mTool.GetAll<NormalEmptyCloneBody>(ParentPokemonRoom.EmptyFile());
        List<Empty> EmptyCheckList = ParentPokemonRoom.GetEmptyList();
        List<NormalEmptyCloneBody> EmptyCloneCheckList = ParentPokemonRoom.GetEmptyCloneList();
        //Debug.Log(this.name);
        //_mTool.DebugLogList<Empty>(EmptyCheckList);
        //_mTool.DebugLogList<NormalEmptyCloneBody>(EmptyCloneCheckList);
        //仅当房间敌人数多余1（也就是房间内有除该敌人以外的敌人）时，搜索敌人
        if (EmptyCheckList.Count + EmptyCloneCheckList.Count >= 1)
        {
            float D = 1000;
            //遍历当前房间内所有敌人
            foreach (Empty e in EmptyCheckList)
            {
                //如果e不是自己,计算距离，如果小于当前距离输出e
                if (e.gameObject != this.gameObject)
                {
                    if ((transform.position - e.transform.position).magnitude < D)
                    {
                        OutPutEmptyTransform = e.transform;
                        D = (transform.position - e.transform.position).magnitude;
                    }
                }
            }
            //遍历当前房间内所有敌人 敌人幻影
            foreach (NormalEmptyCloneBody ec in EmptyCloneCheckList)
            {
                //如果e不是自己的分身,计算距离，如果小于当前距离输出e
                //Debug.Log(ec.ParentEmpty + "+" + name + "+" + (ec.ParentEmpty.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()));
                if (ec.ParentEmpty.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                {
                    if ((transform.position - ec.transform.position).magnitude < D)
                    {
                        OutPutEmptyTransform = ec.transform;
                        D = (transform.position - ec.transform.position).magnitude;
                    }
                }
            }
        }

        //Debug.Log(OutPutEmptyTransform +"+"+ name);
        //检查目标是敌人还是幻影
        if (isEmptyInfatuationDone)
        {
            if (OutPutEmptyTransform != null)
            {
                Empty OutPutEmpty = OutPutEmptyTransform.GetComponent<Empty>();
                if (OutPutEmpty != null) { InfatuationTargetEmpty = OutPutEmpty; }
                else { InfatuationTargetEmpty = null; }
            }
        }
        else { InfatuationTargetEmpty = null; }

        //返回输出目标
        return OutPutEmptyTransform;

    }

    //===============================着迷时搜索其他敌人=================================







    //=========================随时间的受伤时间=====================

    public void UpdateEmptyChangeHP()
    {
        if (rigidbody2D != null) {
            if (rigidbody2D.bodyType != RigidbodyType2D.Static) { rigidbody2D.velocity = Vector2.zero; }
        }
        if (isShake) { ShakeUpdate(); }
        if (isToxicDone) { EmptyToxic(); }
        if (isBurnDone) { EmptyBurn(); }
        if (isParalysisDone) { EmptyParalysisJudge(); } else { isCanNotMoveWhenParalysis = false; }
        if (Weather.GlobalWeather.isHail) { EmptyHail(); }
        if (Weather.GlobalWeather.isSandstorm) { EmptySandStorm(); }
        if (isEmptyCurseDone) { EmptyCurseDmage(); }
        if (isInMistyTerrain) { EmptyGrassyTerrainHeal(); }
    }


    //========================= 麻痹事件========================

    /// <summary>
    /// 敌人的麻痹计时器，每计时0.5s进行一次判定，如果通过判定则敌人可以移动 不通过不可以移动直到判定通过
    /// </summary>
    protected float EmptyParalysisJudgeTimer;
    public bool isCanNotMoveWhenParalysis;
    /// <summary>
    /// 敌人的麻痹计时器，每计时0.5s进行一次判定，
    /// </summary>
    void EmptyParalysisJudge()
    {
        EmptyParalysisJudgeTimer += Time.deltaTime;
        if (EmptyParalysisJudgeTimer >= 0.5)
        {
            if (Random.Range(0.0f, 1.0f) > 0.65f) { isCanNotMoveWhenParalysis = true; }
            else { isCanNotMoveWhenParalysis = false; }
            EmptyParalysisJudgeTimer = 0;
        }

    }
    //=========================麻痹事件========================


    //=========================中毒事件========================

    /// <summary>
    /// 敌人的中毒计时器，每计时两秒中一次毒
    /// </summary>
    protected float EmptyToxicTimer;
    /// <summary>
    /// 根据中毒时间敌人掉血
    /// </summary>
    void EmptyToxic()
    {
        EmptyToxicTimer += Time.deltaTime;
        if(EmptyToxicTimer >= 2)
        {
            EmptyToxicTimer += Time.deltaTime;
            PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * ToxicResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
            EmptyToxicTimer = 0;
        }

    }
    //=========================中毒事件========================

    //=========================中毒事件========================

    /// <summary>
    /// 敌人的烧伤计时器，每计时两秒烧伤一次
    /// </summary>
    protected float EmptyBurnTimer;
    /// <summary>
    /// 根据烧伤时间敌人掉血
    /// </summary>
    void EmptyBurn()
    {
        EmptyBurnTimer += Time.deltaTime;
        if (EmptyBurnTimer >= 2)
        {
            EmptyBurnTimer += Time.deltaTime;
            PokemonHpChange(null , this.gameObject , Mathf.Clamp((((float)maxHP) / 16) * BurnResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0 , 0 , PokemonType.TypeEnum.IgnoreType);
            //EmptyHpChange(Mathf.Clamp((((float)maxHP) / 16) * BurnResistance, 1, isBoos ? 8 : 10), 0, 19);
            EmptyBurnTimer = 0;
        }

    }
    //=========================中毒事件========================

    //=========================冰雹伤害事件========================

    /// <summary>
    /// 敌人的冰雹计时器，每计时两秒中一次毒
    /// </summary>
    protected float EmptyHailTimer;
    /// <summary>
    /// 根据冰雹时间敌人掉血
    /// </summary>
    void EmptyHail()
    {
        if (EmptyType01 != PokemonType.TypeEnum.Ice && EmptyType02 != PokemonType.TypeEnum.Ice && !isHailDef)
        {
            EmptyHailTimer += Time.deltaTime;
            if (EmptyHailTimer >= 2)
            {
                EmptyHailTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isHailPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptyHailTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptyHailTimer = 0;
                }
            }
        }
    }
    //=========================冰雹伤害事件========================

    //=========================沙暴伤害事件========================

    /// <summary>
    /// 敌人的沙暴计时器，每计时两秒中一次毒
    /// </summary>
    protected float EmptySandStormTimer;
    /// <summary>
    /// 根据沙暴时间敌人掉血
    /// </summary>
    void EmptySandStorm()
    {
        if (EmptyType01 != PokemonType.TypeEnum.Ground && EmptyType01 != PokemonType.TypeEnum.Rock && EmptyType01 != PokemonType.TypeEnum.Steel && EmptyType02 != PokemonType.TypeEnum.Ground && EmptyType02 != PokemonType.TypeEnum.Rock && EmptyType02 != PokemonType.TypeEnum.Steel && !isSandStormDef)
        {
            EmptySandStormTimer += Time.deltaTime;
            if (EmptySandStormTimer >= 2)
            {
                EmptySandStormTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isSandstormPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 8) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 16 : 20), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptySandStormTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptySandStormTimer = 0;
                }
            }
        }
    }
    //=========================沙暴伤害事件========================

    //=========================诅咒事件========================

    /// <summary>
    /// 敌人的诅咒计时器，每计时5s诅咒一次
    /// </summary>
    protected float EmptyCurseTimer;
    /// <summary>
    /// 根据诅咒时间敌人掉血
    /// </summary>
    void EmptyCurseDmage()
    {
        if (EmptyCurseTimer == 0)
        {
            PokemonHpChange(null, this.gameObject, Mathf.Clamp(( ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? ((float)EmptyHp) : ((float)maxHP)) / 4), 1, 10000), 0, 0, PokemonType.TypeEnum.IgnoreType);
        }
        EmptyCurseTimer += Time.deltaTime;
        if (EmptyCurseTimer >= 5)
        {
            EmptyCurseTimer = 0;
        }

    }
    //=========================诅咒事件========================


    //=========================青草场地回血事件========================
    /// <summary>
    /// 敌人的青草场地回血计时器，每计时5s青草场地回血一次
    /// </summary>
    protected float EmptyGrassyTerrainTimer;
    /// <summary>
    /// 根据青草场地时间敌人回血
    /// </summary>
    void EmptyGrassyTerrainHeal()
    {
        if (EmptyGrassyTerrainTimer == 0)
        {
            PokemonHpChange(null, this.gameObject, 0, 0, (int)Mathf.Clamp(( (float)maxHP / 16), 1, 10), PokemonType.TypeEnum.IgnoreType);
        }
        EmptyGrassyTerrainTimer += Time.deltaTime;
        if (EmptyGrassyTerrainTimer >= 5)
        {
            EmptyGrassyTerrainTimer = 0;
        }

    }
    //=========================青草场地回血事件========================





    // 在半径范围内寻找攻击目标
    public GameObject FindAtkTarget(float radius)
    {
        GameObject target = null;
        Transform nearlyEmptyObj = InfatuationForRangeRayCastEmpty(radius);
        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || nearlyEmptyObj == null)
        {
            if (isSubsititue && SubsititueTarget != null && Vector3.Distance(transform.position, SubsititueTarget.transform.position) <= radius)
            {
                target = SubsititueTarget;
            }
            else if(Vector3.Distance(transform.position, player.transform.position) <= radius)
            {
                target = player.gameObject;
            }
        }
        else if (nearlyEmptyObj)
        {
            target = nearlyEmptyObj.gameObject;
        }
        return target;
    }

    /// <summary>
    /// ---Update和FixedUpdate中调用---，声明一个函数，当玩家进化后因为之前的玩家对象被销毁，所以需要重新获取玩家
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }


    /// <summary>
    /// 当敌人死亡时，把敌人的所有粒子效果子对象的父子关系移除，防止粒子效果随着敌人一起突然消失
    /// </summary>
    void RemoveChild()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<ParticleSystem>() != null)
            {
                child.transform.parent = transform.parent.parent;
                ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                var main = _ps.main;
                _ps.Stop(true,ParticleSystemStopBehavior.StopEmitting);
                main.loop = false;
            }
        }
    }
    

    /// <summary>
    /// 多用于检测敌人传送后检测传送的点有没有障碍物
    /// </summary>
    /// <returns></returns>
    public bool isThisPointEmpty( Vector3 P )
    {
        RaycastHit2D SearchEmpty01 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left + Vector2.up, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty02 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left + Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty03 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right + Vector2.up, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty04 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right + Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty05 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left, 0.6f, LayerMask.GetMask("Enviroment" , "Water"));
        RaycastHit2D SearchEmpty06 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty07 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty08 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        return !SearchEmpty01 && !SearchEmpty02 && !SearchEmpty03 && !SearchEmpty04 && !SearchEmpty05 && !SearchEmpty06 && !SearchEmpty07 && !SearchEmpty08;
    }

    /// <summary>
    /// 多用于确定敌人是否处于当前房间的范围内
    /// </summary>
    /// <param name="P"></param>
    /// <returns></returns>
    public bool isThisPointInRoom(Vector3 P)
    {
        if( Mathf.Abs(P.x) >= parentRoom.EmptyFile().transform.position.x + parentRoom.RoomSize[2] &&
            Mathf.Abs(P.x) <= parentRoom.EmptyFile().transform.position.x + parentRoom.RoomSize[3] &&
            Mathf.Abs(P.y) >= parentRoom.EmptyFile().transform.position.y + parentRoom.RoomSize[1] &&
            Mathf.Abs(P.y) <= parentRoom.EmptyFile().transform.position.y + parentRoom.RoomSize[0])
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }







    //===================================多身体构造的敌人（三地鼠 ， 大岩蛇等）使用的函数===========================================


    public void UpdataMulitBodyEmptyState()
    {


        bool isOnixSubsititue = false;
        bool isOnixInGrassyTerrain = false;
        bool isOnixInPsychicTerrain = false;
        bool isOnixInElectricTerrain = false;
        bool isOnixInMistyTerrain = false;
        bool isOnixSuperInGrassyTerrain = false;
        bool isOnixSuperInPsychicTerrain = false;
        bool isOnixSuperInElectricTerrain = false;
        bool isOnixSuperInMistyTerrain = false;
        bool isOnixSpeedChange = false;


        foreach (SubEmptyBody b in SubEmptyBodyList)
        {
            if (b.isSubsititue) { isOnixSubsititue = true; SubsititueTarget = b.SubsititueTarget; }
            if (b.isInGrassyTerrain) { isOnixInGrassyTerrain = true; }
            if (b.isInPsychicTerrain) { isOnixInPsychicTerrain = true; }
            if (b.isInElectricTerrain) { isOnixInElectricTerrain = true; }
            if (b.isInMistyTerrain) { isOnixInMistyTerrain = true; }
            if (b.isInSuperGrassyTerrain) { isOnixSuperInGrassyTerrain = true; }
            if (b.isInSuperPsychicTerrain) { isOnixSuperInPsychicTerrain = true; }
            if (b.isInSuperElectricTerrain) { isOnixSuperInElectricTerrain = true; }
            if (b.isInSuperMistyTerrain) { isOnixSuperInMistyTerrain = true; }
            if (b.isSpeedChange) { isOnixSpeedChange = true; }
        }

        if (isOnixSubsititue) { isSubsititue = true; } else { isSubsititue = false; SubsititueTarget = null; }
        if (isOnixInGrassyTerrain) { isInGrassyTerrain = true; } else { isInGrassyTerrain = false; }
        if (isOnixInPsychicTerrain) { isInPsychicTerrain = true; } else { isInPsychicTerrain = false; }
        if (isOnixInElectricTerrain) { isInElectricTerrain = true; } else { isInElectricTerrain = false; }
        if (isOnixInMistyTerrain) { isInMistyTerrain = true; } else { isInMistyTerrain = false; }
        if (isOnixSuperInGrassyTerrain) { isInSuperGrassyTerrain = true; } else { isInSuperGrassyTerrain = false; }
        if (isOnixSuperInPsychicTerrain) { isInSuperPsychicTerrain = true; } else { isInSuperPsychicTerrain = false; }
        if (isOnixSuperInElectricTerrain) { isInSuperElectricTerrain = true; } else { isInSuperElectricTerrain = false; }
        if (isOnixSuperInMistyTerrain) { isInSuperMistyTerrain = true; } else { isInSuperMistyTerrain = false; }
        if (isOnixSpeedChange) { SpeedChange(); } else { SpeedRemove01(0); }

    }




    /// <summary>
    /// 重置所有体节为非无敌状态
    /// </summary>
    public void ResetSubBodyInvincible()
    {
        if (SubEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].isSubBodyEmptyInvincible = false;
                this.Invincible = false;
            }
        }
    }

    IEnumerator SetInvincible(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            // 处理事件
            isSubBodyEmptyInvincible = false;
            ResetSubBodyInvincible();
        }
    }

    //===================================多身体构造的敌人（三地鼠 ， 大岩蛇等）使用的函数===========================================













    //=================镜头摇晃======================

    void ShakeUpdate()
    {
        if (isShake)
        {
            ShakeTime -= Time.deltaTime;
            CSTimer += Time.deltaTime;
            if (CSTimer >= 0.05f) { CSTimer = 0; ShakePower = -ShakePower; }
            if (isHorizontalShake)
            {
                ParentPokemonRoom.transform.GetChild(0).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(1).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(2).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(6).position += Time.deltaTime * ShakePower * Vector3.right;
            }
            else
            {
                ParentPokemonRoom.transform.GetChild(0).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(1).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(2).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(6).position += Time.deltaTime * ShakePower * Vector3.up;
            }
            if (ShakeTime <= 0) { isShake = false; ShakeTime = 0; isHorizontalShake = false; ShakePower = 0; CSTimer = 0; 
                ParentPokemonRoom.transform.GetChild(0).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(1).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(2).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(6).localPosition = Vector3.zero;
            }
        }
        else if(ParentPokemonRoom.transform.GetChild(0).localPosition != Vector3.zero)
        {
            ParentPokemonRoom.transform.GetChild(0).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(1).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(2).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(6).localPosition = Vector3.zero;
        }
    }


    bool isShake;
    float ShakeTime;
    float CSTimer;
    bool isHorizontalShake;
    float ShakePower;
    Vector3 PRoomNowPosition;

    public void CameraShake(float time, float Power, bool isHorizontal)
    {
        if (!isShake) { isShake = true; ShakeTime = time; isHorizontalShake = isHorizontal; ShakePower = Power;  }
    }

    //=================镜头摇晃======================






















    //=================有关使用回声（轮流唱歌攻击）的敌人======================
    /// <summary>
    /// 表示敌人是否是一只会使用【回声（会轮流释放声音攻击）】的敌人
    /// </summary>
    public bool isUseEchoedVoice;
    /// <summary>
    /// 使用回声
    /// </summary>
    public virtual void UseEchoedVoice( int echoedVoiceLevel )
    {

    }

    /// <summary>
    /// 判定当前敌人是否可以回声
    /// </summary>
    public virtual bool isEchoedVoiceisReady()
    {
        return false;
    }
    //=================有关使用回声（轮流唱歌攻击）的敌人======================





















    //=================有关残影生成======================

    /// <summary>
    /// 敌人的残影实体
    /// </summary>
    public NormalEmptyShadow emptyShadow;
    /// <summary>
    /// 生成残影用的协程
    /// </summary>
    public Coroutine ShadowCoroutine;
    /// <summary>
    /// 是否生成残影
    /// </summary>
    public bool isShadowMove = false;


    /// <summary>
    /// 开始残影携程
    /// </summary>
    /// <param name="Interval">生成残影的间隔</param>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    public void StartShadowCoroutine(float Interval, float disappearingSpeed, Color color)
    {
        //Debug.Log("StartSHadow");
        isShadowMove = true; // 开始冲刺
        ShadowCoroutine = StartCoroutine(StartShadow(Interval , disappearingSpeed , color)); // 启动协程
    }


    /// <summary>
    /// 停止残影协程
    /// </summary>
    public void StopShadowCoroutine()
    {
        //Debug.Log("StopSHadow");
        isShadowMove = false; // 设置停止冲刺
        if (ShadowCoroutine != null)
        {
            StopCoroutine(ShadowCoroutine); // 通过引用停止协程
            ShadowCoroutine = null; // 清空引用以避免重复问题
        }
    }


    /// <summary>
    /// 每隔一段时间生成一个残影
    /// </summary>
    /// <param name="Interval">生成残影的间隔</param>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    /// <returns></returns>
    IEnumerator StartShadow( float Interval , float disappearingSpeed, Color color)
    {
        while (isShadowMove)
        {
            InstantiateShadow(disappearingSpeed , color);
            yield return new WaitForSeconds(Interval); // 等待间隔时间
        }
    }

    /// <summary>
    /// 生成一个残影
    /// </summary>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    void InstantiateShadow(float disappearingSpeed , Color color)
    {
        if (!isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
        {
            Instantiate(emptyShadow, transform.position, Quaternion.identity).SetNormalEmptyShadow(disappearingSpeed, GetSkinRenderers(), color);
        }  
    }

    //=================有关残影生成======================



    //生成技能的暴击特效
    public void GetCTEffect(Transform target)
    {
        //获取暴击动画特效
        GameObject CTEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(0);
        //实例化
        Instantiate(CTEffect, target.transform.position + Vector3.right * Random.Range(-0.5f, 0.5f) + Vector3.up * Random.Range(0.0f, 0.8f), Quaternion.identity, target.transform).SetActive(true);
    }




    /// <summary>
    /// 收集所有子级和父级的 Collider2D，避免重复
    /// </summary>
    public void CollectAllColliders(Empty current, HashSet<Empty> visited, List<Collider2D> output)
    {
        if (current == null || visited.Contains(current)) return;

        visited.Add(current);

        Collider2D col = current.GetComponent<Collider2D>();
        if (col != null && !output.Contains(col))
        {
            output.Add(col);
        }

        // 遍历子级
        if (current.ChildrenList != null)
        {
            foreach (Empty child in current.ChildrenList)
            {
                CollectAllColliders(child, visited, output);
            }
        }

        // 遍历父级
        if (current.ParentEmptyByChild != null)
        {
            CollectAllColliders(current.ParentEmptyByChild, visited, output);
        }
    }






























    //■■■■■■■■■■■■■■■■■■■■有关子敌人对象■■■■■■■■■■■■■■■■■■■■■■

    //=================================作为父对象====================================

    /// <summary>
    /// 子敌人对象的家Transform
    /// </summary>
    public Transform ChildHome;

    /// <summary>
    /// 子敌人对象列表
    /// </summary>
    public List<Empty> ChildrenList;

    /// <summary>
    /// 子敌人对象死去
    /// </summary>
    public virtual void ChildDie(Empty child)
    {
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            if (ChildrenList[i] != null && child.gameObject == ChildrenList[i].gameObject) { 
                ChildrenList[i].ChildLeaveHome();
                //ChildrenList[i] = null;  
            }
        }
    }

    /// <summary>
    /// 忽略所有自己和子敌人对象和孙对象之间的碰撞
    /// </summary>
    public virtual void IgnoreCollisionParentChild()
    {
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        Debug.Log(string.Join(",", CList));
        //CList.Add(this.GetComponent<Collider2D>());
        foreach (Empty child in ChildrenList)
        {
            if (child.gameObject != null) { CList.Add(child.transform.GetComponent<Collider2D>()); }
        }
        for (int i = 0; i < (CList.Count - 1); i++)
        {
            for (int j = i + 1; j < CList.Count; j++)
            {
                Physics2D.IgnoreCollision(CList[i], CList[j], true);
            }
        }
    }



    /// <summary>
    /// 忽略某个子敌人对象和其他子敌人对象以及自己的碰撞
    /// </summary>
    public virtual void IgnoreOneChildCollision(Empty child)
    {
        //Debug.Log("NAME" + child.name);
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), child.GetComponent<Collider2D>(), true);
        List<string> t = new List<string> { };
        foreach (Collider2D c in CList)
        {
            t.Add(c.name);
            if (c != null && c.gameObject != child.gameObject)
            {
                //Debug.Log(c.name +"+"+ child.name);
                Physics2D.IgnoreCollision(c, child.GetComponent<Collider2D>(), true);
            }
        }
        if (child.ChildrenList.Count != 0) {
            for (int i = 0; i < child.ChildrenList.Count; i++)
            {
                child.IgnoreOneChildCollision(child.ChildrenList[i]);
            }
        }
        //Debug.Log(string.Join("," , t));
    }

    /// <summary>
    /// 恢复某个子敌人对象和其他子敌人对象以及自己的碰撞
    /// </summary>
    public virtual void ResetOneChildCollision(Empty child)
    {
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        Debug.Log(string.Join(",", CList));
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), child.GetComponent<Collider2D>(), false);
        foreach (Collider2D c in CList)
        {
            if (c != null && c.gameObject != child.gameObject)
            {
                Physics2D.IgnoreCollision(c, child.GetComponent<Collider2D>(), false);
            }
        }
        if (child.ChildrenList.Count != 0)
        {
            for (int i = 0; i < child.ChildrenList.Count; i++)
            {
                child.ResetOneChildCollision(child.ChildrenList[i]);
            }
        }
    }

    /// <summary>
    /// 返回活着的子敌人对象队列 并且更新ChildrenList
    /// </summary>
    /// <returns></returns>
    public virtual List<Empty> GetAliveChildren()
    {
        List<Empty> output = new List<Empty> { };
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            if (ChildrenList[i] != null && !ChildrenList[i].isDie) { output.Add(ChildrenList[i]); }
        }
        ChildrenList.Clear();
        ChildrenList = output;
        return output;
    }

    /// <summary>
    /// 死亡时清空子敌人对象的父对象 并释放子敌人
    /// </summary>
    public virtual void ParentDie()
    {
        GetAliveChildren();
        List<Empty> l = new List<Empty> { };
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            l.Add(ChildrenList[i]);
        }

        for (int i = 0; i < l.Count; i++)
        {
            if (l[i] != null && !l[i].isDie)
            {
                if (l[i].transform.parent == ChildHome)
                {
                    //Debug.Log(ParentEmptyByChild);
                    l[i].ChildLeaveHome();
                    l[i].transform.parent = l[i].ParentPokemonRoom.EmptyFile();
                }
                if (l[i].ParentEmptyByChild != null) { l[i].ParentEmptyByChild = null; }
            }
        }
    }





    //=================================作为子对象====================================

    /// <summary>
    /// 自己作为子对象时的父对象
    /// </summary>
    public Empty ParentEmptyByChild;

    /// <summary>
    /// 子对象离开家 脱离母体
    /// </summary>
    public virtual void ChildLeaveHome()
    {
        Debug.Log(ParentEmptyByChild);
        if (ParentEmptyByChild != null && ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Remove(this);
        }
        //设定父对象和家
        if (ParentEmptyByChild != null)
        {
            transform.parent = ParentPokemonRoom.EmptyFile();
            ParentEmptyByChild = null;
        }
    }

    /// <summary>
    /// 子对象回到家家 返回母体
    /// </summary>
    public virtual void ChildBackHome(Empty parent)
    {
        //设定父对象和家
        ParentEmptyByChild = parent;
        transform.parent = ParentEmptyByChild.ChildHome;
        if (ParentEmptyByChild != null && !ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Add(this);
        }
    }

    /// <summary>
    /// 根据距离寻找离自己最近的父对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual T SearchParentByDistence<T>() where T : Empty
    {
        List<T> OutputList = new List<T> { };
        List<T> TList = _mTool.GetAllFromTransform<T>(ParentPokemonRoom.EmptyFile());
        //如果父对象不为空
        if (ParentEmptyByChild == null)
        {
            for (int i = 0; i < TList.Count; i++)
            {
                T t = TList[i];
                //Debug.Log(t.gameObject.name);
                if (t.transform != this.transform)
                {
                    T ct = t.GetComponent<T>();
                    if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie)
                    {
                        OutputList.Add(ct);
                    }
                }
            }
        }


        if (OutputList.Count == 0)
        {
            return default;
        }
        else
        {
            float MinDistence = 100.0f;
            T output = null;
            for (int i = 0; i < OutputList.Count; i++)
            {
                if (Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position)) < MinDistence)
                {
                    MinDistence = Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position));
                    output = OutputList[i];
                }
            }
            return output;
        }
    }



    //■■■■■■■■■■■■■■■■■■■■有关子对象■■■■■■■■■■■■■■■■■■■■■■





















    //■■■■■■■■■■■■■■■■■■■■有关连携伙伴■■■■■■■■■■■■■■■■■■■■■■


    //伙伴
    public Empty PartnerEmpty
    {
        get { return partnerEmpty; }
        set { partnerEmpty = value; }
    }
    public Empty partnerEmpty ;




    //自己在伙伴关系中的地位
    public enum PositionInPartnershipEnum
    {
        BigBrother, //大哥
        LittleBoy,  //小弟
        NoPartner,  //没有伙伴，或不处于伙伴状态
    }
    public PositionInPartnershipEnum PositionInPartnership
    {
        get { return positionInPartnership; }
        set { positionInPartnership = value; }
    }
    public PositionInPartnershipEnum positionInPartnership = PositionInPartnershipEnum.NoPartner;



    /// <summary>
    /// 在当前房间内根据距离搜索伙伴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public T SearchPartnerInRoomByDistence<T>() where T : Empty
    {
        //Debug.Log(this.name);
        List<T> OutputList = new List<T> { };
        foreach (Transform t in transform.parent)
        {
            if (t != this.transform) {
                T ct = t.GetComponent<T>();
                if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie && !ct.isSleepDone && !ct.isFearDone && !ct.isEmptyFrozenDone)
                {
                    //Debug.Log(ct.name + "+" + ct.isActiveAndEnabled + "+" + (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie));
                    OutputList.Add(ct);
                }
            }
        }

        if (OutputList.Count == 0)
        {
            return default;
        }
        else
        {
            float MinDistence = 100.0f;
            T output = null;
            for (int i = 0; i < OutputList.Count; i++)
            {
                if (Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position)) < MinDistence)
                {
                    MinDistence = Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position));
                    output = OutputList[i];
                }
            }
            partnerEmpty = output;
            JudgePositionInPartnership();
            return output;
        }
    }



    /// <summary>
    /// 通过比较确立伙伴关系中的地位
    /// </summary>
    public void JudgePositionInPartnership()
    {
        //没有伙伴则进入无伙伴状态
        if (partnerEmpty == null) { 
            this.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            return;
        }
        else
        {
            if (this.GetInstanceID() > partnerEmpty.GetInstanceID())
            {
                this.positionInPartnership = PositionInPartnershipEnum.BigBrother;
                partnerEmpty.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
            }
            else
            {
                this.positionInPartnership = PositionInPartnershipEnum.BigBrother;
                partnerEmpty.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
            }
        }
    }




    //自己作为伙伴死亡时
    public void DieAsPartner()
    {
        RemovePartnership();
    }


    /// <summary>
    /// 解除伙伴关系
    /// </summary>
    public void RemovePartnership()
    {
        if (partnerEmpty != null) {
            partnerEmpty.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            partnerEmpty.partnerEmpty = null;
        }

    }




    /**List
         //伙伴
    public List<Empty> PartnerEmpty
    {
        get { return partnerEmpty; }
        set { partnerEmpty = value; }
    }
    List<Empty> partnerEmpty = new List<Empty> { };




    //自己在伙伴关系中的地位
    public enum PositionInPartnershipEnum
    {
        BigBrother, //大哥
        LittleBoy,  //小弟
        NoPartner,  //没有伙伴，或不处于伙伴状态
    }
    public PositionInPartnershipEnum PositionInPartnership
    {
        get { return positionInPartnership; }
        set { positionInPartnership = value; }
    }
    PositionInPartnershipEnum positionInPartnership = PositionInPartnershipEnum.LittleBoy;


    /// <summary>
    /// 在当前房间内根据搜索伙伴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public PositionInPartnershipEnum SearchPartnerInRoom<T>() where T : Empty
    {
        foreach (Transform t in transform.parent)
        {
            if (t != this.transform) {
                T ct = t.GetComponent<T>();
                Debug.Log(ct.name + "+" + ct.isActiveAndEnabled + "+" + (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie));
                if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie)
                {
                    partnerEmpty.Add(ct);
                }
            }
        }
        JudgePositionInPartnership();
        Debug.Log(this.positionInPartnership);
        return this.positionInPartnership;
    }


    /// <summary>
    /// 通过比较确立伙伴关系中的地位
    /// </summary>
    public void JudgePositionInPartnership()
    {
        //初始化地位
        ResetPositionInPartnership();
        //判定地位
        Empty big = this;
        int MaxId = this.GetInstanceID();
        //没有伙伴则进入无伙伴状态
        if (partnerEmpty.Count == 0) { 
            this.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            return;
        }
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            if (MaxId < partnerEmpty[i].GetInstanceID())
            {
                big = partnerEmpty[i];
                MaxId = partnerEmpty[i].GetInstanceID();
            }
        }
        big.positionInPartnership = PositionInPartnershipEnum.BigBrother;
    }



    /// <summary>
    /// 初始化地位
    /// </summary>
    public void ResetPositionInPartnership()
    {
        this.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            partnerEmpty[i].positionInPartnership = PositionInPartnershipEnum.LittleBoy;
        }
    }


    //自己作为伙伴死亡时
    public void DieAsPartner()
    {
        RemovePartnership();
    }


    /// <summary>
    /// 解除伙伴关系
    /// </summary>
    public void RemovePartnership()
    {
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            //从所有敌人伙伴列表中的伙伴列表中移除自己
            partnerEmpty[i].partnerEmpty.Remove(this);
            //所有敌人重置地位
            partnerEmpty[i].ResetPositionInPartnership();
        }
        partnerEmpty.Clear();
    }
    **/


    //■■■■■■■■■■■■■■■■■■■■有关连携伙伴■■■■■■■■■■■■■■■■■■■■■■

}
