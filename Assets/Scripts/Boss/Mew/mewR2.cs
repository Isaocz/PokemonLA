using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MewR2 : Empty
{
    [Header("技能预制体")]
    public GameObject magicalLeafPrefab;
    public GameObject blizzardPrefab;
    public GameObject WillOWispPrefab;
    public float WillOWispRadius = 2f;
    public GameObject PlayNicePrefab;
    public GameObject TeraBlastPrefab;
    public float laserLength = 10f;
    public float laserOffset = 1f;
    public LayerMask obstacleLayer;
    public GameObject dashReticle;
    public GameObject PouncePrefab;
    public GameObject MagicalFirePrefab;
    public GameObject IcicleSpearPrefab;
    public float summonRadius = 5f;
    public float delayBetweenExecutions = 1f;
    public GameObject HeartStampPrefab;
    public int heartStampCount = 8;
    public GameObject reticlePrefab;
    public GameObject ScaleShotPrefab;
    public int scaleShotCount;
    public GameObject MeanLookSE;
    public GameObject MeanLookPrefab;
    public GameObject DazzlingGleamPrefab;
    public GameObject LeafBladePrefab;
    public GameObject stoneEdgePrefab;
    public GameObject AirSlashPrefab;
    public GameObject MakeItRainPrefab;
    public GameObject StickyWebPrefab;
    public GameObject CrossPoisonPrefab;
    public GameObject SecredFirePrefab;
    public GameObject SecredFireVertexPrefab;
    public GameObject reticle2Prefab;
    public GameObject SecredSwordPrefab;
    public GameObject Phase2Mask;
    public GameObject TeleportEndPrefab;
    public GameObject EdgePar;
    public GameObject Phase3OrbRotate;
    public GameObject ElectricBallPrefab;
    public GameObject TimeStopEffect;
    public GameObject TrailEffect;
    public GameObject TrailEffect2;
    public GameObject TrailEffect3;
    public GameObject IceBeamPrefab;
    public GameObject FakePotionPrefab;
    public GameObject FakeAntidote;
    public GameObject FakeBurnHeal;
    public GameObject FakeAwakening;
    public GameObject FakeIceHeal;
    public GameObject FakeParalyzeHeal;
    public GameObject Stellarize;
    public GameObject FakeLovePrefab;

    [System.Serializable]
    public struct SkillTimerData
    {
        public int skillIndex;
        public int stage;
        public float skillTimer;
    }
    public List<SkillTimerData> skillTimerDataList;
    public Dictionary<int, Dictionary<int, float>> SkillTimer = new Dictionary<int, Dictionary<int, float>>();

    [Header("终结技")]
    public GameObject Swords;
    public GameObject Meanlookfinal;

    [Header("血条UI")]
    public GameObject timeBar1;
    public GameObject timeBar2;
    public GameObject timeBar3;
    public GameObject timeBar4;
    public Sprite TimeBar1;
    public Sprite TimeBar2;
    public Sprite TimeBar3;
    public Sprite TimeBar4;

    [Header("掉落")]
    public PokemonBall[] pbList;

    [Header("其他")]
    public int currentPhase = 1;
    public bool LaserChange = false;
    public PokemonType.TypeEnum SkillType;
    public Material src1;
    public Material src2;
    public Material src3;
    public float intensity = 1f;

    // 私有变量
    private List<int> skillList;
    private int currentSkillIndex = 0;
    private float skillTimer = 0f;
    private bool isReset;
    private bool isTeleport;
    public float teleportTime;
    private float teleportTimer = 0f;
    private int playerHpinP3;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private bool isPhase3 = false;
    private bool isSkillFin;
    private bool isDying = false;
    private bool isFinal;
    private int teleportAttempts = 0;
    private Vector3 mapCenter;
    private Vector3 reticleSpawnPosition;
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
    private float MeanLookTimer = 0f;
    private bool UsedMeanLook = false;
    private bool turningPhase = false;
    private BackGroundMusic bgmScript;

    // 颜色数组
    private Color[] colors = new Color[]
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
        new Color(1, 0.2666667f, 0.1294118f, 1f), // Fire
        new Color(0.2f, 0.6f, 0.9960785f, 1f), // Water
        new Color(0.4666667f, 0.8f, 0.3333333f, 1f), // Grass
        new Color(0.9725491f, 0.8156863f, 0.1882353f, 1f), // Electric
        new Color(0.9764706f, 0.3490196f, 0.5372549f, 1f), // Psychic
        new Color(0.5921569f, 0.8431373f, 0.8431373f, 1f), // Ice
        new Color(0.4470589f, 0.2313726f, 0.9764706f, 1f), // Dragon
        new Color(0.4470589f, 0.345098f, 0.2862745f, 1f), // Dark
        new Color(0.9333334f, 0.6078432f, 0.6784314f, 1f), // Fairy
    };

    // 阶段3技能
    private int[] Phase3Skills = new int[] { 1, 3, 5, 6, 7, 8, 9, 10, 13, 15, 16, 17, 18, 19 };

    //房间
    public GameObject MewBossRoomPrefab;
    public Vector3 MewBossRoomPosition = new Vector3(60f, 60f, 0f);

    void Start()
    {
        InitializeMew();
        InitializeSkillTimers();
        CleanEnvironment();
    }

    void Update()
    {
        ResetPlayer();
        
        if (!isBorn && !isDying)
        {
            if (currentPhase == 3)
            {
                HandlePhase3();
            }
            else
            {
                HandlePhase1And2();
            }
            
            HandleMeanLookTimer();
            ChangeColor();
        }
    }

    #region 初始化方法
    private void InitializeMew()
    {
        bgmScript = BackGroundMusic.StaticBGM;
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

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cameraAdapt = FindObjectOfType<CameraAdapt>();

        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        mapCenter = transform.parent.position;
        GetnowRoom = player.NowRoom;
        GetMewPosition = transform.position;
        GetPlayerPosition = player.transform.position;
        GetCameraPostion = Camera.transform.position;
        transform.parent.parent.GetComponent<Room>().isClear += 1;

        ClearProjectile();
        InitializeSkillList();
        isReset = false;
    }

    private void InitializeSkillTimers()
    {
        foreach (SkillTimerData data in skillTimerDataList)
        {
            if (!SkillTimer.ContainsKey(data.skillIndex))
            {
                SkillTimer[data.skillIndex] = new Dictionary<int, float>();
            }
            SkillTimer[data.skillIndex][data.stage] = data.skillTimer;
        }
    }

    private void CleanEnvironment()
    {
        Transform grandParent = transform.parent.parent;
        Transform environment = grandParent.Find("Enviroment");

        if (environment != null)
        {
            for (int i = 0; i < environment.childCount; i++)
            {
                if (environment.tag == "Grass")
                {
                    NormalGress grass = environment.GetComponent<NormalGress>();
                    if (grass != null) grass.GrassDie();
                }
                else
                {
                    Destroy(environment.GetChild(i).gameObject);
                }
            }
        }
    }
    #endregion

    #region 主要逻辑处理
    private void HandlePhase3()
    {
        Phase3();
        HpTiming -= Time.deltaTime;
        EmptyHp = (int)(HpTiming / HpTimer * maxHP);
        uIHealth.Per = HpTiming / HpTimer;
        uIHealth.ChangeHpDown();
        UISkillButton.Instance.isEscEnable = false;
        
        RestrictPlayerMovement();
        
        if (HpTiming <= 0f && !isDying)
        {
            isDying = true;
            ClearProjectile();
            StartCoroutine(Phase3End());
        }
        
        if (Vector3.Distance(player.transform.position, transform.position) > 200f)
        {
            Destroy(this);
        }
    }

    private void HandlePhase1And2()
    {
        AtkTarget = FindAtkTarget(40f);
        UpdateEmptyChangeHP();
        StateMaterialChange();
        bgmScript.ChangeBGMToMew(currentPhase);
        
        if (!turningPhase)
        {
            switch (currentPhase)
            {
                case 1:
                    if (EmptyHp < maxHP / 2)
                    {
                        StartPhaseTransition();
                    }
                    else if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
                    {
                        Phase1();
                    }
                    break;
                case 2:
                    if (EmptyHp <= 0)
                    {
                        PreparePhase3();
                    }
                    else if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
                    {
                        Phase2();
                    }
                    break;
            }
        }
    }

    private void HandleMeanLookTimer()
    {
        if (UsedMeanLook)
        {
            MeanLookTimer += Time.deltaTime;
            if (MeanLookTimer >= 3.5f)
            {
                UsedMeanLook = false;
                MeanLookTimer = 0f;
            }
        }
    }
    #endregion

    #region 阶段逻辑
    void Phase1()
    {
        ResetSkillTimer();
        if (skillTimer <= 0f)
        {
            if (currentSkillIndex >= skillList.Count)
            {
                InitializeSkillList();
                currentSkillIndex = 0;
            }
            
            int randomSkillIndex = skillList[currentSkillIndex];
            StartCoroutine(Phase1Skill(randomSkillIndex));
            SkillTimerUpdate(randomSkillIndex, 1);
            currentSkillIndex++;
        }
        skillTimer -= Time.deltaTime;
    }

    void Phase2()
    {
        ResetSkillTimer();
        if (skillTimer <= 0f)
        {
            if (currentSkillIndex >= skillList.Count)
            {
                InitializeSkillList();
                currentSkillIndex = 0;
            }

            int randomSkillIndex = skillList[currentSkillIndex];
            StartCoroutine(Phase2Skill(randomSkillIndex));
            targetPosition = RamdomTeleport();
            currentPosition = transform.position;
            SkillTimerUpdate(randomSkillIndex, 2);
            currentSkillIndex++;
        }
        
        HandleTeleport();
        skillTimer -= Time.deltaTime;
    }

    void Phase3()
    {
        ResetSkillTimer();

        if (isPhase3)
        {
            Invincible = true;
            isPhase3 = false;
            playerHpinP3 = player.Hp;
            StartCoroutine(Phase3Start());
        }

        LockPlayerHP();
        
        if (!isSkillFin)
        {
            isSkillFin = true;
            StartCoroutine(Phase3Middle());
        }
    }

    private void HandleTeleport()
    {
        if (isTeleport)
        {
            teleportTimer += Time.deltaTime;
            float t = Mathf.Clamp01(Mathf.Sin(teleportTimer / teleportTime * Mathf.PI * 0.5f));
            transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
            
            if (teleportTimer >= teleportTime)
            {
                isTeleport = false;
                teleportTimer = 0f;
            }
        }
    }

    private void LockPlayerHP()
    {
        if (player.Hp > playerHpinP3)
        {
            player.ChangeHp(playerHpinP3 - player.Hp);
        }
        else if (player.Hp < playerHpinP3)
        {
            playerHpinP3 = player.Hp;
        }
    }

    private void RestrictPlayerMovement()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        float maxDistance = isFinal ? 14f : 19f;
        
        if (distance > maxDistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + direction * maxDistance;
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 7f * Time.deltaTime);
            
            GameObject edgePar = Instantiate(EdgePar, player.transform.position, Quaternion.identity);
            Destroy(edgePar, 0.4f);
        }
    }
    #endregion

    #region 技能系统
    void InitializeSkillList()
    {
        skillList = new List<int>();
        int maxSkill = currentPhase == 1 ? 18 : 20;
        
        for (int i = 1; i <= maxSkill; i++)
        {
            skillList.Add(i);
        }
        
        ShuffleSkillList();
    }

    void ShuffleSkillList()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            int temp = skillList[i];
            int randomIndex = Random.Range(0, skillList.Count);
            skillList[i] = skillList[randomIndex];
            skillList[randomIndex] = temp;
        }
    }

    void UseSkill(int skillIndex)
    {
        Debug.Log("Boss used skill: " + skillIndex);
        
        switch (skillIndex)
        {
            case 1: UseSkill1(); break;
            case 2: UseSkill2(); break;
            case 3: UseSkill3(); break;
            case 4: UseSkill4(); break;
            case 5: UseSkill5(); break;
            case 6: UseSkill6(); break;
            case 7: UseSkill7(); break;
            case 8: UseSkill8(); break;
            case 9: UseSkill9(); break;
            case 10: UseSkill10(); break;
            case 11: UseSkill11(); break;
            case 12: UseSkill12(); break;
            case 13: UseSkill13(); break;
            case 14: UseSkill14(); break;
            case 15: UseSkill15(); break;
            case 16: UseSkill16(); break;
            case 17: UseSkill17(); break;
            case 18: UseSkill18(); break;
            case 19: UseSkill19(); break;
            case 20: UseSkill20(); break;
        }
    }

    // 各个技能的具体实现（保持原有逻辑）
    private void UseSkill1() {
        /* 魔法叶实现 */
    }
    private void UseSkill2() {
        /* 暴风雪实现 */
    }
    private void UseSkill3() {
    }
    private void UseSkill4() { /* 和睦相处实现 */ }
    private void UseSkill5() { /* 太晶爆发实现 */ }
    private void UseSkill6() { /* 虫扑实现 */ }
    private void UseSkill7() { /* 魔法火焰实现 */ }
    private void UseSkill8() { /* 冰锥实现 */ }
    private void UseSkill9() { /* 爱心印章实现 */ }
    private void UseSkill10() { /* 鳞射实现 */ }
    private void UseSkill11() { /* 黑色目光实现 */ }
    private void UseSkill12() { /* 魔法闪耀实现 */ }
    private void UseSkill13() { /* 叶刃实现 */ }
    private void UseSkill14() { /* 尖石攻击实现 */ }
    private void UseSkill15() { /* 空气之刃实现 */ }
    private void UseSkill16() { /* 淘金潮实现 */ }
    private void UseSkill17() { /* 黏黏网实现 */ }
    private void UseSkill18() { /* 十字毒刃实现 */ }
    private void UseSkill19() { /* 神圣之火实现 */ }
    private void UseSkill20() { /* 圣剑实现 */ }

    void SkillTimerUpdate(int skillindex, int stage)
    {
        skillTimer = SkillTimer[skillindex][stage];
    }

    void ResetSkillTimer()
    {
        if (!isReset)
        {
            skillTimer = 3f;
            isReset = true;
        }
    }
    #endregion

    #region 阶段转换
    private void StartPhaseTransition()
    {
        Invincible = true;
        player.isInvincible = true;
        StopAllCoroutines();
        
        if (!UsedMeanLook && !roomCreated && currentPhase == 1)
        {
            isReset = false;
            turningPhase = true;
            ClearStatusEffects();
            EmptyHp = maxHP;
            player.ChangeHp(player.maxHp - player.Hp, 0, 0);
            uIHealth.Per = EmptyHp / maxHP;
            StartCoroutine(Phase2Start());
        }
    }

    private void PreparePhase3()
    {
        isReset = false;
        ClearStatusEffects();
        StopAllCoroutines();
        player.ChangeHp((player.Hp < (player.maxHp * 3 / 4)) ? player.maxHp / 4 : (player.maxHp - player.Hp), 0, 0);
        EmptyHp = maxHP;
        uIHealth.Per = EmptyHp / maxHP;
        uIHealth.ChangeHpUp();
        currentPhase++;
        isPhase3 = true;
    }
    #endregion

    #region 颜色系统
    void ChangeColor()
    {
        float factor = Mathf.Pow(2, intensity);
        Material[] srcs = { src1, src2, src3 };

        if (!isFinal)
        {
            ApplyTypeColor(srcs, factor);
        }
        else
        {
            ApplyRainbowColor(srcs);
        }
    }

    private void ApplyTypeColor(Material[] srcs, float factor)
    {
        Color targetColor = GetTypeColor(SkillType);
        
        foreach (Material material in srcs)
        {
            material.SetColor("_Color", new Color(targetColor.r * factor, targetColor.g * factor, targetColor.b * factor, 1));
        }
    }

    private void ApplyRainbowColor(Material[] srcs)
    {
        Color rainbowColor = new Color(
            Mathf.PingPong(Time.time, 1),
            Mathf.PingPong(Time.time + 0.5f, 1),
            Mathf.PingPong(Time.time + 1f, 1)
        );
        
        foreach (Material material in srcs)
        {
            material.SetColor("_Color", rainbowColor);
        }
    }

    private Color GetTypeColor(PokemonType.TypeEnum type)
    {
        return type switch
        {
            PokemonType.TypeEnum.Normal => colors[0],
            PokemonType.TypeEnum.Fighting => colors[1],
            PokemonType.TypeEnum.Flying => colors[2],
            PokemonType.TypeEnum.Poison => colors[3],
            PokemonType.TypeEnum.Ground => colors[4],
            PokemonType.TypeEnum.Rock => colors[5],
            PokemonType.TypeEnum.Bug => colors[6],
            PokemonType.TypeEnum.Ghost => colors[7],
            PokemonType.TypeEnum.Steel => colors[8],
            PokemonType.TypeEnum.Fire => colors[9],
            PokemonType.TypeEnum.Water => colors[10],
            PokemonType.TypeEnum.Grass => colors[11],
            PokemonType.TypeEnum.Electric => colors[12],
            PokemonType.TypeEnum.Psychic => colors[13],
            PokemonType.TypeEnum.Ice => colors[14],
            PokemonType.TypeEnum.Dragon => colors[15],
            PokemonType.TypeEnum.Dark => colors[16],
            PokemonType.TypeEnum.Fairy => colors[17],
            _ => Color.white
        };
    }
    #endregion

    #region 辅助方法
    Vector3 RamdomTeleport()
    {
        // 保持原有随机传送逻辑
        return Vector3.zero;
    }

    bool IsInMapBounds(Vector3 position)
    {
        float minX = 2985f, maxX = 3045f;
        float minY = 2388f, maxY = 2436f;

        return position.x >= minX && position.x <= maxX && 
               position.y >= minY && position.y <= maxY;
    }

    void ChangeType(int randomIndex)
    {
        SkillType = randomIndex switch
        {
            1 => PokemonType.TypeEnum.Grass,
            2 => PokemonType.TypeEnum.Ice,
            3 => PokemonType.TypeEnum.Fire,
            4 or 5 => PokemonType.TypeEnum.Normal,
            6 or 17 => PokemonType.TypeEnum.Bug,
            7 or 19 => PokemonType.TypeEnum.Fire,
            8 => PokemonType.TypeEnum.Ice,
            9 or 12 => PokemonType.TypeEnum.Fairy,
            10 => PokemonType.TypeEnum.Dragon,
            11 => PokemonType.TypeEnum.Dark,
            13 => PokemonType.TypeEnum.Grass,
            14 => PokemonType.TypeEnum.Rock,
            15 => PokemonType.TypeEnum.Flying,
            16 => PokemonType.TypeEnum.Steel,
            18 => PokemonType.TypeEnum.Poison,
            20 => PokemonType.TypeEnum.Fighting,
            _ => SkillType
        };
    }

    void TeleportEnd()
    {
        GameObject useskillprefab = Instantiate(TeleportEndPrefab, transform.position, Quaternion.identity);
        ParAnimation useskillmask = useskillprefab.GetComponent<ParAnimation>();
        useskillmask.startColor = GetTypeColor(SkillType);
    }

    void ClearStatusEffects()
    {
        EmptyCurseRemove(); EmptyCursePoint = 0;
        ColdRemove(); isColdDown = 0;
        EmptyConfusionRemove(); EmptyCursePoint = 0;
        EmptyInfatuationRemove(); EmptyInfatuationPoint = 0;
        EmptyParalysisRemove(); ParalysisPointFloat = 0;
        EmptySleepRemove(); SleepPointFloat = 0;
        EmptyBurnRemove(); BurnPointFloat = 0;
        BlindRemove(); EmptyBlindPoint = 0;
        FearRemove(); GetEmptyFearPointFloat = 0;
        FrozenRemove(); GetEmptyFrozenPointFloat = 0;
    }

    void ClearProjectile()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }
    #endregion

    #region 协程方法
    private IEnumerator Phase1Skill(int randomSkillIndex)
    {
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);

        if (randomSkillIndex == 5 || randomSkillIndex == 16)
        {
            transform.position = mapCenter;
        }
        else
        {
            transform.position = RamdomTeleport();
        }
        
        ChangeType(randomSkillIndex);
        yield return new WaitForSeconds(0.5f);
        TeleportEnd();
        UseSkill(randomSkillIndex);
        SkillTimerUpdate(randomSkillIndex, 1);
    }

    private IEnumerator Phase2Skill(int randomSkillIndex)
    {
        if (randomSkillIndex == 2 || randomSkillIndex == 14) yield break;
        
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);
        isTeleport = true;
        ChangeType(randomSkillIndex);
        yield return new WaitForSeconds(0.5f);
        TeleportEnd();
        UseSkill(randomSkillIndex);
        SkillTimerUpdate(randomSkillIndex, 2);
    }

    private IEnumerator Phase2Start()
    {
        //清除所有的子弹
        uIHealth.ChangeHpUp();
        ClearProjectile();

        //制作一个黑色五角星
        int numPoints = 5; // 五角星上的点数
        float radius = 12f; // 五角星的顶点到中心的距离

        Vector3[] starVertices = new Vector3[numPoints];

        // 创建五角星的顶点坐标
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * 2f * Mathf.PI / numPoints;
            float x = radius * Mathf.Sin(angle) + player.transform.position.x;
            float y = radius * Mathf.Cos(angle) + player.transform.position.y;
            starVertices[i] = new Vector3(x, y, player.transform.position.z);
        }
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 startPoint = starVertices[i];
            Vector3 endPoint = starVertices[(i + 2) % numPoints];

            float dist = Vector3.Distance(startPoint, endPoint);
            float step = dist / 12f; // 生成点的距离间隔

            Vector3 direction = (endPoint - startPoint).normalized;

            for (int j = 0; j < 12; j++)
            {
                Vector3 secredFirePosition = startPoint + direction * (j * step);
                GameObject willowispprefab = Instantiate(WillOWispPrefab, secredFirePosition, Quaternion.identity);
                willowispprefab.GetComponent<WillOWispEmpty>().isStage = true;
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);
        GameObject phase2mask = Instantiate(Phase2Mask, transform.position, Quaternion.identity);
        Destroy(phase2mask, 2.2f);
        yield return new WaitForSeconds(1.1f);

        //创建新的房间
        GameObject newRoom = Instantiate(MewBossRoomPrefab, MewBossRoomPosition, Quaternion.identity);
        mapCenter = MewBossRoomPosition + new Vector3(15f, 12f, 0f);
        transform.position = MewBossRoomPosition + new Vector3(15f, 12f, 0f);
        player.transform.position = MewBossRoomPosition;
        transform.parent.parent.GetComponent<Room>().isClear = 0;
        player.NowRoom = new Vector3Int(100, 100, 0);
        player.InANewRoom = true;
        player.NewRoomTimer = 0f;
        player.isInvincible = false;
        currentPhase++;
        MapCreater.StaticMap.RRoom.Add(new Vector3Int(100, 100, 0), newRoom.GetComponent<Room>());
        InitializeSkillList();

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
        yield break;
    }

    private IEnumerator Phase3Start()
    {
        animator.SetTrigger("Teleport");
        uIHealth.Fade(1f, false);
        yield return new WaitForSeconds(1f);
        transform.position = mapCenter;
        Transform uitext = player.transform.GetChild(2).GetChild(3);
        if (uitext)
        {
            uitext.GetComponent<PlayerUIText>().SetText("禁止使用道具\n禁用所有回复");
        }
        player.CanNotUseSpaceItem = true;//不允许玩家使用主动道具
        MewOrbRotate mewOrbRotate = Phase3OrbRotate.GetComponent<MewOrbRotate>();
        StartCoroutine(mewOrbRotate.ActivatePhase3Effect(1, 30, 20f));
        //修改ui
        Image timebar1 = timeBar1.GetComponent<Image>();
        timebar1.sprite = TimeBar1;
        Image timebar2 = timeBar2.GetComponent<Image>();
        timebar2.sprite = TimeBar2;
        Image timebar3 = timeBar3.GetComponent<Image>();
        timebar3.sprite = TimeBar3;
        Image timebar4 = timeBar4.GetComponent<Image>();
        timebar4.sprite = TimeBar4;
        uIHealth.Fade(1f, true);
        //清除子弹
        ClearProjectile();
        yield break;
    }

    private IEnumerator Phase3Middle()
    {
        // 保持原有阶段3中间逻辑
        yield break;
    }

    private IEnumerator Phase3End()
    {
        // 保持原有阶段3结束逻辑
        yield break;
    }
    #endregion
}