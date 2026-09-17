using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mew : Empty
{
    #region Inspector 配置

    [Header("第一、二阶段基础技能 1-6")]
    public GameObject StarScatterPref; // 技能1
    public GameObject StarShootPref; // 技能2
    public GameObject StampStarPref; // 技能3：旋转星光
    public GameObject StarChargePref; // 技能4：星之冲刺
    public GameObject StarSlashPref; // 技能5：星之刃
    public GameObject StarRingReturnPref; // 技能6：星之使徒（保留旧 Prefab 字段名）

    [Header("第二阶段强技能")]
    [Tooltip("强技能1：镜像星光。Prefab 根对象必须挂载 MirrorStarLight。")]
    public GameObject MirrorStarLightPref;
    [Tooltip("强技能2：天象时钟。Prefab 根对象必须挂载 CelestialClock。")]
    public GameObject CelestialClockPref;
    [Tooltip("强技能3：拟态群星。Prefab 根对象必须挂载 MimicStars。")]
    public GameObject MimicStarsPref;
    [Tooltip("强技能4：星辉波动。Prefab 根对象必须挂载 StarWave。")]
    public GameObject StarWavePref;

    [Header("第二阶段强技能限制圈")]
    [Tooltip("可留空；留空时优先复用 ArenaBoundaryPrefab，再不行则运行时创建。")]
    public MewArenaBoundary Phase2ArenaBoundaryPrefab;
    [Min(4f)] public float Phase2StrongArenaRadius = 18f;
    [Min(0f)] public float Phase2ArenaIntroDuration = 0.85f;
    [Min(0f)] public float Phase2ArenaFadeDuration = 0.65f;

    [Header("第二阶段技能节奏")]
    [Tooltip("二阶段基础技能传送消失等待，不属于技能自身前摇。")]
    [Min(0f)] public float Phase2TeleportOutTime = 0.42f;
    [Tooltip("传送到新位置后的显现等待，不属于技能自身前摇。")]
    [Min(0f)] public float Phase2TeleportInTime = 0.38f;
    [Min(0f)] public float Phase2BasicSkillInterval = 0.35f;
    [Min(0f)] public float Phase2StrongSkillInterval = 0.8f;

    [Header("第二阶段房间边界")]
    [Tooltip("以 mapCenter 为中心的房间半尺寸，替代旧版硬编码世界坐标。")]
    public Vector2 Phase2RoomHalfExtents = new Vector2(30f, 24f);
    [Min(0f)] public float Phase2RoomEdgePadding = 2f;


    [Header("阶段转场与通用特效")]
    public GameObject Phase2Mask;
    public GameObject TeleportEndPrefab;
    public GameObject EdgePar;

    [Header("第三阶段限制圈")]
    [Tooltip("可选。留空时会在运行时自动创建限制圈对象。")]
    public MewArenaBoundary ArenaBoundaryPrefab;
    [Min(1f)] public float ArenaRadius = 22f;
    [Min(1f)] public float FinalArenaRadius = 16f;
    [Min(0f)] public float ArenaIntroDuration = 1.25f;
    [Tooltip("限制圈只在终幕准备前存在；默认在前 45 秒从 22 缩到 16。")]
    [Min(0.1f)] public float ArenaShrinkDuration = 45f;

    [Header("第三阶段：星海归还")]
    [Tooltip("保留旧星尘幻想资源引用；新尾杀由 MewFinale 自动创建。")]
    public GameObject StardustFantasyPref;
    [Tooltip("普通 StarLight Prefab，必须带 BarrageProjectile 与 Rigidbody2D。可覆盖技能 Prefab 内的 projectilePrefab。")]
    public GameObject StardustProjectilePrefab;
    [HideInInspector] public float Phase3Duration = 42f; // Legacy scale; completion follows the skill sequence.
    [Tooltip("最后一幕的缩圈准备时长。42 秒预设使用 8 秒；第 38 秒解除限制圈。")]
    [Min(5f)] public float Phase3FinaleDuration = 8f;

    [Header("血条 UI 调整")]
    public GameObject timeBar1;
    public GameObject timeBar2;
    public GameObject timeBar3;
    public GameObject timeBar4;

    [Header("第二阶段紫色血条")]
    [Tooltip("只替换非空 Sprite。按照当前 UI 层级分别填入紫色框、底、缓降条和当前血量条。")]
    public Sprite Phase2Bar1;
    public Sprite Phase2Bar2;
    public Sprite Phase2Bar3;
    public Sprite Phase2Bar4;

    [Header("第三阶段时间条")]
    public Sprite TimeBar1;
    public Sprite TimeBar2;
    public Sprite TimeBar3;
    public Sprite TimeBar4;

    [Header("房间与掉落")]
    public GameObject MewBossRoomPrefab;
    public Vector3 MewBossRoomPosition = new Vector3(60f, 60f, 0f);
    public PokemonBall[] pbList;
    public PokemonBall OrdinaryRewardPrefab;
    public GameObject MaxPotionRewardPrefab;
    public GameObject FullHealRewardPrefab;

    public enum EncounterResult { NormalBattleFailed, FinalTrialFailed, FinalTrialCleared }
    public static Mew ActiveEncounter { get; private set; }
    public bool IsEnding => isDying;
    public float StarScaleForPlayer => player != null ? player.MewStarScale : 1f;
    private EncounterResult encounterResult;
    private Room encounterRoom;
    private Transform originalParent;
    private bool savedInvincible, savedMovementLock, savedItemLock, savedEscape;
    private bool phaseThreeInputCaptured;
    private bool protectionHeld;
    private GameObject temporaryBattleRoom;
    private AudioClip previousMusic;
    private float previousMusicVolume;
    private Rigidbody2D protectedPlayerBody;
    private RigidbodyConstraints2D savedPlayerConstraints;

    [Header("音频")]
    public BackGroundMusic bgmScript;

    [Header("第一阶段全局时间")]
    [Tooltip("技能前：传送消失动画等待。它不是技能前摇。")]
    [Min(0f)] public float Phase1TeleportOutTime = 0.5f;

    [Tooltip("技能前：到达新位置后的显现等待。随后才创建技能对象。它不是技能前摇。")]
    [Min(0f)] public float Phase1TeleportInTime = 0.5f;
    [Tooltip("技能完成后、下一次传送开始前的额外停顿。默认 0；传送本身已有淡出和淡入时间。")]
    [Min(0f)] public float Phase1ExtraInterval = 0f;

    [Header("运行与调试")]
    public int currentPhase = 1;
    public float teleportTime;
    public bool testing;
    [Range(1, 6)] public int Phase1TestingSkillIndex = 1;
    public PokemonType.TypeEnum SkillType;

    #endregion

    #region 运行时状态

    private Vector3 mapCenter;
    public Vector2 BattleCenter => mapCenter;
    private Bounds? phaseTwoFloorBounds;
    private bool turningPhase;

    // 第一阶段严格按 1 -> 6 顺序释放，并等待当前技能完整结束。
    private int phaseOneSkillIndex = 1;
    private bool phaseOneSkillRunning;
    private bool phaseOneEnvironmentCleared;
    private Coroutine phaseOneSkillRoutine;
    private MewBaseSkill activeBasicSkill;

    // 第二阶段：随机3次基础技能，然后顺序释放1次已配置的强技能。
    private Coroutine phaseTwoRoutine;
    private MewBaseSkill activePhaseTwoSkill;
    private MewArenaBoundary phaseTwoArenaBoundary;
    private readonly List<int> phaseTwoBasicBag = new List<int>(6);
    private int phaseTwoBasicBagCursor;
    private int phaseTwoStrongSkillIndex;

    private int playerHpinP3;
    private bool isPhase3;

    private bool isDying = false;
    private bool isFinal;
    private bool phaseThreeTimerRunning;
    private bool phaseThreeArenaReady;
    private bool phaseThreeFinaleReleased;
    private Coroutine phaseThreeRoutine;
    private MewFinale activePhaseThreeSkill;
    private bool phaseThreeTrialFinished;
    private bool phaseThreeTrialCleared;
    private MewArenaBoundary arenaBoundary;

    private GameObject Camera;
    private bool roomCreated = false;
    private Vector3Int GetnowRoom;
    private Vector3 GetPlayerPosition;
    private Vector3 GetMewPosition;
    private Vector3 GetCameraPostion;

    private CameraController cinemachineController;
    private CameraAdapt cameraAdapt;
    private GameObject AtkTarget;

    private float HpTimer = 60f;
    private float HpTiming = 60f;
    public float TrialRemainingSeconds => activePhaseThreeSkill != null ? activePhaseThreeSkill.RemainingSeconds : HpTiming;

    public static bool MewBossKilled = false;

    private Color[] colors =
    {
        new Color(0.7294118f, 0.7333333f, 0.6627451f, 1f), // Normal
        new Color(0.7333333f, 0.3372549f, 0.2666667f, 1f), // Fighting
        new Color(0.6588235f, 0.5647059f, 0.9333334f, 1f), // Flying
        new Color(0.6666667f, 0.3333333f, 0.6f, 1f), // Poison
        new Color(0.8588236f, 0.7568628f, 0.3921569f, 1f), // Ground
        new Color(0.7098039f, 0.6313726f, 0.227451f, 1f), // Rock
        new Color(0.6666667f, 0.7333333f, 0.1215686f, 1f), // Bug
        new Color(0.4431373f, 0.345098f, 0.6f, 1f), // Ghost
        new Color(0.6705883f, 0.6666667f, 0.7294118f, 1f), // Steel
        new Color(1f, 0.2666667f, 0.1294118f, 1f), // Fire
        new Color(0.2f, 0.6f, 0.9960785f, 1f), // Water
        new Color(0.4666667f, 0.8f, 0.3333333f, 1f), // Grass
        new Color(0.9725491f, 0.8156863f, 0.1882353f, 1f), // Electric
        new Color(0.9764706f, 0.3490196f, 0.5372549f, 1f), // Psychic
        new Color(0.5921569f, 0.8431373f, 0.8431373f, 1f), // Ice
        new Color(0.4470589f, 0.2313726f, 0.9764706f, 1f), // Dragon
        new Color(0.4470589f, 0.345098f, 0.2862745f, 1f), // Dark
        new Color(0.9333334f, 0.6078432f, 0.6784314f, 1f), // Fairy
    };

    #endregion

    #region Unity 生命周期

    private void Start()
    {
        bgmScript = BackGroundMusic.StaticBGM;

        InitializeBossStats();
        CacheComponents();
        CacheBattleSceneState();
        PrepareBattle();
        StartOverEvent();
        StartCoroutine(ClearEnvironmentAfterIntro());
    }

    private void Update()
    {
        ResetPlayer();
        if (!isDying && player != null && player.Hp <= 0)
            TryProtectPlayer(player);

        if (isBorn || isDying)
        {
            return;
        }

        if (currentPhase == 3)
        {
            UpdatePhaseThree();
        }
        else
        {
            UpdateNormalPhase();
        }

    }

    #endregion

    #region 初始化

    private void InitializeBossStats()
    {
        EmptyType01 = PokemonType.TypeEnum.Psychic;
        EmptyType02 = 0;

        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);

        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
    }

    private void CacheComponents()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cameraAdapt = FindObjectOfType<CameraAdapt>();
    }

    private void CacheBattleSceneState()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        mapCenter = transform.parent.position;
        GetnowRoom = player.NowRoom;
        GetMewPosition = transform.position;
        GetPlayerPosition = player.transform.position;
        GetCameraPostion = Camera.transform.position;
        originalParent = transform.parent;
        encounterRoom = transform.parent.parent.GetComponent<Room>();
        savedItemLock = player.CanNotUseSpaceItem;
        savedEscape = UISkillButton.Instance == null || UISkillButton.Instance.isEscEnable;
        if (bgmScript != null && bgmScript.BGM != null)
        {
            previousMusic = bgmScript.BGM.clip;
            previousMusicVolume = bgmScript.BGM.volume;
        }
        ActiveEncounter = this;

        transform.parent.parent.GetComponent<Room>().isClear += 1;
    }

    private void PrepareBattle()
    {
        ClearProjectile();
        phaseOneSkillIndex = 1;
        phaseOneSkillRunning = false;
        phaseOneEnvironmentCleared = false;
        phaseOneSkillRoutine = null;
        activeBasicSkill = null;

        phaseTwoRoutine = null;
        activePhaseTwoSkill = null;
        phaseTwoArenaBoundary = null;
        phaseTwoBasicBag.Clear();
        phaseTwoBasicBagCursor = 0;
        phaseTwoStrongSkillIndex = 0;

        phaseThreeRoutine = null;
        activePhaseThreeSkill = null;
        phaseThreeTimerRunning = false;
        phaseThreeArenaReady = false;
        phaseThreeFinaleReleased = false;
        isFinal = false;
        HpTimer = Mathf.Max(1f, MewFinale.GetTrialSeconds(this));
        HpTiming = HpTimer;
    }


    private IEnumerator ClearEnvironmentAfterIntro()
    {
        // 文档要求开始动画结束后再清除环境障碍物。
        while (isBorn && !isDying)
        {
            yield return null;
        }

        if (!isDying)
        {
            ClearEnvironmentObjects();
            // Destroy 会在帧末真正执行，等待一帧后再允许第一阶段开始施法。
            yield return null;
            phaseOneEnvironmentCleared = true;
        }
    }

    private void ClearEnvironmentObjects()
    {
        if (transform.parent == null || transform.parent.parent == null)
        {
            return;
        }

        Transform roomRoot = transform.parent.parent;
        Transform enviroment = roomRoot.Find("Enviroment");

        if (enviroment == null)
        {
            return;
        }

        // 必须倒序删除；正序删除会因为 childCount 变化而跳过一半对象。
        for (int i = enviroment.childCount - 1; i >= 0; i--)
        {
            Transform child = enviroment.GetChild(i);
            NormalGress grass = child.GetComponent<NormalGress>();

            if (child.CompareTag("Grass") && grass != null)
            {
                grass.GrassDie();
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }

    #endregion

    #region 每帧阶段控制

    private void UpdatePhaseThree()
    {
        if (!phaseThreeInputCaptured)
        {
            savedEscape = UISkillButton.Instance == null || UISkillButton.Instance.isEscEnable;
            savedItemLock = player != null && player.CanNotUseSpaceItem;
            phaseThreeInputCaptured = true;
        }
        Phase3();
        LockPhaseThreeHealing();

        if (phaseThreeTimerRunning)
        {
            if (activePhaseThreeSkill != null) HpTimer = activePhaseThreeSkill.TotalSeconds;
            HpTiming = activePhaseThreeSkill != null ? activePhaseThreeSkill.RemainingSeconds : phaseThreeTrialFinished ? 0f : HpTimer;
            float timeRatio = HpTimer > 0f
                ? Mathf.Clamp01(HpTiming / HpTimer)
                : 0f;

            EmptyHp = Mathf.RoundToInt(timeRatio * maxHP);
            uIHealth.Per = timeRatio;
            uIHealth.ChangeHpDown();
        }

        UISkillButton.Instance.isEscEnable = false;
        UpdateArenaBoundary();

        if (phaseThreeTimerRunning && phaseThreeTrialFinished && !isDying)
        {
            phaseThreeTimerRunning = false;
            BeginEncounterEnd(phaseThreeTrialCleared ? EncounterResult.FinalTrialCleared : EncounterResult.FinalTrialFailed);
        }

        if (player != null &&
            Vector3.Distance(player.transform.position, transform.position) > 200f)
        {
            Destroy(this);
        }
    }

    private void UpdateArenaBoundary()
    {
        if (!phaseThreeArenaReady)
        {
            return;
        }

        if (arenaBoundary == null)
        {
            CreateArenaBoundary();
        }

        if (arenaBoundary == null)
        {
            return;
        }

        // 圆心固定在三阶段房间中心，不能使用 transform.position，
        // 否则限制圈会在 Mew 传送时跟着移动。
        // 缩圈命令只在 Phase3Start 中发送一次；每帧重复 SetTargetRadius
        // 会让部分 MewArenaBoundary 实现不断重置插值计时。
        arenaBoundary.SetCenter(mapCenter);
    }

    private void CreateArenaBoundary()
    {
        if (arenaBoundary != null || player == null)
        {
            return;
        }

        if (ArenaBoundaryPrefab != null)
        {
            arenaBoundary = Instantiate(
                ArenaBoundaryPrefab,
                mapCenter,
                Quaternion.identity);
        }
        else
        {
            GameObject boundaryObject = new GameObject("Mew Arena Boundary");
            boundaryObject.transform.position = mapCenter;
            arenaBoundary = boundaryObject.AddComponent<MewArenaBoundary>();
        }

        arenaBoundary.SetContactEffect(EdgePar);
        arenaBoundary.Activate(
            mapCenter,
            player.transform,
            ArenaRadius,
            ArenaIntroDuration);
    }

    private void CloseArenaBoundary(float fadeDuration = 0.65f)
    {
        if (arenaBoundary == null)
        {
            return;
        }

        arenaBoundary.Deactivate(fadeDuration, true);
        arenaBoundary = null;
    }

    /// <summary>
    /// 由第三阶段终符在终幕开始时调用。
    /// 停止 UpdateArenaBoundary 重新创建限制圈并淡出边界。
    /// 终幕空间提示由技能圈本身承担，不再显示文字。
    /// </summary>
    public void ReleasePhaseThreeArenaBoundary(
        float fadeDuration = 0.35f)
    {
        phaseThreeFinaleReleased = true;
        phaseThreeArenaReady = false;
        isFinal = false;
        CloseArenaBoundary(Mathf.Max(0f, fadeDuration));
    }

    private void SetPhaseThreeInstruction(string message)
    {
        if (player == null || player.transform.childCount <= 2)
        {
            return;
        }

        Transform uiRoot = player.transform.GetChild(2);
        if (uiRoot.childCount <= 3)
        {
            return;
        }

        PlayerUIText playerText =
            uiRoot.GetChild(3).GetComponent<PlayerUIText>();
        if (playerText != null)
        {
            playerText.SetText(message);
        }
    }

    private void UpdateNormalPhase()
    {
        AtkTarget = FindAtkTarget(40f);
        UpdateEmptyChangeHP();
        StateMaterialChange();
        bgmScript.ChangeBGMToMew(currentPhase);

        if (turningPhase)
        {
            return;
        }

        switch (currentPhase)
        {
            case 1:
                UpdatePhaseOne();
                break;

            case 2:
                UpdatePhaseTwo();
                break;
        }
    }

    private void UpdatePhaseOne()
    {
        if (EmptyHp < maxHP / 2)
        {
            Invincible = true;
            player.isInvincible = true;

            CancelActiveBasicSkill();
            if (phaseOneSkillRoutine != null)
            {
                StopCoroutine(phaseOneSkillRoutine);
                phaseOneSkillRoutine = null;
            }
            phaseOneSkillRunning = false;
            StopAllCoroutines();

            if (!roomCreated && currentPhase == 1)
            {
                turningPhase = true;
                ClearStatusEffects();
                EmptyHp = maxHP;
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                uIHealth.Per = EmptyHp / maxHP;
                StartCoroutine(Phase2Start());
            }

            return;
        }

        if (!isEmptyFrozenDone &&
            !isSleepDone &&
            !isCanNotMoveWhenParalysis &&
            !isSilence)
        {
            Phase1();
        }
    }

    private void UpdatePhaseTwo()
    {
        if (EmptyHp <= 0 && currentPhase == 2)
        {
            StopPhaseTwoCombat();
            ClearStatusEffects();
            StopAllCoroutines();

            int phaseThreeRecovery = player.Hp < player.maxHp * 3 / 4
                ? player.maxHp / 4 : player.maxHp - player.Hp;
            // ChangeHp(0, 0, 0) enters the damage branch, not the healing branch.
            if (phaseThreeRecovery > 0) player.ChangeHp(phaseThreeRecovery, 0, 0);

            EmptyHp = maxHP;
            HpTimer = Mathf.Max(1f, MewFinale.GetTrialSeconds(this));
            HpTiming = HpTimer;
            phaseThreeTimerRunning = false;
            phaseThreeArenaReady = false;
            phaseThreeFinaleReleased = false;
            isFinal = false;
            uIHealth.Per = 1f;
            uIHealth.ChangeHpUp();
            currentPhase++;
            isPhase3 = true;
            return;
        }

        if (!isEmptyFrozenDone &&
            !isSleepDone &&
            !isCanNotMoveWhenParalysis &&
            !isSilence)
        {
            Phase2();
        }
    }


    #endregion

    #region 技能调度

    private void Phase1()
    {
        if (!phaseOneEnvironmentCleared ||
            phaseOneSkillRunning ||
            turningPhase ||
            currentPhase != 1)
        {
            return;
        }

        int nextSkillIndex = testing
            ? Mathf.Clamp(Phase1TestingSkillIndex, 1, 6)
            : phaseOneSkillIndex;

        phaseOneSkillRoutine = StartCoroutine(
            Phase1Skill(nextSkillIndex));
    }

    private void Phase2()
    {
        if (phaseTwoRoutine != null ||
            currentPhase != 2 ||
            turningPhase ||
            isDying)
        {
            return;
        }

        phaseTwoRoutine = StartCoroutine(PhaseTwoCombatLoop());
    }

    private void Phase3()
    {
        if (!isPhase3)
        {
            return;
        }

        Invincible = true;
        isPhase3 = false;
        playerHpinP3 = player != null ? player.Hp : 0;
        phaseThreeRoutine = StartCoroutine(Phase3Start());
    }

    private void LockPhaseThreeHealing()
    {
        if (player == null)
        {
            return;
        }

        if (player.Hp > playerHpinP3)
        {
            player.ChangeHp(playerHpinP3 - player.Hp);
        }
        else if (player.Hp < playerHpinP3)
        {
            playerHpinP3 = player.Hp;
        }
    }


    #endregion

    #region 第一、二阶段基础技能 1-6

    private MewBaseSkill UseBasicSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1:
                return SpawnBasicSkill<StarScatter>(StarScatterPref, skillIndex);
            case 2:
                return SpawnBasicSkill<StarShoot>(StarShootPref, skillIndex);
            case 3:
                return SpawnBasicSkill<StampStar>(StampStarPref, skillIndex);
            case 4:
                return SpawnBasicSkill<StarCharge>(StarChargePref, skillIndex);
            case 5:
                return SpawnBasicSkill<StarSlash>(StarSlashPref, skillIndex);
            case 6:
                return SpawnBasicSkill<StarRingReturn>(StarRingReturnPref, skillIndex);
            default:
                Debug.LogError("基础技能索引无效：" + skillIndex, this);
                return null;
        }
    }

    private T SpawnBasicSkill<T>(GameObject prefab, int skillIndex)
        where T : MewBaseSkill
    {
        if (prefab == null)
        {
            Debug.LogError(
                "技能 " + skillIndex + " 没有设置 Prefab。",
                this);
            return null;
        }

        GameObject skillObject = Instantiate(
            prefab,
            transform.position,
            Quaternion.identity);

        T skill = skillObject.GetComponent<T>();
        if (skill == null)
        {
            Debug.LogError(
                prefab.name + " 缺少 " + typeof(T).Name + " 组件。",
                skillObject);
            Destroy(skillObject);
            return null;
        }

        MewSkillContext context = new MewSkillContext(
            this,
            (MewSkillPhase)Mathf.Clamp(currentPhase, 1, 3),
            player != null ? player.transform : null,
            skillObject.transform,
            mapCenter,
            currentPhase >= 2 ? ArenaRadius : 0f,
            arenaBoundary != null ? arenaBoundary.transform : null);

        skill.Initialize(context, true);
        return skill;
    }

    private void CancelActiveBasicSkill()
    {
        if (activeBasicSkill != null && !activeBasicSkill.IsFinished)
        {
            activeBasicSkill.Cancel(true);
        }

        activeBasicSkill = null;
    }

    #endregion

    #region 传送与地图边界

    private Vector3 RamdomTeleport()
    {
        const int maximumAttempts = 150;

        for (int attempt = 0; attempt < maximumAttempts; attempt++)
        {
            Vector3 randomPosition;
            float minimumPlayerDistance;
            bool requireMapBounds;

            if (currentPhase == 1)
            {
                randomPosition = transform.parent.position + new Vector3(
                    Random.Range(-12f, 12f),
                    Random.Range(-7f, 7f),
                    0f);
                minimumPlayerDistance = 3f;
                requireMapBounds = false;
            }
            else if (currentPhase == 2)
            {
                Transform target = AtkTarget != null
                    ? AtkTarget.transform
                    : player.transform;

                Vector2 randomDirection = Random.insideUnitCircle;
                if (randomDirection.sqrMagnitude < 0.0001f)
                {
                    randomDirection = Vector2.right;
                }
                randomDirection.Normalize();

                randomPosition = target.position +
                    (Vector3)(randomDirection * Random.Range(7f, 10f));
                minimumPlayerDistance = 1.5f;
                requireMapBounds = true;
            }
            else
            {
                return transform.position;
            }

            if (IsTeleportPositionValid(
                    randomPosition,
                    minimumPlayerDistance,
                    requireMapBounds))
            {
                return randomPosition;
            }
        }

        if (currentPhase == 2)
        {
            // A strong skill may leave the caster outside. Never use that position as the fallback.
            for (float radius = 3f; radius <= 18f; radius += 2f)
                for (int i = 0; i < 24; i++)
                {
                    float angle = i * Mathf.PI / 12f;
                    Vector3 candidate = mapCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
                    if (IsTeleportPositionValid(candidate, 1.5f, true)) return candidate;
                }
            if (IsTeleportPositionValid(mapCenter, 0f, true)) return mapCenter;
            // Caller cancels the teleport instead of blindly accepting an invalid candidate.
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }
        Debug.LogWarning(
            "连续150次未找到安全传送点，本次保留当前位置。",
            this);
        return transform.position;
    }

    private bool IsTeleportPositionValid(
        Vector3 position,
        float minimumPlayerDistance,
        bool requireMapBounds)
    {
        if (requireMapBounds && !IsInMapBounds(position))
        {
            return false;
        }

        if (requireMapBounds)
        {
            // An empty point beyond a wall passes an overlap test. Also require a wall-free connection to the room center.
            foreach (RaycastHit2D hit in Physics2D.LinecastAll(mapCenter, position))
                if (hit.collider != null && (hit.collider.CompareTag("Room") || hit.collider.CompareTag("Enviroment"))) return false;
        }

        if (player != null &&
            Vector3.Distance(position, player.transform.position) <= minimumPlayerDistance)
        {
            return false;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1f);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            if (collider.CompareTag("Room") || collider.CompareTag("Enviroment"))
            {
                return false;
            }
        }

        return true;
    }
    private bool IsInMapBounds(Vector3 position)
    {
        if (phaseTwoFloorBounds.HasValue)
        {
            Bounds floor = phaseTwoFloorBounds.Value;
            float padding = Mathf.Max(1f, Phase2RoomEdgePadding);
            if (position.x < floor.min.x + padding || position.x > floor.max.x - padding ||
                position.y < floor.min.y + padding || position.y > floor.max.y - padding) return false;
        }
        // 二阶段房间可能放在任意世界坐标，因此不能继续使用旧版固定坐标。
        float halfX = Mathf.Max(
            1f,
            Mathf.Abs(Phase2RoomHalfExtents.x) - Phase2RoomEdgePadding);
        float halfY = Mathf.Max(
            1f,
            Mathf.Abs(Phase2RoomHalfExtents.y) - Phase2RoomEdgePadding);

        return Mathf.Abs(position.x - mapCenter.x) <= halfX &&
               Mathf.Abs(position.y - mapCenter.y) <= halfY;
    }

    #endregion

    #region 第一阶段
    private IEnumerator Phase1Skill(int skillIndex)
    {
        phaseOneSkillRunning = true;

        BeginTeleportAnimation();
        if (Phase1TeleportOutTime > 0f)
        {
            yield return new WaitForSeconds(Phase1TeleportOutTime);
        }

        // 文档要求：一阶段每次释放技能前随机传送到当前房间中的一点。
        transform.position = RamdomTeleport();
        SkillType = PokemonType.TypeEnum.Psychic;

        if (Phase1TeleportInTime > 0f)
        {
            yield return new WaitForSeconds(Phase1TeleportInTime);
        }

        TeleportEnd();
        activeBasicSkill = UseBasicSkill(skillIndex);

        if (activeBasicSkill != null)
        {
            yield return activeBasicSkill.WaitForCompletion();
        }

        activeBasicSkill = null;

        if (currentPhase == 1 && !turningPhase && Phase1ExtraInterval > 0f)
        {
            yield return new WaitForSeconds(Phase1ExtraInterval);
        }

        if (!testing)
        {
            phaseOneSkillIndex = skillIndex >= 6 ? 1 : skillIndex + 1;
        }

        phaseOneSkillRunning = false;
        phaseOneSkillRoutine = null;
    }
    #endregion

    #region 第二阶段

    #region 第二阶段循环与基础技能

    private IEnumerator PhaseTwoCombatLoop()
    {
        while (currentPhase == 2 && !turningPhase && !isDying)
        {
            RefillPhaseTwoBasicBag();

            // 文档逻辑：每轮随机释放3次基础技能。
            for (int i = 0; i < 3; i++)
            {
                while (IsPhaseTwoActionLocked() &&
                       currentPhase == 2 &&
                       !isDying)
                {
                    yield return null;
                }

                if (currentPhase != 2 || turningPhase || isDying)
                {
                    break;
                }

                int basicSkillIndex = DrawPhaseTwoBasicSkill();
                yield return CastPhaseTwoBasicSkill(basicSkillIndex);

                if (currentPhase == 2 && Phase2BasicSkillInterval > 0f)
                {
                    yield return new WaitForSeconds(Phase2BasicSkillInterval);
                }
            }

            if (currentPhase != 2 || turningPhase || isDying)
            {
                break;
            }

            // 强技能按 1 -> 2 -> 3 -> 4 顺序。未配置的 Prefab 会自动跳过。
            int strongSkillIndex = FindNextAvailableStrongSkillIndex();
            if (strongSkillIndex >= 0)
            {
                yield return CastPhaseTwoStrongSkill(strongSkillIndex);
                phaseTwoStrongSkillIndex = (strongSkillIndex + 1) % 4;
            }
            else
            {
                Debug.LogWarning(
                    "第二阶段没有配置任何强技能 Prefab；本轮只执行3次基础技能。",
                    this);
            }

            if (currentPhase == 2 && Phase2StrongSkillInterval > 0f)
            {
                yield return new WaitForSeconds(Phase2StrongSkillInterval);
            }
        }

        phaseTwoRoutine = null;
    }

    private bool IsPhaseTwoActionLocked()
    {
        return isEmptyFrozenDone ||
               isSleepDone ||
               isCanNotMoveWhenParalysis ||
               isSilence;
    }

    private void RefillPhaseTwoBasicBag()
    {
        phaseTwoBasicBag.Clear();
        for (int i = 1; i <= 6; i++)
        {
            phaseTwoBasicBag.Add(i);
        }

        // Fisher-Yates。每轮前三个技能互不重复。
        for (int i = phaseTwoBasicBag.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = phaseTwoBasicBag[i];
            phaseTwoBasicBag[i] = phaseTwoBasicBag[randomIndex];
            phaseTwoBasicBag[randomIndex] = temp;
        }

        phaseTwoBasicBagCursor = 0;
    }

    private int DrawPhaseTwoBasicSkill()
    {
        if (phaseTwoBasicBag.Count != 6 ||
            phaseTwoBasicBagCursor >= phaseTwoBasicBag.Count)
        {
            RefillPhaseTwoBasicBag();
        }

        return phaseTwoBasicBag[phaseTwoBasicBagCursor++];
    }

    private IEnumerator CastPhaseTwoBasicSkill(int skillIndex)
    {
        BeginTeleportAnimation();

        if (Phase2TeleportOutTime > 0f)
        {
            yield return new WaitForSeconds(Phase2TeleportOutTime);
        }

        Vector3 destination = RamdomTeleport();
        if (float.IsNaN(destination.x))
        {
            animator.ResetTrigger("Teleport");
            animator.Play("BossMewIdle", 0, 0f);
            yield break;
        }
        yield return MoveBossDuringTeleport(destination, teleportTime);

        SkillType = PokemonType.TypeEnum.Psychic;

        if (Phase2TeleportInTime > 0f)
        {
            yield return new WaitForSeconds(Phase2TeleportInTime);
        }

        TeleportEnd();

        activePhaseTwoSkill = UseBasicSkill(skillIndex);
        activeBasicSkill = activePhaseTwoSkill;

        if (activePhaseTwoSkill != null)
        {
            yield return activePhaseTwoSkill.WaitForCompletion();
        }

        activePhaseTwoSkill = null;
        activeBasicSkill = null;
    }

    private IEnumerator MoveBossDuringTeleport(
        Vector3 destination,
        float duration)
    {
        Vector3 startPosition = transform.position;
        duration = Mathf.Max(0f, duration);

        if (duration <= 0.001f)
        {
            transform.position = destination;
            yield break;
        }

        float timer = 0f;
        while (timer < duration && !isDying)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startPosition, destination, t);
            yield return null;
        }

        if (!isDying) transform.position = destination;
    }

    private void BeginTeleportAnimation()
    {
        // Hit -> Teleport has exit time/blending in the controller. Start the clip now
        // so all existing relocation delays measure the actual teleport animation.
        if (animator == null || isDying) return;
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Teleport");
        animator.Play("BossMewTeleport", 0, 0f);
    }
    public IEnumerator TeleportForSkill(Vector3 destination)
    {
        if (isDying) yield break;
        BeginTeleportAnimation();
        yield return new WaitForSeconds(Mathf.Max(0.5f, Phase2TeleportOutTime));
        if (isDying) yield break;
        transform.position = new Vector3(destination.x, destination.y, transform.position.z);
        if (rigidbody2D != null) rigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(Mathf.Max(0.38f, Phase2TeleportInTime));
        if (!isDying) TeleportEnd();
    }

    public IEnumerator ReturnFromSkill(Vector3 destination)
    {
        if (isDying) yield break;
        animator.ResetTrigger("Teleport");
        animator.Play("BossMewIdle", 0, 0f);
        if (rigidbody2D != null) rigidbody2D.velocity = Vector2.zero;
        yield return MoveBossDuringTeleport(destination, 0.7f);
    }
    public IEnumerator MoveToFinaleCenter(float duration)
    {
        if (rigidbody2D != null) rigidbody2D.velocity = Vector2.zero;
        animator.ResetTrigger("Teleport");
        animator.Play("BossMewIdle", 0, 0f);
        yield return MoveBossDuringTeleport(mapCenter, duration);
    }

    #endregion

    #region 第二阶段强技能（①镜像星光 ②天象时钟 ③拟态群星 ④星辉波动）

    private IEnumerator CastPhaseTwoStrongSkill(int strongSkillIndex)
    {
        GameObject prefab = GetPhaseTwoStrongSkillPrefab(strongSkillIndex);
        if (prefab == null)
        {
            yield break;
        }

        Invincible = true;
        SkillType = PokemonType.TypeEnum.Psychic;

        CreatePhaseTwoArenaBoundary();

        if (Phase2ArenaIntroDuration > 0f)
        {
            yield return new WaitForSeconds(Phase2ArenaIntroDuration);
        }

        TeleportEnd();
        activePhaseTwoSkill = SpawnPhaseTwoStrongSkill(
            prefab,
            strongSkillIndex);

        if (activePhaseTwoSkill != null)
        {
            yield return activePhaseTwoSkill.WaitForCompletion();
        }

        activePhaseTwoSkill = null;

        // 天象时钟会在最终3圈弹幕前主动关闭限制圈。
        // 其余强技能仍由这里统一关闭，避免重复等待淡出时间。
        bool boundaryStillActive = phaseTwoArenaBoundary != null;
        ClosePhaseTwoArenaBoundary();

        if (boundaryStillActive && Phase2ArenaFadeDuration > 0f)
        {
            yield return new WaitForSeconds(Phase2ArenaFadeDuration);
        }

        if (currentPhase == 2 && !turningPhase && !isDying)
        {
            Invincible = false;
        }
    }

    private MewBaseSkill SpawnPhaseTwoStrongSkill(
        GameObject prefab,
        int strongSkillIndex)
    {
        GameObject skillObject = Instantiate(
            prefab,
            transform.position,
            Quaternion.identity);

        MewBaseSkill skill = skillObject.GetComponent<MewBaseSkill>();
        if (skill == null)
        {
            Debug.LogError(
                prefab.name +
                " 缺少 MewBaseSkill 派生组件。强技能索引：" +
                (strongSkillIndex + 1),
                skillObject);
            Destroy(skillObject);
            return null;
        }

        MewSkillContext context = new MewSkillContext(
            this,
            MewSkillPhase.Phase2,
            player != null ? player.transform : null,
            skillObject.transform,
            mapCenter,
            Phase2StrongArenaRadius,
            phaseTwoArenaBoundary != null
                ? phaseTwoArenaBoundary.transform
                : null);

        skill.Initialize(context, true);
        return skill;
    }

    private int FindNextAvailableStrongSkillIndex()
    {
        for (int offset = 0; offset < 4; offset++)
        {
            int index = (phaseTwoStrongSkillIndex + offset) % 4;
            if (GetPhaseTwoStrongSkillPrefab(index) != null)
            {
                return index;
            }
        }

        return -1;
    }

    private GameObject GetPhaseTwoStrongSkillPrefab(int index)
    {
        switch (index)
        {
            case 0:
                return MirrorStarLightPref;
            case 1:
                return CelestialClockPref;
            case 2:
                return MimicStarsPref;
            case 3:
                return StarWavePref;
            default:
                return null;
        }
    }

    private void CreatePhaseTwoArenaBoundary()
    {
        if (phaseTwoArenaBoundary != null || player == null)
        {
            return;
        }

        MewArenaBoundary prefab = Phase2ArenaBoundaryPrefab != null
            ? Phase2ArenaBoundaryPrefab
            : ArenaBoundaryPrefab;

        if (prefab != null)
        {
            phaseTwoArenaBoundary = Instantiate(
                prefab,
                mapCenter,
                Quaternion.identity);
        }
        else
        {
            GameObject boundaryObject =
                new GameObject("Mew Phase 2 Arena Boundary");
            boundaryObject.transform.position = mapCenter;
            phaseTwoArenaBoundary =
                boundaryObject.AddComponent<MewArenaBoundary>();
        }

        phaseTwoArenaBoundary.SetContactEffect(EdgePar);
        phaseTwoArenaBoundary.Activate(
            mapCenter,
            player.transform,
            Phase2StrongArenaRadius,
            Phase2ArenaIntroDuration);
    }

    /// <summary>
    /// 允许强技能在自身流程中提前关闭二阶段限制圈。
    /// 天象时钟会在旋转一圈后调用，并在限制圈淡出后释放最终3圈弹幕。
    /// </summary>
    public void ReleasePhaseTwoArenaBoundary(float fadeDuration = -1f)
    {
        ClosePhaseTwoArenaBoundary(fadeDuration);
    }

    private void ClosePhaseTwoArenaBoundary(float fadeDuration = -1f)
    {
        if (phaseTwoArenaBoundary == null)
        {
            return;
        }

        float usedFadeDuration = fadeDuration >= 0f
            ? fadeDuration
            : Phase2ArenaFadeDuration;

        phaseTwoArenaBoundary.Deactivate(
            usedFadeDuration,
            true);
        phaseTwoArenaBoundary = null;
    }

    #endregion

    #region 第二阶段清理与转场

    private void StopPhaseTwoCombat()
    {
        if (activePhaseTwoSkill != null &&
            !activePhaseTwoSkill.IsFinished)
        {
            activePhaseTwoSkill.Cancel(true);
        }

        activePhaseTwoSkill = null;
        activeBasicSkill = null;

        if (phaseTwoRoutine != null)
        {
            StopCoroutine(phaseTwoRoutine);
            phaseTwoRoutine = null;
        }

        ClosePhaseTwoArenaBoundary();
    }

    private void ResetPhaseTwoCombatState()
    {
        phaseTwoRoutine = null;
        activePhaseTwoSkill = null;
        activeBasicSkill = null;
        phaseTwoBasicBag.Clear();
        phaseTwoBasicBagCursor = 0;
        phaseTwoStrongSkillIndex = 0;
        ClosePhaseTwoArenaBoundary();
    }

    private IEnumerator Phase2Start()
    {
        // 清除弹幕并让旧红色血条淡出。
        uIHealth.ChangeHpUp();
        ClearProjectile();
        MirrorStarLight mirrorTemplate = MirrorStarLightPref != null ? MirrorStarLightPref.GetComponent<MirrorStarLight>() : null;
        if (mirrorTemplate != null) StartCoroutine(MewStarPool.Prewarm(this, mirrorTemplate.ProjectileTemplate, 256));

        // 保留一秒阶段切换停顿。旧版此处只计算五角星坐标，
        // 没有生成任何对象，属于无效遗留逻辑，现已删除。
        yield return new WaitForSeconds(1f);

        GameObject phase2mask = Instantiate(Phase2Mask, transform.position, Quaternion.identity);
        Destroy(phase2mask, 2.2f);
        yield return new WaitForSeconds(1.1f);

        //创建新的房间
        GameObject newRoom = Instantiate(MewBossRoomPrefab, MewBossRoomPosition, Quaternion.identity);
        temporaryBattleRoom = newRoom;
        Transform floor = newRoom.transform.Find("Floor");
        phaseTwoFloorBounds = null;
        if (floor != null)
            foreach (Renderer renderer in floor.GetComponentsInChildren<Renderer>())
            {
                Bounds bounds = phaseTwoFloorBounds ?? renderer.bounds;
                bounds.Encapsulate(renderer.bounds);
                phaseTwoFloorBounds = bounds;
            }
        mapCenter = MewBossRoomPosition;
        transform.position = MewBossRoomPosition + new Vector3(0f, 5f, 0f);
        player.transform.position = MewBossRoomPosition;
        transform.parent.parent.GetComponent<Room>().isClear = 0;
        player.NowRoom = new Vector3Int(100, 100, 0);
        player.InANewRoom = true;
        player.NewRoomTimer = 0f;
        player.isInvincible = false;
        currentPhase++;
        roomCreated = true;
        ResetPhaseTwoCombatState();

        // 二阶段血条由红色切换为紫色。
        ApplyPhase2HealthBarStyle();
        uIHealth.Per = 1f;
        uIHealth.ChangeHpUp();

        MapCreater.StaticMap.RRoom[new Vector3Int(100, 100, 0)] = newRoom.GetComponent<Room>();

        Transform mewTransform = newRoom.transform.Find("Empty");
        if (mewTransform != null)
        {
            //将梦幻移动到Empty子对象下
            transform.SetParent(mewTransform);
        }
        //色相头，启动！
        cameraAdapt.ActivateVcam();
        cinemachineController = FindObjectOfType<CameraController>();
        cinemachineController.MewCameraFollow();
        cameraAdapt.HideCameraMasks();

        //四个围绕着的球
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);
        turningPhase = false;
        Invincible = false;
    }

    #endregion

    #endregion

    #region 第三阶段

    private IEnumerator Phase3Start()
    {
        // 第三阶段不受替身影响，且整个阶段只释放最终符卡。
        BeginTeleportAnimation();
        uIHealth.Fade(1f, false);
        yield return new WaitForSeconds(1f);

        transform.position = mapCenter;
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        SetPhaseThreeInstruction(
            "禁止使用道具\n禁用所有回复");

        if (player != null)
        {
            player.CanNotUseSpaceItem = true;
            playerHpinP3 = player.Hp;
        }

        HpTimer = Mathf.Max(1f, MewFinale.GetTrialSeconds(this));
        HpTiming = HpTimer;
        Phase3FinaleDuration = Mathf.Clamp(
            Phase3FinaleDuration,
            5f,
            Mathf.Max(5f, HpTimer - 0.5f));
        ArenaRadius = Mathf.Max(12f, Phase2StrongArenaRadius);
        FinalArenaRadius = ArenaRadius;
        ArenaShrinkDuration = Mathf.Max(
            0.1f,
            HpTimer - Phase3FinaleDuration);

        ApplyTimeBarStyle();
        uIHealth.Per = 1f;
        uIHealth.Fade(1f, true);
        ClearProjectile();

        // 限制圈先以半径 22 显形；显形结束后，
        // 在终幕前的 45 秒内缩到 16。最后 15 秒由技能主动解除，
        // 并使用技能圈表现中心蓄力。
        isFinal = false;
        phaseThreeFinaleReleased = false;
        phaseThreeArenaReady = true;
        CreateArenaBoundary();

        if (ArenaIntroDuration > 0f)
        {
            yield return new WaitForSeconds(ArenaIntroDuration);
        }

        isFinal = true;
        if (arenaBoundary != null)
        {
            arenaBoundary.SetTargetRadius(
                FinalArenaRadius,
                ArenaShrinkDuration);
        }

        SkillType = PokemonType.TypeEnum.Psychic;
        TeleportEnd();
        activePhaseThreeSkill = SpawnFinale();
        phaseThreeTimerRunning = true;
        phaseThreeRoutine = null;
    }

    private MewFinale SpawnFinale()
    {
        GameObject skillObject = new GameObject("Mew Finale - The Stars Return");
        skillObject.transform.position = mapCenter;
        MewFinale skill = skillObject.AddComponent<MewFinale>();
        skill.Configure(StardustProjectilePrefab, this);
        phaseThreeTrialFinished = false;
        phaseThreeTrialCleared = false;
        skill.Finished += (finishedSkill, reason) =>
        {
            phaseThreeTrialFinished = true;
            phaseThreeTrialCleared = reason == MewSkillFinishReason.Completed && skill.TrialCleared;
        };

        MewSkillContext context = new MewSkillContext(
            this,
            MewSkillPhase.Phase3,
            player != null ? player.transform : null,
            skillObject.transform,
            mapCenter,
            ArenaRadius,
            arenaBoundary != null ? arenaBoundary.transform : null);

        skill.Initialize(context, true);
        return skill;
    }

    private void StopPhaseThreeFinalSkill()
    {
        if (activePhaseThreeSkill != null &&
            !activePhaseThreeSkill.IsFinished)
        {
            activePhaseThreeSkill.Cancel(true);
        }

        activePhaseThreeSkill = null;

        if (phaseThreeRoutine != null)
        {
            StopCoroutine(phaseThreeRoutine);
            phaseThreeRoutine = null;
        }
    }

    public bool TryProtectPlayer(PlayerControler target)
    {
        if (target == null || target != player || ActiveEncounter != this) return false;
        target.Hp = 1;
        if (!isDying)
            BeginEncounterEnd(currentPhase >= 3 ? EncounterResult.FinalTrialFailed : EncounterResult.NormalBattleFailed);
        return true;
    }

    private void BeginEncounterEnd(EncounterResult result)
    {
        if (isDying) return;
        isDying = true;
        encounterResult = result;
        savedInvincible = player.isInvincibleAlways;
        savedMovementLock = player.isCanNotMove;
        if (currentPhase < 3)
        {
            savedItemLock = player.CanNotUseSpaceItem;
            savedEscape = UISkillButton.Instance == null || UISkillButton.Instance.isEscEnable;
        }
        protectionHeld = true;
        if (result != EncounterResult.FinalTrialCleared) player.BeginMewFaint();
        player.isInvincibleAlways = true;
        player.isCanNotMove = true;
        player.CanNotUseSpaceItem = true;
        protectedPlayerBody = player.GetComponent<Rigidbody2D>();
        if (protectedPlayerBody != null)
        {
            savedPlayerConstraints = protectedPlayerBody.constraints;
            protectedPlayerBody.velocity = Vector2.zero;
            protectedPlayerBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Invincible = true;
        if (result != EncounterResult.FinalTrialCleared) FreezeRescueProjectiles();
        StopAllCoroutines();
        CancelActiveBasicSkill();
        StopPhaseTwoCombat();
        phaseThreeTimerRunning = false;
        StopPhaseThreeFinalSkill();
        phaseThreeArenaReady = false;
        CloseArenaBoundary(0f);
        if (result == EncounterResult.FinalTrialCleared) ClearProjectile();
        if (rigidbody2D != null) rigidbody2D.velocity = Vector2.zero;
        foreach (Collider2D hitbox in GetComponentsInChildren<Collider2D>()) hitbox.enabled = false;
        if (uIHealth != null) uIHealth.Fade(0.4f, false);
        if (UISkillButton.Instance != null) UISkillButton.Instance.isEscEnable = false;
        if (bgmScript != null) bgmScript.FadeOut(0.12f, 1.5f);
        StartCoroutine(EndEncounter());
    }

    private IEnumerator EndEncounter()
    {
        bool failed = encounterResult != EncounterResult.FinalTrialCleared;
        if (failed)
        {
            player.Hp = 1;
            if (UIHealthBar.Instance != null)
            {
                UIHealthBar.Instance.Per = 1f / Mathf.Max(1, player.maxHp);
                UIHealthBar.Instance.NowHpText.text = "1";
                UIHealthBar.Instance.ChangeHpDown();
            }
            MewMercyEffect effect = gameObject.AddComponent<MewMercyEffect>();
            yield return effect.Play(player.transform, true, () => StartCoroutine(DissolveRescueProjectiles()));
            Destroy(effect);
        }
        else yield return new WaitForSeconds(0.6f);

        if (roomCreated)
        {
            MewMercyEffect returnWhite = null;
            if (encounterResult != EncounterResult.NormalBattleFailed)
            {
                returnWhite = gameObject.AddComponent<MewMercyEffect>();
                yield return returnWhite.WhiteScreen(true);
            }
            if (encounterResult == EncounterResult.NormalBattleFailed)
            {
                if (Phase2Mask != null) Destroy(Instantiate(Phase2Mask, transform.position, Quaternion.identity), 2.2f);
                yield return new WaitForSeconds(1.1f);
            }
            player.NowRoom = GetnowRoom;
            player.transform.position = GetPlayerPosition;
            player.InANewRoom = true;
            player.NewRoomTimer = 0f;
            if (cameraAdapt != null)
            {
                cameraAdapt.DeactivateVcam();
                cameraAdapt.ShowCameraMasks();
            }
            if (Camera != null) Camera.transform.position = GetCameraPostion;
            transform.SetParent(originalParent, true);
            transform.position = GetMewPosition;
            if (returnWhite != null)
            {
                yield return new WaitForSecondsRealtime(0.15f);
                yield return returnWhite.WhiteScreen(false);
                Destroy(returnWhite);
            }
        }

        // A gentle approach replaces the combat teleports; gifts remain in the original room.
        Vector3 from = transform.position;
        Vector3 besidePlayer = player.transform.position + Vector3.up * 2.5f;
        animator.ResetTrigger("Teleport");
        animator.Play("BossMewIdle", 0, 0f);
        for (float t = 0f; t < 1.5f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, besidePlayer, Mathf.SmoothStep(0f, 1f, t / 1.5f));
            yield return null;
        }
        transform.position = besidePlayer;
        yield return FloatGift(MaxPotionRewardPrefab, player.transform.position + Vector3.left * 1.3f);
        yield return FloatGift(FullHealRewardPrefab, player.transform.position + Vector3.right * 1.3f);
        if (encounterResult != EncounterResult.NormalBattleFailed)
        {
            MewBossKilled = true;
            PokemonBall reward = OrdinaryRewardPrefab;
            if (encounterResult == EncounterResult.FinalTrialCleared && pbList != null && pbList.Length > 0)
                reward = pbList[Random.Range(0, pbList.Length)];
            if (reward != null)
            {
                PokemonBall ball = Instantiate(reward, player.transform.position + Vector3.up * 1.2f, Quaternion.identity);
                ball.PassiveDropPer = encounterResult == EncounterResult.FinalTrialCleared ? 1f : 0f;
            }
            player.ChangeEx((int)(Exp * 1.8f));
            player.ChangeHPW(HWP);
            if (FloorNum.GlobalFloorNum != null && ScoreCounter.Instance != null)
                ScoreCounter.Instance.EmptyBounsAP += APBounsPoint.EmptyBouns(this, FloorNum.GlobalFloorNum.FloorNumber);
            if (DestoryEvent != null) DestoryEvent();
        }
        if (encounterRoom != null)
        {
            if (!roomCreated) encounterRoom.isClear = Mathf.Max(0, encounterRoom.isClear - 1);
            encounterRoom.RemoveEmptyList(this);
        }
        yield return new WaitForSeconds(1.8f);
        RestoreEncounterProtection();
        isFinal = false;
        DropItem = null;
        animator.SetTrigger("Die"); // Existing farewell clip; do not invoke enemy death rewards.
        yield return FadeDepartureShadows();
        yield return new WaitForSeconds(2.6f);
        Destroy(gameObject);
    }

    private IEnumerator FadeDepartureShadows()
    {
        // BossMewBye scales the visible Mew to zero over its first 0.5 seconds,
        // but only keys the shadows' scale. Fade their alpha over the same interval.
        var shadows = new List<SpriteRenderer>();
        var colors = new List<Color>();
        foreach (string path in new[] { "Shadow", "Shadow (1)" })
        {
            Transform shadow = transform.Find(path);
            if (shadow == null) continue;
            SpriteRenderer renderer = shadow.GetComponent<SpriteRenderer>();
            if (renderer == null) continue;
            shadows.Add(renderer);
            colors.Add(renderer.color);
        }
        for (float t = 0f; t < 0.5f; t += Time.deltaTime)
        {
            float opacity = 1f - Mathf.SmoothStep(0f, 1f, t / 0.5f);
            for (int i = 0; i < shadows.Count; i++)
                if (shadows[i] != null)
                    shadows[i].color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * opacity);
            yield return null;
        }
        foreach (SpriteRenderer shadow in shadows) if (shadow != null) shadow.enabled = false;
    }

    private IEnumerator FloatGift(GameObject prefab, Vector3 destination)
    {
        if (prefab == null) yield break;
        GameObject gift = Instantiate(prefab, transform.position, Quaternion.identity);
        Collider2D[] colliders = gift.GetComponentsInChildren<Collider2D>();
        bool[] enabledStates = new bool[colliders.Length];
        for (int i = 0; i < colliders.Length; i++) { enabledStates[i] = colliders[i].enabled; colliders[i].enabled = false; }
        Rigidbody2D body = gift.GetComponent<Rigidbody2D>();
        bool simulated = body != null && body.simulated;
        if (body != null) body.simulated = false;
        Vector3 start = gift.transform.position;
        for (float t = 0f; t < 0.65f; t += Time.deltaTime)
        {
            if (gift == null) yield break;
            gift.transform.position = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f, 1f, t / 0.65f));
            yield return null;
        }
        if (gift == null) yield break;
        gift.transform.position = destination;
        if (body != null) body.simulated = simulated;
        for (int i = 0; i < colliders.Length; i++) if (colliders[i] != null) colliders[i].enabled = enabledStates[i];
    }

    private void RestoreEncounterProtection()
    {
        if (!protectionHeld) return;
        protectionHeld = false;
        if (protectedPlayerBody != null) protectedPlayerBody.constraints = savedPlayerConstraints;
        if (player != null)
        {
            player.isInvincibleAlways = savedInvincible;
            player.isCanNotMove = savedMovementLock;
            player.CanNotUseSpaceItem = savedItemLock;
            player.GrantMewRecoveryGrace();
        }
        if (UISkillButton.Instance != null) UISkillButton.Instance.isEscEnable = savedEscape;
    }

    #endregion

    #region 通用辅助方法


    private void TeleportEnd()
    {
        GameObject useskillprefab = Instantiate(TeleportEndPrefab, transform.position, Quaternion.identity);
        ParAnimation useskillmask = useskillprefab.GetComponent<ParAnimation>();
        switch (SkillType)
        {
            case PokemonType.TypeEnum.Normal: useskillmask.startColor = colors[0]; break;
            case PokemonType.TypeEnum.Fighting: useskillmask.startColor = colors[1]; break;
            case PokemonType.TypeEnum.Flying: useskillmask.startColor = colors[2]; break;
            case PokemonType.TypeEnum.Poison: useskillmask.startColor = colors[3]; break;
            case PokemonType.TypeEnum.Ground: useskillmask.startColor = colors[4]; break;
            case PokemonType.TypeEnum.Rock: useskillmask.startColor = colors[5]; break;
            case PokemonType.TypeEnum.Bug: useskillmask.startColor = colors[6]; break;
            case PokemonType.TypeEnum.Ghost: useskillmask.startColor = colors[7]; break;
            case PokemonType.TypeEnum.Steel: useskillmask.startColor = colors[8]; break;
            case PokemonType.TypeEnum.Fire: useskillmask.startColor = colors[9]; break;
            case PokemonType.TypeEnum.Water: useskillmask.startColor = colors[10]; break;
            case PokemonType.TypeEnum.Grass: useskillmask.startColor = colors[11]; break;
            case PokemonType.TypeEnum.Electric: useskillmask.startColor = colors[12]; break;
            case PokemonType.TypeEnum.Psychic: useskillmask.startColor = colors[13]; break;
            case PokemonType.TypeEnum.Ice: useskillmask.startColor = colors[14]; break;
            case PokemonType.TypeEnum.Dragon: useskillmask.startColor = colors[15]; break;
            case PokemonType.TypeEnum.Dark: useskillmask.startColor = colors[16]; break;
            case PokemonType.TypeEnum.Fairy: useskillmask.startColor = colors[17]; break;
        }
    }
    private void ApplyPhase2HealthBarStyle()
    {
        ApplyHealthBarSprites(
            Phase2Bar1,
            Phase2Bar2,
            Phase2Bar3,
            Phase2Bar4);
    }

    private void ApplyTimeBarStyle()
    {
        ApplyHealthBarSprites(
            TimeBar1,
            TimeBar2,
            TimeBar3,
            TimeBar4);
    }

    private void ApplyHealthBarSprites(
        Sprite bar1,
        Sprite bar2,
        Sprite bar3,
        Sprite bar4)
    {
        SetImageSprite(timeBar1, bar1);
        SetImageSprite(timeBar2, bar2);
        SetImageSprite(timeBar3, bar3);
        SetImageSprite(timeBar4, bar4);
    }

    private static void SetImageSprite(
        GameObject target,
        Sprite sprite)
    {
        if (target == null || sprite == null)
        {
            return;
        }

        Image image = target.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
    }

    private void ClearStatusEffects()
    {
        //清除所有debuff，包括异常状态累计数
        EmptyCurseRemove();
        EmptyCursePoint = 0;
        ColdRemove();
        isColdDown = 0;
        EmptyConfusionRemove();
        EmptyCursePoint = 0;
        EmptyInfatuationRemove();
        EmptyInfatuationPoint = 0;
        EmptyParalysisRemove();
        ParalysisPointFloat = 0;
        EmptySleepRemove();
        SleepPointFloat = 0;
        EmptyBurnRemove();
        BurnPointFloat = 0;
        BlindRemove();
        EmptyBlindPoint = 0;
        FearRemove();
        GetEmptyFearPointFloat = 0;
        FrozenRemove();
        GetEmptyFrozenPointFloat = 0;
    }

    private GameObject[] rescueProjectiles;

    private void FreezeRescueProjectiles()
    {
        rescueProjectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject obj in rescueProjectiles)
        {
            BarrageProjectile barrage = obj.GetComponent<BarrageProjectile>();
            if (barrage != null) barrage.FreezeForRescue();
            foreach (Collider2D collider in obj.GetComponentsInChildren<Collider2D>()) collider.enabled = false;
            foreach (Rigidbody2D body in obj.GetComponentsInChildren<Rigidbody2D>()) body.simulated = false;
            foreach (MonoBehaviour behaviour in obj.GetComponentsInChildren<MonoBehaviour>())
            { behaviour.StopAllCoroutines(); behaviour.enabled = false; }
        }
    }

    private IEnumerator DissolveRescueProjectiles()
    {
        if (rescueProjectiles == null) yield break;
        Vector2 center = player.transform.position;
        float maxDistance = 1f;
        var sprites = new SpriteRenderer[rescueProjectiles.Length][];
        var colors = new Color[rescueProjectiles.Length][];
        var distances = new float[rescueProjectiles.Length];
        for (int i = 0; i < rescueProjectiles.Length; i++)
        {
            GameObject obj = rescueProjectiles[i];
            if (obj == null) continue;
            distances[i] = Vector2.Distance(center, obj.transform.position);
            maxDistance = Mathf.Max(maxDistance, distances[i]);
            sprites[i] = obj.GetComponentsInChildren<SpriteRenderer>();
            colors[i] = new Color[sprites[i].Length];
            for (int j = 0; j < sprites[i].Length; j++) colors[i][j] = sprites[i][j].color;
        }
        for (float t = 0f; t < 1.7f; t += Time.unscaledDeltaTime)
        {
            for (int i = 0; i < rescueProjectiles.Length; i++)
            {
                GameObject obj = rescueProjectiles[i];
                if (obj == null || !obj.activeSelf || sprites[i] == null) continue;
                float fade = Mathf.Clamp01((t - distances[i] / maxDistance * 1.2f) / 0.4f);
                for (int j = 0; j < sprites[i].Length; j++)
                    if (sprites[i][j] != null)
                    {
                        Color color = Color.Lerp(colors[i][j], Color.white, fade);
                        color.a = colors[i][j].a * (1f - fade);
                        sprites[i][j].color = color;
                    }
                if (fade >= 1f) obj.SetActive(false);
            }
            yield return null;
        }
        foreach (GameObject obj in rescueProjectiles)
            if (obj != null)
            {
                BarrageProjectile barrage = obj.GetComponent<BarrageProjectile>();
                if (barrage != null) barrage.Despawn();
                else Destroy(obj);
            }
        rescueProjectiles = null;
    }

    private void ClearProjectile()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject projectile in projectiles)
        {
            BarrageProjectile barrage = projectile.GetComponent<BarrageProjectile>();
            if (barrage != null) barrage.Despawn();
            else Destroy(projectile);
        }
    }

    private void OnDestroy()
    {
        if (ActiveEncounter == this) ActiveEncounter = null;
        if (currentPhase == 3 && !protectionHeld)
        {
            if (player != null) player.CanNotUseSpaceItem = savedItemLock;
            if (UISkillButton.Instance != null) UISkillButton.Instance.isEscEnable = savedEscape;
        }
        RestoreEncounterProtection();
        if (isDying && cameraAdapt != null && roomCreated) cameraAdapt.ShowCameraMasks();
        if (isDying && bgmScript != null && bgmScript.BGM != null)
        {
            bgmScript.BGM.clip = previousMusic;
            if (previousMusic != null) bgmScript.BGM.Play();
            bgmScript.FadeIn(previousMusicVolume, 1f);
        }
        if (isDying && temporaryBattleRoom != null && player != null && player.NowRoom == GetnowRoom)
        {
            if (MapCreater.StaticMap != null) MapCreater.StaticMap.RRoom.Remove(new Vector3Int(100, 100, 0));
            Destroy(temporaryBattleRoom);
        }
        CancelActiveBasicSkill();
        StopPhaseThreeFinalSkill();

        if (activePhaseTwoSkill != null &&
            !activePhaseTwoSkill.IsFinished)
        {
            activePhaseTwoSkill.Cancel(true);
        }

        if (phaseTwoArenaBoundary != null)
        {
            Destroy(phaseTwoArenaBoundary.gameObject);
            phaseTwoArenaBoundary = null;
        }

        if (arenaBoundary != null)
        {
            Destroy(arenaBoundary.gameObject);
            arenaBoundary = null;
        }
    }

    #endregion
}
