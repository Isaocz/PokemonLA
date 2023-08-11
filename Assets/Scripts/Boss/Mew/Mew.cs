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
    public GameObject MeanLookPrefab;//技能11
    public GameObject DazzlingGleamPrefab;//技能12

    float skillCooldown;
    //Audio
    public BackGroundMusic bgmScript;
    //阶段
    private int currentPhase = 1; // 当前阶段
    private float skillTimer = 0f; // 技能计时器
    private List<GameObject> heartStamps = new List<GameObject>();//对HeartStamp进行存储
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
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            UpdateEmptyChangeHP();
            bgmScript.ChangeBGMToMew();
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
        if (EmptyHp <= 0)
        {
            currentPhase++;
            Debug.Log("进入二阶段");
            EmptyHp = CalculateBossHealth();
            ClearStatusEffects();
        }
        else if (skillTimer <= 0f)
        {
            // 随机选择一个技能释放
            RamdomTeleport();
            int randomSkillIndex = Random.Range(1, 13);
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
            EmptyHp = 1;
            ClearStatusEffects();
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

                            // 由于ScaleShot生成后方向不同，所以需要给ScaleShot添加一个控制移动的脚本
                            ScaleShotEmpty scaleShotMovement = scaleShot.GetComponent<ScaleShotEmpty>();
                        }
                    }
                }
                break;
                case 11: //技能11：黑色目光
                StartCoroutine(ReleaseMeanLook());
                IEnumerator ReleaseMeanLook()
                {
                    yield return new WaitForSeconds(1f);
                    GameObject blackCircle = Instantiate(MeanLookPrefab, player.transform.position, Quaternion.identity);

                    // 获取黑色圆的半径
                    float circleRadius = blackCircle.GetComponent<CircleCollider2D>().radius;

                    // 在黑色圆范围内限制玩家的移动
                    while (blackCircle != null)
                    {
                        // 计算玩家与黑色圆的距离
                        float distance = Vector2.Distance(player.transform.position, blackCircle.transform.position);

                        if (distance > circleRadius)
                        {
                            // 如果玩家距离黑色圆的距离大于圆半径，则将玩家移动回黑色圆的范围内
                            Vector3 direction = (player.transform.position - blackCircle.transform.position).normalized;
                            player.transform.position = blackCircle.transform.position + direction * circleRadius;
                        }

                        yield return null;
                    }
                }
                break;
            case 12:
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
            case 9: if (stage == 1) skillTimer = 4.5f; break;
            case 10: if (stage == 1) skillTimer = 4.5f; break;
            case 11: if (stage == 1) skillTimer = 2f; break;
            case 12: if (stage == 1) skillTimer = 3.2f; break;
        }
    }
    void RamdomTeleport()
    {
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
        // 在这里编写清除异常状态的代码
        isSilence = false;
        Debug.Log("Boss cleared status effects");
    }
    int CalculateBossHealth()
    {
        int playerHealth = player.Hp;
        int playerMaxHealth = player.maxHp;
        return Mathf.FloorToInt(maxHP * (playerHealth / (float)playerMaxHealth));
    }
}
