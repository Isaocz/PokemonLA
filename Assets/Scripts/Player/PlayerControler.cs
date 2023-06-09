using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : Pokemon
{
    // Start is called before the first frame update


    public string PlayerNameChinese;
    public Sprite PlayerHead;

    //声明一个目前角色的进化型,一个布尔变量表示该宝可梦是否来自于进化
    public PlayerControler EvolutionPlayer;
    public bool isEvolution;
    public bool isNeedInherit;
    public GameObject EvolutionAnimation;
    protected GameObject EvoAnimaObj;

    //声明一个2D刚体组件，以获得小山猪的刚体组件
    new Rigidbody2D rigidbody2D;

    //声明两个变量，获取按键信息
    private float horizontal;
    private float vertical;

    //声明一个2D向量变量，以存储刚体的二维坐标
    Vector2 position;

    public delegate void ComeInANewRoom(PlayerControler player);
    public ComeInANewRoom ComeInANewRoomEvent;

    public delegate void ClearThisRoom(PlayerControler player);
    public ClearThisRoom ClearThisRoomEvent;

    public delegate void UpdataPassiveItem(PlayerControler player);
    public UpdataPassiveItem UpdataPassiveItemEvent;

    //声明一个二维向量表示朝向最初朝向右边,另一个表示位移量,
    Vector2 look = new Vector2(0, -1);
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


    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill01;
    float Skill01Timer = 0;
    bool isSkill01CD = false;
    bool isSkill01lunch = false;
    bool isSkill = false;
    public SkillBar01 skillBar01;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill02;
    float Skill02Timer = 0;
    bool isSkill02CD = false;
    bool isSkill02lunch = false;
    public SkillBar01 skillBar02;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill03;
    float Skill03Timer = 0;
    bool isSkill03CD = false;
    bool isSkill03lunch = false;
    public SkillBar01 skillBar03;

    //声明一个游戏对象，表示玩家的技能1,以及技能1的冷却计时器和技能1是否冷却,是否在使用技能
    public Skill Skill04;
    float Skill04Timer = 0;
    bool isSkill04CD = false;
    bool isSkill04lunch = false;
    public SkillBar01 skillBar04;

    public GameObject spaceItem;
    public GameObject SpaceItemList;
    public Image SpaceItemImage;


    //声明一个数组型变量，用来储存角色学习新招式的等级,以及一个整形变量检测当前等级是否习得技能
    protected int[] GetSkillLevel;
    int levelChecker = 0;
    public List<Skill> SkillList = new List<Skill>();
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

    PlayerControler ThisPlayer;

    public int NatureIndex;

    bool isDie;

    //初始化玩家的必要函数
    protected void Instance()
    {
        //当前最大生命值等于一级时的最大生命值
        //当前生命值等于最大生命值
        //初始化当前血量和最大血量的文字UI
        playerData = GetComponent<PlayerData>();

        //获得小山猪的刚体组件和动画组件
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxEx = Exp[Level-1];

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
        UIExpBar.Instance.Leveltext.text = LevelForSkill.ToString();
        UIHeadIcon.StaticHeadIcon.ChangeHeadIcon(PlayerHead);
        ComeInANewRoomEvent += ChangeRoomBgm;

    }


    protected void InstanceNewSkillPanel()
    {
        uIPanelGwtNewSkill.PokemonNameChinese = PlayerNameChinese;
    }



    // Update is called once per frame
    protected void UpdatePlayer()
    {
        if (!isDie)
        {
            //每帧获取一次十字键的按键信息
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");


            //每帧检测一次当前是否为无敌状态，如果是，则计时器计时，如果计时器时间小于0，则变为不无敌状态
            if (isInvincible)
            {
                InvincileTimer -= Time.deltaTime;

                //在无敌时间计时器运行的前0。15秒内被击退
                if (InvincileTimer > TimeInvincible - 0.15f)
                {

                    Vector2 position = rigidbody2D.position;
                    position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * Time.deltaTime, NowRoom.x * 30 - 12, NowRoom.x * 30 + 12);
                    position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * Time.deltaTime, NowRoom.y * 24 - 7.3f, NowRoom.y * 24 + 7.3f);
                    rigidbody2D.position = position;
                }
                if (InvincileTimer <= 0)
                {
                    isInvincible = false;
                }
            }


            if (isStateInvincible)
            {
                StateInvincileTimer -= Time.deltaTime;
                if (StateInvincileTimer <= 0)
                {
                    isStateInvincible = false;
                }
            }



            //当按下q键时发射skill01

            if (Input.GetKeyDown(KeyCode.Q) && isSkill01CD == false && Skill01 != null && !isSleepDone && !isSkill)
            {
                //当动画进行到第8帧时会发射技能1，并技能1进入CD
                animator.SetTrigger("Skill");
                isSkill01CD = true;
                isSkill = true;
                isSkill01lunch = true;
                skillBar01.isCDStart = true;
            }
            //如果技能1在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
            if (isSkill01CD && !isSleepDone)
            {
                Skill01Timer += Time.deltaTime;
                if (Skill01Timer >= (isParalysisDone ? 1.8f : 1.0f) * Skill01.ColdDown * (1 - (SpeedAbilityPoint / 500)))
                {
                    isSkill01CD = false;
                    Skill01Timer = 0;
                }
            }

            //当按下w键时发射skill02

            if (Input.GetKeyDown(KeyCode.W) && isSkill02CD == false && Skill02 != null && !isSleepDone && !isSkill)
            {
                //当动画进行到第8帧时会发射技能2，并技能2进入CD
                animator.SetTrigger("Skill");
                isSkill02CD = true;
                isSkill = true;
                isSkill02lunch = true;
                skillBar02.isCDStart = true;
            }
            //如果技能2在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
            if (isSkill02CD && !isSleepDone)
            {
                Skill02Timer += Time.deltaTime;
                if (Skill02Timer >= (isParalysisDone ? 1.8f : 1.0f) * Skill02.ColdDown * (1 - (SpeedAbilityPoint / 500)))
                {
                    isSkill02CD = false;
                    Skill02Timer = 0;
                }
            }

            //当按下e键时发射skill03

            if (Input.GetKeyDown(KeyCode.E) && isSkill03CD == false && Skill03 != null && !isSleepDone && !isSkill)
            {
                //当动画进行到第8帧时会发射技能3，并技能3进入CD
                animator.SetTrigger("Skill");
                isSkill03CD = true;
                isSkill = true;
                isSkill03lunch = true;
                skillBar03.isCDStart = true;
            }
            //如果技能3在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
            if (isSkill03CD && !isSleepDone)
            {
                Skill03Timer += Time.deltaTime;
                if (Skill03Timer >= (isParalysisDone ? 1.8f : 1.0f) * Skill03.ColdDown * (1 - (SpeedAbilityPoint / 500)))
                {
                    isSkill03CD = false;
                    Skill03Timer = 0;
                }
            }

            //当按下r键时发射skill04

            if (Input.GetKeyDown(KeyCode.R) && isSkill04CD == false && Skill04 != null && !isSleepDone && !isSkill)
            {
                //当动画进行到第8帧时会发射技能4，并技能4进入CD
                animator.SetTrigger("Skill");
                isSkill04CD = true;
                isSkill = true;
                isSkill04lunch = true;
                skillBar04.isCDStart = true;
            }
            //如果技能1在cd期间，cd计时器时间开始增加，当计时器满变为可发射状态，计时器归零
            if (isSkill04CD && !isSleepDone)
            {
                Skill04Timer += Time.deltaTime;
                if (Skill04Timer >= (isParalysisDone ? 1.8f : 1.0f) * Skill04.ColdDown * (1 - (SpeedAbilityPoint / 500)))
                {
                    isSkill04CD = false;
                    Skill04Timer = 0;
                }
            }
            if (InANewRoom == true)
            {
                NewRoomTimer += Time.deltaTime;

                RestoreStrengthAndTeraType();
                if (ComeInANewRoomEvent != null && !isComeInANewRoomEvent)
                {
                    ComeInANewRoomEvent(this);
                    isComeInANewRoomEvent = true;
                }
                CheckStateInInputNewRoom();
                isSpaceItemCanBeUse = false;
                if (NewRoomTimer >= 1.2f)
                {
                    isToxicDoneInNewRoom = false;
                    isBornDoneInNewRoom = false;
                    InANewRoom = false;
                    isStrengthAndTeraTypeBeRestore = false;
                    isComeInANewRoomEvent = false;
                    NewRoomTimer = 0;
                    isSpaceItemCanBeUse = true;
                }
            }


            if (isSpaceItemCanBeUse && Input.GetKeyDown(KeyCode.Space) && spaceItem != null)
            {
                spaceitemUseUI.UIAnimationStart(spaceItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
                UseSpaceItem.UsedSpaceItem(this);
            }

            if (UpdataPassiveItemEvent != null)
            {
                UpdataPassiveItemEvent(ThisPlayer);
            }
        }
    }
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
            PlayerTeraTypeJOR = 0;
            playerData.RestoreJORSata();
            isStrengthAndTeraTypeBeRestore = true;
        }
    }




    void ChangeRoomBgm(PlayerControler player)
    {
        if(NowRoom == MapCreater.StaticMap.PCRoomPoint)
        {
            BackGroundMusic.StaticBGM.ChangeBGMToPC();
        }
        else if(NowRoom == MapCreater.StaticMap.StoreRoomPoint)
        {
            BackGroundMusic.StaticBGM.ChangeBGMToStore();
        }
        else if (NowRoom == MapCreater.StaticMap.BossRoomPoint)
        {
            if (MapCreater.StaticMap.RRoom[NowRoom].isClear == 0) { BackGroundMusic.StaticBGM.ChangeBGMToBossWin(); }
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
        if (!isDie && !isSkill && !isTP)
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
            animator.SetFloat("LookX", look.x);
            animator.SetFloat("LookY", look.y);
            animator.SetFloat("Speed", move.magnitude);
        }
    }

    public void ChangeEvoPanelText()
    {
        EvoAnimaObj.GetComponent<EvolutionPS>().ChangeText(PlayerNameChinese , EvolutionPlayer.PlayerNameChinese);
    }

    public void Evolution()
    {
        PlayerControler e =  Instantiate(EvolutionPlayer , transform.position , Quaternion.identity);
        animator.updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1;
        e.uIPanelGwtNewSkill = uIPanelGwtNewSkill;
        e.Skill01 = Skill01;
        e.Skill02 = Skill02;
        e.Skill03 = Skill03;
        e.Skill04 = Skill04;
        e.Ex = Ex;

        e.Money = Money;
        e.Stone = Stone;
        e.Money = Money;
        e.Level = Level;
        UnityEditorInternal.ComponentUtility.CopyComponent(playerData);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(e.GetComponent<PlayerData>());
        e.spaceItem = spaceItem;
        e.NatureIndex = NatureIndex;
        e.Hp = (e.Level + 10 + (int)(((float)Level * e.HpPlayerPoint * 2) / 100.0f)) - (maxHp - Hp);
        Destroy(gameObject);
    }


    public void RefreshSkillCD()
    {
        if (isSkill01CD) { Skill01Timer = (isParalysisDone ? 1.8f : 1.0f) * Skill01.ColdDown * (1 - (SpeedAbilityPoint / 500)); skillBar01.CDPlus((isParalysisDone ? 1.8f : 1.0f) * Skill01.ColdDown * (1 - (SpeedAbilityPoint / 500))); }
        if (isSkill02CD) { Skill02Timer = (isParalysisDone ? 1.8f : 1.0f) * Skill02.ColdDown * (1 - (SpeedAbilityPoint / 500)); skillBar02.CDPlus((isParalysisDone ? 1.8f : 1.0f) * Skill02.ColdDown * (1 - (SpeedAbilityPoint / 500))); }
        if (isSkill03CD) { Skill03Timer = (isParalysisDone ? 1.8f : 1.0f) * Skill03.ColdDown * (1 - (SpeedAbilityPoint / 500)); skillBar03.CDPlus((isParalysisDone ? 1.8f : 1.0f) * Skill03.ColdDown * (1 - (SpeedAbilityPoint / 500))); }
        if (isSkill04CD) { Skill04Timer = (isParalysisDone ? 1.8f : 1.0f) * Skill04.ColdDown * (1 - (SpeedAbilityPoint / 500)); skillBar04.CDPlus((isParalysisDone ? 1.8f : 1.0f) * Skill04.ColdDown * (1 - (SpeedAbilityPoint / 500))); }
    }


    //声明一个根据等级计算当前最大生命值的函数
    protected void MaxHpForLevel(int level)
    {
        maxHp = level + 10 + (int)(((float)level*HpPlayerPoint*2) / 100.0f);
    }

    protected int AbilityForLevel(int level , int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }




    public void EndureStart()
    {
        playerData.isEndure = true;
        playerUIState.StatePlus(8);
    }
    public void EndureOver()
    {
        playerData.isEndure = false;
        playerUIState.StateDestory(8);

    }

    //声明一个改变生命的函数ChangeHp，改变的点数为ChangePoint，当改变点数为负时触发无敌时间，当改变点数为正时不触发无敌时间
    public void ChangeHp(float ChangePoint , float ChangePointSp , int SkillType)
    {

        if (ChangePoint > 0 || ChangePointSp > 0)
        {
            
            nowHp = Mathf.Clamp(nowHp + (int)(ChangePoint+ChangePointSp), 0, maxHp);

            //血量上升时对血条UI输出当前血量，并调用血条上升的函数
            UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
            UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);
            UIHealthBar.Instance.ChangeHpUp();
        }
        else
        {
            //如果无敌结束，不无敌的话变为不无敌状态，无敌时间计时器时间设置为无敌时间
            if (isInvincible)
            {
                return;
            }
            else
            {
                if(SkillType != 19)
                {
                    nowHp = Mathf.Clamp(nowHp + (int)((ChangePoint / DefAbilityPoint + ChangePointSp / SpdAbilityPoint - 2) * (Type.TYPE[SkillType][PlayerType01] * Type.TYPE[SkillType][PlayerType02] * (PlayerTeraTypeJOR == 0 ? Type.TYPE[SkillType][PlayerTeraType] : Type.TYPE[SkillType][PlayerTeraTypeJOR]  ) )*((playerData.TypeDefAlways[SkillType] + playerData.TypeDefJustOneRoom[SkillType]) > 0 ? Mathf.Pow(1.2f, (playerData.TypeDefAlways[SkillType] + playerData.TypeDefJustOneRoom[SkillType])) : Mathf.Pow(0.8f, (playerData.TypeDefAlways[SkillType] + playerData.TypeDefJustOneRoom[SkillType])))), (nowHp > 1) ? (playerData.isEndure ? 1 : 0) : 0, maxHp);
                   
                }
                else
                {
                    nowHp = Mathf.Clamp(nowHp + Mathf.Clamp((int)ChangePoint ,-100000 , -1), (nowHp > 1)?(playerData.isEndure?1:0):0, maxHp);
                }
                isInvincible = true;
                InvincileTimer = TimeInvincible;

                if (isSleepDone) { SleepRemove(); }
                UIHealthBar.Instance.Per = (float)nowHp / (float)maxHp;
                UIHealthBar.Instance.ChangeHpDown();
                UIHealthBar.Instance.NowHpText.text = string.Format("{000}", nowHp);

                if(nowHp <= 0) { isDie = true; animator.SetTrigger("Die");rigidbody2D.bodyType = RigidbodyType2D.Static;}
                //血量上升时对血条UI输出当前血量，并调用血条上升的函数

                //输出被击打的动画管理器参数
                animator.SetTrigger("Hit");
            }
        }
    }





    public void CallDieMask()
    {
        TPMask.In.BlackTime = 0;
        TPMask.In.TPStart = true;
        TPMask.In.transform.GetChild(0).gameObject.SetActive(true);
        TPMask.In.transform.GetChild(0).GetComponent<DiePanel>().Die(PlayerNameChinese);
    }



    //声明一个改变金钱的函数ChangeMoney
    public void ChangeMoney(int ChangePoint)
    {

        //改变金钱数，上限为99，下限为0，之后向UI对象输出金钱的改变值，并调用UI改变金钱数的函数
        nowMoney = Mathf.Clamp(nowMoney + ChangePoint + (playerData.IsPassiveGetList[10]?1:0), 0, 99);
        UIMoneyBar.Instance._Money += ChangePoint + (playerData.IsPassiveGetList[10] ? 1 : 0);
        UIMoneyBar.Instance.MoneyChange();
        
    }

    public void ChangeStone(int ChangePoint)
    {

        //改变石头数，上限为99，下限为0，之后向UI对象输出金钱的改变值，并调用UI改变金钱数的函数
        nowStone = Mathf.Clamp(nowStone + ChangePoint, 0, 99);
        UIMoneyBar.Instance._Stone += ChangePoint;
        UIMoneyBar.Instance.StoneChange();

    }




    public void ChangeHPW(Vector2Int HWP)
    {
        switch (HWP.x) {
            case 1:
                playerData.HPHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[18] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
            case 2:
                playerData.AtkHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[19] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
            case 3:
                playerData.DefHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[20] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
            case 4:
                playerData.SpAHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[21] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
            case 5:
                playerData.SpDHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[22] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
            case 6:
                playerData.SpeHardWorkAlways += (HWP.y) * 0.25f + (playerData.IsPassiveGetList[24] ? 0.15f : 0) + (playerData.IsPassiveGetList[23] ? 0.4f : 0) + (playerData.IsPassiveGetList[29] ? 0.4f : 0);
                break;
        }

        float LuckHPDPlusPer = Random.Range(0.0f, 1.0f);
        if (!playerData.IsPassiveGetList[17])
        {
            if (LuckHPDPlusPer >= 0.85f && LuckHPDPlusPer <= 0.95f)
            {
                playerData.LuckHardWorkAlways += 0.5f;
            }
            else if (LuckHPDPlusPer > 0.95f && LuckHPDPlusPer <= 0.985f)
            {
                playerData.LuckHardWorkAlways += 0.75f;
            }
            else if (LuckHPDPlusPer > 0.985f && LuckHPDPlusPer <= 1.0f)
            {
                playerData.LuckHardWorkAlways += 1.25f;
            }
        }
        else
        {
            if (LuckHPDPlusPer >= 0.6f && LuckHPDPlusPer <= 0.86f)
            {
                playerData.LuckHardWorkAlways += 0.5f;
            }
            else if (LuckHPDPlusPer > 0.86f && LuckHPDPlusPer <= 0.96f)
            {
                playerData.LuckHardWorkAlways += 0.75f;
            }
            else if (LuckHPDPlusPer > 0.96f && LuckHPDPlusPer <= 1.0f)
            {
                playerData.LuckHardWorkAlways += 1.25f;
            }
        }

        ReFreshAbllityPoint();
    }





    //声明一个改变经验值的函数ChangeEx
    public void ChangeEx(int ChangePoint)
    {

        //当改变的经验值为正时可以改变
        if (ChangePoint > 0)
        {
            //改变经验值
            nowEx = Mathf.Clamp((int)(nowEx + (ChangePoint * (playerData.IsPassiveGetList[12]?1.25:1) * (playerData.IsPassiveGetList[29] ? 1.25 : 1) )), 0, 80000);

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

    public void ReFreshAbllityPoint()
    {
        MaxHpForLevel(Level);
        maxHp = (int)((maxHp + playerData.HPHardWorkAlways + playerData.HPHardWorkJustOneRoom) * Mathf.Pow(1.2f, (playerData.HPBounsAlways + playerData.HPBounsJustOneRoom)));
        AtkAbility = (int)((AbilityForLevel(Level, AtkPlayerPoint) + playerData.AtkHardWorkAlways + playerData.AtkHardWorkJustOneRoom) * Mathf.Pow(1.2f , (playerData.AtkBounsAlways + playerData.AtkBounsJustOneRoom)));
        SpAAbility = (int)((AbilityForLevel(Level, SpAPlayerPoint) + playerData.SpAHardWorkAlways + playerData.SpAHardWorkJustOneRoom) * Mathf.Pow(1.2f , (playerData.SpABounsAlways + playerData.SpABounsJustOneRoom))); 
        DefAbility = (int)((AbilityForLevel(Level, DefPlayerPoint) + playerData.DefHardWorkAlways + playerData.DefHardWorkJustOneRoom) * Mathf.Pow(1.2f , (playerData.DefBounsAlways + playerData.DefBounsJustOneRoom))); 
        SpDAbility = (int)((AbilityForLevel(Level, SpdPlayerPoint) + playerData.SpDHardWorkAlways + playerData.SpDHardWorkJustOneRoom) * Mathf.Pow(1.2f , (playerData.SpDBounsAlways + playerData.SpDBounsJustOneRoom)));
        SpeedAbility = (int)((AbilityForLevel(Level, SpeedPlayerPoint) + playerData.SpeHardWorkAlways + playerData.SpeHardWorkJustOneRoom) * Mathf.Pow(1.2f , (playerData.SpeBounsAlways + playerData.SpeBounsJustOneRoom)));
        speed = Mathf.Clamp(!isSleepDone?((MoveSpePlayerPoint + playerData.MoveSpeHardWorkAlways + playerData.MoveSpeHardWorkJustOneRoom) * Mathf.Pow(1.2f, (playerData.MoveSpwBounsAlways + playerData.MoveSpeBounsJustOneRoom))):0.5f , 0.2f , 10);
        LuckPoint = (int)((LuckPlayerPoint + playerData.LuckHardWorkAlways + playerData.LuckHardWorkJustOneRoom) * Mathf.Pow(1.2f, (playerData.LuckBounsAlways + playerData.LuckBounsJustOneRoom)));
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
        
        
    }




    public void LearnNewSkill()
    {
        if (LevelForSkill == GetSkillLevel[levelChecker])
        {
            if (levelChecker < SkillList.Count)
            {
                if (Skill01 == null || Skill02 == null || Skill03 == null || Skill04 == null)
                {
                    uIPanelGwtNewSkill.NewSkillPanzelJump(SkillList[levelChecker]);
                }
                else
                {
                    uIPanelGwtNewSkill.NewSkillPanzelJump(SkillList[levelChecker]);
                    Debug.Log(SkillList[levelChecker]);
                }
            }
            else { Debug.Log("Error : SkillList OverFlow"); }
            levelChecker++;
        }
    }



    public void GetNewSkill(Skill NewSkill , int SkillNumber)
    {
        switch (SkillNumber)
        {
            case 1:
                Skill01 = NewSkill;
                skillBar01.GetSkill(Skill01);
                SkillPanel.StaticSkillPanel.transform.GetChild(6).gameObject.SetActive(true);
                SkillPanel.StaticSkillPanel.transform.GetChild(6).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill);
                break;
            case 2:
                Skill02 = NewSkill;
                skillBar02.GetSkill(Skill02);
                SkillPanel.StaticSkillPanel.transform.GetChild(7).gameObject.SetActive(true);
                SkillPanel.StaticSkillPanel.transform.GetChild(7).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill);
                break;
            case 3:
                Skill03 = NewSkill;
                skillBar03.GetSkill(Skill03);
                SkillPanel.StaticSkillPanel.transform.GetChild(8).gameObject.SetActive(true);
                SkillPanel.StaticSkillPanel.transform.GetChild(8).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill);
                break;
            case 4:
                Skill04 = NewSkill;
                skillBar04.GetSkill(Skill04);
                SkillPanel.StaticSkillPanel.transform.GetChild(9).gameObject.SetActive(true);
                SkillPanel.StaticSkillPanel.transform.GetChild(9).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill);
                break;
        }
    }





    //声明一个函数，调用时结束表示正在使用技能，无法移动的状态
    public void SkillNow()
    {
        isSkill = false;
    }


    //声明一个函数，当调用时发射技能01
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

    //声明一个函数，当调用时发射无前摇类技能
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

    //声明一个函数，当调用此函数时根据招式方向生成技能1
    protected void LaunchSkill01(Vector2 Direction)
    {
        Skill skillObj = null;
        if (!Skill01.isNotDirection) {
            if (Direction.Equals(new Vector2(1, 0))) {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 0), Skill01.isNotMoveWithPlayer ? null : transform);
            } else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 180), Skill01.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 90), Skill01.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill01, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 270), Skill01.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill01, rigidbody2D.position, Quaternion.identity, Skill01.isNotMoveWithPlayer ? null : transform);
        }
        skillObj.player = this;
    }

    protected void LaunchSkill02(Vector2 Direction)
    {
        Skill skillObj = null;
        if (!Skill02.isNotDirection) {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 0), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 180), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 90), Skill02.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill02, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 270), Skill02.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill02, rigidbody2D.position, Quaternion.identity, Skill02.isNotMoveWithPlayer?null:transform);
        }
        skillObj.player = this;
    }

    protected void LaunchSkill03(Vector2 Direction)
    {
        Skill skillObj = null;
        if (!Skill03.isNotDirection)
        {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 0), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 180), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 90), Skill03.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill03, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 270), Skill03.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill03, rigidbody2D.position, Quaternion.identity, Skill03.isNotMoveWithPlayer ? null : transform);
        }
        skillObj.player = this;
    }

    protected void LaunchSkill04(Vector2 Direction)
    {
        Skill skillObj = null;
        if (!Skill04.isNotDirection)
        {
            if (Direction.Equals(new Vector2(1, 0)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 0), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(-1, 0)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 180), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, 1)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 90), Skill04.isNotMoveWithPlayer ? null : transform);
            }
            else if (Direction.Equals(new Vector2(0, -1)))
            {
                skillObj = Instantiate(Skill04, rigidbody2D.position + (Vector2.up * 0.4f) + (Direction * 1), Quaternion.Euler(0, 0, 270), Skill04.isNotMoveWithPlayer ? null : transform);
            }
        }
        else
        {
            skillObj = Instantiate(Skill04, rigidbody2D.position, Quaternion.identity, Skill04.isNotMoveWithPlayer ? null : transform);
        }
        skillObj.player = this;
    }

    Vector3Int TpVector3;
    public void TP(Vector3Int TPVector3)
    {
        TpVector3 = TPVector3;
        animator.SetTrigger("TP");
        isTP = true;
        isTPMove = true;
    }

    
    public void TPStart()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 1.15f;
    }
    public void TPDoit()
    {
        gameObject.transform.position = new Vector3(30.0f * TpVector3.x, 24.0f * TpVector3.y - 2.0f, 0);
        GameObject Maincamera = GameObject.FindWithTag("MainCamera");
        Maincamera.transform.position = new Vector3(30.0f * TpVector3.x, 24.0f * TpVector3.y + 0.7f, -10);
        UiMiniMap.Instance.MiniMapMove(new Vector3(NowRoom.x - TpVector3.x, NowRoom.y - TpVector3.y, 0));
        NowRoom = TpVector3;
        InANewRoom = true;
        UiMiniMap.Instance.SeeMapOver();
    }
    public void TPEnd()
    {
        isTP = false;
        isTPMove = false;
    }

    void InstanceNature(int NatureIndex)
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

}
