using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics.Eventing.Reader;

public class Mew : Empty
{
    public GameObject magicalLeafPrefab;//技能1
    public float delayBetweenLeaves = 0.3f; // 每片MagicalLeaf之间的延迟时间
    int Leafnum;//魔法叶数量
    public GameObject blizzardPrefab;//技能2
    public GameObject WillOWispPrefab;//技能3
    public int WillOWispDegree = 2;//WillOWisp重复次数
    public int numWillOWisp = 12; // WillOWisp的数量
    public float WillOWispRadius = 2f; // WillOWisp的生成半径
    public float moveSpeed = 4f; // WillOWisp的移动速度
    public GameObject PlayNicePrefab;//技能4
    public GameObject TeraBlastPrefab;//技能5
    public float laserLength = 10f; // 激光的长度
    public float laserOffset = 1f; // 激光的偏移量
    public LayerMask obstacleLayer;
    public GameObject CursePrefab;//技能6
    public GameObject MagicalFirePrefab;//技能7
    public GameObject IcicleSpearPrefab;//技能8
    public float summonRadius = 5f;
    public float delayBetweenExecutions = 1f;
    public GameObject HeartStampPrefab;//技能9
    public float HeartStampRadius=1.5f;
    public int heartStampCount = 8;
    public GameObject reticlePrefab; //Reticle预制体
    public GameObject ScaleShotPrefab; //技能10
    public int scaleShotCount;
    public GameObject MeanLookSE;//黑色目光特效
    public GameObject MeanLookPrefab;//技能11
    public GameObject DazzlingGleamPrefab;//技能12
    public GameObject LeafBladePrefab; // 技能13
    public GameObject stoneEdgePrefab;//技能14
    private Vector3 mapCenter;  // 地图中心点
    private Vector3 reticleSpawnPosition; // Reticle生成位置
    public GameObject AirSlashPrefab;//技能15
    public GameObject MakeItRainPrefab;//技能16
    public GameObject StickyWebPrefab;//技能17
    public GameObject CrossPoisonPrefab;//技能18
    public GameObject SecredFirePrefab;//技能19
    public GameObject SecredFireCentrePrefab;
    public GameObject SecredFireVertexPrefab;
    public GameObject reticle2Prefab;//Reticle2预制体
    public GameObject SecredSwordPrefab;//技能20
    public GameObject Phase2Mask;
    public GameObject UseSkillPrefab;

    //切换房间时等待
    private float MeanLookTimer = 0f;
    private bool UsedMeanLook = false;
    private bool turningPhase = false;

    //Audio
    public BackGroundMusic bgmScript;
    //阶段
    public int currentPhase = 1; // 当前阶段
    private float skillTimer = 0f; // 技能计时器
    private List<GameObject> heartStamps = new List<GameObject>();//对HeartStamp进行存储
    private float MoreAttackTimer;//二阶段更多技能计时器
    private int[] MoreAttackSkill = { 1, 8, 9, 10, 11, 13, 15, 17, 18, 20 };
    private int MoreAttackSkillIndex;
    private bool isMasked = false;//更多技能释放遮罩

    //随机传送
    private int teleportAttempts = 0;//随机传送计数器

    //房间
    public GameObject MewBossRoomPrefab;
    public Vector3 MewBossRoomPosition = new Vector3(60f, 60f, 0f);
    private bool roomCreated = false;
    private Vector3Int GetnowRoom;

    //摄像跟随
    private CameraController cinemachineController;
    private CameraAdapt cameraAdapt;

    // Start is called before the first frame update
    void Start()
    {
        //Audio
        bgmScript = BackGroundMusic.StaticBGM;

        //梦幻的基础属性
        EmptyType01 = Type.TypeEnum.Psychic;//梦幻的属性为超能
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, 100);//上限等级100
        EmptyHpForLevel(Emptylevel);//设置初始血量

        //能力等级
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;//经验值

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cinemachineController = FindObjectOfType<CameraController>();
        cameraAdapt = FindObjectOfType<CameraAdapt>();

        //地图
        mapCenter = transform.parent.position;
        GetnowRoom = player.NowRoom;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            UpdateEmptyChangeHP();
            ClearStatusEffects();
            if (currentPhase == 3)
            {
                bgmScript.ChangeBGMINSIST();
                EmptyDie();
            }
            else
            {
                bgmScript.ChangeBGMToMew();
                if (!turningPhase)
                {
                    switch (currentPhase)
                    {
                        case 1:
                            Phase1();
                            break;
                        case 2:
                            Phase2();
                            MoreAttack();
                            break;
                        case 3:
                            Phase3();
                            break;
                    }
                }
            }
            if (UsedMeanLook)
            {
                MeanLookTimer += Time.deltaTime;
                if(MeanLookTimer >= 3.5f)
                {
                    UsedMeanLook = false;
                    MeanLookTimer = 0f;
                }

            }
        }
    }
    void Phase1()
    {
        if (!UsedMeanLook && EmptyHp < maxHP)//需要测试，测试后修改
        {
            if (!roomCreated && currentPhase == 1)
            {
                turningPhase = true;
                StopAllCoroutines();//停止所有技能释放
                EmptyHp = maxHP;
                StartCoroutine(Phase2Start());
            }
            currentPhase++;
            Debug.Log("进入二阶段");
        }
        else if (skillTimer <= 0f)
        {
            skillTimer = 1.1f;
            StartCoroutine(Phase1Skill());
        }

        // 技能计时器递减
            skillTimer -= Time.deltaTime;
    }
    void Phase2()
    {
        if (EmptyHp <= maxHP*4/5 && currentPhase == 2)//需要测试，测试后修改
        {
            currentPhase++;
            Debug.Log("进入三阶段");
            EmptyHp = maxHP;
        }
        else if (skillTimer <= 0f)
        {
            // 随机选择一个技能释放
            int randomSkillIndex = Random.Range(1, 21);
            StartCoroutine(Phase2Skill(randomSkillIndex));
            SkillTimerUpdate(randomSkillIndex, 2);
        }

        // 技能计时器递减
        skillTimer -= Time.deltaTime;
    }
    private void MoreAttack()//二阶段的额外攻击模式
    {
        MoreAttackTimer += Time.deltaTime;
        if (MoreAttackTimer >= 7f)
        {
            //当前技能遮罩
            if (!isMasked)
            {
                isMasked = true;
                UseSkillMask(MoreAttackSkillIndex);
            }
            if (MoreAttackTimer >= 8)
            {
                if (MoreAttackSkillIndex >= MoreAttackSkill.Length)
                {
                    MoreAttackSkillIndex = 0; // 回到第一个技能
                }
                UseSkill(MoreAttackSkill[MoreAttackSkillIndex]);
                MoreAttackSkillIndex++;
                isMasked = false;
                MoreAttackTimer = 0f;
            }
        }
    }
    void Phase3()
    {
        //让梦幻无敌，此时无法受伤
        Invincible = true;
    }
    void UseSkill(int skillIndex)
    {
        Debug.Log("Boss used skill: " + skillIndex);
        switch (skillIndex)
        {
            case 1:
                //技能1：魔法叶
                StartCoroutine(ReleaseLeaves());
                IEnumerator ReleaseLeaves()
                {
                    if (currentPhase == 1)
                    {
                        Leafnum = 3;
                    }
                    else
                    {
                        Leafnum = 5;
                    }
                    for (int i = 0; i < Leafnum; i++)
                    {
                        // 在Mew位置实例化魔法叶
                        GameObject magicalLeaf = Instantiate(magicalLeafPrefab, transform.position, Quaternion.identity);
                        magicalLeaf.GetComponent<MagicalLeafEmpty>().empty = this;
                        Destroy(magicalLeaf, 6f); // 6秒后销毁魔法叶对象
                        yield return new WaitForSeconds(delayBetweenLeaves);
                    }
                }
                break;
            case 2:
                //技能2：暴风雪
                if(currentPhase!= 1)
                {
                    return;
                }
                GameObject blizzard = Instantiate(blizzardPrefab, transform.position, Quaternion.identity);
                blizzard.GetComponent<BlizzardEmpty>().empty = this;
                Destroy(blizzard, 6f);//6秒后销毁暴风雪对象
                break;
            case 3://技能3：磷火
                float WaitingWillOWisp = 1f;
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    if (currentPhase != 1)
                    {
                        numWillOWisp = 20;
                        WillOWispDegree = 5;
                        WaitingWillOWisp = 0.8f;
                    }
                    for (int j = 0; j < WillOWispDegree; j++)
                    {
                        float increaseAngle = 10f;
                        float angleStep = 360f / numWillOWisp; // 计算每个WillOWisp之间的角度间隔
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = increaseAngle + i * angleStep; // 计算当前WillOWisp的角度
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius; // 计算当前WillOWisp的生成位置
                            WillOWispEmpty willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity).GetComponent<WillOWispEmpty>();
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.Initialize(moveSpeed, direction); // 设置WillOWisp的移动速度
                            willOWisp.mew = this.gameObject;
                            Destroy(willOWisp, 4f);
                        }
                        yield return new WaitForSeconds(WaitingWillOWisp);
                    }
                }
                break;
            case 4://技能4：和睦相处
                if(currentPhase!= 1)
                {
                    return;
                }
                GameObject PlayNice = Instantiate(PlayNicePrefab, transform.position, Quaternion.identity);
                Destroy(PlayNice, 5f);
                break;
            case 5://技能5：太晶爆发
                //释放3道激光，分别位于90度、210度、330度的位置
                transform.position = mapCenter;
                //创建三条激光
                float[] angles = { 90f, 210f, 330f };
                for (int i = 0; i < angles.Length; i++)
                {
                    //计算激光的起始点和终点
                    float angle = angles[i];
                    Vector3 startPoint = mapCenter;
                    Vector3 endPoint = mapCenter + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                    TeraBlastEmpty Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity).GetComponent<TeraBlastEmpty>();
                    Terablast.SetEndpoints(startPoint, endPoint, angle);
                    Terablast.SetColors(Color.yellow, Color.red);
                    Terablast.empty = this;
                }
                break;
            case 6://技能6：咒术
                if(currentPhase != 1)
                {
                    return;
                }
                StartCoroutine(ReleaseCurse());
                IEnumerator ReleaseCurse()
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject Curse = Instantiate(CursePrefab, player.transform.position, Quaternion.identity);
                        Curse.GetComponent<Curse>().empty = this;
                        Destroy(Curse, 4f);
                        yield return new WaitForSeconds(0.6f);
                    }
                }
                break;
            case 7://技能7：魔法火焰
                StartCoroutine(ReleaseMagicalFire());
                IEnumerator ReleaseMagicalFire()
                {
                    float intervalTime= 1f;
                    int Times = 3;
                    if (currentPhase != 1)
                    {
                        intervalTime = 0.8f;
                        Times = 4;
                    }
                    float angleIncrement = 360f / 8;
                    float rotationSpeed = 60f;
                    for (int j = 0; j < Times; j++) {
                        for (int i = 0; i < 8; i++)
                        {
                            float angle = i * angleIncrement;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                            MagicalFireEmpty magicalFire = Instantiate(MagicalFirePrefab, transform.position, rotation).GetComponent<MagicalFireEmpty>();
                            magicalFire.ps(transform.position, rotationSpeed);
                            magicalFire.empty = this;
                            Destroy(magicalFire, 7f);

                        }
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;

            case 8://技能8：冰锥
                StartCoroutine(SummonIcicleSpears());
                IEnumerator SummonIcicleSpears()
                {
                    yield return new WaitForSeconds(1f);
                    int numExecutions = 3;
                    int icicleCount = 8;
                    float delayBetweenExecutions = 1f;
                    if (currentPhase != 1)
                    {
                        delayBetweenExecutions = 0.8f;
                    }
                    for (int j = 0; j < numExecutions; j++)
                    {
                        for (int i = 0; i < icicleCount; i++)
                        {
                            float angle = i * (360f / icicleCount);
                            Vector2 spawnPosition = player.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * summonRadius);
                            IcicleSpearEmpty IcicleSpear = Instantiate(IcicleSpearPrefab, spawnPosition, Quaternion.identity).GetComponent<IcicleSpearEmpty>();
                            IcicleSpear.sf(player.transform.position);
                            IcicleSpear.empty = this;
                        }
                        yield return new WaitForSeconds(delayBetweenExecutions);
                    }
                }
                break;
            case 9://技能9：爱心印章
                StartCoroutine(ReleaseHeartStamp());
                IEnumerator ReleaseHeartStamp()
                {
                    float intervalTime = 1.5f;
                    int Times = 4;
                    if (currentPhase != 1)
                    {
                        intervalTime = 1.2f;
                        Times = 5;
                    }
                    for (int j = 0; j < Times; j++) {
                        float angleIncrement = 360f / heartStampCount;
                        for (int i = 0; i < heartStampCount; i++)
                        {
                            float angle = i * angleIncrement;
                            Vector3 heartStampPosition = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * HeartStampRadius;

                            HeartStampEmpty heartStamp = Instantiate(HeartStampPrefab, heartStampPosition, Quaternion.identity).GetComponent<HeartStampEmpty>();
                            heartStamp.empty = this;
                            heartStamps.Add(heartStamp.gameObject);
                            if (heartStamp != null)
                            {
                                heartStamp.SetTarget(player.transform.position);
                            }
                        }
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 10://技能10：鳞射
                // 生成ScaleShot
                StartCoroutine(ReleaseScaleShoot());
                IEnumerator ReleaseScaleShoot()
                {
                    int Times = 3;
                    if (currentPhase != 1)
                    {
                        Times = 5;
                    }
                    for (int j = 0; j < Times; j++)
                    {
                        Vector3 randomPoint = (Vector2)player.transform.position + Random.insideUnitCircle.normalized * 3f;
                        // 创建Reticle并设置位置
                        GameObject reticle = Instantiate(reticlePrefab, randomPoint, Quaternion.identity);
                        Destroy(reticle, 2f);
                        yield return new WaitForSeconds(1.5f);
                        for (int i = 0; i < scaleShotCount; i++)
                        {
                            // 计算ScaleShot生成的位置和方向
                            Vector3 scaleShotPosition = randomPoint;
                            float angleIncrement = 360f / scaleShotCount;
                            float angle = i * angleIncrement;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                            // 创建ScaleShot
                            GameObject scaleShot = Instantiate(ScaleShotPrefab, scaleShotPosition, rotation);
                            scaleShot.GetComponent<ScaleShotEmpty>().empty = this;
                        }
                    }
                }
                break;
            case 11: //技能11：黑色目光
                if (UsedMeanLook)
                {
                    //防止连续使用两次黑色目光
                    return;
                }
                GameObject MeanLookse = Instantiate(MeanLookSE, transform.position, Quaternion.identity);
                Destroy(MeanLookse, 1f);
                StartCoroutine(ReleaseMeanLook());
                IEnumerator ReleaseMeanLook()
                {
                    yield return new WaitForSeconds(1f);
                    GameObject blackCircle = Instantiate(MeanLookPrefab, player.transform.position, Quaternion.identity);
                    UsedMeanLook = true;

                    // 获取黑色目光的半径
                    float circleRadius = blackCircle.GetComponent<CircleCollider2D>().radius;

                    // 在黑色目光内限制玩家的移动
                    while (blackCircle != null)
                    {
                        // 计算玩家与黑色目光的距离
                        float distance = Vector2.Distance(player.transform.position, blackCircle.transform.position);

                        if (distance > circleRadius)
                        {
                            // 如果玩家距离黑色目光的距离大于圆半径，则将玩家移动回黑色目光的范围内
                            Vector3 direction = (player.transform.position - blackCircle.transform.position).normalized;
                            player.transform.position = blackCircle.transform.position + direction * circleRadius;
                        }

                        yield return null;
                    }
                }
                break;
            case 12://技能12：魔法闪耀
                GameObject dazzlingGleam = Instantiate(DazzlingGleamPrefab, transform.position, Quaternion.identity);
                Destroy(dazzlingGleam, 5f);
                // 在3.5秒后对圈内的玩家造成伤害
                StartCoroutine(DamagePlayersWithDelay());
                IEnumerator DamagePlayersWithDelay()
                {
                    yield return new WaitForSeconds(3.5f);

                    // 获取圈内的玩家并对其造成伤害
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5.85f);
                    foreach (Collider2D collider in colliders)
                    {
                        // 判断碰撞体是否属于玩家
                        PlayerControler playerinside = collider.GetComponent<PlayerControler>();
                        if (playerinside != null)
                        {
                            Pokemon.PokemonHpChange(this.gameObject, playerinside.gameObject, 0, 120, 0, Type.TypeEnum.Fairy);
                            playerinside.KnockOutPoint = 5f;
                            playerinside.KnockOutDirection = (playerinside.transform.position - transform.position).normalized;
                        }
                    }
                }
                break;
            case 13://技能13：叶刃
                StartCoroutine(ReleaseLeafBlade());
                IEnumerator ReleaseLeafBlade()
                {
                    int shootCount = 10; // 发射次数
                    float shootInterval = 0.3f; // 发射间隔
                    if(currentPhase != 1)
                    {
                        shootCount = 15;
                        shootInterval = 0.2f;
                    }
                    for (int i = 0; i < shootCount; i++)
                    {
                        // 实例化LeafBlade
                        GameObject LeafBlade = Instantiate(LeafBladePrefab, transform.position, Quaternion.identity);
                        LeafBlade.GetComponent<LeafBladeEmpty>().empty = this;
                        // 等待发射间隔
                        yield return new WaitForSeconds(shootInterval);
                    }
                }
                break;
            case 14://技能14：尖石攻击
                if (currentPhase != 1)
                {
                    return;
                }
                StartCoroutine(ReleaseStoneEdge());
                IEnumerator ReleaseStoneEdge()
                {
                    int Times = 2;
                    float mapLength = 26f;
                    for (int i = 0; i< Times; i++)
                    {
                        //选择一个不生成StoneEdge的位置
                        int emptyPosition = Random.Range(1, 14);
                        for (int j = 1; j <= 13; j++)
                        {
                            if (j != emptyPosition)
                            {
                                // 生成Reticle的位置
                                float spaceLength = mapLength / 13f;
                                float startX = mapCenter.x + (j - 7) * spaceLength;
                                reticleSpawnPosition = new Vector3(startX, mapCenter.y, mapCenter.z);

                                // 生成Reticle
                                GameObject reticle = Instantiate(reticlePrefab, reticleSpawnPosition, Quaternion.identity);
                                Destroy(reticle, 2f);
                                //生成尖石
                                Vector3 StoneEdgeSpawnPosition = new Vector3(reticleSpawnPosition.x, reticleSpawnPosition.y + 14f, reticleSpawnPosition.z);
                                GameObject stoneEdge = Instantiate(stoneEdgePrefab, StoneEdgeSpawnPosition, Quaternion.identity);
                                stoneEdge.GetComponent<StoneEdgeEmpty>().empty = this;
                            }
                        }
                        yield return new WaitForSeconds(5f);
                    }
                }
                break;
            case 15://技能15：空气之刃
                StartCoroutine(ReleaseAirSlash());
                IEnumerator ReleaseAirSlash()
                {
                    float intervalTime = 1.3f;
                    int Times = 3;
                    if (currentPhase != 1)
                    {
                        intervalTime = 0.8f;
                        Times = 5;
                    }
                    //每次释放空气之刃后等待1.3f
                    for(int i = 0;i< Times;i++)
                    {
                        GameObject airSlash = Instantiate(AirSlashPrefab, transform.position, Quaternion.identity);
                        airSlash.GetComponent<AirSlashMew>().empty = this;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 16://技能16：淘金潮
                StartCoroutine(ReleaseMakeItRain());
                IEnumerator ReleaseMakeItRain()
                {
                    transform.position = mapCenter;
                    float angle = 0f;
                    float angleIncrement = 8f;
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            float currentAngle = angle + j * 120f;
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            MakeItRainEmpty makeitrain = Instantiate(MakeItRainPrefab, transform.position, Quaternion.identity).GetComponent<MakeItRainEmpty>();
                            makeitrain.MIRrotate(direction);
                            makeitrain.empty = this;
                        }
                        angle += angleIncrement;
                        yield return new WaitForSeconds(0.07f);
                    }
                }
                break;
            case 17://技能17：黏黏网
                if (currentPhase == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector3 randomPosition = mapCenter + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);
                        GameObject StickyWeb = Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
                    }
                }
                else
                {
                    for (int i = 0; i < 14; i++)
                    {
                        Vector3 randomPosition = mapCenter + new Vector3(Random.Range(-24.0f, 24.0f), Random.Range(-14.0f, 14.0f), 0);
                        GameObject StickyWeb = Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
                    }
                }
                break;
            case 18://技能18：十字毒刃
                StartCoroutine(ReleaseCrossPoison());
                IEnumerator ReleaseCrossPoison()
                {
                    //释放技能后等待的时间
                    yield return new WaitForSeconds(1f);
                    float angle = 0f;
                    float angleIncrement = 45f;
                    int Times = 4;
                    float intervalTime = 1f;
                    if (currentPhase != 1)
                    {
                        Times = 5;
                        angleIncrement = 60f;
                        intervalTime = 0.8f;
                    }
                    for (int i= 0; i < Times; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            float currentAngle = angle + j * 90f;
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            CrossPoisonEmpty crossPoison = Instantiate(CrossPoisonPrefab, transform.position, Quaternion.identity).GetComponent<CrossPoisonEmpty>();
                            crossPoison.CProtate(direction);
                            crossPoison.empty = this;
                        }
                        //技能间隔等待时间
                        angle += angleIncrement;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 19://技能19：神圣之火
                StartCoroutine(ReleaseScaredFire());
                IEnumerator ReleaseScaredFire()
                {
                    int numPoints = 5; // 五角星上的点数
                    float radius = 12f; // 五角星的顶点到中心的距离

                    Vector3[] starVertices = new Vector3[numPoints];
                    Vector3[] secredFirePositions = new Vector3[numPoints * 14]; // 存储SecredFire的位置

                    // 创建五角星的顶点坐标
                    for (int i = 0; i < numPoints; i++)
                    {
                        float angle = i * 2f * Mathf.PI / numPoints;
                        float x = radius * Mathf.Sin(angle) + player.transform.position.x;
                        float y = radius * Mathf.Cos(angle) + player.transform.position.y;
                        starVertices[i] = new Vector3(x, y, player.transform.position.z);
                    }
                    // 生成圆弧中心的SecredFire
                    for (int i = 0; i < numPoints; i++)
                    {
                        Vector3 startPoint = starVertices[i];
                        Vector3 endPoint = starVertices[(i + 1) % numPoints];

                        Vector3 center = (startPoint + endPoint) / 2f;

                        GameObject secredFireCenter = Instantiate(SecredFireCentrePrefab, center, Quaternion.identity);
                        secredFireCenter.GetComponent<SecredFireEmpryCentre>().empty = this;
                        secredFirePositions[numPoints * 12 + i] = center;
                    }
                    // 在每个五角星顶点生成SecredFire
                    for (int i = 0; i < numPoints; i++)
                    {
                        GameObject secredFireVertex = Instantiate(SecredFireVertexPrefab, starVertices[i], Quaternion.identity);
                        secredFireVertex.GetComponent<SecredFireEmptyVertex>().empty = this;
                        secredFirePositions[numPoints * 12 + numPoints + i] = starVertices[i];
                    }

                    // 在每条线上均匀分布生成SecredFire
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
                            SecredFireEmpty secredFire = Instantiate(SecredFirePrefab, secredFirePosition, Quaternion.identity).GetComponent<SecredFireEmpty>();
                            secredFirePositions[(i * 12) + j] = secredFirePosition;
                            secredFire.empty = this;
                            secredFire.Initialize(player.transform.position, 2f);
                            yield return null;
                        }
                    }
                    yield return null;
                }
                break;
            case 20://技能20：圣剑
                StartCoroutine(ReleaseSecredSword());
                IEnumerator ReleaseSecredSword()
                {   
                    for(int i = 0;i<3; i++)  
                    {
                        Instantiate(reticle2Prefab, player.transform.position, Quaternion.identity);
                        for(int j = 0; j < 6; j++)
                        {
                            float angle = j * 60;
                            float radius = 8f;
                            Vector3 spawnPos = player.transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
                            SecredSwordEmpty secredSword = Instantiate(SecredSwordPrefab, spawnPos, Quaternion.identity).GetComponent<SecredSwordEmpty>();
                            secredSword.Initialize(angle, radius);
                            
                        }
                        yield return new WaitForSeconds(3f);
                    }

                }
                break;
        }
        //二阶段总共移除的技能：暴风雪（2）、和睦相处（4）、咒术（6）和尖石攻击（14）
    }
    void SkillTimerUpdate(int skillindex, int stage)//技能重置时间调整
    {
        switch (skillindex)
        {
            case 1:skillTimer = 2.4f;
                break;
            case 2:
                if (stage == 1)
                    skillTimer = 1.5f;
                break;
            case 3:
                if (stage == 1)
                    skillTimer = 3f;
                else
                    skillTimer = 3.5f;
                break;
            case 4:if (stage == 1)
                    skillTimer = 2f;
                break;
            case 5:if (stage == 1) skillTimer = 4.7f;
                else skillTimer = 9.2f;
                break;
            case 6:if (stage == 1)skillTimer = 2.9f;break;
            case 7:if (stage == 1) skillTimer = 2.6f;
                else skillTimer = 3.6f; 
                break;
            case 8: if (stage == 1) skillTimer = 5.5f;
                else skillTimer = 4.4f; 
                    break;
            case 9:  skillTimer = 4.8f; break;
            case 10: if (stage == 1) skillTimer = 4.3f;
                else skillTimer = 3f;
                break;
            case 11: if (stage == 1) skillTimer = 2f;
                else skillTimer = 1.7f;
                break;
            case 12: skillTimer = 3.7f; break;
            case 13: if (stage == 1) skillTimer = 3.3f;
                else skillTimer = 3.2f;
                break;
            case 14: if (stage == 1) skillTimer = 10f; break;
            case 15: if(stage == 1) skillTimer = 4.2f;
                else skillTimer = 5f;
                break;
            case 16: skillTimer = 9f; break;
            case 17: skillTimer = 1.8f;
                break;
            case 18: if (stage == 1) skillTimer = 10f;
                else skillTimer = 6.5f;
                break;
            case 19: skillTimer = 6f; break;
            case 20: skillTimer = 6f; break;
        }
    }
    void RamdomTeleport()
    {
        bool collided = false;
        Vector3 randomPosition;
        teleportAttempts++;
        if (teleportAttempts > 150)
        {
            // 如果尝试次数超过150次，则取消随机传送
            Debug.Log("没有成功找到合适的传送位置");
            return;
        }
        //一阶段随机传送：地图内随机位置
        if (currentPhase == 1)
        {
            // 在房间内随机选择一个位置w
            randomPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);

            // 检查与"Wall"和"Environment"标签的对象是否相撞
            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room") || collider.CompareTag("Enviroment"))
                {
                    collided = true;
                    break;
                }
            }
            float playerRadius = 3f;
            float distanceToPlayer = Vector3.Distance(randomPosition, player.transform.position);

            if (distanceToPlayer <= playerRadius)
            {
                collided = true;
            }
            if (collided)
            {
                //如果相撞重新寻找位置
                RamdomTeleport();
            }
            else
            {
                //反之进行瞬间移动
                transform.position = randomPosition;
                //更新计数器
                teleportAttempts = 0;
            }
        }
        //二阶段随机传送：玩家周围
        if(currentPhase == 2)
        {
            float minDistance = 5f;
            float maxDistance = 10f;
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minDistance, maxDistance);
            randomPosition = player.transform.position + randomDirection * randomDistance;
            // 检查与"Room"和"Environment"标签的对象是否相撞
            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room") || collider.CompareTag("Enviroment"))
                {
                    collided = true;
                    break;
                }
            }
            //检查是否地图边界内
            if (!IsInMapBounds(randomPosition))
                collided = true;
            if (collided)
            {
                //如果相撞重新寻找位置
                RamdomTeleport();
            }
            else
            {
                //反之进行瞬间移动
                transform.position = randomPosition;
                //更新计数器
                teleportAttempts = 0;
            }
        }
    }
    bool IsInMapBounds(Vector3 position)
    {
        // 根据地图边界的坐标范围，判断位置是否在地图内
        float minX = 2985f;
        float maxX = 3045f;
        float minY = 2388f;
        float maxY = 2436f;

        bool isInBounds = position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
        return isInBounds;
    }
    private IEnumerator Phase1Skill()
    {
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);
        RamdomTeleport();

        //随机选择技能释放
        int randomSkillIndex = Random.Range(1, 19);
        UseSkillMask(randomSkillIndex);
        yield return new WaitForSeconds(0.5f);
        UseSkill(randomSkillIndex);
        SkillTimerUpdate(randomSkillIndex, 1);
    }
    private IEnumerator Phase2Skill(int randomSkillIndex)
    {
        if (randomSkillIndex == 2 || randomSkillIndex == 4 || randomSkillIndex == 6 || randomSkillIndex == 14)
        {
            yield break;
        }
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);
        RamdomTeleport();
        UseSkillMask(randomSkillIndex);
        yield return new WaitForSeconds(0.5f);
        UseSkill(randomSkillIndex);
        SkillTimerUpdate(randomSkillIndex, 2);
    }
    private IEnumerator Phase2Start()
    {
        //清除所有的子弹
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }

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
        // 在每条线上均匀分布生成SecredFire
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
                Destroy(willowispprefab, 4f);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);
        GameObject phase2mask = Instantiate(Phase2Mask, transform.position, Quaternion.identity);
        Destroy(phase2mask, 2.2f);
        yield return new WaitForSeconds(1.1f);
;
        //创建新的房间
        GameObject newRoom = Instantiate(MewBossRoomPrefab, MewBossRoomPosition, Quaternion.identity);
        mapCenter = MewBossRoomPosition + new Vector3(15f, 12f, 0f);
        transform.position = MewBossRoomPosition + new Vector3(15f, 12f, 0f);
        player.transform.position = MewBossRoomPosition;
        player.NowRoom = new Vector3Int(100, 100, 0);
        player.InANewRoom = true;
        player.NewRoomTimer = 0f;

        Transform mewTransform = newRoom.transform.Find("Empty");
        if (mewTransform != null)
        {
            //将梦幻移动到Empty子对象下
            transform.SetParent(mewTransform);
        }
        //色相头，启动！
        cinemachineController.MewCameraFollow();
        cameraAdapt.HideCameraMasks();
        yield return new WaitForSeconds(1.5f);
        turningPhase = false;

    }
    private void UseSkillMask(int randomSkillIndex)
    {
        GameObject useskillprefab = Instantiate(UseSkillPrefab, transform.position, Quaternion.identity);
        UseSkill useskillmask = useskillprefab.GetComponent<UseSkill>();
        useskillmask.originalColor = Color.white;
        switch (randomSkillIndex)
        {
            case 1: useskillmask.targetColor = Color.green; break;
            case 2: useskillmask.targetColor = Color.cyan; break;
            case 3: useskillmask.targetColor = new Color(0.2f, 0f, 0.2f); break;//紫黑色
            case 4: useskillmask.targetColor = new Color(1f, 0.75f, 0.8f); break;//粉色
            case 5: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); useskillmask.originalColor = Color.red; break;
            case 6: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); break;
            case 7: useskillmask.targetColor = Color.red; break;
            case 8: useskillmask.targetColor = Color.cyan; break;
            case 9: useskillmask.targetColor = new Color(1f, 0f, 0.6f); break;//洋红色
            case 10: useskillmask.targetColor = new Color(0f, 0f, 0.5f); break;//深蓝色
            case 11: useskillmask.targetColor = Color.black; break;
            case 12: useskillmask.targetColor = new Color(1f, 0.75f, 0.8f); break;
            case 13: useskillmask.targetColor = Color.green; break;
            case 14: useskillmask.targetColor = new Color(0.7f, 0.5f, 0.3f); break;//浅棕色
            case 15: useskillmask.targetColor = Color.cyan; break;
            case 16: useskillmask.targetColor = Color.grey; break;
            case 17: useskillmask.targetColor = new Color(0.5f, 1f, 0.5f); break;//浅绿色
            case 18: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); break;//紫色
            case 19: useskillmask.targetColor = Color.red; break;
            case 20: useskillmask.targetColor = new Color(1f, 0.5f, 0f); break;
        }
    }
    void ClearStatusEffects()
    {
        //清除所有debuff
        this.EmptyCurseRemove();
        this.ColdRemove();
        this.EmptyConfusionRemove();
        this.EmptyInfatuationRemove();
        this.EmptyParalysisRemove();
        this.EmptySleepRemove();
        this.EmptyBurnRemove();
        this.BlindRemove();
        this.FearRemove();
        this.FrozenRemove();
    }
}
