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
    /// ��ɫ�����к�
    /// </summary>
    public int PlayerIndex;
    


    //=======================================��ɫ������===================================
    /// <summary>
    /// ��ɫ������
    /// </summary>
    public string PlayerNameChinese;
    /// <summary>
    /// ��ɫͷ��ͼ��
    /// </summary>
    public Sprite PlayerHead;
    /// <summary>
    /// ��ɫ���ǹ�
    /// </summary>
    public Sprite PlayerCandy;
    /// <summary>
    /// ��ɫ���ǹ�����
    /// </summary>
    public Sprite PlayerCandyHD;
    /// <summary>
    /// ��ɫ���� 0С���� 1������ 2������
    /// </summary>
    public int PlayerBodySize;
    /// <summary>
    /// ���ݽ�ɫ���ʹ�С�������ͷŵ�λ�ã�SkillOffsetforBodySize[0]Ϊ���ĵ�y��ƫ������SkillOffsetforBodySize[1]Ϊx��ƫ������SkillOffsetforBodySize[2]y��ƫ������
    /// </summary>
    public float[] SkillOffsetforBodySize;
    /// <summary>
    /// Ŀǰ��ɫ�Ľ�����
    /// </summary>
    public PlayerControler EvolutionPlayer;
    /// <summary>
    /// ��ǰ��ɫ�Ƿ���Խ���Ϊ��һ�׶�
    /// </summary>
    public bool isEvolution;
    /// <summary>
    /// һ������������ʾ�ñ������Ƿ������ڽ���
    /// </summary>
    public bool isNeedInherit;
    /// <summary>
    /// ������������Ч
    /// </summary>
    public GameObject EvolutionAnimation;
    protected GameObject EvoAnimaObj;
    /// <summary>
    /// ��ɫ�Ƿ���Խ������ж�
    /// </summary>
    /// <returns></returns>
    public delegate bool JudgeEvolution();
    public JudgeEvolution JudgeEvolutionForEachLevel;
    protected bool NotJudgeEvolution() { return false; }
    /// <summary>
    /// ����һ��2D����������Ի�ý�ɫ�ĸ������
    /// </summary>
    new Rigidbody2D rigidbody2D;
    /// <summary>
    /// ����������������ȡ�����������Ϣ
    /// </summary>
    public float PlayerMoveHorizontal { get { return horizontal; } }
    private float horizontal;
    public float PlayerMoveVertical { get { return vertical; } }
    private float vertical;
    /// <summary>
    /// ����һ��2D�����������Դ洢����Ķ�ά����
    /// </summary>
    Vector2 position;

    public delegate void ComeInANewRoom(PlayerControler player);
    public ComeInANewRoom ComeInANewRoomEvent;


    public delegate void ClearThisRoom(PlayerControler player);
    public ClearThisRoom ClearThisRoomEvent;

    public delegate void UpdataPassiveItem(PlayerControler player);
    public UpdataPassiveItem UpdataPassiveItemEvent;

    //����һ����ά������ʾ������������ұ�,��һ����ʾλ����,
    public Vector2 look = new Vector2(0, -1);
    Vector2 move;
    Vector2 Direction;

    //����һ�����ͱ����������ֵ��һ�����α�����������ֵ,�Լ�һ�����ͱ���������������ֵ��������������
    public int maxHp;
    public int Hp
    {
        get { return nowHp; }
        set { nowHp = value; }
    }
    int nowHp;

    //����һ�����α������ڽ�Ǯ,�Լ�һ�����ͱ����������ڽ�Ǯ��������������
    public int Money
    {
        get { return nowMoney; }
        set { nowMoney = value; }
    }
    int nowMoney;

    //����һ�����α�������ʯͷ,�Լ�һ�����ͱ�����������ʯͷ��������������
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

    public GameObject FloatingDamage;//����˺���ʾ

    public HardworkShow HardworkFloatingShow;

    List<HardworkShow> LastHardworkShow = new List<HardworkShow> { };


    //����һ�����α�ʾ��ǰ�ȼ������ֵ��һ�����α�����ʾ�ȼ����Լ�һ������ֵ��һ�����α������ھ���ֵ,�Լ�һ�����ͱ����������ھ���ֵ��������������
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


    //����һ�������ͱ��������޵�ʱ�䣬һ�������ͱ�����Ϊ�޵�ʱ���ʱ����һ�������ͱ����ж��Ƿ��޵�
    public float TimeInvincible;
    float InvincileTimer = 0.0f;
    public bool isInvincible = false;




    //����һ�������ͱ�������������ֵ��
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

    //�����ͱ�����ʾ����Ƿ񴥷�Z���� �������������������ٱ�����
    public bool isInZ
    {
        get { return isinz; }
        set { isinz = value; }
    }
    bool isinz = false;


    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ
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


    //������ҵ���������
    public int PlayerType01;
    public int PlayerType02;
    public int PlayerTeraType;
    public int PlayerTeraTypeJOR;

    //��ҵ��ĸ��츳����
    public Skill InitialSkill01;
    public Skill InitialSkill02;
    public Skill InitialSkill03;
    public Skill InitialSkill04;


    public Skill[] InitialSkillCandidateList;

    //��ɫ�������б�
    public enum PlayerAbilityList
    {
        ������ = 0,
        �ٶ� = 1,
        ѩ�� = 2,
        ��֬�� = 3,
        Ҷ�ӷ��� = 4,
        ��Ļ = 5,
        Ů�������� = 6,
        ���� = 7,
        ��Ӧ�� = 8,
        Σ��Ԥ֪ = 9,
        ����֮�� = 10,
        ����Ƥ�� = 11,
        ���� = 12,
        ��ʤ = 13,
        �㾻֮�� = 14,
        ����� = 15,
        ͬ�� = 16,
        ������ = 17,
        ħ���� = 18,

    }
    //��ǰ��ɫ���Ի������
    public PlayerAbilityList playerAbility01;
    public PlayerAbilityList playerAbility02;
    public PlayerAbilityList playerAbilityDream;

    //���������ɫĿǰ������
    public PlayerAbilityList PlayerAbility { get { return playerAbility; } set { playerAbility = value; } }    PlayerAbilityList playerAbility;




    //����һ����Ϸ���󣬱�ʾ��ҵļ���1,�Լ�����1����ȴ��ʱ���ͼ���1�Ƿ���ȴ,�Ƿ���ʹ�ü���
    public Skill Skill01;
    public float _Skill01Timer { get { return Skill01Timer; } set { Skill01Timer = value; } }
    float Skill01Timer = 0;
    public bool isSkill01CD = false;
    bool isSkill01lunch = false;
    bool isSkill = false;
    public SkillBar01 skillBar01;
    public bool IsSkill01ButtonDown { get { return isSkill01ButtonDown; } set { isSkill01ButtonDown = value; } }
    bool isSkill01ButtonDown;

    //����һ����Ϸ���󣬱�ʾ��ҵļ���1,�Լ�����1����ȴ��ʱ���ͼ���1�Ƿ���ȴ,�Ƿ���ʹ�ü���
    public Skill Skill02;
    public float _Skill02Timer { get { return Skill02Timer; } set { Skill02Timer = value; } }
    float Skill02Timer = 0;
    public bool isSkill02CD = false;
    bool isSkill02lunch = false;
    public SkillBar01 skillBar02;
    public bool IsSkill02ButtonDown { get { return isSkill02ButtonDown; } set { isSkill02ButtonDown = value; } }
    bool isSkill02ButtonDown;

    //����һ����Ϸ���󣬱�ʾ��ҵļ���1,�Լ�����1����ȴ��ʱ���ͼ���1�Ƿ���ȴ,�Ƿ���ʹ�ü���
    public Skill Skill03;
    public float _Skill03Timer { get { return Skill03Timer; } set { Skill03Timer = value; } }
    float Skill03Timer = 0;
    public bool isSkill03CD = false;
    bool isSkill03lunch = false;
    public SkillBar01 skillBar03;
    public bool IsSkill03ButtonDown { get { return isSkill03ButtonDown; } set { isSkill03ButtonDown = value; } }
    bool isSkill03ButtonDown;

    //����һ����Ϸ���󣬱�ʾ��ҵļ���1,�Լ�����1����ȴ��ʱ���ͼ���1�Ƿ���ȴ,�Ƿ���ʹ�ü���
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

    //�жϼ����Ƿ񱻷�ӡ
    public bool Is01imprison;
    public float imprisonTime01;
    public bool Is02imprison;
    public float imprisonTime02;
    public bool Is03imprison;
    public float imprisonTime03;
    public bool Is04imprison;
    public float imprisonTime04;


    /// <summary>
    /// �����ҵ�UI״̬��(״̬���·�)
    /// </summary>
    public PlayerUIState playerUIStateOther;

    //����һ�������ͱ��������������ɫѧϰ����ʽ�ĵȼ�,�Լ�һ�����α�����⵱ǰ�ȼ��Ƿ�ϰ�ü���
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


    //���ڸ�����ת״̬
    public bool isRapidSpin
    {
        get { return isRapidspin; }
        set { isRapidspin = value; }
    }
    bool isRapidspin = false;

    //���ڽӰ�״̬
    public bool isBatonPass
    {
        get { return isbatonPass; }
        set { isbatonPass = value; }
    }
    bool isbatonPass = false;

    //���ڵ��Ư����ͨ״̬
    public bool isMagnetRisePlus
    {
        get { return ismagnetRisePlus; }
        set { ismagnetRisePlus = value; }
    }
    bool ismagnetRisePlus = false;



    //���ڲݴ��� ��isInGress==0ʱ�����ڲ��� ÿ��һƬ����ײ+1
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
    /// ��ɫͼƬ�����λ�� ������Ծ֮���ָ�Ϊ��ֵ
    /// </summary>
    public Vector3 PlayerLocalPosition
    {
        get { return playerLocalPosition; }
        set { playerLocalPosition = value; }
    }
    Vector3 playerLocalPosition = Vector3.zero;


    /// <summary>
    /// ��ɫͼƬ��������� ������Ծ֮���ָ�Ϊ��ֵ
    /// </summary>
    public Vector3 PlayerLocalScal
    {
        get { return playerLocalScal; }
        set { playerLocalScal = value; }
    }
    Vector3 playerLocalScal = new Vector3(1.0f , 1.0f , 1.0f);




    //=================================��ʼ��=====================================

    /// <summary>
    /// ����һ�����ݵȼ����㵱ǰ�������ֵ�ĺ���
    /// </summary>
    /// <param name="level"></param>
    protected void MaxHpForLevel(int level)
    {
        maxHp = level + 10 + (int)(((float)level * HpPlayerPoint * 2) / 100.0f);
    }
    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    protected int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }


    //��ʼ����ҵı�Ҫ����
    /// <summary>
    /// ��ʼ��
    /// </summary>
    protected void Instance()
    {

        for (int i = -10; i < 10;i++)
        {
            Debug.Log(_mTool.AbllityChangeFunction(i,PlayerAbility == PlayerAbilityList.�㾻֮��));
        }
        
        DontDestroyOnLoad(this);
        //��ǰ�������ֵ����һ��ʱ���������ֵ
        //��ǰ����ֵ�����������ֵ
        //��ʼ����ǰѪ�������Ѫ��������UI
        playerData = GetComponent<PlayerData>();
        playerSkillList = GetComponent<PlayerSkillList>();
        playerSubSkillList = GetComponent<SubSkillList>();
        FollowBaby = transform.GetChild(5).GetChild(0).gameObject;
        NotFollowBaby = transform.GetChild(5).GetChild(1).gameObject;
        ButterflyManger = transform.GetChild(5).GetChild(2).GetComponent<PlayerButterflyManger>();
        
        //���Сɽ��ĸ�������Ͷ������
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

        if (PlayerAbility == PlayerAbilityList.�ٶ�) { TimeStateInvincible *= 2.5f; }
        if (PlayerAbility == PlayerAbilityList.����) { playerData.MoveSpwBounsAlways += 1; ReFreshAbllityPoint(); }
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


    //=================================��ʼ��=====================================








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
            //�������ⰴť
            {
                if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other) {
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill1"))) { isSkill01ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill2"))) { isSkill02ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill3"))) { isSkill03ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill4"))) { isSkill04ButtonDown = true; }
                    if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("UseItem"))) { isSpaceItemButtonDown = true; }
                }

            }

            //����ʱ���Ƿ�ֹͣ��ֹͣ������

            //��������
            {
                UpdateAbility();
            }
            
            //��ʱ���Ѫ���߸ı�״̬�ĺ���
            {
                if (isInGrassyTerrain) { PlayerGrassyTerrainHeal(); }
                UpdatePlayerChangeHP();
            }

            //ÿ֡��ȡһ��ʮ�ּ��İ�����Ϣ
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

            //���� ÿ֡���һ�ε�ǰ�Ƿ�Ϊ�޵�״̬������ǣ����ʱ����ʱ�������ʱ��ʱ��С��0�����Ϊ���޵�״̬
            {
                if (isInvincible)
                {
                    InvincileTimer -= Time.deltaTime;

                    //���޵�ʱ���ʱ�����е�ǰ0��15���ڱ�����
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

            //�쳣״̬CD
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

            //������ȴ
            {
                //�������1��cd�ڼ䣬cd��ʱ��ʱ�俪ʼ���ӣ�����ʱ������Ϊ�ɷ���״̬����ʱ������
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
                //�������2��cd�ڼ䣬cd��ʱ��ʱ�俪ʼ���ӣ�����ʱ������Ϊ�ɷ���״̬����ʱ������
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
                //�������3��cd�ڼ䣬cd��ʱ��ʱ�俪ʼ���ӣ�����ʱ������Ϊ�ɷ���״̬����ʱ������
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
                //�������1��cd�ڼ䣬cd��ʱ��ʱ�俪ʼ���ӣ�����ʱ������Ϊ�ɷ���״̬����ʱ������
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

            //ʹ�ü���
            {
                //������ս��
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
                                    //���������е���8֡ʱ�ᷢ�似��1��������1����CD
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
                                    //���������е���8֡ʱ�ᷢ�似��2��������2����CD
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
                                    //���������е���8֡ʱ�ᷢ�似��3��������3����CD
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
                                    //���������е���8֡ʱ�ᷢ�似��4��������4����CD
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
                    //������q��ʱ����skill01

                    if (isSkill01ButtonDown && isSkill01CD == false && Skill01 != null && !isSkill && !Is01imprison)
                    {
                        if ((Skill01.useSkillConditions(this)))
                        {
                            //���������е���8֡ʱ�ᷢ�似��1��������1����CD
                            animator.SetTrigger("Skill");
                            isSkill01CD = true;
                            isSkill = true;
                            isSkill01lunch = true;
                            skillBar01.isCDStart = true;
                        }
                    }


                    //������w��ʱ����skill02

                    if (isSkill02ButtonDown && isSkill02CD == false && Skill02 != null && !isSkill && !Is02imprison)
                    {
                        if ((Skill02.useSkillConditions(this)))
                        {
                            //���������е���8֡ʱ�ᷢ�似��2��������2����CD
                            animator.SetTrigger("Skill");
                            isSkill02CD = true;
                            isSkill = true;
                            isSkill02lunch = true;
                            skillBar02.isCDStart = true;
                        }
                    }


                    //������e��ʱ����skill03

                    if (isSkill03ButtonDown && isSkill03CD == false && Skill03 != null && !isSkill && !Is03imprison)
                    {
                        if ((Skill03.useSkillConditions(this)))
                        {
                            //���������е���8֡ʱ�ᷢ�似��3��������3����CD
                            animator.SetTrigger("Skill");
                            isSkill03CD = true;
                            isSkill = true;
                            isSkill03lunch = true;
                            skillBar03.isCDStart = true;
                        }
                    }


                    //������r��ʱ����skill04

                    if (isSkill04ButtonDown && isSkill04CD == false && Skill04 != null && !isSkill && !Is04imprison)
                    {

                        if ((Skill04.useSkillConditions(this)))
                        {
                            //���������е���8֡ʱ�ᷢ�似��4��������4����CD
                            animator.SetTrigger("Skill");
                            isSkill04CD = true;
                            isSkill = true;
                            isSkill04lunch = true;
                            skillBar04.isCDStart = true;
                        }
                    }
                }
            }

            //�����·���
            {
                if (InANewRoom == true)
                {
                    NewRoomTimer += Time.deltaTime;

                    RestoreStrengthAndTeraType();
                    if (ComeInANewRoomEvent != null && !isComeInANewRoomEvent)
                    {
                        if (playerAbility == PlayerAbilityList.ѩ��) { isSnowCloakTrigger = false; CancelInvoke("RemoveSnowCloak"); }
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

            //ʹ��һ���Ե���
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

            //������������Ч��
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
        //�������ⰴ������״̬
        {
            isSkill01ButtonDown = false;
            isSkill02ButtonDown = false;
            isSkill03ButtonDown = false;
            isSkill04ButtonDown = false;
            isSpaceItemButtonDown = false;
        }
    }



    //=========================��ݳ��ػ�Ѫ�¼�========================
    /// <summary>
    /// ��ɫ����ݳ��ػ�Ѫ��ʱ����ÿ��ʱ5s��ݳ��ػ�Ѫһ��
    /// </summary>
    protected float PlayerGrassyTerrainTimer;
    /// <summary>
    /// ������ݳ���ʱ����˻�Ѫ
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
    //=========================��ݳ��ػ�Ѫ�¼�========================




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
            // ���ݲ��� case
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
        //2D����position���ڸ������������,֮����position��xy�������ʮ�ּ�x�ٶ�x��λʱ�䣬����ø����λ�õ���position
        if (!isDie && !isSkill && !isTP && !isCanNotMove && !isPlayerFrozenDone)
        {
            position = rigidbody2D.position;
            position.x = position.x + horizontal * speed * Time.deltaTime;
            position.y = position.y + vertical * speed * Time.deltaTime;
            rigidbody2D.position = position;
            


            //λ�Ʊ���Ϊʮ�ּ�����ֵ
            move = new Vector2(horizontal, vertical);

            //��������λ��ʱ���Ըı䶯���������λ�ƶ�������ı�
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

















    //======================================�����͸ı�����ֵ====================================================

    /// <summary>
    /// ����ͷ��״̬
    /// </summary>
    public void EndureStart()
    {
        playerData.isEndure = true;
        playerUIState.StatePlus(8);
    }


    /// <summary>
    /// ��������ͷ��״̬
    /// </summary>
    public void EndureOver()
    {
        playerData.isEndure = false;
        playerUIState.StateDestory(8);

    }


    /// <summary>
    /// ����Ƿ��ڷ����״̬
    /// </summary>
    public bool isReflect;


    /// <summary>
    /// ����Ƿ��ڹ�ǽ״̬
    /// </summary>
    public bool isLightScreen;


    /// <summary>
    ///����һ���ı������ĺ���ChangeHp���ı�ĵ���ΪChangePoint�����ı����Ϊ��ʱ�����޵�ʱ�䣬���ı����Ϊ��ʱ�������޵�ʱ��
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

            //Ѫ������ʱ��Ѫ��UI�����ǰѪ����������Ѫ�������ĺ���
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
            //����޵н��������޵еĻ���Ϊ���޵�״̬���޵�ʱ���ʱ��ʱ������Ϊ�޵�ʱ��
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
                    //��AP
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
                //Ѫ������ʱ��Ѫ��UI�����ǰѪ����������Ѫ�������ĺ���



                HitEvent(ChangePoint , ChangePointSp , SkillType , Crit);

                //���������Ķ�������������
                animator.SetTrigger("Hit");
            }
        }
    }

    /// <summary>
    /// �ܵ��˺�ʱ�������¼�
    /// </summary>
    void HitEvent(float ChangePoint, float ChangePointSp, int SkillType, bool Crit = false)
    {

        Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;
        //����096 �侲ͷ��
        if (playerData.IsPassiveGetList[96] && enumVaue == Type.TypeEnum.Ice)
        {
            playerData.PassiveItemClamMind();
        }

        //����092 ���߶���
        if (playerData.IsPassiveGetList[92])
        {
            playerData.KingsShieldDone();
        }

        //����133 �Խ𻬰�
        if (playerData.IsPassiveGetList[133])
        {
            if (Random.Range(0.0f, 1.0f) > 0.5f) { ChangeMoney(-1); }
        }

        //����164 ����׷����ͨ
        if (Skill01 != null && Skill01.SkillIndex == 164 ) { MinusSkillCDTime(1 , 1 , false); }
        if (Skill02 != null && Skill02.SkillIndex == 164 ) { MinusSkillCDTime(2 , 1 , false); }
        if (Skill03 != null && Skill03.SkillIndex == 164 ) { MinusSkillCDTime(3 , 1 , false); }
        if (Skill04 != null && Skill04.SkillIndex == 164 ) { MinusSkillCDTime(4 , 1 , false); }

    }

    /// <summary>
    /// ��׼��Ѫ����ֵ�Ƕ��پ͵�����
    /// </summary>
    /// <param name="ChangePoint">�ı���</param>
    public void ChangeHp(int ChangePoint)
    {
        nowHp = Mathf.Clamp(nowHp + ChangePoint, 0, maxHp);
        if (ChangePoint > 0)
        {
            //Ѫ������ʱ��Ѫ��UI�����ǰѪ����������Ѫ�������ĺ���
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
        //��Ѫ���á�ħ���˺�����������ʾ
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
    /// ��ɫ����
    /// </summary>
    public void PlayerDie()
    {
        isDie = true; 
        animator.SetTrigger("Die");
        rigidbody2D.bodyType = RigidbodyType2D.Static;
    }


    bool isTimePunishDone;

    /// <summary>
    /// ��ɫ������ߺ�������UI
    /// </summary>
    public void CallDieMask()
    {
        if (isDie) {
            //����072 ���ʯ
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


    //======================================�����͸ı�����ֵ====================================================















    //======================================�ı���ҳ�����======================================

    /// <summary>
    /// ����һ���ı��Ǯ�ĺ���ChangeMoney
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeMoney(int ChangePoint)
    {

        int BrforeMoney = nowMoney;
        //�ı��Ǯ��������Ϊ99������Ϊ0��֮����UI���������Ǯ�ĸı�ֵ��������UI�ı��Ǯ���ĺ���
        nowMoney = Mathf.Clamp(nowMoney + ChangePoint + (playerData.IsPassiveGetList[10]?1:0), 0, 99);
        UIMoneyBar.Instance._Money += nowMoney - BrforeMoney;
        UIMoneyBar.Instance.MoneyChange();
    }


    /// <summary>
    /// ����һ���ı�ʯͷ�ĺ���
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeStone(int ChangePoint)
    {
        int BrforeStone = nowStone;

        //�ı�ʯͷ��������Ϊ99������Ϊ0��֮����UI���������Ǯ�ĸı�ֵ��������UI�ı��Ǯ���ĺ���
        nowStone = Mathf.Clamp(nowStone + ChangePoint, 0, 99);
        UIMoneyBar.Instance._Stone += nowStone - BrforeStone;
        UIMoneyBar.Instance.StoneChange();
    }




    /// <summary>
    /// ����һ���ı���֮��Ƭ�ĺ���
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeHearsScale(int ChangePoint)
    {

        //�ı���֮��Ƭ��������Ϊ99������Ϊ0��
        nowHeartScale = Mathf.Clamp(nowHeartScale + ChangePoint, 0, 99);
    }


    /// <summary>
    /// ����һ���ı�PPUP�ĺ���
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangePPUp(int ChangePoint)
    {

        //�ı�PP������������Ϊ99������Ϊ0��
        nowPPUp = Mathf.Clamp(nowPPUp + ChangePoint, 0, 99);
    }


    /// <summary>
    /// ����һ���ı侫ͨ���ӵĺ���
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeMSeed(int ChangePoint)
    {

        //�ı侫ͨ������������Ϊ99������Ϊ0��
        nowSeedofMastery = Mathf.Clamp(nowSeedofMastery + ChangePoint, 0, 99);
    }

    //======================================�ı���ҳ�����======================================













    //====================================================��ɫ�ɳ�====================================================



    /// <summary>
    /// �ı�Ŭ��ֵ
    /// </summary>
    /// <param name="HWP"></param>
    public void ChangeHPW(Vector2Int HWP)
    {
        if (HWP.y > 0) {
            float f = 0;

            //�Ը�ӳ�
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
    /// //����һ���ı侭��ֵ�Լ������ȼ��ĺ���ChangeEx
    /// </summary>
    /// <param name="ChangePoint"></param>
    public void ChangeEx(int ChangePoint)
    {

        //���ı�ľ���ֵΪ��ʱ���Ըı�
        if (ChangePoint > 0)
        {
            //�ı侭��ֵ
            nowEx = Mathf.Clamp((int)(nowEx + (ChangePoint * 1.3f * (playerData.IsPassiveGetList[12]?1.25:1) * (playerData.IsPassiveGetList[29] ? 1.25 : 1) )), 0, 80000);

            //�����ǰ����ֵС���������ֵ��������UI����仯���
            if (nowEx < maxEx)
            {
                UIExpBar.Instance.Per = (float)nowEx / (float)maxEx;
                UIExpBar.Instance.ExpUp();
            }
            //�����ǰ����ֵ���������ֵ�����ӵȼ������ٵ�ǰ����ֵ�����ı������ֵ����������������������UI    
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
    /// ˢ�½�ɫ����ֵ
    /// </summary>
    public void ReFreshAbllityPoint()
    {
        MaxHpForLevel(Level);


        //�·�����ǰ7���������Ϸ����󣬵�8���������Ϸ���һ����֮��ʼ˥��
        maxHp = (int)((maxHp + playerData.HPHardWorkAlways + playerData.HPHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.HPBounsAlways + playerData.HPBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        AtkAbility = (int)((AbilityForLevel(Level, AtkPlayerPoint) + playerData.AtkHardWorkAlways + playerData.AtkHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        SpAAbility = (int)((AbilityForLevel(Level, SpAPlayerPoint) + playerData.SpAHardWorkAlways + playerData.SpAHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpABounsAlways + playerData.SpABounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        DefAbility = (int)((AbilityForLevel(Level, DefPlayerPoint) + playerData.DefHardWorkAlways + playerData.DefHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.DefBounsAlways + playerData.DefBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        SpDAbility = (int)((AbilityForLevel(Level, SpdPlayerPoint) + playerData.SpDHardWorkAlways + playerData.SpDHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        SpeedAbility = (int)((AbilityForLevel(Level, SpeedPlayerPoint) + playerData.SpeHardWorkAlways + playerData.SpeHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));
        speed = Mathf.Clamp(!isSleepDone ? ((MoveSpePlayerPoint + playerData.MoveSpeHardWorkAlways + playerData.MoveSpeHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��)) : 1.0f, 1.0f, 10);
        LuckPoint = (int)((LuckPlayerPoint + playerData.LuckHardWorkAlways + playerData.LuckHardWorkJustOneRoom) * _mTool.AbllityChangeFunction(playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom, PlayerAbility == PlayerAbilityList.�㾻֮��));

        //�Ϸ�����˥����ָ������
        //maxHp = (int)((maxHp + playerData.HPHardWorkAlways + playerData.HPHardWorkJustOneRoom) * Mathf.Pow((((playerData.HPBounsAlways + playerData.HPBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮��  ? 1.1f : 1.2f) : 1.2f)), (playerData.HPBounsAlways + playerData.HPBounsJustOneRoom)));
        //AtkAbility = (int)((AbilityForLevel(Level, AtkPlayerPoint) + playerData.AtkHardWorkAlways + playerData.AtkHardWorkJustOneRoom) * Mathf.Pow((((playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom < 0)? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f )) , (playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom)));
        //SpAAbility = (int)((AbilityForLevel(Level, SpAPlayerPoint) + playerData.SpAHardWorkAlways + playerData.SpAHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpABounsAlways + playerData.SpABounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.SpABounsAlways + playerData.SpABounsJustOneRoom))); 
        //DefAbility = (int)((AbilityForLevel(Level, DefPlayerPoint) + playerData.DefHardWorkAlways + playerData.DefHardWorkJustOneRoom) * Mathf.Pow((((playerData.DefBounsAlways + playerData.DefBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.DefBounsAlways + playerData.DefBounsJustOneRoom))); 
        //SpDAbility = (int)((AbilityForLevel(Level, SpdPlayerPoint) + playerData.SpDHardWorkAlways + playerData.SpDHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom)));
        //SpeedAbility = (int)((AbilityForLevel(Level, SpeedPlayerPoint) + playerData.SpeHardWorkAlways + playerData.SpeHardWorkJustOneRoom) * Mathf.Pow((((playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom)));
        //speed = Mathf.Clamp(!isSleepDone?((MoveSpePlayerPoint + playerData.MoveSpeHardWorkAlways + playerData.MoveSpeHardWorkJustOneRoom) * Mathf.Pow((((playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom))):1.0f , 1.0f , 10);
        //LuckPoint = (int)((LuckPlayerPoint + playerData.LuckHardWorkAlways + playerData.LuckHardWorkJustOneRoom) * Mathf.Pow((((playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom < 0) ? (PlayerAbility == PlayerAbilityList.�㾻֮�� ? 1.1f : 1.2f) : 1.2f)), (playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom)));
        
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
    /// ������ҵ�״̬����ֵUI
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
    /// ���������ȼ�ѧϰ�¼���
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
    /// ��������ѧϰ�¼���
    /// </summary>
    /// <param name="LearnSkill"></param>
    public void LearnNewSkillByOtherWay( Skill LearnSkill )
    {
        uIPanelGwtNewSkill.NewSkillPanzelJump(LearnSkill , false);
    }


    /// <summary>
    /// ����¼���
    /// </summary>
    /// <param name="NewSkill"></param>
    /// <param name="OldSkill"></param>
    /// <param name="SkillNumber"></param>
    /// <param name="isLearnSkill"></param>
    public void GetNewSkill(Skill NewSkill ,Skill OldSkill , int SkillNumber , bool isLearnSkill)
    {
        //��AP
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
    /// �������������еĽ�ɫ����
    /// </summary>
    public void ChangeEvoPanelText()
    {
        EvoAnimaObj.GetComponent<EvolutionPS>().ChangeText(PlayerNameChinese, EvolutionPlayer.PlayerNameChinese);
    }


    /// <summary>
    /// ��ʼ��������
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
    /// �������
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

        if (playerAbility == playerAbility01) { e.playerAbility = (e.playerAbility01 == PlayerAbilityList.������)? e.playerAbility02 : e.playerAbility01; }
        else if (playerAbility == playerAbility02) { e.playerAbility = (e.playerAbility02 == PlayerAbilityList.������) ? e.playerAbility01 : e.playerAbility02; }
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

    //====================================================��ɫ�ɳ�====================================================
















    //====================================================�ͷż��ܺͼ�����ȴ====================================================

    /// <summary>
    /// ��Ӹ�����
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
    /// �Ƴ�������
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
    /// ˢ�¼���
    /// </summary>
    public void RefreshSkillCD()
    {
        if (isSkill01CD) { Skill01Timer = GetSkillIndexCD(1); skillBar01.CDPlus(GetSkillIndexCD(1)); }
        if (isSkill02CD) { Skill02Timer = GetSkillIndexCD(2); skillBar02.CDPlus(GetSkillIndexCD(2)); }
        if (isSkill03CD) { Skill03Timer = GetSkillIndexCD(3); skillBar03.CDPlus(GetSkillIndexCD(3)); }
        if (isSkill04CD) { Skill04Timer = GetSkillIndexCD(4); skillBar04.CDPlus(GetSkillIndexCD(4)); }
    }


    /// <summary>
    /// ���ĳ�����ܵ�cd
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
    /// �õ�����1,2,3,4��cd
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
    /// ����CD��int ���ٵڼ��ż��� �� float ���ٵİٷֱȣ����isTimeMode==true����ٹ̶�ʱ�䣩 ��bool �Ƿ�Ϊ�̶�����ģʽ��
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
    /// ����CD�����أ�������ڼ���s������s���ܵ���ȴ �� float ���ٵİٷֱȣ����isTimeMode==true����ٹ̶�ʱ�䣩 ��bool �Ƿ�Ϊ�̶�����ģʽ��
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
    /// //����һ������������ʱ������ʾ����ʹ�ü��ܣ��޷��ƶ���״̬
    /// </summary>
    public void SkillNow()
    {
        isSkill = false;
    }


    /// <summary>
    /// С�������������
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
    /// ����һ��������������ʱ������ǰҡ����
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
    /// ����һ��������������ʱ������ǰҡ�༼��
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
    /// ����һ�������������ô˺���ʱ������ʽ�������ɼ���1
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
    /// ����һ�������������ô˺���ʱ������ʽ�������ɼ���2
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
    /// ����һ�������������ô˺���ʱ������ʽ�������ɼ���3
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
    /// ����һ�������������ô˺���ʱ������ʽ�������ɼ���4
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





    //====================================================�ͷż��ܺͼ�����ȴ====================================================










    //====================================================TP====================================================

    Vector3Int TpVector3;
    /// <summary>
    /// ���뿪ʼtp
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
    /// ��ʼ����
    /// </summary>
    public void TPStart()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 1.15f;
    }

    /// <summary>
    /// �ƶ�
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
    /// ��������
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
    /// ��ø����Ը������������ 0�������� 1���� 2���� 3�ع� 4�ط� 5����
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
    /// ��ø����Ը������������ 0�������� 1���� 2���� 3�ع� 4�ط� 5����
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

    //=========================��ʱ�������ʱ��=====================

    public void UpdatePlayerChangeHP()
    {
        if (Weather.GlobalWeather.isHail) { PlayerHail(); }
        if (Weather.GlobalWeather.isSandstorm) { PlayerSandStorm(); }
    }

    //=========================�����˺��¼�========================

    /// <summary>
    /// ��ҵı�����ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float PlayerHailTimer;
    /// <summary>
    /// ���ݱ���ʱ����ҵ�Ѫ
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
    //=========================�����˺��¼�========================

    //=========================ɳ���˺��¼�========================

    /// <summary>
    /// ��ҵ�ɳ����ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float PlayerSandStormTimer;
    /// <summary>
    /// ����ɳ��ʱ����ҵ�Ѫ
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
    //=========================ɳ���˺��¼�========================


    // == ����ӿ� == 
    public Vector2 GetSpeed()
    {
        return new Vector2(speed * horizontal, speed * vertical);
    }
    // == ����ӿ� == 









    //===========================���д�������ʱʹ�õĺ���======================================


    //--ѩ��
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




    //--�ٶ�
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
    


    //--Ҷ�ӷ���
    void TriggerLeafGuard()
    {
        if (playerAbility == PlayerAbilityList.Ҷ�ӷ���)
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

    //��Update�д���������
    void UpdateAbility()
    {
        TriggerLeafGuard();
    }



    //===========================���д�������ʱʹ�õĺ���======================================








    //===========================���д�����������ʱʹ�õĺ���======================================


    /// <summary>
    /// ���ݱ�������ÿ֡���õ��¼�
    /// </summary>
    void UpdatePasssive()
    {
        //����064 Ƥ���𷢵��
        if (playerData.IsPassiveGetList[64])
        {
            if (!isCanNotMove && (horizontal != 0 || vertical != 0)) { playerData.AtkHardWorkJustOneRoom += Time.deltaTime * 0.25f; playerData.SpAHardWorkJustOneRoom += Time.deltaTime * 0.25f; }
            else { playerData.AtkHardWorkJustOneRoom = Mathf.Clamp(playerData.AtkHardWorkJustOneRoom - Time.deltaTime * 0.5f , 0 , 10000); playerData.SpAHardWorkJustOneRoom = Mathf.Clamp(playerData.SpAHardWorkJustOneRoom - Time.deltaTime * 0.5f, 0, 10000); }
            ReFreshAbllityPoint();
        }

        //����096 �侲ͷ��
        if (playerData.IsPassiveGetList[96])
        {
            if (Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus)
            {
                playerData.PassiveItemClamMind();
            }
        }
    }

    //===========================���д�����������ʱʹ�õĺ���======================================

}
