using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class PlayerControler : Pokemon
{
    // Start is called before the first frame update

    
    /// <summary>
    /// 角色的序列号
    /// </summary>
    public int PlayerIndex;
    


    //=======================================角色的数据===================================
    /// <summary>
    /// 角色中文名
    /// </summary>
    public string PlayerNameChinese;
    /// <summary>
    /// 角色头像图标
    /// </summary>
    public Sprite PlayerHead;
    /// <summary>
    /// 角色的糖果
    /// </summary>
    public Sprite PlayerCandy;
    /// <summary>
    /// 角色的糖果高清
    /// </summary>
    public Sprite PlayerCandyHD;
    /// <summary>
    /// 角色体型 0小体型 1中体型 2大体形
    /// </summary>
    public int PlayerBodySize;
    /// <summary>
    /// 根据角色体型大小，技能释放的位置，SkillOffsetforBodySize[0]为中心点y轴偏移量，SkillOffsetforBodySize[1]为x轴偏移量，SkillOffsetforBodySize[2]y轴偏移量，
    /// </summary>
    public float[] SkillOffsetforBodySize;
    /// <summary>
    /// 目前角色的进化型
    /// </summary>
    public PlayerControler EvolutionPlayer;
    /// <summary>
    /// 当前角色是否可以进化为下一阶段
    /// </summary>
    public bool isEvolution;
    /// <summary>
    /// 一个布尔变量表示该宝可梦是否来自于进化
    /// </summary>
    public bool isNeedInherit;
    /// <summary>
    /// 进化的粒子特效
    /// </summary>
    public GameObject EvolutionAnimation;
    protected GameObject EvoAnimaObj;
    /// <summary>
    /// 角色是否可以进化的判定
    /// </summary>
    /// <returns></returns>
    public delegate bool JudgeEvolution();
    public JudgeEvolution JudgeEvolutionForEachLevel;
    protected bool NotJudgeEvolution() { return false; }
    /// <summary>
    /// 声明一个2D刚体组件，以获得角色的刚体组件
    /// </summary>
    new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 声明两个变量，获取方向键按键信息
    /// </summary>
    public float PlayerMoveHorizontal { get { return horizontal; } }
    private float horizontal;
    public float PlayerMoveVertical { get { return vertical; } }
    private float vertical;
    /// <summary>
    /// 声明一个2D向量变量，以存储刚体的二维坐标
    /// </summary>
    Vector2 position;

    public delegate void ComeInANewRoom(PlayerControler player);
    public ComeInANewRoom ComeInANewRoomEvent;


    public delegate void ClearThisRoom(PlayerControler player);
    public ClearThisRoom ClearThisRoomEvent;

    public delegate void UpdataPassiveItem(PlayerControler player);
    public UpdataPassiveItem UpdataPassiveItemEvent;

    //声明一个二维向量表示朝向最初朝向右边,另一个表示位移量,
    public Vector2 look = new Vector2(0, -1);
    Vector2 move;
    Vector2 Direction;

    //声明一个整型变量最大生命值，一个整形变量现在生命值,以及一个整型变量代表现在生命值以用于其他函数
    public int maxHp;
    public int Hp
    {
        get { return nowHp; }
        set { nowHp = value; }
    }
    int nowHp;

    //声明一个整形变量现在金钱,以及一个整型变量代表现在金钱以用于其他函数
    public int Money
    {
        get { return nowMoney; }
        set { nowMoney = value; }
    }
    int nowMoney;

    //声明一个整形变量现在石头,以及一个整型变量代表现在石头以用于其他函数
    public int Stone
    {
        get { return nowStone; }
        set { nowStone = value; }
    }
    int nowStone;

    public int HeartScale
    {
        get { return nowHeartScale; }
        set { nowHeartScale = value; }
    }
    int nowHeartScale;

    public int PPUp
    {
        get { return nowPPUp; }
        set { nowPPUp = value; }
    }
    int nowPPUp;

    public int SeedofMastery
    {
        get { return nowSeedofMastery; }
        set { nowSeedofMastery = value; }
    }
    int nowSeedofMastery;

    public GameObject FloatingDamage;//造成伤害显示

    public HardworkShow HardworkFloatingShow;

    List<HardworkShow> LastHardworkShow = new List<HardworkShow> { };


    //声明一个整形表示当前等级最大经验值，一个整形变量表示等级，以及一个经验值表，一个整形变量现在经验值,以及一个整型变量代表现在经验值以用于其他函数
    protected int[] Exp;
    public int maxEx;
    public int Level;
    public int LevelForSkill=1;
    public int Ex
    {
        get { return nowEx; }
        set { nowEx = value; }
    }
    int nowEx;
    public int iCount;


    //声明一个浮点型变量代表无敌时间，一个浮点型变量作为无敌时间计时器，一个布尔型变量判断是否无敌
    public float TimeInvincible;
    float InvincileTimer = 0.0f;
    public bool isInvincible = false;




    //声明一个浮点型变量，代表被击退值。
    public float KnockOutPoint
    {
        get { return konckout; }
        set { konckout = value; }
    }
    float konckout = 6.5f;
    public Vector2 KnockOutDirection
    {
        get { return koDirection; }
        set { koDirection = value; }
    }
    Vector2 koDirection = new Vector2(0f,0f);

    //布尔型变量表示玩家是否触发Z互动 触发后其他互动不可再被触发
    public bool isInZ
    {
        get { return isinz; }
        set { isinz = value; }
    }
    bool isinz = false;


    //声明六个整形数据，表示角色的六项种族值,以及六项当前能力值
    public int HpPlayerPoint;
    public int AtkPlayerPoint;
    public int SpAPlayerPoint;
    public int DefPlayerPoint;
    public int SpdPlayerPoint;
    public int SpeedPlayerPoint;
    public int MoveSpePlayerPoint;
    public int LuckPlayerPoint;

    public int AtkAbilityPoint { get { return AtkAbility; } set { AtkAbility = value; } }
    int AtkAbility;
    public int SpAAbilityPoint { get { return SpAAbility; } set { SpAAbility = value; } }
    int SpAAbility;
    public int DefAbilityPoint { get { return DefAbility; } set { DefAbility = value; } }
    int DefAbility;
    public int SpdAbilityPoint { get { return SpDAbility; } set { SpDAbility = value; } }
    int SpDAbility;
    public int SpeedAbilityPoint { get { return SpeedAbility; } set { SpeedAbility = value; } }
    int SpeedAbility;


    //声明玩家的两个属性
    public int PlayerType01;
    public int PlayerType02;
    public int PlayerTeraType;
    public int PlayerTeraTypeJOR;

    //玩家的四个天赋技能
    public Skill InitialSkill01;
    public Skill InitialSkill02;
    public Skill InitialSkill03;
    public Skill InitialSkill04;


    public Skill[] InitialSkillCandidateList;

    //角色的特性列表
    public enum PlayerAbilityList
    {
        无特性 = 0,
        迟钝 = 1,
        雪隐 = 2,
        厚脂肪 = 3,
        叶子防守 = 4,
        甜幕 = 5,
        女王的威严 = 6,
        逃跑 = 7,
        适应力 = 8,
        危险预知 = 9,
        迷人之躯 = 10,
        妖精皮肤 = 11,
        激流 = 12,
        好胜 = 13,
        恒净之躯 = 14,
        轻金属 = 15,
        同步 = 16,
        精神力 = 17,
        魔法镜 = 18,

    }
    //当前角色可以获得特性
    public PlayerAbilityList playerAbility01;
    public PlayerAbilityList playerAbility02;
    public PlayerAbilityList playerAbilityDream;

    //对于这个角色目前的特性
    public PlayerAbilityList PlayerAbility { get { return playerAbility; } set { playerAbility = value; } }    PlayerAbilityList playerAbility;




    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill01;
    public float _Skill01Timer { get { return Skill01Timer; } set { Skill01Timer = value; } }
    float Skill01Timer = 0;
    public bool isSkill01CD = false;
    bool isSkill01lunch = false;
    bool isSkill = false;
    public SkillBar01 skillBar01;
    public bool IsSkill01ButtonDown { get { return isSkill01ButtonDown; } set { isSkill01ButtonDown = value; } }
    bool isSkill01ButtonDown;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill02;
    public float _Skill02Timer { get { return Skill02Timer; } set { Skill02Timer = value; } }
    float Skill02Timer = 0;
    public bool isSkill02CD = false;
    bool isSkill02lunch = false;
    public SkillBar01 skillBar02;
    public bool IsSkill02ButtonDown { get { return isSkill02ButtonDown; } set { isSkill02ButtonDown = value; } }
    bool isSkill02ButtonDown;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill03;
    public float _Skill03Timer { get { return Skill03Timer; } set { Skill03Timer = value; } }
    float Skill03Timer = 0;
    public bool isSkill03CD = false;
    bool isSkill03lunch = false;
    public SkillBar01 skillBar03;
    public bool IsSkill03ButtonDown { get { return isSkill03ButtonDown; } set { isSkill03ButtonDown = value; } }
    bool isSkill03ButtonDown;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill04;
    public float _Skill04Timer { get { return Skill04Timer; } set { Skill04Timer = value; } }
    float Skill04Timer = 0;
    public bool isSkill04CD = false;
    bool isSkill04lunch = false;
    public SkillBar01 skillBar04;
    public bool IsSkill04ButtonDown { get { return isSkill04ButtonDown; } set { isSkill04ButtonDown = value; } }
    bool isSkill04ButtonDown;

    public GameObject spaceItem;
    public GameObject SpaceItemList;
    public Image SpaceItemImage;
    public bool IsSpaceItemButtonDown { get { return isSpaceItemButtonDown; } set { isSpaceItemButtonDown = value; } }
    bool isSpaceItemButtonDown;

    //判断技能是否被封印
    public bool Is01imprison;
    public float imprisonTime01;
    public bool Is02imprison;
    public float imprisonTime02;
    public bool Is03imprison;
    public float imprisonTime03;
    public bool Is04imprison;
    public float imprisonTime04;


    /// <summary>
    /// 获得玩家的UI状态栏(状态栏下方)
    /// </summary>
    public PlayerUIState playerUIStateOther;

    //声明一个数组型变量，用来储存角色学习新招式的等级,以及一个整形变量检测当前等级是否习得技能
    public int GetSkillLevel;
    int StartLevel;
    int levelChecker = 0;
    public UIPanelGwtNewSkill uIPanelGwtNewSkill;

    
    public Vector3Int NowRoom = new Vector3Int (0, 0, 0);
    public bool InANewRoom;
    public float NewRoomTimer;

    public SpaceItemUseUI spaceitemUseUI;
    public GameObject PassiveItemGetUI;
    public bool isTP;
    public bool isTPMove;

    public int LuckPoint;

    public bool isSpaceItemCanBeUse;

    public PlayerData playerData;
    public PlayerSkillList playerSkillList;
    public SubSkillList playerSubSkillList;
    public GameObject FollowBaby;
    public GameObject NotFollowBaby;
    public PlayerButterflyManger ButterflyManger;

    PlayerControler ThisPlayer;

    public int NatureIndex;

    public bool isCanNotMove;
    public bool isCanNotTurnDirection;
    public bool isInvincibleAlways;

    bool isDie;


    //处于高速旋转状态
    public bool isRapidSpin
    {
        get { return isRapidspin; }
        set { isRapidspin = value; }
    }
    bool isRapidspin = false;

    //处于接棒状态
    public bool isBatonPass
    {
        get { return isbatonPass; }
        set { isbatonPass = value; }
    }
    bool isbatonPass = false;

    //处于电磁漂浮精通状态
    public bool isMagnetRisePlus
    {
        get { return ismagnetRisePlus; }
        set { ismagnetRisePlus = value; }
    }
    bool ismagnetRisePlus = false;



    //处于草丛中 当isInGress==0时代表不在草中 每和一片草碰撞+1
    public List<GameObject> InGressCount
    {
        get { return inGressGameObjCount; }
        set { inGressGameObjCount = value; }
    }
    List<GameObject> inGressGameObjCount = new List<GameObject> { };


    public Skill EvolutionSkill;
    protected bool isCanEvolution;

    public bool CanNotUseSpaceItem
    {
        get { return isCanNotUseSpaceItem; }
        set { isCanNotUseSpaceItem = value; }
    }
    bool isCanNotUseSpaceItem = false;






    /// <summary>
    /// 角色图片的相对位置 ，在跳跃之后会恢复为此值
    /// </summary>
    public Vector3 PlayerLocalPosition
    {
        get { return playerLocalPosition; }
        set { playerLocalPosition = value; }
    }
    Vector3 playerLocalPosition = Vector3.zero;


    /// <summary>
    /// 角色图片的相对缩放 ，在跳跃之后会恢复为此值
    /// </summary>
    public Vector3 PlayerLocalScal
    {
        get { return playerLocalScal; }
        set { playerLocalScal = value; }
    }
    Vector3 playerLocalScal = new Vector3(1.0f , 1.0f , 1.0f);




    //=================================初始化=====================================

    /// <summary>
    /// 声明一个根据等级计算当前最大生命值的函数
    /// </summary>
    /// <param name="level"></param>
    protected void MaxHpForLevel(int level)
    {
        maxHp = level + 10 + (int)(((float)level * HpPlayerPoint * 2) / 100.0f);
    }
    /// <summary>
    /// 初始化能力
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    protected int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }


    //初始化玩家的必要函数
    /// <summary>
    /// 初始化
    /// </summary>
    protected void Instance()
    {

        for (int i = -10; i < 10;i++)
        {
            Debug.Log(_mTool.AbllityChangeFunction(i,PlayerAbility == PlayerAbilityList.恒净之躯));
        }
        
        DontDestroyOnLoad(this);
        //当前最大生命值等于一级时的最大生命值
        //当前生命值等于最大生命值
        //初始化当前血量和最大血量的文字UI
        playerData = GetComponent<PlayerData>();
        playerSkillList = GetComponent<PlayerSkillList>();
        playerSubSkillList = GetComponent<SubSkillList>();
        FollowBaby = transform.GetChild(5).GetChild(0).gameObject;
        NotFollowBaby = transform.GetChild(5).GetChild(1).gameObject;
        ButterflyManger = transform.GetChild(5).GetChild(2).GetComponent<PlayerButterflyManger>();
        
        //获得小山猪的刚体组件和动画组件
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxEx = Exp[Level-1];
        StartLevel = Level;

        skillBar01.player = gameObject.GetComponent<PlayerControler>();
        skillBar02.player = gameObject.GetComponent<PlayerControler>();
        skillBar03.player = gameObject.GetComponent<PlayerControler>();
        skillBar04.player = gameObject.GetComponent<PlayerControler>();
        look = Vector2.down;
        ThisPlayer = GetComponent<PlayerControler>();

        if (!isNeedInherit)
        {
            NatureIndex = Random.Range(0, 25);
            InstanceNature(NatureIndex);
        }
        ReFreshAbllityPoint();
        if (!isNeedInherit)
        {
            
            nowHp = maxHp;
        }
        UIHealthBar.Instance.MaxHpText.text = string.Format("{000}", maxHp);
        UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
        UIHealthBar.Instance.InstanceHpBar();
        UIExpBar.Instance.Leveltext.text = LevelForSkill.ToString();
        UIHeadIcon.StaticHeadIcon.ChangeHeadIcon(PlayerHead);
        ComeInANewRoomEvent += ChangeRoomBgm;
        isCanNotUseSpaceItem = false;

        if (PlayerAbility == PlayerAbilityList.迟钝) { TimeStateInvincible *= 2.5f; }
        if (PlayerAbility == PlayerAbilityList.逃跑) { playerData.MoveSpwBounsAlways += 1; ReFreshAbllityPoint(); }
        if (playerData.ResurrectionFossilDone) { PlayerType01 = (int)Type.TypeEnum.Rock; }
        playerData.Type01Always = (Type.TypeEnum)PlayerType01;
        playerData.Type02Always = (Type.TypeEnum)PlayerType02;


        if (!isEvolution && EvolutionSkill != null && (
            (Skill01 != null && (Skill01.SkillIndex == EvolutionSkill.SkillIndex || Skill01.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill02 != null && (Skill02.SkillIndex == EvolutionSkill.SkillIndex || Skill02.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill03 != null && (Skill03.SkillIndex == EvolutionSkill.SkillIndex || Skill03.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill04 != null && (Skill04.SkillIndex == EvolutionSkill.SkillIndex || Skill04.SkillIndex == EvolutionSkill.SkillIndex + 1))

            ))
        {
            isCanEvolution = true;
        }

        playerLocalPosition = transform.GetChild(3).localPosition;
        playerLocalScal = transform.GetChild(3).localScale;

        if (!isNeedInherit)
        {
            if (InitialSkill01 != null) { Skill01 = InitialSkill01; }
            if (InitialSkill02 != null) { Skill02 = InitialSkill02; }
            if (InitialSkill03 != null) { Skill03 = InitialSkill03; }
            if (InitialSkill04 != null) { Skill04 = InitialSkill04; }
        }
    }


    //=================================初始化=====================================








    protected void InstanceNewSkillPanel()
    {
        uIPanelGwtNewSkill.PokemonNameChinese = PlayerNameChinese;
    }

    public void TeraTypeJORChange(int TeraType)
    {
        PlayerTeraTypeJOR = TeraType;
        if (PlayerTeraTypeJOR != 0 || PlayerTeraType != 0) {
            if (PlayerTeraType != 0) { MarterialChangeToTera(PlayerTeraType); } 
            else if (PlayerTeraTypeJOR != 0) { MarterialChangeToTera(PlayerTeraTypeJOR); } 
        }
        else
        {
            MarterialChangeToNurmal();
        }
        SetTerablast(Skill01);
        skillBar01.GetSkill(Skill01);
        SetTerablast(Skill02);
        skillBar02.GetSkill(Skill02);
        SetTerablast(Skill03);
        skillBar03.GetSkill(Skill03);
        SetTerablast(Skill04);
        skillBar04.GetSkill(Skill04);

    }

    public void TeraTypeChange(int TeraType)
    {
        PlayerTeraType = TeraType;
        if (PlayerTeraTypeJOR != 0 || PlayerTeraType != 0)
        {
            if (PlayerTeraType != 0) { MarterialChangeToTera(PlayerTeraType); }
            else if (PlayerTeraTypeJOR != 0) { MarterialChangeToTera(PlayerTeraTypeJOR); }
        }
        else
        {
            MarterialChangeToNurmal();
        }

        SetTerablast(Skill01);
        skillBar01.GetSkill(Skill01);
        SetTerablast(Skill02);
        skillBar02.GetSkill(Skill02);
        SetTerablast(Skill03);
        skillBar03.GetSkill(Skill03);
        SetTerablast(Skill04);
        skillBar04.GetSkill(Skill04);
    }

    public void SetTerablast(Skill s )
    {
        int NowTeraTypr = (PlayerTeraTypeJOR == 0) ? PlayerTeraType : PlayerTeraTypeJOR;
        if (NowTeraTypr != 0)
        {
            if (s != null && s.GetComponent<TeraBlast>() != null) { s.SkillType = NowTeraTypr == 0 ? 1 : NowTeraTypr;  }
        }
    }

    // Update is called once per frame
    protected void UpdatePlayer()
    {
        if (!isDie)
        {
            //技能虚拟按钮
            {
                if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other) {
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill1"))) { isSkill01ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill2"))) { isSkill02ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill3"))) { isSkill03ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill4"))) { isSkill04ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("UseItem"))) { isSpaceItemButtonDown = true; }
                }

            }

            //检验时间是否停止，停止不进化

            //触发特性
            {
                UpdateAbility();
            }
            
            //随时间掉血或者改变状态的函数
            {
                if (isInGrassyTerrain) { PlayerGrassyTerrainHeal(); }
                UpdatePlayerChangeHP();
            }

            //每帧获取一次十字键的按键信息
            {
                Vector2 MoveSpeed = Vector2.zero;
                if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other) {
                    if (MoveStick.joystick != null &&(!Mathf.Approximately(MoveStick.joystick.Horizontal, 0) || !Mathf.Approximately(MoveStick.joystick.Horizontal, 0)) && new Vector2(MoveStick.joystick.Horizontal, MoveStick.joystick.Vertical).magnitude > 0.0001)
                    {
                        Vector2 StickVector = new Vector2(MoveStick.joystick.Horizontal, MoveStick.joystick.Vertical).normalized;
                        float a = _mTool.Angle_360Y(StickVector, Vector2.right);
                        if (a > 22.5f && a <= 67.5f) { MoveSpeed = new Vector2(1f, 1f); }
                        else if (a > 67.5f && a <= 112.5f) { MoveSpeed = new Vector2(0f, 1f); }
                        else if (a > 112.5f && a <= 157.5f) { MoveSpeed = new Vector2(-1f, 1f); }
                        else if (a > 157.5f && a <= 202.5f) { MoveSpeed = new Vector2(-1f, 0f); }
                        else if (a > 202.5f && a <= 247.5f) { MoveSpeed = new Vector2(-1f, -1f); }
                        else if (a > 247.5f && a <= 292.5f) { MoveSpeed = new Vector2(0f, -1f); }
                        else if (a > 292.5f && a <= 337.5f) { MoveSpeed = new Vector2(1f, -1f); }
                        else { MoveSpeed = new Vector2(1f, 0f); }
                        if (isConfusionDone) { MoveSpeed = -MoveSpeed; }


                        /*
                        MoveSpeed = Quaternion.AngleAxis(a, Vector3.forward) * Vector2.right;
                        */
                    }
                    
                    if (MoveArrow.arrow != null && ( MoveArrow.arrow.isUpArrowPressDown || MoveArrow.arrow.isDownArrowPressDown || MoveArrow.arrow.isLeftArrowPressDown || MoveArrow.arrow.isRightArrowPressDown))
                    {
                        if (MoveArrow.arrow.isUpArrowPressDown) { MoveSpeed = new Vector2(MoveSpeed.x, 1);  }
                        if (MoveArrow.arrow.isDownArrowPressDown) { MoveSpeed = new Vector2(MoveSpeed.x, -1);  }
                        if (MoveArrow.arrow.isLeftArrowPressDown) { MoveSpeed = new Vector2(-1, MoveSpeed.x);  }
                        if (MoveArrow.arrow.isRightArrowPressDown) { MoveSpeed = new Vector2(1, MoveSpeed.x);  }
                    }
                    else if(!(MoveStick.joystick != null && MoveStick.joystick.Horizontal != 0 || MoveStick.joystick.Vertical != 0)) {
                        MoveSpeed = Vector2.zero;
                    }
                }

                if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Left")))
                {
                    MoveSpeed.x = -1f * (isConfusionDone ? -1 : 1);
                }
                else if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Right")))
                {
                    MoveSpeed.x = 1f * (isConfusionDone ? -1 : 1);
                }

                if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Up")))
                {
                    MoveSpeed.y = 1f * (isConfusionDone ? -1 : 1);
                }
                else if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Down")))
                {
                    MoveSpeed.y = -1f * (isConfusionDone ? -1 : 1);
                }

                if (MoveSpeed != Vector2.zero)
                {
                    MoveSpeed = MoveSpeed.normalized;
                }
                horizontal = MoveSpeed.x;
                vertical = MoveSpeed.y;

                //Vector2 MoveSpeed = (new Vector2(Input.GetAxis("Horizontal") * (isConfusionDone ? -1 : 1), Input.GetAxis("Vertical") * (isConfusionDone ? -1 : 1))).normalized * (Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")) , Mathf.Abs(Input.GetAxis("Vertical"))));
                //horizontal = MoveSpeed.x;
                //vertical = MoveSpeed.y;

                /*
                horizontal = Input.GetAxis("Horizontal") * (isConfusionDone ? -1 : 1);
                vertical = Input.GetAxis("Vertical") * (isConfusionDone ? -1 : 1);
                */
            }

            //击退 每帧检测一次当前是否为无敌状态，如果是，则计时器计时，如果计时器时间小于0，则变为不无敌状态
            {
                if (isInvincible)
                {
                    InvincileTimer -= Time.deltaTime;

                    //在无敌时间计时器运行的前0。15秒内被击退
                    if (InvincileTimer > TimeInvincible - 0.15f)
                    {
                        float CollidorOffset = 0;
                        float CollidorRadiusH = 0;
                        float CollidorRadiusV = 0;
                        BoxCollider2D boxc = GetComponent<BoxCollider2D>();
                        CollidorOffset = boxc.offset.y; CollidorRadiusH = (boxc.size.x / 2) + boxc.edgeRadius; CollidorRadiusV = (boxc.size.y / 2) + boxc.edgeRadius;


                        RaycastHit2D SearchED = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.down, CollidorRadiusV + koDirection.x * 3.5f * konckout * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room", "Water"));
                        RaycastHit2D SearchEU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.up, CollidorRadiusV + koDirection.x * 3.5f * konckout * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room", "Water"));
                        RaycastHit2D SearchER = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.right, CollidorRadiusH + koDirection.x * 3.5f * konckout * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room", "Water"));
                        RaycastHit2D SearchEL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.left, CollidorRadiusH + koDirection.x * 3.5f * konckout * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room", "Water"));


                        if ((SearchED.collider != null && (SearchED.transform.tag == "Enviroment" || SearchED.transform.tag == "Room" || SearchED.transform.tag == "Water"))
                            || (SearchEU.collider != null && (SearchEU.transform.tag == "Enviroment" || SearchEU.transform.tag == "Room" || SearchEU.transform.tag == "Water"))
                            || (SearchER.collider != null && (SearchER.transform.tag == "Enviroment" || SearchER.transform.tag == "Room" || SearchER.transform.tag == "Water"))
                            || (SearchEL.collider != null && (SearchEL.transform.tag == "Enviroment" || SearchEL.transform.tag == "Room" || SearchEL.transform.tag == "Water"))) { }
                        else
                        {
                            Vector2 position = rigidbody2D.position;
                            position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.x * 30 + MapCreater.StaticMap.RRoom[NowRoom].RoomSize[2], NowRoom.x * 30 + MapCreater.StaticMap.RRoom[NowRoom].RoomSize[3]);
                            position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.y * 24 + MapCreater.StaticMap.RRoom[NowRoom].RoomSize[1], NowRoom.y * 24 + MapCreater.StaticMap.RRoom[NowRoom].RoomSize[0]);
                            /*
                            if (NowRoom != new Vector3Int(100, 100, 0))
                            {
                                position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.x * 30 - 12 , NowRoom.x * 30 + 12);
                                position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.y * 24 - 7.3f, NowRoom.y * 24 + 7.3f);
                            }
                            else
                            {
                                position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.x * 30 - 12, NowRoom.x * 30 + 41.5f);
                                position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * (playerData.IsPassiveGetList[98] ? 2 : 1) * Time.deltaTime, NowRoom.y * 24 - 7.3f, NowRoom.y * 24 + 33.5f);
                            }
                            */
                            rigidbody2D.position = position;
                        }
                    }
                    if (InvincileTimer <= 0)
                    {
                        isInvincible = false;
                    }
                }
            }

            //异常状态CD
            {
                if (isStateInvincible)
                {
                    StateInvincileTimer -= Time.deltaTime;
                    if (StateInvincileTimer <= 0)
                    {
                        isStateInvincible = false;
                    }
                }
            }

            //技能冷却
            {
                //如果技能1在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
                if (isSkill01CD  /* && ((Skill01.useSkillConditions(this))) */ )
                {
                    if (!Is01imprison)
                    {
                        Skill01Timer += Time.deltaTime;
                        if (Skill01Timer >= GetSkillIndexCD(1))//(isParalysisDone ? 1.8f : 1.0f) * ( Skill01.ColdDown * (Skill01.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)))
                        {
                            isSkill01CD = false;
                            Skill01Timer = 0;
                        }
                    }
                    else
                    {
                        imprisonTime01 -= Time.deltaTime;
                        if(imprisonTime01 <= 0)
                        {
                            Is01imprison = false;
                        }
                    }
                }
                //如果技能2在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
                if (isSkill02CD  /*  && ((Skill02.useSkillConditions(this))) */  )
                {
                    if (!Is02imprison)
                    {
                        Skill02Timer += Time.deltaTime;
                        if (Skill02Timer >= GetSkillIndexCD(2))//(isParalysisDone ? 1.8f : 1.0f) * ( Skill02.ColdDown * (Skill02.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)))
                        {
                            isSkill02CD = false;
                            Skill02Timer = 0;
                        }
                    }
                    else
                    {
                        imprisonTime02 -= Time.deltaTime;
                        if (imprisonTime02 <= 0)
                        {
                            Is02imprison = false;
                        }
                    }
                }
                //如果技能3在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
                if (isSkill03CD  /*  && ((Skill03.useSkillConditions(this)))  */  )
                {
                    if (!Is03imprison)
                    {
                        Skill03Timer += Time.deltaTime;
                        if (Skill03Timer >= GetSkillIndexCD(3))//(isParalysisDone ? 1.8f : 1.0f) * ( Skill03.ColdDown * (Skill03.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)))
                        {
                            isSkill03CD = false;
                            Skill03Timer = 0;
                        }
                    }
                    else
                    {
                        imprisonTime03 -= Time.deltaTime;
                        if (imprisonTime03 <= 0)
                        {
                            Is03imprison = false;
                        }
                    }
                }
                //如果技能1在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
                if (isSkill04CD  /*  && ((Skill04.useSkillConditions(this)))  */ )
                {
                    if (!Is04imprison)
                    {
                        Skill04Timer += Time.deltaTime;
                        if (Skill04Timer >= GetSkillIndexCD(4))//(isParalysisDone ? 1.8f : 1.0f) * ( Skill04.ColdDown * (Skill04.isPPUP ? 0.625f : 1) )* (1 - ((float)SpeedAbilityPoint / 500)))
                        {
                            isSkill04CD = false;
                            Skill04Timer = 0;
                        }
                    }
                    else
                    {
                        imprisonTime04 -= Time.deltaTime;
                        if (imprisonTime04 <= 0)
                        {
                            Is04imprison = false;
                        }
                    }
                }
            }

            //使用技能
            {
                //宝可梦战棋
                if (playerData.IsPassiveGetList[66])
                {
                    if (isSkill01ButtonDown || isSkill02ButtonDown || isSkill03ButtonDown || isSkill04ButtonDown )
                    {
                        Debug.Log(Is01imprison + "+" + Is02imprison + "+" + Is03imprison + "+" + Is04imprison);
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                if (isSkill01CD == false && Skill01 != null && !isSkill && (Skill01.useSkillConditions(this)) && !Is01imprison)
                                {
                                    //当动画进行到第8帧时会发射技能1，并技能1进入CD
                                    animator.SetTrigger("Skill");
                                    isSkill01CD = true;
                                    isSkill = true;
                                    isSkill01lunch = true;
                                    skillBar01.isCDStart = true;
                                }
                                break;
                            case 1:
                                if (isSkill02CD == false && Skill02 != null && !isSkill && (Skill02.useSkillConditions(this)) && !Is02imprison)
                                {
                                    //当动画进行到第8帧时会发射技能2，并技能2进入CD
                                    animator.SetTrigger("Skill");
                                    isSkill02CD = true;
                                    isSkill = true;
                                    isSkill02lunch = true;
                                    skillBar02.isCDStart = true;
                                }
                                break;
                            case 2:
                                if (isSkill03CD == false && Skill02 != null && !isSkill && (Skill02.useSkillConditions(this)) && !Is03imprison)
                                {
                                    //当动画进行到第8帧时会发射技能3，并技能3进入CD
                                    animator.SetTrigger("Skill");
                                    isSkill03CD = true;
                                    isSkill = true;
                                    isSkill03lunch = true;
                                    skillBar03.isCDStart = true;
                                }
                                break;
                            case 3:
                                if (isSkill04CD == false && Skill01 != null && !isSkill && (Skill01.useSkillConditions(this)) && !Is04imprison)
                                {
                                    //当动画进行到第8帧时会发射技能4，并技能4进入CD
                                    animator.SetTrigger("Skill");
                                    isSkill04CD = true;
                                    isSkill = true;
                                    isSkill04lunch = true;
                                    skillBar04.isCDStart = true;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    //当按下q键时发射skill01

                    if (isSkill01ButtonDown && isSkill01CD == false && Skill01 != null && !isSkill && !Is01imprison)
                    {
                        if ((Skill01.useSkillConditions(this)))
                        {
                            //当动画进行到第8帧时会发射技能1，并技能1进入CD
                            animator.SetTrigger("Skill");
                            isSkill01CD = true;
                            isSkill = true;
                            isSkill01lunch = true;
                            skillBar01.isCDStart = true;
                        }
                    }


                    //当按下w键时发射skill02

                    if (isSkill02ButtonDown && isSkill02CD == false && Skill02 != null && !isSkill && !Is02imprison)
                    {
                        if ((Skill02.useSkillConditions(this)))
                        {
                            //当动画进行到第8帧时会发射技能2，并技能2进入CD
                            animator.SetTrigger("Skill");
                            isSkill02CD = true;
                            isSkill = true;
                            isSkill02lunch = true;
                            skillBar02.isCDStart = true;
                        }
                    }


                    //当按下e键时发射skill03

                    if (isSkill03ButtonDown && isSkill03CD == false && Skill03 != null && !isSkill && !Is03imprison)
                    {
                        if ((Skill03.useSkillConditions(this)))
                        {
                            //当动画进行到第8帧时会发射技能3，并技能3进入CD
                            animator.SetTrigger("Skill");
                            isSkill03CD = true;
                            isSkill = true;
                            isSkill03lunch = true;
                            skillBar03.isCDStart = true;
                        }
                    }


                    //当按下r键时发射skill04

                    if (isSkill04ButtonDown && isSkill04CD == false && Skill04 != null && !isSkill && !Is04imprison)
                    {

                        if ((Skill04.useSkillConditions(this)))
                        {
                            //当动画进行到第8帧时会发射技能4，并技能4进入CD
                            animator.SetTrigger("Skill");
                            isSkill04CD = true;
                            isSkill = true;
                            isSkill04lunch = true;
                            skillBar04.isCDStart = true;
                        }
                    }
                }
            }

            //进入新房间
            {
                if (InANewRoom == true)
                {
                    NewRoomTimer += Time.deltaTime;

                    RestoreStrengthAndTeraType();
                    if (ComeInANewRoomEvent != null && !isComeInANewRoomEvent)
                    {
                        if (playerAbility == PlayerAbilityList.雪隐) { isSnowCloakTrigger = false; CancelInvoke("RemoveSnowCloak"); }
                        Debug.Log(ComeInANewRoomEvent);
                        ComeInANewRoomEvent(this);
                        isComeInANewRoomEvent = true;
                    }
                    ConfusionRemove();
                    CheckStateInInputNewRoom();
                    isSpaceItemCanBeUse = false;
                    if (NewRoomTimer >= 1.2f)
                    {
                        //Debug.Log(MapCreater.StaticMap.RRoom[NowRoom].RoomSize[0]);
                        isToxicDoneInNewRoom = false;
                        isBornDoneInNewRoom = false;
                        InANewRoom = false;
                        isStrengthAndTeraTypeBeRestore = false;
                        isComeInANewRoomEvent = false;
                        NewRoomTimer = 0;
                        isSpaceItemCanBeUse = true;
                        CanNotUseSpaceItem = false;

                        Camera MainCamera = null;
                        if (CameraAdapt.MainCamera != null) { MainCamera = CameraAdapt.MainCamera.GetComponent<Camera>(); }
                        if  (MainCamera != null &&
                            (!MapCreater.StaticMap.RRoom.ContainsKey(NowRoom) || 
                            !(transform.position.x >= (float)NowRoom.x*30.0f-15.0f && transform.position.x <= (float)NowRoom.x * 30.0f + 15.0f && transform.position.y >= (float)NowRoom.y * 24.0f - 12.0f && transform.position.y <= (float)NowRoom.y * 24.0f + 12.0f) ||
                            !(MainCamera.transform.position.x >= (float)NowRoom.x * 30.0f - 15.0f && MainCamera.transform.position.x <= (float)NowRoom.x * 30.0f + 15.0f && MainCamera.transform.position.y >= (float)NowRoom.y * 24.0f - 12.0f && MainCamera.transform.position.y <= (float)NowRoom.y * 24.0f + 12.0f)
                            ))
                        {
                            Vector3Int LastNowRoom = NowRoom;
                            NowRoom = new Vector3Int((int)Mathf.Round(transform.position.x / 30.0f), (int)Mathf.Round(transform.position.y / 24.0f), 0);
                            MainCamera.transform.position = new Vector3((float)NowRoom.x *30.0f , (float)NowRoom.y * 24.0f +0.7f , -11.0f);
                            InANewRoom = true;
                            NewRoomTimer = 0f;
                            Debug.Log(NowRoom + "+" + LastNowRoom);
                            UiMiniMap.Instance.SeeMapOver();
                            UiMiniMap.Instance.MiniMapMove(LastNowRoom - NowRoom);
                        }
                    }
                }
            }

            //使用一次性道具
            {
                if (isSpaceItemCanBeUse && isSpaceItemButtonDown && !isCanNotUseSpaceItem && spaceItem != null)
                {
                    if (UseSpaceItem.UseSpaceItemConditions(this))
                    {
                        spaceitemUseUI.UIAnimationStart(spaceItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
                        UseSpaceItem.UsedSpaceItem(this);
                    }
                }
            }

            //触发被动道具效果
            {
                UpdatePasssive();
                if (UpdataPassiveItemEvent != null)
                {
                    UpdataPassiveItemEvent(ThisPlayer);
                }
            }

        }
    }

    private void LateUpdate()
    {
        //重置虚拟按键按下状态
        {
            isSkill01ButtonDown = false;
            isSkill02ButtonDown = false;
            isSkill03ButtonDown = false;
            isSkill04ButtonDown = false;
            isSpaceItemButtonDown = false;
        }
    }



    //=========================青草场地回血事件========================
    /// <summary>
    /// 角色的青草场地回血计时器，每计时5s青草场地回血一次
    /// </summary>
    protected float PlayerGrassyTerrainTimer;
    /// <summary>
    /// 根据青草场地时间敌人回血
    /// </summary>
    void PlayerGrassyTerrainHeal()
    {
        if (PlayerGrassyTerrainTimer == 0)
        {
            PokemonHpChange(null, this.gameObject, 0, 0, (int)Mathf.Clamp(((float)maxHp / 16), 1, 10), Type.TypeEnum.IgnoreType);
        }
        PlayerGrassyTerrainTimer += Time.deltaTime;
        if (PlayerGrassyTerrainTimer >= 5)
        {
            PlayerGrassyTerrainTimer = 0;
        }

    }
    //=========================青草场地回血事件========================




    bool isComeInANewRoomEvent;
    bool isToxicDoneInNewRoom;
    bool isBornDoneInNewRoom;
    void CheckStateInInputNewRoom()
    {
        if (isToxicDone && !isToxicDoneInNewRoom)
        {
            KnockOutPoint = 1;
            ChangeHp((float)(-maxHp/20),0,0);
            isToxicDoneInNewRoom = true;
        }
        if (isBurnDone && !isBornDoneInNewRoom)
        {
            KnockOutPoint = 1;
            ChangeHp((float)(-maxHp / 20), 0, 0);
            isBornDoneInNewRoom = true;
        }
    }

    bool isStrengthAndTeraTypeBeRestore = false;

    void RestoreStrengthAndTeraType()
    {
        
        if (!isStrengthAndTeraTypeBeRestore)
        {
            TeraTypeJORChange(0);
            if (!isBatonPass) {
                playerData.RestoreJORSata(); }
            isStrengthAndTeraTypeBeRestore = true;
            ReFreshAbllityPoint();
        }
    }




    void ChangeRoomBgm(PlayerControler player)
    {
        
        if (!MapCreater.StaticMap)
        {
            // 兼容测试 case
            // BackGroundMusic.StaticBGM.ChangeBGMToTown();
            return;
        }
        if (NowRoom == MapCreater.StaticMap.PCRoomPoint)
        {
            BackGroundMusic.StaticBGM.ChangeBGMToPC();
        }
        else if(NowRoom == MapCreater.StaticMap.StoreRoomPoint)
        {
            BackGroundMusic.StaticBGM.ChangeBGMToStore();
        }
        else if (NowRoom == MapCreater.StaticMap.BossRoomPoint)
        {
            if (MapCreater.StaticMap.RRoom[NowRoom].isClear <= 0) { BackGroundMusic.StaticBGM.ChangeBGMToBossWin(); }
            else { BackGroundMusic.StaticBGM.ChangeBGMToBoss(); }
        }
        else
        {
            BackGroundMusic.StaticBGM.ChangeBGMToTown();
        }
    }


    protected void FixedUpdatePlayer()
    {
        //2D向量position等于刚体组件的坐标,之后让position的xy坐标加上十字键x速度x单位时间，最后让刚体的位置等于position
        if (!isDie && !isSkill && !isTP && !isCanNotMove && !isPlayerFrozenDone)
        {
            position = rigidbody2D.position;
            position.x = position.x + horizontal * speed * Time.deltaTime;
            position.y = position.y + vertical * speed * Time.deltaTime;
            rigidbody2D.position = position;
            


            //位移变量为十字键操纵值
            move = new Vector2(horizontal, vertical);

            //仅当发生位移时可以改变动画，如果不位移动画不会改变
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                move = new Vector2(horizontal, vertical);
                if (move.x > move.y && -move.x > move.y)
                {
                    look.Set(0, -1);
                }
                else if (move.x > move.y && -move.x <= move.y)
                {
                    look.Set(1, 0);
                }
                else if (move.x <= move.y && -move.x > move.y)
                {
                    look.Set(-1, 0);
                }
                else if (move.x <= move.y && -move.x <= move.y)
                {
                    look.Set(0, 1);
                }
            }

            if (!isCanNotTurnDirection) {
                animator.SetFloat("LookX", look.x);
                animator.SetFloat("LookY", look.y);
            }
            animator.SetFloat("Speed", move.magnitude);
        }
    }

















    //======================================死亡和改变生命值====================================================

    /// <summary>
    /// 气势头戴状态
    /// </summary>
    public void EndureStart()
    {
        playerData.isEndure = true;
        playerUIState.StatePlus(8);
    }


    /// <summary>
    /// 结束气势头戴状态
    /// </summary>
    public void EndureOver()
    {
        playerData.isEndure = false;
        playerUIState.StateDestory(8);

    }


    /// <summary>
    /// 玩家是否处于反射壁状态
    /// </summary>
    public bool isReflect;


    /// <summary>
    /// 玩家是否处于光墙状态
    /// </summary>
    public bool isLightScreen;


    /// <summary>
    ///声明一个改变生命的函数ChangeHp，改变的点数为ChangePoint，当改变点数为负时触发无敌时间，当改变点数为正时不触发无敌时间
    /// </summary>
    /// <param name="ChangePoint"></param>
    /// <param name="ChangePointSp"></param>
    /// <param name="SkillType"></param>
    public void ChangeHp(float ChangePoint , float ChangePointSp , int SkillType, bool Crit = false)
    {

        if (ChangePoint > 0 || ChangePointSp > 0)
        {
            int Recover = (int)(ChangePoint + ChangePointSp);
            nowHp = Mathf.Clamp(nowHp + (int)(ChangePoint+ChangePointSp), 0, maxHp);
            DmgShow(Recover, true, Crit);

            //血量上升时对血条UI输出当前血量，并调用血条上升的函数
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
            UIHealthBar.Instance.ChangeHpUp();
        }
        else
        {
            Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;
            if ((int)SkillType != 19) {
                ChangePoint = ChangePoint * (playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)))
                    * (playerData.IsPassiveGetList[58] ? 1.5f : 1f);
                ChangePointSp = ChangePointSp * (playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)))
                    * (playerData.IsPassiveGetList[58] ? 1.5f : 1f);
                ChangePoint = ChangePoint * (isReflect ? 0.75f : 1);
                ChangePointSp = ChangePointSp * (isLightScreen ? 0.75f : 1);
            }
            //如果无敌结束，不无敌的话变为不无敌状态，无敌时间计时器时间设置为无敌时间
            if (isInvincible || isInvincibleAlways)
            {
                return;
            }
            else
            {
                int ChangeHP = Hp;

                if((int)SkillType != 19)
                {
                    if (!isInPsychicTerrain)
                    {
                        int startHp = nowHp;
                        nowHp = Mathf.Clamp(nowHp + (int)((ChangePoint / DefAbilityPoint + ChangePointSp / SpdAbilityPoint - 2) * (Type.TYPE[(int)SkillType][PlayerType01] * Type.TYPE[(int)SkillType][PlayerType02] * (PlayerTeraTypeJOR == 0 ? Type.TYPE[(int)SkillType][PlayerTeraType] : Type.TYPE[(int)SkillType][PlayerTeraTypeJOR])) * ((playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType])))), (nowHp > 1) ? (playerData.isEndure ? 1 : 0) : 0, maxHp);
                        int allDmg = startHp - nowHp;
                        DmgShow(allDmg, false, Crit);
                    }
                    else
                    {
                        if (Mathf.Abs((int)((ChangePoint / DefAbilityPoint + ChangePointSp / SpdAbilityPoint - 2) * (Type.TYPE[(int)SkillType][PlayerType01] * Type.TYPE[(int)SkillType][PlayerType02] * (PlayerTeraTypeJOR == 0 ? Type.TYPE[(int)SkillType][PlayerTeraType] : Type.TYPE[(int)SkillType][PlayerTeraTypeJOR])) * ((playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType]))))) > (int)(maxHp / 16))
                        {
                            int startHp = nowHp;
                            nowHp = Mathf.Clamp(nowHp + (int)((ChangePoint / DefAbilityPoint + ChangePointSp / SpdAbilityPoint - 2) * (Type.TYPE[(int)SkillType][PlayerType01] * Type.TYPE[(int)SkillType][PlayerType02] * (PlayerTeraTypeJOR == 0 ? Type.TYPE[(int)SkillType][PlayerTeraType] : Type.TYPE[(int)SkillType][PlayerTeraTypeJOR])) * ((playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (playerData.TypeDefAlways[(int)SkillType] + playerData.TypeDefJustOneRoom[(int)SkillType])))), (nowHp > 1) ? (playerData.isEndure ? 1 : 0) : 0, maxHp);
                            int allDmg = startHp - nowHp;
                            DmgShow(allDmg, false, Crit);
                        }
                    }
                }
                else
                {
                    if (!isInPsychicTerrain)
                    {
                        int startHp = nowHp;
                        nowHp = Mathf.Clamp(nowHp + Mathf.Clamp((int)ChangePoint, -100000, -1), (nowHp > 1) ? (playerData.isEndure ? 1 : 0) : 0, maxHp);
                        int allDmg = startHp - nowHp;
                        DmgShow(allDmg, false, Crit);
                    }
                    else
                    {
                        if (Mathf.Abs(Mathf.Clamp((int)ChangePoint, -100000, -1)) > (int)(maxHp / 16))
                        {
                            int startHp = nowHp;
                            nowHp = Mathf.Clamp(nowHp + Mathf.Clamp((int)ChangePoint, -100000, -1), (nowHp > 1) ? (playerData.isEndure ? 1 : 0) : 0, maxHp);
                            int allDmg = startHp - nowHp;
                            DmgShow(allDmg, false, Crit);
                        }
                    }
                }
                isInvincible = true;
                InvincileTimer = TimeInvincible;

                ChangeHP = ChangeHP - Hp;
                if (ChangeHP > 0)
                {                
                    //给AP
                    if (FloorNum.GlobalFloorNum != null && ScoreCounter.Instance != null)
                    {
                        ScoreCounter.Instance.DmagePunishAP += APBounsPoint.DmagePunish(ChangeHP);
                    }
                }


                if (isSleepDone) { SleepRemove(); }
                if (UIHealthBar.Instance != null) {
                    UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
                    UIHealthBar.Instance.ChangeHpDown();
                    UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
                }

                if(nowHp <= 0) { PlayerDie(); }
                //血量上升时对血条UI输出当前血量，并调用血条上升的函数



                HitEvent(ChangePoint , ChangePointSp , SkillType , Crit);

                //输出被击打的动画管理器参数
                animator.SetTrigger("Hit");
            }
        }
    }

    /// <summary>
    /// 受到伤害时触发的事件
    /// </summary>
    void HitEvent(float ChangePoint, float ChangePointSp, int SkillType, bool Crit = false)
    {

        Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;
        //道具096 冷静头脑
        if (playerData.IsPassiveGetList[96] && enumVaue == Type.TypeEnum.Ice)
        {
            playerData.PassiveItemClamMind();
        }

        //道具092 王者盾牌
        if (playerData.IsPassiveGetList[92])
        {
            playerData.KingsShieldDone();
        }

        //道具133 淘金滑板
        if (playerData.IsPassiveGetList[133])
        {
            if (Random.Range(0.0f, 1.0f) > 0.5f) { ChangeMoney(-1); }
        }

        //技能164 恶意追击精通
        if (Skill01 != null && Skill01.SkillIndex == 164 ) { MinusSkillCDTime(1 , 1 , false); }
        if (Skill02 != null && Skill02.SkillIndex == 164 ) { MinusSkillCDTime(2 , 1 , false); }
        if (Skill03 != null && Skill03.SkillIndex == 164 ) { MinusSkillCDTime(3 , 1 , false); }
        if (Skill04 != null && Skill04.SkillIndex == 164 ) { MinusSkillCDTime(4 , 1 , false); }

    }

    /// <summary>
    /// 精准掉血，数值是多少就掉多少
    /// </summary>
    /// <param name="ChangePoint">改变量</param>
    public void ChangeHp(int ChangePoint)
    {
        nowHp = Mathf.Clamp(nowHp + ChangePoint, 0, maxHp);
        if (ChangePoint > 0)
        {
            //血量上升时对血条UI输出当前血量，并调用血条上升的函数
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
            UIHealthBar.Instance.ChangeHpUp();
        }
        else
        {
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.ChangeHpDown();
            UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
        }
        //掉血条用“魔法伤害，暴击”表示
        DmgShow(ChangePoint, ChangePoint > 0 ? true : false, true, true);
    }

    private void DmgShow(int dmg, bool recover, bool crit, bool magic = false)
    {
        if (InitializePlayerSetting.GlobalPlayerSetting.isShowDamage && FloatingDamage)
        {
            GameObject fd = Instantiate(FloatingDamage, transform.position +  Vector3.right * Random.Range(-0.8f,0.8f), Quaternion.identity);
            fd.transform.GetComponent<damageShow>().SetText(dmg, crit, recover, magic, true);
        }
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public void PlayerDie()
    {
        isDie = true; 
        animator.SetTrigger("Die");
        rigidbody2D.bodyType = RigidbodyType2D.Static;
    }


    bool isTimePunishDone;

    /// <summary>
    /// 角色复活或者呼出死亡UI
    /// </summary>
    public void CallDieMask()
    {
        if (isDie) {
            //道具072 复活化石
            if (playerData.IsPassiveGetList[72])
            {
                animator.Play("Idle" , 0);
                animator.ResetTrigger("Die");
                isDie = false;
                PokemonHpChange(null, this.gameObject, 0, 0, (int)(maxHp / 2.0f), Type.TypeEnum.IgnoreType);
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                playerData.IsPassiveGetList[72] = false;
                spaceitemUseUI.UIAnimationStart(PassiveItemGameObjList.ObjList.List[20].GetComponent<SpriteRenderer>().sprite);
                PlayerType01 = (int)Type.TypeEnum.Rock;
                playerData.Type01Always = Type.TypeEnum.Rock;
                playerData.ResurrectionFossilDone = true;
            }
            else {
                if (TPMask.In != null)
                {
                    if (!isTimePunishDone) {
                        isTimePunishDone = true;
                        MapCreater m = FindObjectOfType<MapCreater>();
                        if (FloorNum.GlobalFloorNum != null && m != null && ScoreCounter.Instance != null)
                        {
                            ScoreCounter.Instance.TimePunishAP += APBounsPoint.TimePunish(m.MapTime, FloorNum.GlobalFloorNum.FloorNumber);
                            ScoreCounter.Instance.ClearGameBouns = false;
                        }
                    }


                    TPMask.In.BlackTime = 0;
                    TPMask.In.TPStart = true;
                    TPMask.In.transform.GetChild(0).gameObject.SetActive(true);
                    TPMask.In.transform.GetChild(0).GetComponent<DiePanel>().Die(PlayerNameChinese);
                }
            }
        }
    }


    //======================================死亡和改变生命值====================================================















    //======================================改变玩家持有物======================================

    /// <summary>
    /// 声明一个改变金钱的函数ChangeMoney
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeMoney(int ChangePoint)
    {

        int BrforeMoney = nowMoney;
        //改变金钱数，上限为99，下限为0，之后向UI对象输出金钱的改变值，并调用UI改变金钱数的函数
        nowMoney = Mathf.Clamp(nowMoney + ChangePoint + (playerData.IsPassiveGetList[10]?1:0), 0, 99);
        UIMoneyBar.Instance._Money += nowMoney - BrforeMoney;
        UIMoneyBar.Instance.MoneyChange();
    }


    /// <summary>
    /// 声明一个改变石头的函数
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeStone(int ChangePoint)
    {
        int BrforeStone = nowStone;

        //改变石头数，上限为99，下限为0，之后向UI对象输出金钱的改变值，并调用UI改变金钱数的函数
        nowStone = Mathf.Clamp(nowStone + ChangePoint, 0, 99);
        UIMoneyBar.Instance._Stone += nowStone - BrforeStone;
        UIMoneyBar.Instance.StoneChange();
    }




    /// <summary>
    /// 声明一个改变心之鳞片的函数
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeHearsScale(int ChangePoint)
    {

        //改变心之鳞片数，上限为99，下限为0，
        nowHeartScale = Mathf.Clamp(nowHeartScale + ChangePoint, 0, 99);
    }


    /// <summary>
    /// 声明一个改变PPUP的函数
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangePPUp(int ChangePoint)
    {

        //改变PP提升数，上限为99，下限为0，
        nowPPUp = Mathf.Clamp(nowPPUp + ChangePoint, 0, 99);
    }


    /// <summary>
    /// 声明一个改变精通种子的函数
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeMSeed(int ChangePoint)
    {

        //改变精通种子数，上限为99，下限为0，
        nowSeedofMastery = Mathf.Clamp(nowSeedofMastery + ChangePoint, 0, 99);
    }

    //======================================改变玩家持有物======================================













    //====================================================角色成长====================================================



    /// <summary>
    /// 改变努力值
    /// </summary>
    /// <param name="HWP"></param>
    public void ChangeHPW(Vector2Int HWP)
    {
        if (HWP.y > 0) {
            float f = 0;

            //性格加成
            if   (GetNatureUpAbllity(NatureIndex) != 0 && HWP.x == GetNatureUpAbllity(NatureIndex) + 1) { f += 0.15f; }
            else if (GetNatureDownAbllity(NatureIndex) != 0 && HWP.x == GetNatureDownAbllity(NatureIndex) + 1){ f -= 0.15f; }
            switch (HWP.x) {
                case 1:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[18] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.HPHardWorkAlways += f;
                    HWPShow(f, 0);
                    break;
                case 2:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[19] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.AtkHardWorkAlways += f;
                    HWPShow(f, 1);
                    break;
                case 3:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[20] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.DefHardWorkAlways += f;
                    HWPShow(f, 2);
                    break;
                case 4:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[21] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.SpAHardWorkAlways += f;
                    HWPShow(f, 3);
                    break;
                case 5:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[22] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.SpDHardWorkAlways += f;
                    HWPShow(f, 4);
                    break;
                case 6:
                    f += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[89] ? 0.25f : 0) + (playerData.IsPassiveGetList[23] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                    f = Mathf.Round(f * 100) / 100;
                    playerData.SpeHardWorkAlways += f;
                    HWPShow(f, 5);
                    break;
            }
        }
        else
        {
            switch (HWP.x)
            {
                case 1:
                    playerData.HPHardWorkAlways += (HWP.y) * 0.25f;
                    break;
                case 2:
                    playerData.AtkHardWorkAlways += (HWP.y) * 0.25f;
                    break;
                case 3:
                    playerData.DefHardWorkAlways += (HWP.y) * 0.25f;
                    break;
                case 4:
                    playerData.SpAHardWorkAlways += (HWP.y) * 0.25f;
                    break;
                case 5:
                    playerData.SpDHardWorkAlways += (HWP.y) * 0.25f;
                    break;
                case 6:
                    playerData.SpeHardWorkAlways += (HWP.y) * 0.25f;
                    break;
            }
            HWPShow((HWP.y) * 0.25f , HWP.x - 1);
        }
        float LuckHPDPlusPer = Random.Range(0.0f, 1.0f);
        if (!playerData.IsPassiveGetList[17])
        {
            if (LuckHPDPlusPer >= 0.85f && LuckHPDPlusPer <= 0.95f)
            {
                playerData.LuckHardWorkAlways += 0.5f;
                HWPShow(0.5f, 7);
            }
            else if (LuckHPDPlusPer > 0.95f && LuckHPDPlusPer <= 0.985f)
            {
                playerData.LuckHardWorkAlways += 0.75f;
                HWPShow(0.75f, 7);
            }
            else if (LuckHPDPlusPer > 0.985f && LuckHPDPlusPer <= 1.0f)
            {
                playerData.LuckHardWorkAlways += 1.25f;
                HWPShow(1.25f, 7);
            }
        }
        else
        {
            if (LuckHPDPlusPer >= 0.6f && LuckHPDPlusPer <= 0.86f)
            {
                playerData.LuckHardWorkAlways += 0.5f;
                HWPShow(0.5f, 7);
            }
            else if (LuckHPDPlusPer > 0.86f && LuckHPDPlusPer <= 0.96f)
            {
                playerData.LuckHardWorkAlways += 0.75f;
                HWPShow(0.75f, 7);
            }
            else if (LuckHPDPlusPer > 0.96f && LuckHPDPlusPer <= 1.0f)
            {
                playerData.LuckHardWorkAlways += 1.25f;
                HWPShow(1.25f, 7);
            }
        }

        ReFreshAbllityPoint();
    }


    public void HWPShow(float Value , int Type)
    {
        if (InitializePlayerSetting.GlobalPlayerSetting.isShowHardworking && HardworkFloatingShow)
        {
            Vector3 Offset = Vector3.zero;
            if (LastHardworkShow.Count != 0)
            {
                LastHardworkShow.RemoveAll(item => item == null);
                if (LastHardworkShow.Count != 0 && (LastHardworkShow[0].transform.position - transform.position).y < 0.6f) { 
                    Offset = Vector3.up * (0.6f + (LastHardworkShow[LastHardworkShow.Count - 1].transform.position - transform.position).y); 
                }
            }
            
            
            HardworkShow h = Instantiate(HardworkFloatingShow, transform.position + Offset + Vector3.right * Random.Range(-0.5f, 0.5f), Quaternion.identity);
            if (LastHardworkShow.Count != 0 && (LastHardworkShow[0].transform.position - transform.position).y < 0.6f) {  LastHardworkShow.Add(h); }
            else { LastHardworkShow.Insert(0, h); }
            h.SetText(Value,Type);
        }
    }



    /// <summary>
    /// //声明一个改变经验值以及提升等级的函数ChangeEx
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeEx(int ChangePoint)
    {

        //当改变的经验值为正时可以改变
        if (ChangePoint > 0)
        {
            //改变经验值
            nowEx = Mathf.Clamp((int)(nowEx + (ChangePoint * 1.3f * (playerData.IsPassiveGetList[12]?1.25:1) * (playerData.IsPassiveGetList[29] ? 1.25 : 1) )), 0, 80000);

            //如果当前经验值小于于最大经验值，向经验条UI输出变化结果
            if (nowEx < maxEx)
            {
                UIExpBar.Instance.Per = (float)nowEx / (float)maxEx;
                UIExpBar.Instance.ExpUp();
            }
            //如果当前经验值大于最大经验值，增加等级，减少当前经验值，并改变最大经验值，计算溢出次数，并输出给UI    
            else
            {
                for (; nowEx >= maxEx; UIExpBar.Instance.Icount++)
                {
                    Level++;
                    if (JudgeEvolutionForEachLevel())
                    {
                        isCanEvolution = true;
                    }
                    if (!isEvolution && isCanEvolution)
                    {
                        EvolutionStart();
                    }
                    nowEx = nowEx - maxEx;
                    maxEx = Exp[Level - 1];
                    int HpBewton = maxHp - nowHp;
                    ReFreshAbllityPoint();
                    nowHp = maxHp-HpBewton;
                    
                    HpBewton = 0;
                    UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
                    UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
                    UIHealthBar.Instance.MaxHpText.text = string.Format("{000}", maxHp);
                    
                    UIHealthBar.Instance.ChangeHpUp();

                }
                
                UIExpBar.Instance.Per = (float)nowEx / (float)maxEx;
                UIExpBar.Instance.ExpUpOverflow();
            }
            
        }

    }


    /// <summary>
    /// 刷新角色能力值
    /// </summary>
    public void ReFreshAbllityPoint()
    {
        MaxHpForLevel(Level);


        //新方法在前7级增长比老方法大，第8级增长和老方法一样，之后开始衰减
        maxHp = (int)((maxHp + playerData.HPHardWorkAlways + playerData.HPHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.HPBounsAlways + playerData.HPBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        AtkAbility = (int)((AbilityForLevel(Level, AtkPlayerPoint) + playerData.AtkHardWorkAlways + playerData.AtkHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        SpAAbility = (int)((AbilityForLevel(Level, SpAPlayerPoint) + playerData.SpAHardWorkAlways + playerData.SpAHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpABounsAlways + playerData.SpABounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        DefAbility = (int)((AbilityForLevel(Level, DefPlayerPoint) + playerData.DefHardWorkAlways + playerData.DefHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.DefBounsAlways + playerData.DefBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        SpDAbility = (int)((AbilityForLevel(Level, SpdPlayerPoint) + playerData.SpDHardWorkAlways + playerData.SpDHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        SpeedAbility = (int)((AbilityForLevel(Level, SpeedPlayerPoint) + playerData.SpeHardWorkAlways + playerData.SpeHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));
        speed = Mathf.Clamp(!isSleepDone ? ((MoveSpePlayerPoint + playerData.MoveSpeHardWorkAlways + playerData.MoveSpeHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯)) : 1.0f, 1.0f, 10);
        LuckPoint = (int)((LuckPlayerPoint + playerData.LuckHardWorkAlways + playerData.LuckHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom, PlayerAbility == PlayerAbilityList.恒净之躯));

        //老方法无衰减，指数增长
        //maxHp = (int)((maxHp + playerData.HPHardWorkAlways + playerData.HPHardWorkJustOneRoom) * Mathf.Pow((((playerData.HPBounsAlways + playerData.HPBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯  ? 1.1f : 1.2f) : 1.2f)), (playerData.HPBounsAlways + playerData.HPBounsJustOneRoom)));
        //AtkAbility = (int)((AbilityForLevel(Level, AtkPlayerPoint) + playerData.AtkHardWorkAlways + playerData.AtkHardWorkJustOneRoom) * Mathf.Pow((((playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom < 0)? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f )) , (playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom)));
        //SpAAbility = (int)((AbilityForLevel(Level, SpAPlayerPoint) + playerData.SpAHardWorkAlways + playerData.SpAHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpABounsAlways + playerData.SpABounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.SpABounsAlways + playerData.SpABounsJustOneRoom))); 
        //DefAbility = (int)((AbilityForLevel(Level, DefPlayerPoint) + playerData.DefHardWorkAlways + playerData.DefHardWorkJustOneRoom) * Mathf.Pow((((playerData.DefBounsAlways + playerData.DefBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.DefBounsAlways + playerData.DefBounsJustOneRoom))); 
        //SpDAbility = (int)((AbilityForLevel(Level, SpdPlayerPoint) + playerData.SpDHardWorkAlways + playerData.SpDHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom)));
        //SpeedAbility = (int)((AbilityForLevel(Level, SpeedPlayerPoint) + playerData.SpeHardWorkAlways + playerData.SpeHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom)));
        //speed = Mathf.Clamp(!isSleepDone?((MoveSpePlayerPoint + playerData.MoveSpeHardWorkAlways + playerData.MoveSpeHardWorkJustOneRoom) * Mathf.Pow((((playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom))):1.0f , 1.0f , 10);
        //LuckPoint = (int)((LuckPlayerPoint + playerData.LuckHardWorkAlways + playerData.LuckHardWorkJustOneRoom) * Mathf.Pow((((playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.恒净之躯 ? 1.1f : 1.2f) : 1.2f)), (playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom)));
        
        UIHealthBar.Instance.MaxHpText.text = maxHp.ToString();
        UIHealthBar.Instance.NowHpText.text = nowHp.ToString();
        float x = (UIHealthBar.Instance.Per - (float)nowHp / (float)maxHp);
        if (x > 0) {
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.ChangeHpDown();
        }else if (x < 0)
        {
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.ChangeHpUp();
        }
        RefreshAbllityUI();
    }

    /// <summary>
    /// 更新玩家的状态提升值UI
    /// </summary>
    public void RefreshAbllityUI()
    {

        switch (InitializePlayerSetting.GlobalPlayerSetting.isShowBouns)
        {
            case 0:
                playerUIState.RemoveAllAbllityChangeMark();
                playerUIStateOther.RemoveAllAbllityChangeMark();
                break;
            case 1:
                playerUIStateOther.RemoveAllAbllityChangeMark();
                playerUIState.AbllityChange(0, playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom);
                playerUIState.AbllityChange(1, playerData.DefBounsAlways + playerData.DefBounsJustOneRoom);
                playerUIState.AbllityChange(2, playerData.SpABounsAlways + playerData.SpABounsJustOneRoom);
                playerUIState.AbllityChange(3, playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom);
                playerUIState.AbllityChange(4, playerData.HPBounsAlways + playerData.HPBounsJustOneRoom);
                playerUIState.AbllityChange(5, playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom);
                playerUIState.AbllityChange(6, playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom);
                playerUIState.AbllityChange(7, playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom);
                break;
            case 2:
                playerUIState.RemoveAllAbllityChangeMark();
                playerUIStateOther.AbllityChange(0, playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom);
                playerUIStateOther.AbllityChange(1, playerData.DefBounsAlways + playerData.DefBounsJustOneRoom);
                playerUIStateOther.AbllityChange(2, playerData.SpABounsAlways + playerData.SpABounsJustOneRoom);
                playerUIStateOther.AbllityChange(3, playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom);
                playerUIStateOther.AbllityChange(4, playerData.HPBounsAlways + playerData.HPBounsJustOneRoom);
                playerUIStateOther.AbllityChange(5, playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom);
                playerUIStateOther.AbllityChange(6, playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom);
                playerUIStateOther.AbllityChange(7, playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom);
                break;
        }



    }



    /// <summary>
    /// 根据提升等级学习新技能
    /// </summary>
    public void LearnNewSkill()
    {
        if ((LevelForSkill-StartLevel) % GetSkillLevel == 0 && LevelForSkill != StartLevel)
        {
            if (playerSkillList.SkillLearnList.Count > 0)
            {
                uIPanelGwtNewSkill.SelectSkill(playerSkillList.RandomLearnASkill(LevelForSkill), playerSkillList.RandomLearnASkill(LevelForSkill), playerSkillList.RandomLearnASkill(LevelForSkill));
            }
            else { Debug.Log("Error : SkillList OverFlow"); }
            levelChecker++;
        }
    }


    /// <summary>
    /// 其他渠道学习新技能
    /// </summary>
    /// <param name="LearnSkill"></param>
    public void LearnNewSkillByOtherWay( Skill LearnSkill )
    {
        uIPanelGwtNewSkill.NewSkillPanzelJump(LearnSkill , false);
    }


    /// <summary>
    /// 获得新技能
    /// </summary>
    /// <param name="NewSkill"></param>
    /// <param name="OldSkill"></param>
    /// <param name="SkillNumber"></param>
    /// <param name="isLearnSkill"></param>
    public void GetNewSkill(Skill NewSkill ,Skill OldSkill , int SkillNumber , bool isLearnSkill)
    {
        //给AP
        if (FloorNum.GlobalFloorNum != null && ScoreCounter.Instance != null)
        {
            ScoreCounter.Instance.SkillBounsAP += APBounsPoint.SkillBouns;
        }

        switch (SkillNumber)
        {
            case 1:
                Skill01 = NewSkill;
                skillBar01.GetSkill(Skill01);
                if (SkillPanel.StaticSkillPanel != null)
                {
                    SkillPanel.StaticSkillPanel.transform.GetChild(6).gameObject.SetActive(true);
                    SkillPanel.StaticSkillPanel.transform.GetChild(6).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill, this);
                }
                break;
            case 2:
                Skill02 = NewSkill;
                skillBar02.GetSkill(Skill02);
                if (SkillPanel.StaticSkillPanel != null) {
                    SkillPanel.StaticSkillPanel.transform.GetChild(7).gameObject.SetActive(true);
                    SkillPanel.StaticSkillPanel.transform.GetChild(7).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill, this);
                }
                break;
            case 3:
                Skill03 = NewSkill;
                skillBar03.GetSkill(Skill03);
                if (SkillPanel.StaticSkillPanel != null)
                {
                    SkillPanel.StaticSkillPanel.transform.GetChild(8).gameObject.SetActive(true);
                    SkillPanel.StaticSkillPanel.transform.GetChild(8).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill, this);
                }
                break;
            case 4:
                Skill04 = NewSkill;
                skillBar04.GetSkill(Skill04);
                if (SkillPanel.StaticSkillPanel != null)
                {
                    SkillPanel.StaticSkillPanel.transform.GetChild(9).gameObject.SetActive(true);
                    SkillPanel.StaticSkillPanel.transform.GetChild(9).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill, this);
                }
                break;
        }
        if (OldSkill != null)
        {
            if (OldSkill.isPPUP == true && OldSkill.PlusSkill != null && OldSkill.PlusSkill == NewSkill) { NewSkill.isPPUP = true; }
            OldSkill.isPPUP = false;
        }
        playerSkillList.RemoveSkillInList(NewSkill, OldSkill);
        if (!isEvolution && EvolutionSkill != null && (NewSkill.SkillIndex == EvolutionSkill.SkillIndex || NewSkill.SkillIndex == EvolutionSkill.SkillIndex+1) )
        {
            isCanEvolution = true;
        }
        if (!isEvolution && EvolutionSkill != null)
        {
            if ((Skill01 != null && (Skill01.SkillIndex == EvolutionSkill.SkillIndex || Skill01.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill02 != null && (Skill02.SkillIndex == EvolutionSkill.SkillIndex || Skill02.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill03 != null && (Skill03.SkillIndex == EvolutionSkill.SkillIndex || Skill03.SkillIndex == EvolutionSkill.SkillIndex + 1)) ||
            (Skill04 != null && (Skill04.SkillIndex == EvolutionSkill.SkillIndex || Skill04.SkillIndex == EvolutionSkill.SkillIndex + 1)))
            {
                isCanEvolution = true;

            }
            else if ((Skill01 != null && (Skill01.SkillIndex != EvolutionSkill.SkillIndex && Skill01.SkillIndex != EvolutionSkill.SkillIndex + 1)) &&
            (Skill02 != null && (Skill02.SkillIndex != EvolutionSkill.SkillIndex && Skill02.SkillIndex != EvolutionSkill.SkillIndex + 1)) &&
            (Skill03 != null && (Skill03.SkillIndex != EvolutionSkill.SkillIndex && Skill03.SkillIndex != EvolutionSkill.SkillIndex + 1)) &&
            (Skill04 != null && (Skill04.SkillIndex != EvolutionSkill.SkillIndex && Skill04.SkillIndex != EvolutionSkill.SkillIndex + 1)))
            {
                isCanEvolution = false;
            }
        }
        Debug.Log("isCanEvolution" + isCanEvolution);
    }


    /// <summary>
    /// 更换进化界面中的角色名字
    /// </summary>
    public void ChangeEvoPanelText()
    {
        EvoAnimaObj.GetComponent<EvolutionPS>().ChangeText(PlayerNameChinese, EvolutionPlayer.PlayerNameChinese);
    }


    /// <summary>
    /// 开始进化动画
    /// </summary>
    protected void EvolutionStart()
    {
        EvoAnimaObj = Instantiate(EvolutionAnimation, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        EvoAnimaObj.GetComponent<EvolutionPS>().Name = PlayerNameChinese;
        UISkillButton.Instance.isEscEnable = false;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
        UISkillButton.Instance.isEscEnable = false;
        animator.SetTrigger("Evolution");
        isEvolution = true;
        SpeedRemove01(0);
        PlayerFrozenRemove();
    }


    /// <summary>
    /// 进化完成
    /// </summary>
    public void Evolution()
    {
        RemoveSnowCloak();
        RemoveOblivious();
        PlayerControler e = Instantiate(EvolutionPlayer, transform.position, Quaternion.identity);
        animator.updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1;
        UISkillButton.Instance.isEscEnable = true;
        e.isSpaceItemCanBeUse = true;

        if (playerAbility == playerAbility01) { e.playerAbility = (e.playerAbility01 == PlayerAbilityList.无特性)? e.playerAbility02 : e.playerAbility01; }
        else if (playerAbility == playerAbility02) { e.playerAbility = (e.playerAbility02 == PlayerAbilityList.无特性) ? e.playerAbility01 : e.playerAbility02; }
        else if (playerAbility == playerAbilityDream) { e.playerAbility = e.playerAbilityDream; }
        else { e.playerAbility = playerAbility; }

        e.uIPanelGwtNewSkill = uIPanelGwtNewSkill;
        e.Skill01 = Skill01;
        e.Skill02 = Skill02;
        e.Skill03 = Skill03;
        e.Skill04 = Skill04;
        e.isSkill01ButtonDown = false;
        e.isSkill02ButtonDown = false;
        e.isSkill03ButtonDown = false;
        e.isSkill04ButtonDown = false;
        e.Ex = Ex;

        e.UpdataPassiveItemEvent += UpdataPassiveItemEvent;
        e.ClearThisRoomEvent += ClearThisRoomEvent;
        e.ComeInANewRoomEvent += ComeInANewRoomEvent;

        e.Money = Money;
        e.Stone = Stone;
        e.Money = Money;
        e.HeartScale = HeartScale;
        e.PPUp = PPUp;
        e.SeedofMastery = SeedofMastery;
        e.Level = Level;
        e.LevelForSkill = LevelForSkill;
        e.NowRoom = NowRoom;
        e.InANewRoom = InANewRoom;
        CopyState(e);
        /*
        UnityEditorInternal.ComponentUtility.CopyComponent(playerData); 
        UnityEditorInternal.ComponentUtility.PasteComponentValues(e.GetComponent<PlayerData>());
        UnityEditorInternal.ComponentUtility.CopyComponent(playerSubSkillList); 
        UnityEditorInternal.ComponentUtility.PasteComponentValues(e.GetComponent<SubSkillList>());
        */
        e.GetComponent<PlayerData>().CopyData(playerData);
        e.GetComponent<SubSkillList>().CopyList(playerSubSkillList);

        GameObject EBaby = e.transform.GetChild(5).gameObject;
        if (transform.GetChild(5).GetChild(0).childCount > 0)
        {
            foreach (Transform baby in transform.GetChild(5).GetChild(0))
            {
                Instantiate(baby, baby.transform.position, Quaternion.identity, EBaby.transform.GetChild(0));
            }
        }
        if (transform.GetChild(5).GetChild(1).childCount > 0)
        {
            foreach (Transform baby in transform.GetChild(5).GetChild(1))
            {
                Instantiate(baby, baby.transform.position, Quaternion.identity, EBaby.transform.GetChild(1));
            }
        }
        if (transform.GetChild(5).GetChild(2).childCount > 0)
        {
            foreach (Transform baby in transform.GetChild(5).GetChild(2))
            {
                Instantiate(baby, baby.transform.position, Quaternion.identity, EBaby.transform.GetChild(2));
            }
        }
        e.TeraTypeChange(PlayerTeraType);
        e.TeraTypeJORChange(PlayerTeraTypeJOR);
        


        e.spaceItem = spaceItem;
        if (e.spaceItem != null)
        {
            e.SpaceItemImage.color = new Color(1, 1, 1, 1);
            e.SpaceItemImage.sprite = spaceItem.GetComponent<SpaceItem>().UIImage;
        }
        else
        {
            e.SpaceItemImage.color = new Color(0, 0, 0, 0);
            e.SpaceItemImage.sprite = null;
        }

        e.NatureIndex = NatureIndex;
        e.Hp = (e.Level + 10 + (int)(((float)Level * e.HpPlayerPoint * 2) / 100.0f)) - (maxHp - Hp);

        e.SpeedRemove01(0);
        Destroy(gameObject);
        UISkillButton.Instance.isEscEnable = true;
    }

    //====================================================角色成长====================================================
















    //====================================================释放技能和技能冷却====================================================

    /// <summary>
    /// 添加副技能
    /// </summary>
    /// <param name="sub"></param>
    public void AddASubSkill(SubSkill sub)
    {
        if (!playerSubSkillList.SubSList.Contains(sub))
        {
            playerSubSkillList.SubSList.Add(sub);
        }
    }


    /// <summary>
    /// 移除副技能
    /// </summary>
    /// <param name="sub"></param>
    public void RemoveASubSkill(SubSkill sub)
    {
        Debug.Log(sub);
        if (playerSubSkillList.SubSList.Contains(sub))
        {
            playerSubSkillList.SubSList.Remove(sub);
        }
    }


    /// <summary>
    /// 刷新技能
    /// </summary>
    public void RefreshSkillCD()
    {
        if (isSkill01CD) { Skill01Timer = GetSkillIndexCD(1); skillBar01.CDPlus(GetSkillIndexCD(1)); }
        if (isSkill02CD) { Skill02Timer = GetSkillIndexCD(2); skillBar02.CDPlus(GetSkillIndexCD(2)); }
        if (isSkill03CD) { Skill03Timer = GetSkillIndexCD(3); skillBar03.CDPlus(GetSkillIndexCD(3)); }
        if (isSkill04CD) { Skill04Timer = GetSkillIndexCD(4); skillBar04.CDPlus(GetSkillIndexCD(4)); }
    }


    /// <summary>
    /// 获得某个技能的cd
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public float GetSkillCD(Skill s)
    {
        float Output = s.ColdDown;
        if (Skill01 != null && s.SkillIndex == Skill01.SkillIndex)
        {
            Output = GetSkillIndexCD(1);
        }
        if (Skill02 != null && s.SkillIndex == Skill02.SkillIndex)
        {
            Output = GetSkillIndexCD(2);
        }
        if (Skill03 != null && s.SkillIndex == Skill03.SkillIndex)
        {
            Output = GetSkillIndexCD(3);
        }
        if (Skill04 != null && s.SkillIndex == Skill04.SkillIndex)
        {
            Output = GetSkillIndexCD(4);
        }
        return Output;
    }


    /// <summary>
    /// 得到技能1,2,3,4的cd
    /// </summary>
    /// <param name="SkillIndex"></param>
    /// <returns></returns>
    public float GetSkillIndexCD(int SkillIndex)
    {
        float OutPut = 0.0f;
        float Skill01CDTime = (Skill01 == null) ? 0 : ((isParalysisDone ? 1.8f : 1.0f) * (Skill01.ColdDown * (Skill01.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)) * (((playerData.IsPassiveGetList[116]) && (Skill01.Damage == 0) && (Skill01.SpDamage == 0)) ?  0.5f : 1.0f));
        float Skill02CDTime = (Skill02 == null) ? 0 : ((isParalysisDone ? 1.8f : 1.0f) * (Skill02.ColdDown * (Skill02.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)) * (((playerData.IsPassiveGetList[116]) && (Skill02.Damage == 0) && (Skill02.SpDamage == 0)) ? 0.5f : 1.0f));
        float Skill03CDTime = (Skill03 == null) ? 0 : ((isParalysisDone ? 1.8f : 1.0f) * (Skill03.ColdDown * (Skill03.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)) * (((playerData.IsPassiveGetList[116]) && (Skill03.Damage == 0) && (Skill03.SpDamage == 0)) ? 0.5f : 1.0f));
        float Skill04CDTime = (Skill04 == null) ? 0 : ((isParalysisDone ? 1.8f : 1.0f) * (Skill04.ColdDown * (Skill04.isPPUP ? 0.625f : 1)) * (1 - ((float)SpeedAbilityPoint / 500)) * (((playerData.IsPassiveGetList[116]) && (Skill04.Damage == 0) && (Skill04.SpDamage == 0)) ? 0.5f : 1.0f));
        if (playerData.IsPassiveGetList[62])
        {
            OutPut = (Skill01CDTime + Skill02CDTime + Skill03CDTime + Skill04CDTime) / 4.0f;
        }
        else
        {
            switch (SkillIndex)
            {
                case 1:
                    OutPut = Skill01CDTime;
                    break;
                case 2:
                    OutPut = Skill02CDTime;
                    break;
                case 3:
                    OutPut = Skill03CDTime;
                    break;
                case 4:
                    OutPut = Skill04CDTime;
                    break;
            }
        }
        return OutPut;
    }


    /// <summary>
    /// 减少CD（int 减少第几号技能 ， float 减少的百分比（如果isTimeMode==true则减少固定时间） ，bool 是否为固定数量模式）
    /// </summary>
    /// <param name="SkillIndex"></param>
    /// <param name="MinusCDTimerPer"></param>
    /// <param name="isTimeMode"></param>
    public void MinusSkillCDTime(int SkillIndex, float MinusCDTimerPer, bool isTimeMode)
    {
        switch (SkillIndex)
        {
            case 1:
                Skill01Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(1) * MinusCDTimerPer);
                skillBar01.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(1) * MinusCDTimerPer));
                break;
            case 2:
                Skill02Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(2) * MinusCDTimerPer);
                skillBar02.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(2) * MinusCDTimerPer));
                break;
            case 3:
                Skill03Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(3) * MinusCDTimerPer);
                skillBar03.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(3) * MinusCDTimerPer));
                break;
            case 4:
                Skill04Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(4) * MinusCDTimerPer);
                skillBar04.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(4) * MinusCDTimerPer));
                break;
        }
    }

    /// <summary>
    /// 减少CD的重载（如果存在技能s，减少s技能的冷却 ， float 减少的百分比（如果isTimeMode==true则减少固定时间） ，bool 是否为固定数量模式）
    /// </summary>
    /// <param name="SkillIndex"></param>
    /// <param name="MinusCDTimerPer"></param>
    /// <param name="isTimeMode"></param>
    public void MinusSkillCDTime(Skill s, float MinusCDTimerPer, bool isTimeMode)
    {
        if (Skill01 != null && s.SkillIndex == Skill01.SkillIndex)
        {
            Skill01Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(1) * MinusCDTimerPer);
            skillBar01.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(1) * MinusCDTimerPer));
        }
        if (Skill02 != null && s.SkillIndex == Skill02.SkillIndex)
        {
            Skill02Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(2) * MinusCDTimerPer);
            skillBar02.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(2) * MinusCDTimerPer));
        }
        if (Skill03 != null && s.SkillIndex == Skill03.SkillIndex)
        {
            Skill03Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(3) * MinusCDTimerPer);
            skillBar03.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(3) * MinusCDTimerPer));
        }
        if (Skill04 != null && s.SkillIndex == Skill04.SkillIndex)
        {
            Skill04Timer += (isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(4) * MinusCDTimerPer);
            skillBar04.CDPlus((isTimeMode ? MinusCDTimerPer : GetSkillIndexCD(4) * MinusCDTimerPer));
        }
    }

    /// <summary>
    /// //声明一个函数，调用时结束表示正在使用技能，无法移动的状态
    /// </summary>
    public void SkillNow()
    {
        isSkill = false;
    }


    /// <summary>
    /// 小跟班跟随玩家射击
    /// </summary>
    void FollowBabyLunch(Vector2Int Dir)
    {
        for (int i = 0; i < FollowBaby.transform.childCount; i++)
        {
            FollowBaby b = FollowBaby.transform.GetChild(i).GetComponent<FollowBaby>();
            if (b != null)
            {
                b.FollowBabyShot(Dir);
            }
        }
    }


    /// <summary>
    /// 声明一个函数，当调用时发射有前摇技能
    /// </summary>
    public void GetAnimationSkill01Launch()
    {
        if (isSkill01lunch) {
            LaunchSkill01(look);
            isSkill01lunch = false;
        }
        if (isSkill02lunch)
        {
            LaunchSkill02(look);
            isSkill02lunch = false;
        }
        if (isSkill03lunch)
        {
            LaunchSkill03(look);
            isSkill03lunch = false;
        }
        if (isSkill04lunch)
        {
            LaunchSkill04(look);
            isSkill04lunch = false;
        }
    }


    /// <summary>
    /// 声明一个函数，当调用时发射无前摇类技能
    /// </summary>
    public void GetAnimationSkill02Launch()
    {
        if (isSkill01lunch && Skill01.isImmediately)
        {
            LaunchSkill01(look);
            isSkill01lunch = false;
        }
        if (isSkill02lunch && Skill02.isImmediately)
        {
            LaunchSkill02(look);
            isSkill02lunch = false;
        }
        if (isSkill03lunch && Skill03.isImmediately)
        {
            LaunchSkill03(look);
            isSkill03lunch = false;
        }
        if (isSkill04lunch && Skill04.isImmediately)
        {
            LaunchSkill04(look);
            isSkill04lunch = false;
        }
    }


    /// <summary>
    /// 声明一个函数，当调用此函数时根据招式方向生成技能1
    /// </summary>
    /// <param name="Direction"></param>
    protected void LaunchSkill01(Vector2 Direction)
    {
        Skill skillObj = null;
        FollowBabyLunch(new Vector2Int((int)Direction.x , (int)Direction.y)) ;
        if (!Skill01.isNotDirection) {
            if (Direction.Equals(new Vector2(1, 0))) {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill01.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 0), Skill01.isNotMoveWithPlayer ? null : transform);
            } else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill01.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 180), Skill01.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill01.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 90), Skill01.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill01.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 270), Skill01.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill01, rigidbody2D.position, Quaternion.identity, Skill01.isNotMoveWithPlayer ? null : transform);
        }
        playerSubSkillList.CallSubSkill(skillObj);
        skillObj.player = this;
    }


    /// <summary>
    /// 声明一个函数，当调用此函数时根据招式方向生成技能2
    /// </summary>
    /// <param name="Direction"></param>
    protected void LaunchSkill02(Vector2 Direction)
    {
        Skill skillObj = null;
        FollowBabyLunch(new Vector2Int((int)Direction.x, (int)Direction.y));
        if (!Skill02.isNotDirection) {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill02.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 0), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill02.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 180), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill02.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 90), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill02.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 270), Skill02.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill02, rigidbody2D.position, Quaternion.identity, Skill02.isNotMoveWithPlayer?null:transform);
        }
        playerSubSkillList.CallSubSkill(skillObj);
        skillObj.player = this;
    }


    /// <summary>
    /// 声明一个函数，当调用此函数时根据招式方向生成技能3
    /// </summary>
    /// <param name="Direction"></param>
    protected void LaunchSkill03(Vector2 Direction)
    {
        Skill skillObj = null;
        FollowBabyLunch(new Vector2Int((int)Direction.x, (int)Direction.y));
        if (!Skill03.isNotDirection)
        {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill03.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 0), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill03.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 180), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill03.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 90), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill03.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 270), Skill03.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill03, rigidbody2D.position, Quaternion.identity, Skill03.isNotMoveWithPlayer ? null : transform);
        }
        playerSubSkillList.CallSubSkill(skillObj);
        skillObj.player = this;
    }


    /// <summary>
    /// 声明一个函数，当调用此函数时根据招式方向生成技能4
    /// </summary>
    /// <param name="Direction"></param>
    protected void LaunchSkill04(Vector2 Direction)
    {
        Skill skillObj = null;
        FollowBabyLunch(new Vector2Int((int)Direction.x, (int)Direction.y));
        if (!Skill04.isNotDirection)
        {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill04.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 0), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill04.DirctionDistance) + (Direction * SkillOffsetforBodySize[1]), Quaternion.Euler(0, 0, 180), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill04.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 90), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * SkillOffsetforBodySize[0]) + (Direction * Skill04.DirctionDistance) + (Direction * SkillOffsetforBodySize[2]), Quaternion.Euler(0, 0, 270), Skill04.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill04, rigidbody2D.position, Quaternion.identity, Skill04.isNotMoveWithPlayer ? null : transform);
        }
        playerSubSkillList.CallSubSkill(skillObj);
        skillObj.player = this;
    }





    //====================================================释放技能和技能冷却====================================================










    //====================================================TP====================================================

    Vector3Int TpVector3;
    /// <summary>
    /// 输入开始tp
    /// </summary>
    /// <param name="TPVector3"></param>
    public void TP(Vector3Int TPVector3)
    {
        TpVector3 = TPVector3;
        animator.SetTrigger("TP");
        isTP = true;
        isTPMove = true;
    }

    /// <summary>
    /// 开始黑屏
    /// </summary>
    public void TPStart()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 1.15f;
    }

    /// <summary>
    /// 移动
    /// </summary>
    public void TPDoit()
    {
        Vector3 TPAlpha = new Vector3(0.0f , -2.0f , 0.0f);
        if (TpVector3 != new Vector3Int(0, 0, 0) && TpVector3 != MapCreater.StaticMap.PCRoomPoint && TpVector3 != MapCreater.StaticMap.StoreRoomPoint && MapCreater.StaticMap.RRoom.ContainsKey(TpVector3)) 
        {
            Debug.Log("TP");
            if (!MapCreater.StaticMap.RRoom[TpVector3].isBlockerIn[0] && MapCreater.StaticMap.RRoom.ContainsKey(TpVector3 + Vector3Int.up) ) {  TPAlpha = new Vector3(0.0f, 7.47f, 0.0f); }
            else if (!MapCreater.StaticMap.RRoom[TpVector3].isBlockerIn[1] && MapCreater.StaticMap.RRoom.ContainsKey(TpVector3 + Vector3Int.down)) {  TPAlpha = new Vector3(0.0f, -7.45f, 0.0f); }
            else if(!MapCreater.StaticMap.RRoom[TpVector3].isBlockerIn[2] && MapCreater.StaticMap.RRoom.ContainsKey(TpVector3 + Vector3Int.left)) {  TPAlpha = new Vector3(-12.74f, -0.4f, 0.0f); }
            else if(!MapCreater.StaticMap.RRoom[TpVector3].isBlockerIn[3] && MapCreater.StaticMap.RRoom.ContainsKey(TpVector3 + Vector3Int.right)) { TPAlpha = new Vector3(12.74f, -0.4f, 0.0f); }
        }

        gameObject.transform.position = new Vector3(30.0f * TpVector3.x + TPAlpha.x, 24.0f * TpVector3.y + TPAlpha.y, 0);
        GameObject Maincamera = GameObject.FindWithTag("MainCamera");
        Maincamera.transform.position = new Vector3(30.0f * TpVector3.x, 24.0f * TpVector3.y + 0.7f, -10);
        UiMiniMap.Instance.MiniMapMove(new Vector3(NowRoom.x - TpVector3.x, NowRoom.y - TpVector3.y, 0));
        MapCreater.StaticMap.RRoom[NowRoom].GetAllItem();
        NowRoom = TpVector3;
        InANewRoom = true;
        UiMiniMap.Instance.SeeMapOver();
    }

    /// <summary>
    /// 结束黑屏
    /// </summary>
    public void TPEnd()
    {
        isTP = false;
        isTPMove = false;
    }


    //====================================================TP====================================================



    public void InstanceNature(int NatureIndex)
    {
        switch (NatureIndex)
        {
            case 0:    break;
            case 1:  playerData.AtkBounsAlways++; playerData.DefBounsAlways--; break;
            case 2:  playerData.AtkBounsAlways++; playerData.SpABounsAlways--; break;
            case 3:  playerData.AtkBounsAlways++; playerData.SpDBounsAlways--; break;
            case 4:  playerData.AtkBounsAlways++; playerData.SpeBounsAlways--; break;

            case 5:  playerData.DefBounsAlways++; playerData.AtkBounsAlways--; break;
            case 6:    break;
            case 7:  playerData.DefBounsAlways++; playerData.SpABounsAlways--; break;
            case 8:  playerData.DefBounsAlways++; playerData.SpDBounsAlways--; break;
            case 9:  playerData.DefBounsAlways++; playerData.SpeBounsAlways--; break;

            case 10: playerData.SpABounsAlways++; playerData.AtkBounsAlways--; break;
            case 11: playerData.SpABounsAlways++; playerData.DefBounsAlways--; break;
            case 12:   break;
            case 13: playerData.SpABounsAlways++; playerData.SpDBounsAlways--; break;
            case 14: playerData.SpABounsAlways++; playerData.SpeBounsAlways--; break;

            case 15: playerData.SpDBounsAlways++; playerData.AtkBounsAlways--; break;
            case 16: playerData.SpDBounsAlways++; playerData.DefBounsAlways--; break;
            case 17: playerData.SpDBounsAlways++; playerData.SpABounsAlways--; break;
            case 18:   break;
            case 19: playerData.SpDBounsAlways++; playerData.SpeBounsAlways--; break;

            case 20: playerData.SpeBounsAlways++; playerData.AtkBounsAlways--; break;
            case 21: playerData.SpeBounsAlways++; playerData.DefBounsAlways--; break;
            case 22: playerData.SpeBounsAlways++; playerData.SpABounsAlways--; break;
            case 23: playerData.SpeBounsAlways++; playerData.SpDBounsAlways--; break;
            case 24:   break;
        }
    }

    /// <summary>
    /// 获得根据性格的能力增加项 0无增加项 1攻击 2防御 3特攻 4特防 5攻速
    /// </summary>
    /// <param name="NatureIndex"></param>
    /// <returns></returns>
    public int GetNatureUpAbllity(int NatureIndex)
    {
        switch (NatureIndex)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 1;
            case 3: return 1;
            case 4: return 1;

            case 5: return 2;
            case 6: return 0;
            case 7: return 2;
            case 8: return 2;
            case 9: return 2;

            case 10: return 3;
            case 11: return 3;
            case 12: return 0;
            case 13: return 3;
            case 14: return 3;

            case 15: return 4;
            case 16: return 4;
            case 17: return 4;
            case 18: return 0;
            case 19: return 4;

            case 20: return 5;
            case 21: return 5;
            case 22: return 5;
            case 23: return 5;
            case 24: return 0;
            default: return 0;
        }
    }

    /// <summary>
    /// 获得根据性格的能力减少项 0无增加项 1攻击 2防御 3特攻 4特防 5攻速
    /// </summary>
    /// <param name="NatureIndex"></param>
    /// <returns></returns>
    public int GetNatureDownAbllity(int NatureIndex)
    {
        switch (NatureIndex)
        {
            case 0: return 0;
            case 1: return 2;
            case 2: return 3;
            case 3: return 4;
            case 4: return 5;

            case 5: return 1;
            case 6: return 0;
            case 7: return 3;
            case 8: return 4;
            case 9: return 5;

            case 10: return 1;
            case 11: return 2;
            case 12: return 0;
            case 13: return 4;
            case 14: return 5;

            case 15: return 1;
            case 16: return 2;
            case 17: return 3;
            case 18: return 0;
            case 19: return 5;

            case 20: return 1;
            case 21: return 2;
            case 22: return 3;
            case 23: return 4;
            case 24: return 0;
            default: return 0;
        }
    }

    public void RemoveNature(int NatureIndex)
    {
        switch (NatureIndex)
        {
            case 0: break;
            case 1: playerData.AtkBounsAlways--; playerData.DefBounsAlways++; break;
            case 2: playerData.AtkBounsAlways--; playerData.SpABounsAlways++; break;
            case 3: playerData.AtkBounsAlways--; playerData.SpDBounsAlways++; break;
            case 4: playerData.AtkBounsAlways--; playerData.SpeBounsAlways++; break;

            case 5: playerData.DefBounsAlways--; playerData.AtkBounsAlways++; break;
            case 6: break;
            case 7: playerData.DefBounsAlways--; playerData.SpABounsAlways++; break;
            case 8: playerData.DefBounsAlways--; playerData.SpDBounsAlways++; break;
            case 9: playerData.DefBounsAlways--; playerData.SpeBounsAlways++; break;

            case 10: playerData.SpABounsAlways--; playerData.AtkBounsAlways++; break;
            case 11: playerData.SpABounsAlways--; playerData.DefBounsAlways++; break;
            case 12: break;
            case 13: playerData.SpABounsAlways--; playerData.SpDBounsAlways++; break;
            case 14: playerData.SpABounsAlways--; playerData.SpeBounsAlways++; break;

            case 15: playerData.SpDBounsAlways--; playerData.AtkBounsAlways++; break;
            case 16: playerData.SpDBounsAlways--; playerData.DefBounsAlways++; break;
            case 17: playerData.SpDBounsAlways--; playerData.SpABounsAlways++; break;
            case 18: break;
            case 19: playerData.SpDBounsAlways--; playerData.SpeBounsAlways++; break;

            case 20: playerData.SpeBounsAlways--; playerData.AtkBounsAlways++; break;
            case 21: playerData.SpeBounsAlways--; playerData.DefBounsAlways++; break;
            case 22: playerData.SpeBounsAlways--; playerData.SpABounsAlways++; break;
            case 23: playerData.SpeBounsAlways--; playerData.SpDBounsAlways++; break;
            case 24: break;
        }
    }

    //=========================随时间的受伤时间=====================

    public void UpdatePlayerChangeHP()
    {
        if (Weather.GlobalWeather.isHail) { PlayerHail(); }
        if (Weather.GlobalWeather.isSandstorm) { PlayerSandStorm(); }
    }

    //=========================冰雹伤害事件========================

    /// <summary>
    /// 玩家的冰雹计时器，每计时两秒中一次毒
    /// </summary>
    protected float PlayerHailTimer;
    /// <summary>
    /// 根据冰雹时间玩家掉血
    /// </summary>
    void PlayerHail()
    {
        if (PlayerType01 != 15 && PlayerType02 != 15 && PlayerTeraType != 15 && PlayerTeraTypeJOR != 15)
        {
            if (!playerData.IsPassiveGetList[122])
            {
                if (NowRoom != MapCreater.StaticMap.PCRoomPoint && NowRoom != MapCreater.StaticMap.StoreRoomPoint)
                {
                    PlayerHailTimer += Time.deltaTime;
                    if (PlayerHailTimer >= 2.4f)
                    {
                        PlayerHailTimer += Time.deltaTime;
                        if (Weather.GlobalWeather.isHailPlus) { Pokemon.PokemonHpChange(null, gameObject, Mathf.Clamp((((float)maxHp) / 20), 1, 16), 0, 0, Type.TypeEnum.IgnoreType); }
                        else { Pokemon.PokemonHpChange(null, gameObject, Mathf.Clamp((((float)maxHp) / 20), 1, 16), 0, 0, Type.TypeEnum.IgnoreType); }
                        KnockOutPoint = 0;
                        KnockOutDirection = Vector2.zero;
                        PlayerHailTimer = 0;
                    }
                }
            }
        }
    }
    //=========================冰雹伤害事件========================

    //=========================沙暴伤害事件========================

    /// <summary>
    /// 玩家的沙暴计时器，每计时两秒中一次毒
    /// </summary>
    protected float PlayerSandStormTimer;
    /// <summary>
    /// 根据沙暴时间玩家掉血
    /// </summary>
    void PlayerSandStorm()
    {
        if (PlayerType01 != 5 && PlayerType01 != 6 && PlayerType01 != 9 && PlayerType02 != 5 && PlayerType02 != 6 && PlayerType02 != 9 && PlayerTeraType != 5 && PlayerTeraType != 6 && PlayerTeraType != 9 && PlayerTeraTypeJOR != 5 && PlayerTeraTypeJOR != 6 && PlayerTeraTypeJOR != 9)
        {
            if (!playerData.IsPassiveGetList[122]) {
                if (NowRoom != MapCreater.StaticMap.PCRoomPoint && NowRoom != MapCreater.StaticMap.StoreRoomPoint) {
                    PlayerSandStormTimer += Time.deltaTime;
                    if (PlayerSandStormTimer >= 2.4f)
                    {
                        PlayerSandStormTimer += Time.deltaTime;
                        if (Weather.GlobalWeather.isSandstormPlus) { Pokemon.PokemonHpChange(null, gameObject, Mathf.Clamp((((float)maxHp) / 20), 1, 16), 0, 0, Type.TypeEnum.IgnoreType); }
                        else { Pokemon.PokemonHpChange(null, gameObject, Mathf.Clamp((((float)maxHp) / 20), 1, 16), 0, 0, Type.TypeEnum.IgnoreType); }
                        KnockOutPoint = 0;
                        KnockOutDirection = Vector2.zero;
                        PlayerSandStormTimer = 0;
                    }
                }
            }
        }
    }
    //=========================沙暴伤害事件========================


    // == 对外接口 == 
    public Vector2 GetSpeed()
    {
        return new Vector2(speed * horizontal, speed * vertical);
    }
    // == 对外接口 == 









    //===========================所有触发特性时使用的函数======================================


    //--雪隐
    public void TriggerSnowCloak()
    {
        CancelInvoke("RemoveSnowCloak");
        if (!isSnowCloakTrigger) {

            isSnowCloakTrigger = true;
            playerData.MoveSpeBounsJustOneRoom += 2;
            ReFreshAbllityPoint();

        }
        Invoke("RemoveSnowCloak", 3.0f);
    }

    void RemoveSnowCloak()
    {
        if (isSnowCloakTrigger) {
            isSnowCloakTrigger = false;
            playerData.MoveSpeBounsJustOneRoom -= 2;
            ReFreshAbllityPoint();
        }
    }




    //--迟钝
    public void TriggerOblivious()
    {
        if (!isObliviousTrigger)
        {
            isObliviousTrigger = true;
            Invoke("RemoveOblivious", 2.5f);
        }
    }

    void RemoveOblivious()
    {
        if (isObliviousTrigger)
        {
            isObliviousTrigger = false;
        }
    }
    


    //--叶子防守
    void TriggerLeafGuard()
    {
        if (playerAbility == PlayerAbilityList.叶子防守)
        {
            if (InGressCount.Count != 0 || Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
            {
                isLeafGuardTrigger = true;
            }
            else
            {
                isLeafGuardTrigger = false;
            }
        }
    }

    //在Update中触发的特性
    void UpdateAbility()
    {
        TriggerLeafGuard();
    }



    //===========================所有触发特性时使用的函数======================================








    //===========================所有触发被动道具时使用的函数======================================


    /// <summary>
    /// 根据被动道具每帧调用的事件
    /// </summary>
    void UpdatePasssive()
    {
        //道具064 皮卡丘发电机
        if (playerData.IsPassiveGetList[64])
        {
            if (!isCanNotMove && (horizontal != 0 || vertical != 0)) { playerData.AtkHardWorkJustOneRoom += Time.deltaTime * 0.25f; playerData.SpAHardWorkJustOneRoom += Time.deltaTime * 0.25f; }
            else { playerData.AtkHardWorkJustOneRoom = Mathf.Clamp(playerData.AtkHardWorkJustOneRoom - Time.deltaTime * 0.5f , 0 , 10000); playerData.SpAHardWorkJustOneRoom = Mathf.Clamp(playerData.SpAHardWorkJustOneRoom - Time.deltaTime * 0.5f, 0, 10000); }
            ReFreshAbllityPoint();
        }

        //道具096 冷静头脑
        if (playerData.IsPassiveGetList[96])
        {
            if (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus)
            {
                playerData.PassiveItemClamMind();
            }
        }
    }

    //===========================所有触发被动道具时使用的函数======================================

}
