using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mew : Empty
{
    public GameObject magicalLeafPrefab;//技能1
    public float delayBetweenLeaves = 0.3f; // 每片MagicalLeaf之间的延迟时间
    int Leafnum;//魔法叶数量
    public GameObject blizzardPrefab;//技能2
    public GameObject WillOWispPrefab;//技能3
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
    public int icicleCount = 8;
    public float delayBetweenExecutions = 1f;
    public int numExecutions = 3;
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
    public float shootInterval = 0.3f; // 发射间隔
    public int shootCount = 10; // 发射次数
    public GameObject stoneEdgePrefab;//技能14
    private Vector3 mapCenter;  // 地图中心点
    private Vector3 reticleSpawnPosition; // Reticle生成位置
    public GameObject AirSlashPrefab;//技能15
    public GameObject MakeItRainPrefab;//技能16
    public GameObject StickyWebPrefab;//技能17
    public GameObject CrossPoisonPrefab;//技能18

    float skillCooldown;
    //Audio
    public BackGroundMusic bgmScript;
    //阶段
    private int currentPhase = 1; // 当前阶段
    private float skillTimer = 0f; // 技能计时器
    private List<GameObject> heartStamps = new List<GameObject>();//对HeartStamp进行存储
    //随机传送计数器
    private int teleportAttempts = 0;

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

        //地图
        mapCenter = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            UpdateEmptyChangeHP();
            bgmScript.ChangeBGMToMew();
            ClearStatusEffects();
            if (currentPhase == 3)
            {
                EmptyDie();
            }
            else
            {
                switch (currentPhase)
                {
                    case 1:Phase1();
                        break;
                    case 2:Phase2();
                        break;
                    case 3:Phase3();
                        break;
                }
            }
        }
    }
    void Phase1()
    {
        if (EmptyHp <= maxHP/4)
        {
            currentPhase++;
            Debug.Log("进入二阶段");
            EmptyHp = maxHP;
        }
        else if (skillTimer <= 0f)
        {
            // 随机选择一个技能释放
            RamdomTeleport();
            int randomSkillIndex = Random.Range(1, 19);
            UseSkill(randomSkillIndex);
            SkillTimerUpdate(randomSkillIndex, 1);
        }

        // 技能计时器递减
            skillTimer -= Time.deltaTime;
    }
    void Phase2()
    {
        if (EmptyHp <= 0)
        {
            currentPhase++;
            Debug.Log("进入三阶段");
            EmptyHp = maxHP;
        }
        else if (skillTimer <= 0f)
        {
            // 随机选择一个技能释放
            int randomSkillIndex = Random.Range(1, 25);
            UseSkill(randomSkillIndex);

            // 重置技能计时器
            skillTimer = skillCooldown;
        }

        // 技能计时器递减
        skillTimer -= Time.deltaTime;
    }
    void Phase3()
    {
        //让梦幻无敌，此时无法受伤
        Invincible = true;
    }
    void UseSkill(int skillIndex)
    {
        // 在这里编写释放技能的代码
        Debug.Log("Boss used skill: " + skillIndex);
        switch (skillIndex)
        {
            case 1:
                //技能1：魔法叶
                if (currentPhase == 1)
                {
                    Leafnum = 3;
                }
                else
                {
                    Leafnum = 5;
                }
                StartCoroutine(ReleaseLeaves());
                IEnumerator ReleaseLeaves()
                {
                    for (int i = 0; i < Leafnum; i++)
                    {
                        // 在Mew位置实例化魔法叶
                        GameObject magicalLeaf = Instantiate(magicalLeafPrefab, transform.position, Quaternion.identity);
                        Destroy(magicalLeaf, 6f); // 6秒后销毁魔法叶对象
                        yield return new WaitForSeconds(delayBetweenLeaves);
                    }
                }
                break;
            case 2:
                //技能2：暴风雪
                GameObject blizzard = Instantiate(blizzardPrefab, transform.position, Quaternion.identity);
                Destroy(blizzard, 6f);//6秒后销毁暴风雪对象
                break;
            case 3://技能3：磷火
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float angleStep = 360f / numWillOWisp; // 计算每个WillOWisp之间的角度间隔
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = i * angleStep; // 计算当前WillOWisp的角度
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius; // 计算当前WillOWisp的生成位置
                            GameObject willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity);
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.GetComponent<WillOWispEmpty>().Initialize(moveSpeed, direction); // 设置WillOWisp的移动速度
                            Destroy(willOWisp, 4f);
                        }
                        yield return new WaitForSeconds(1f);
                    }
                }
                break;
            case 4://技能4：和睦相处
                GameObject PlayNice = Instantiate(PlayNicePrefab, transform.position, Quaternion.identity);
                Destroy(PlayNice, 5f);
                break;
            case 5://技能5：太晶爆发
                //释放3道激光，分别位于90度、210度、330度的位置
                transform.position = transform.parent.position;
                //创建三条激光
                float[] angles = { 90f, 210f, 330f };
                for (int i = 0; i < angles.Length; i++)
                {
                    //计算激光的起始点和终点
                    float angle = angles[i];
                    Vector3 startPoint = transform.parent.position;
                    Vector3 endPoint = new Vector3(15f * Mathf.Cos(Mathf.Deg2Rad * angle), 15f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                    GameObject Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity);
                    TeraBlastEmpty terablast = Terablast.GetComponent<TeraBlastEmpty>();
                    terablast.SetEndpoints(startPoint, endPoint, angle);
                    terablast.SetColors(Color.yellow, Color.red);
                }
                break;
            case 6://技能6：咒术
                StartCoroutine(ReleaseCurse());
                IEnumerator ReleaseCurse()
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject Curse = Instantiate(CursePrefab, player.transform.position, Quaternion.identity);
                        Destroy(Curse, 4f);
                        yield return new WaitForSeconds(0.6f);
                    }
                }
                break;
            case 7://技能7：魔法火焰
                StartCoroutine(ReleaseMagicalFire());
                IEnumerator ReleaseMagicalFire()
                {
                    float angleIncrement = 360f / 8;
                    float rotationSpeed = 60f;
                    for (int j = 0; j < 3; j++) {
                        for (int i = 0; i < 8; i++)
                        {
                            float angle = i * angleIncrement;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                            GameObject magicalFire = Instantiate(MagicalFirePrefab, transform.position, rotation);
                            MagicalFireEmpty magicalFireScript = magicalFire.GetComponent<MagicalFireEmpty>();
                            magicalFireScript.ps(transform.position, rotationSpeed);
                            Destroy(magicalFire, 7f);

                        }
                        yield return new WaitForSeconds(1f);
                    }
                }
                break;

            case 8://技能8：冰锥
                StartCoroutine(SummonIcicleSpears());
                break;
            case 9://技能9：爱心印章
                StartCoroutine(ReleaseHeartStamp());
                IEnumerator ReleaseHeartStamp()
                {
                    for (int j = 0; j < 3; j++) {
                        float angleIncrement = 360f / heartStampCount;
                        for (int i = 0; i < heartStampCount; i++)
                        {
                            float angle = i * angleIncrement;
                            Vector3 heartStampPosition = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * HeartStampRadius;

                            GameObject heartStamp = Instantiate(HeartStampPrefab, heartStampPosition, Quaternion.identity);
                            heartStamps.Add(heartStamp);
                            HeartStampEmpty heartStampMovement = heartStamp.GetComponent<HeartStampEmpty>();

                            if (heartStampMovement != null)
                            {
                                heartStampMovement.SetTarget(player.transform.position);
                            }
                        }
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                break;
            case 10://技能10：鳞射
                // 生成ScaleShot
                StartCoroutine(ReleaseScaleShoot());
                IEnumerator ReleaseScaleShoot()
                {
                    for (int j = 0; j < 3; j++)
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
                        }
                    }
                }
                break;
                case 11: //技能11：黑色目光
                GameObject MeanLookse = Instantiate(MeanLookSE, transform.position, Quaternion.identity);
                Destroy(MeanLookse, 1f);
                StartCoroutine(ReleaseMeanLook());
                IEnumerator ReleaseMeanLook()
                {
                    yield return new WaitForSeconds(1f);
                    GameObject blackCircle = Instantiate(MeanLookPrefab, player.transform.position, Quaternion.identity);

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
                        }
                    }
                }
                break;
            case 13://技能13：叶刃
                StartCoroutine(ReleaseLeafBlade());
                IEnumerator ReleaseLeafBlade()
                {
                    for (int i = 0; i < shootCount; i++)
                    {
                        // 实例化LeafBlade
                        GameObject LeafBlade = Instantiate(LeafBladePrefab, transform.position, Quaternion.identity);
                        // 等待发射间隔
                        yield return new WaitForSeconds(shootInterval);
                    }
                }
                break;
            case 14://技能14：尖石攻击
                StartCoroutine(ReleaseStoneEdge());
                IEnumerator ReleaseStoneEdge()
                {
                    for (int i = 0; i< 2; i++)
                    {
                        float mapLength = 26f;
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
                    //每次释放空气之刃后等待1.3f
                    for(int i = 0;i< 3;i++)
                    {
                        GameObject airSlash = Instantiate(AirSlashPrefab, transform.position, Quaternion.identity);
                        yield return new WaitForSeconds(1.3f);
                    }
                }
                break;
            case 16://技能16：淘金潮
                transform.position = transform.parent.position;
                StartCoroutine(ReleaseMakeItRain());
                IEnumerator ReleaseMakeItRain()
                {
                    float angle = 0f;
                    float angleIncrement = 8f;
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            float currentAngle = angle + j * 120f;
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            GameObject makeitrain = Instantiate(MakeItRainPrefab, transform.position, Quaternion.identity);
                            MakeItRainEmpty Makeitrain = makeitrain.GetComponent<MakeItRainEmpty>();
                            Makeitrain.MIRrotate(direction);
                        }
                        angle += angleIncrement;
                        yield return new WaitForSeconds(0.07f);
                    }
                }
                break;
            case 17://技能17：黏黏网
                for(int i = 0; i < 5; i++)
                {
                    Vector3 randomPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);
                    GameObject StickyWeb = Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
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
                    for (int i= 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            float currentAngle = angle + j * 90f;
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            GameObject crossPoison = Instantiate(CrossPoisonPrefab, transform.position, Quaternion.identity);
                            CrossPoisonEmpty crosspoison = crossPoison.GetComponent<CrossPoisonEmpty>();
                            crosspoison.CProtate(direction);
                        }
                        //技能间隔等待时间
                        angle += angleIncrement;
                        yield return new WaitForSeconds(1f);
                    }
                }
                break;


        }
    }
    void SkillTimerUpdate(int skillindex, int stage)//技能重置时间调整
    {
        switch (skillindex)
        {
            case 1:if(stage == 1)skillTimer = 2.4f;break;
            case 2:if(stage == 1)skillTimer = 1.5f;break;
            case 3:if (stage == 1) skillTimer = 3f;break;
            case 4:if (stage == 1)skillTimer = 1.5f;break;
            case 5:if (stage == 1)skillTimer = 4.7f;break;
            case 6:if (stage == 1)skillTimer = 2.9f;break;
            case 7:if (stage == 1) skillTimer = 2.6f;break;
            case 8: if (stage == 1) skillTimer = 4.5f; break;
            case 9: if (stage == 1) skillTimer = 4.8f; break;
            case 10: if (stage == 1) skillTimer = 4.3f; break;
            case 11: if (stage == 1) skillTimer = 2f; break;
            case 12: if (stage == 1) skillTimer = 3.7f; break;
            case 13: if (stage == 1) skillTimer = 3.3f; break;
            case 14: if (stage == 1) skillTimer = 10f; break;
            case 15: if (stage == 1) skillTimer = 4.2f; break;
            case 16: if (stage == 1) skillTimer = 9f; break;
            case 17: if (stage == 1) skillTimer = 1.8f; break;
            case 18: if (stage == 1) skillTimer = 10f; break;
        }
    }
    void RamdomTeleport()
    {
        teleportAttempts++;
        if (teleportAttempts > 150)
        {
            // 如果尝试次数超过150次，则取消随机传送
            Debug.Log("没有成功找到合适的传送位置");
            return;
        }
        // 在房间内随机选择一个位置
        Vector3 randomPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);

        // 检查与"Wall"和"Environment"标签的对象是否相撞
        Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 1f);
        bool collided = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Room") || collider.CompareTag("Enviroment"))
            {
                collided = true;
                break;
            }
            float playerRadius = 3f;
            float distanceToPlayer = Vector3.Distance(randomPosition, player.transform.position);
            if (distanceToPlayer <= playerRadius)
            {
                collided = true;
                break;
            }
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

    private IEnumerator SummonIcicleSpears()
    {
        for (int execution = 0; execution < numExecutions; execution++)
        {
            for (int i = 0; i < icicleCount; i++)
            {
                float angle = i * (360f / icicleCount);
                Vector2 spawnPosition = player.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * summonRadius);
                GameObject IcicleSpear = Instantiate(IcicleSpearPrefab, spawnPosition, Quaternion.identity);

                IcicleSpearEmpty icicleSpear = IcicleSpear.GetComponent<IcicleSpearEmpty>();
                icicleSpear.sf(player.transform.position);
            }
            yield return new WaitForSeconds(delayBetweenExecutions);
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
    int CalculateBossHealth()
    {
        int playerHealth = player.Hp;
        int playerMaxHealth = player.maxHp;
        return Mathf.FloorToInt(maxHP * (playerHealth / (float)playerMaxHealth));
    }
}
