using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Dynamic;

public class Mew : Empty
{
    public GameObject magicalLeafPrefab;//����1
    public float delayBetweenLeaves = 0.3f; // ÿƬMagicalLeaf֮����ӳ�ʱ��
    int Leafnum;//ħ��Ҷ����
    public GameObject blizzardPrefab;//����2
    public GameObject WillOWispPrefab;//����3
    public int WillOWispDegree = 2;//WillOWisp�ظ�����
    public int numWillOWisp = 12; // WillOWisp������
    public float WillOWispRadius = 2f; // WillOWisp�����ɰ뾶
    public float moveSpeed = 4f; // WillOWisp���ƶ��ٶ�
    public GameObject PlayNicePrefab;//����4
    public GameObject TeraBlastPrefab;//����5
    public float laserLength = 10f; // ����ĳ���
    public float laserOffset = 1f; // �����ƫ����
    public LayerMask obstacleLayer;
    public GameObject CursePrefab;//����6
    public GameObject MagicalFirePrefab;//����7
    public GameObject IcicleSpearPrefab;//����8
    public float summonRadius = 5f;
    public float delayBetweenExecutions = 1f;
    public GameObject HeartStampPrefab;//����9
    public float HeartStampRadius=1.5f;
    public int heartStampCount = 8;
    public GameObject reticlePrefab; //ReticleԤ����
    public GameObject ScaleShotPrefab; //����10
    public int scaleShotCount;
    public GameObject MeanLookSE;//��ɫĿ����Ч
    public GameObject MeanLookPrefab;//����11
    public GameObject DazzlingGleamPrefab;//����12
    public GameObject LeafBladePrefab; // ����13
    public GameObject stoneEdgePrefab;//����14
    private Vector3 mapCenter;  // ��ͼ���ĵ�
    private Vector3 reticleSpawnPosition; // Reticle����λ��
    public GameObject AirSlashPrefab;//����15
    public GameObject MakeItRainPrefab;//����16
    public GameObject StickyWebPrefab;//����17
    public GameObject CrossPoisonPrefab;//����18
    public GameObject SecredFirePrefab;//����19
    public GameObject SecredFireCentrePrefab;
    public GameObject SecredFireVertexPrefab;
    public GameObject reticle2Prefab;//Reticle2Ԥ����
    public GameObject SecredSwordPrefab;//����20
    public GameObject Phase2Mask;
    public GameObject UseSkillPrefab;
    public GameObject Phase3OrbRotate;
    public GameObject ElectricBallPrefab;//����21
    public GameObject TimeStopEffect;
    public GameObject TrailEffect;
    public GameObject IceBeamPrefab;//����22
    public GameObject FakePotionPrefab;//����ҩ
    public GameObject FakeLovePrefab;//����

    //�л�����ʱ�ȴ�
    private float MeanLookTimer = 0f;
    private bool UsedMeanLook = false;
    private bool turningPhase = false;

    //Audio
    public BackGroundMusic bgmScript;
    //�׶�
    public int currentPhase = 1; // ��ǰ�׶�
    public bool LaserChange = false;
    private float skillTimer = 0f; // ���ܼ�ʱ��
    private List<GameObject> heartStamps = new List<GameObject>();//��HeartStamp���д洢
    private float MoreAttackTimer;//���׶θ��༼�ܼ�ʱ��
    private int[] MoreAttackSkill = { 1, 8, 9, 10, 11, 13, 15, 17, 18, 20 };
    private int MoreAttackSkillIndex;
    private bool isMasked = false;//���༼���ͷ�����
    private bool isPhase3 = false;

    //�������
    private int teleportAttempts = 0;//������ͼ�����

    //����
    public GameObject MewBossRoomPrefab;
    public Vector3 MewBossRoomPosition = new Vector3(60f, 60f, 0f);
    private bool roomCreated = false;
    private Vector3Int GetnowRoom;

    //�������
    private CameraController cinemachineController;
    private CameraAdapt cameraAdapt;

    //ʱ��Ѫ��
    private float HpTimer = 216f;
    private float HpTiming = 216f;

    // Start is called before the first frame update
    void Start()
    {
        //Audio
        bgmScript = BackGroundMusic.StaticBGM;

        //�λõĻ�������
        EmptyType01 = Type.TypeEnum.Psychic;//�λõ�����Ϊ����
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, MaxLevel);//���޵ȼ�100
        EmptyHpForLevel(Emptylevel);//���ó�ʼѪ��

        //�����ȼ�
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;//����ֵ

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cameraAdapt = FindObjectOfType<CameraAdapt>();

        //��ͼ
        mapCenter = transform.parent.position;
        GetnowRoom = player.NowRoom;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            ClearStatusEffects();
            if (currentPhase == 3)
            {
                bgmScript.ChangeBGMINSIST();
                Phase3();
                HpTiming -= Time.deltaTime;
                uIHealth.Per = HpTiming / HpTimer;
                uIHealth.ChangeHpDown();
                //������ҵ��ƶ��뾶
                float distance = Vector2.Distance(player.transform.position, transform.position);
                if (distance > 20f)
                {
                    // �����Ҿ����ɫĿ��ľ������Բ�뾶��������ƶ��غ�ɫĿ��ķ�Χ��
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    player.transform.position = transform.position + direction * 20f;
                }
                EmptyDie();
            }
            else
            {
                UpdateEmptyChangeHP();
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
        if (!UsedMeanLook && EmptyHp < maxHP)//��Ҫ���ԣ����Ժ��޸�
        {
            if (!roomCreated && currentPhase == 1)
            {
                turningPhase = true;
                StopAllCoroutines();//ֹͣ���м����ͷ�
                EmptyHp = maxHP;
                uIHealth.Per = EmptyHp / maxHP;
                StartCoroutine(Phase2Start());
            }
            currentPhase++;
            Debug.Log("������׶�");
        }
        else if (skillTimer <= 0f)
        {
            skillTimer = 1.1f;
            StartCoroutine(Phase1Skill());
        }
        // ���ܼ�ʱ���ݼ�
            skillTimer -= Time.deltaTime;
    }
    void Phase2()
    {
        if (EmptyHp <= maxHP*4/5 && currentPhase == 2)//��Ҫ���ԣ����Ժ��޸�
        {
            EmptyHp = maxHP;
            uIHealth.Per = EmptyHp / maxHP;
            uIHealth.ChangeHpUp();
            currentPhase++;
            Debug.Log("�������׶�");
            isPhase3 = true;
        }
        else if (skillTimer <= 0f)
        {
            // ���ѡ��һ�������ͷ�
            int randomSkillIndex = Random.Range(1, 21);
            StartCoroutine(Phase2Skill(randomSkillIndex));
            SkillTimerUpdate(randomSkillIndex, 2);
        }

        // ���ܼ�ʱ���ݼ�
        skillTimer -= Time.deltaTime;
    }
    private void MoreAttack()//���׶εĶ��⹥��ģʽ
    {
        MoreAttackTimer += Time.deltaTime;
        if (MoreAttackTimer >= 7f)
        {
            //��ǰ��������
            if (!isMasked)
            {
                isMasked = true;
                UseSkillMask(MoreAttackSkillIndex);
            }
            if (MoreAttackTimer >= 8)
            {
                if (MoreAttackSkillIndex >= MoreAttackSkill.Length)
                {
                    MoreAttackSkillIndex = 0; // �ص���һ������
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
        if (isPhase3)
        {
            transform.position = mapCenter;
            MewOrbRotate mewOrbRotate = Phase3OrbRotate.GetComponent<MewOrbRotate>();
            mewOrbRotate.ActivatePhase3Effect();
            Invincible = true;
            
            isPhase3 = false;
            Debug.Log("ʹ�ü���");
            StartCoroutine(Phase3Skill());
        }
    }
    void UseSkill(int skillIndex)
    {
        Debug.Log("Boss used skill: " + skillIndex);
        switch (skillIndex)
        {
            case 1:
                //����1��ħ��Ҷ
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
                        // ��Mewλ��ʵ����ħ��Ҷ
                        GameObject magicalLeaf = Instantiate(magicalLeafPrefab, transform.position, Quaternion.identity);
                        magicalLeaf.GetComponent<MagicalLeafEmpty>().empty = this;
                        Destroy(magicalLeaf, 6f); // 6�������ħ��Ҷ����
                        yield return new WaitForSeconds(delayBetweenLeaves);
                    }
                }
                break;
            case 2:
                //����2������ѩ
                if(currentPhase!= 1)
                {
                    return;
                }
                GameObject blizzard = Instantiate(blizzardPrefab, transform.position, Quaternion.identity);
                blizzard.GetComponent<BlizzardEmpty>().empty = this;
                Destroy(blizzard, 6f);//6������ٱ���ѩ����
                break;
            case 3://����3���׻�
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
                        float angleStep = 360f / numWillOWisp; // ����ÿ��WillOWisp֮��ĽǶȼ��
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = increaseAngle + i * angleStep; // ���㵱ǰWillOWisp�ĽǶ�
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius; // ���㵱ǰWillOWisp������λ��
                            WillOWispEmpty willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity).GetComponent<WillOWispEmpty>();
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.Initialize(moveSpeed, direction); // ����WillOWisp���ƶ��ٶ�
                            willOWisp.mew = this.gameObject;
                            Destroy(willOWisp, 4f);
                        }
                        yield return new WaitForSeconds(WaitingWillOWisp);
                    }
                }
                break;
            case 4://����4�������ദ
                if(currentPhase!= 1)
                {
                    return;
                }
                GameObject PlayNice = Instantiate(PlayNicePrefab, transform.position, Quaternion.identity);
                Destroy(PlayNice, 5f);
                break;
            case 5://����5��̫������
                //�ͷ�3�����⣬�ֱ�λ��90�ȡ�210�ȡ�330�ȵ�λ��
                transform.position = mapCenter;
                //������������
                float[] angles = { 90f, 210f, 330f };
                for (int i = 0; i < angles.Length; i++)
                {
                    //���㼤�����ʼ����յ�
                    float angle = angles[i];
                    Vector3 startPoint = mapCenter;
                    Vector3 endPoint = mapCenter + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                    TeraBlastEmpty Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity).GetComponent<TeraBlastEmpty>();
                    Terablast.SetEndpoints(startPoint, endPoint, angle);
                    Terablast.SetColors(Color.yellow, Color.red);
                    Terablast.empty = this;
                }
                break;
            case 6://����6������
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
            case 7://����7��ħ������
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
                    float rotationSpeed = 30f;
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

            case 8://����8����׶
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
            case 9://����9������ӡ��
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
            case 10://����10������
                // ����ScaleShot
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
                        // ����Reticle������λ��
                        GameObject reticle = Instantiate(reticlePrefab, randomPoint, Quaternion.identity);
                        Destroy(reticle, 2f);
                        yield return new WaitForSeconds(1.5f);
                        for (int i = 0; i < scaleShotCount; i++)
                        {
                            // ����ScaleShot���ɵ�λ�úͷ���
                            Vector3 scaleShotPosition = randomPoint;
                            float angleIncrement = 360f / scaleShotCount;
                            float angle = i * angleIncrement;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                            // ����ScaleShot
                            GameObject scaleShot = Instantiate(ScaleShotPrefab, scaleShotPosition, rotation);
                            scaleShot.GetComponent<ScaleShotEmpty>().empty = this;
                        }
                    }
                }
                break;
            case 11: //����11����ɫĿ��
                if (UsedMeanLook)
                {
                    //��ֹ����ʹ�����κ�ɫĿ��
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

                    // ��ȡ��ɫĿ��İ뾶
                    float circleRadius = blackCircle.GetComponent<CircleCollider2D>().radius;

                    // �ں�ɫĿ����������ҵ��ƶ�
                    while (blackCircle != null)
                    {
                        // ����������ɫĿ��ľ���
                        float distance = Vector2.Distance(player.transform.position, blackCircle.transform.position);

                        if (distance > circleRadius)
                        {
                            // �����Ҿ����ɫĿ��ľ������Բ�뾶��������ƶ��غ�ɫĿ��ķ�Χ��
                            Vector3 direction = (player.transform.position - blackCircle.transform.position).normalized;
                            player.transform.position = blackCircle.transform.position + direction * circleRadius;
                        }

                        yield return null;
                    }
                }
                break;
            case 12://����12��ħ����ҫ
                GameObject dazzlingGleam = Instantiate(DazzlingGleamPrefab, transform.position, Quaternion.identity);
                Destroy(dazzlingGleam, 5f);
                // ��3.5����Ȧ�ڵ��������˺�
                StartCoroutine(DamagePlayersWithDelay());
                IEnumerator DamagePlayersWithDelay()
                {
                    yield return new WaitForSeconds(3.5f);

                    // ��ȡȦ�ڵ���Ҳ���������˺�
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5.85f);
                    foreach (Collider2D collider in colliders)
                    {
                        // �ж���ײ���Ƿ��������
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
            case 13://����13��Ҷ��
                StartCoroutine(ReleaseLeafBlade());
                IEnumerator ReleaseLeafBlade()
                {
                    int shootCount = 10; // �������
                    float shootInterval = 0.3f; // ������
                    if(currentPhase != 1)
                    {
                        shootCount = 15;
                        shootInterval = 0.2f;
                    }
                    for (int i = 0; i < shootCount; i++)
                    {
                        // ʵ����LeafBlade
                        GameObject LeafBlade = Instantiate(LeafBladePrefab, transform.position, Quaternion.identity);
                        LeafBlade.GetComponent<LeafBladeEmpty>().empty = this;
                        // �ȴ�������
                        yield return new WaitForSeconds(shootInterval);
                    }
                }
                break;
            case 14://����14����ʯ����
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
                        //ѡ��һ��������StoneEdge��λ��
                        int emptyPosition = Random.Range(1, 14);
                        for (int j = 1; j <= 13; j++)
                        {
                            if (j != emptyPosition)
                            {
                                // ����Reticle��λ��
                                float spaceLength = mapLength / 13f;
                                float startX = mapCenter.x + (j - 7) * spaceLength;
                                reticleSpawnPosition = new Vector3(startX, mapCenter.y, mapCenter.z);
                                // ����Reticle
                                GameObject reticle = Instantiate(reticlePrefab, reticleSpawnPosition, Quaternion.identity);
                                Destroy(reticle, 2f);
                                //���ɼ�ʯ
                                Vector3 StoneEdgeSpawnPosition = new Vector3(reticleSpawnPosition.x, reticleSpawnPosition.y + 14f, reticleSpawnPosition.z);
                                GameObject stoneEdge = Instantiate(stoneEdgePrefab, StoneEdgeSpawnPosition, Quaternion.identity);
                                stoneEdge.GetComponent<StoneEdgeEmpty>().empty = this;
                            }
                        }
                        yield return new WaitForSeconds(5f);
                    }
                }
                break;
            case 15://����15������֮��
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
                    //ÿ���ͷſ���֮�к�ȴ�1.3f
                    for(int i = 0;i< Times;i++)
                    {
                        GameObject airSlash = Instantiate(AirSlashPrefab, transform.position, Quaternion.identity);
                        airSlash.GetComponent<AirSlashMew>().empty = this;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 16://����16���Խ�
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
            case 17://����17������
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
            case 18://����18��ʮ�ֶ���
                StartCoroutine(ReleaseCrossPoison());
                IEnumerator ReleaseCrossPoison()
                {
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
                        //���ܼ���ȴ�ʱ��
                        angle += angleIncrement;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 19://����19����ʥ֮��
                StartCoroutine(ReleaseScaredFire());
                IEnumerator ReleaseScaredFire()
                {
                    int numPoints = 5; // ������ϵĵ���
                    float radius = 15f; // ����ǵĶ��㵽���ĵľ���

                    Vector3[] starVertices = new Vector3[numPoints];
                    Vector3[] secredFirePositions = new Vector3[numPoints * 14]; // �洢SecredFire��λ��

                    // ��������ǵĶ�������
                    for (int i = 0; i < numPoints; i++)
                    {
                        float angle = i * 2f * Mathf.PI / numPoints;
                        float x = radius * Mathf.Sin(angle) + player.transform.position.x;
                        float y = radius * Mathf.Cos(angle) + player.transform.position.y;
                        starVertices[i] = new Vector3(x, y, player.transform.position.z);
                    }
                    // ����Բ�����ĵ�SecredFire
                    for (int i = 0; i < numPoints; i++)
                    {
                        Vector3 startPoint = starVertices[i];
                        Vector3 endPoint = starVertices[(i + 1) % numPoints];

                        Vector3 center = (startPoint + endPoint) / 2f;

                        GameObject secredFireCenter = Instantiate(SecredFireCentrePrefab, center, Quaternion.identity);
                        secredFireCenter.GetComponent<SecredFireEmpryCentre>().empty = this;
                        secredFirePositions[numPoints * 12 + i] = center;
                    }
                    // ��ÿ������Ƕ�������SecredFire
                    for (int i = 0; i < numPoints; i++)
                    {
                        GameObject secredFireVertex = Instantiate(SecredFireVertexPrefab, starVertices[i], Quaternion.identity);
                        secredFireVertex.GetComponent<SecredFireEmptyVertex>().empty = this;
                        secredFirePositions[numPoints * 12 + numPoints + i] = starVertices[i];
                    }

                    // ��ÿ�����Ͼ��ȷֲ�����SecredFire
                    for (int i = 0; i < numPoints; i++)
                    {
                        Vector3 startPoint = starVertices[i];
                        Vector3 endPoint = starVertices[(i + 2) % numPoints];

                        float dist = Vector3.Distance(startPoint, endPoint);
                        float step = dist / 12f; // ���ɵ�ľ�����

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
            case 20://����20��ʥ��
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
                            secredSword.empty = this;
                            secredSword.Initialize(angle, radius);
                            
                        }
                        yield return new WaitForSeconds(3f);
                    }

                }
                break;
        }
        //���׶��ܹ��Ƴ��ļ��ܣ�����ѩ��2���������ദ��4����������6���ͼ�ʯ������14��
    }
    void SkillTimerUpdate(int skillindex, int stage)//��������ʱ�����
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
                else if (stage == 2) skillTimer = 1.7f;
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
            // ������Դ�������150�Σ���ȡ���������
            Debug.Log("û�гɹ��ҵ����ʵĴ���λ��");
            return;
        }
        //һ�׶�������ͣ���ͼ�����λ��
        if (currentPhase == 1)
        {
            // �ڷ��������ѡ��һ��λ��w
            randomPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);

            // �����"Wall"��"Environment"��ǩ�Ķ����Ƿ���ײ
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
                //�����ײ����Ѱ��λ��
                RamdomTeleport();
            }
            else
            {
                //��֮����˲���ƶ�
                transform.position = randomPosition;
                //���¼�����
                teleportAttempts = 0;
            }
        }
        //���׶�������ͣ������Χ
        if(currentPhase == 2)
        {
            float minDistance = 5f;
            float maxDistance = 10f;
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minDistance, maxDistance);
            randomPosition = player.transform.position + randomDirection * randomDistance;
            // �����"Room"��"Environment"��ǩ�Ķ����Ƿ���ײ
            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room") || collider.CompareTag("Enviroment"))
                {
                    collided = true;
                    break;
                }
            }
            //����Ƿ��ͼ�߽���
            if (!IsInMapBounds(randomPosition))
                collided = true;
            if (collided)
            {
                //�����ײ����Ѱ��λ��
                RamdomTeleport();
            }
            else
            {
                //��֮����˲���ƶ�
                transform.position = randomPosition;
                //���¼�����
                teleportAttempts = 0;
            }
        }
    }
    bool IsInMapBounds(Vector3 position)
    {
        // ���ݵ�ͼ�߽�����귶Χ���ж�λ���Ƿ��ڵ�ͼ��
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

        //���ѡ�����ͷ�
        int randomSkillIndex = Random.Range(1, 19);
        if(randomSkillIndex == 5||randomSkillIndex == 16)
        {
            transform.position = mapCenter;//������Ϊ�Խ𳱻���̫������ʱ���͵����м�
        }
        else
        {
            RamdomTeleport();
        }
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
        if (randomSkillIndex == 5 || randomSkillIndex == 16)
        {
            transform.position = mapCenter;
        }
        else
        {
            RamdomTeleport();
        }
        UseSkillMask(randomSkillIndex);
        yield return new WaitForSeconds(0.5f);
        UseSkill(randomSkillIndex);
        SkillTimerUpdate(randomSkillIndex, 2);
    }

    private IEnumerator Phase3Skill()
    {
        yield return new WaitForSeconds(1f);
        UseSkillMask(1);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * (360f / 8);
                Vector2 spawnPosition = transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * 2f);
                GameObject magicalleaf = Instantiate(magicalLeafPrefab, spawnPosition, Quaternion.identity);
                magicalleaf.GetComponent<MagicalLeafEmpty>().empty = this;
                Destroy(magicalleaf, 6f);
            }
            yield return new WaitForSeconds(2f);
        }//����1��ħ��Ҷ-11��
        yield return new WaitForSeconds(1f);
        UseSkillMask(3);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 8; j++)
        {
            float increaseAngle = 7f;
            float angleStep = 360f / 20;
            for (int i = 0; i < 20; i++)
            {
                float angle = j * increaseAngle + i * angleStep; 
                Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius;
                WillOWispEmpty willOwisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity).GetComponent<WillOWispEmpty>();
                Vector3 direction = (spawnPos - transform.position).normalized;
                willOwisp.Initialize(6f, direction); 
                willOwisp.mew = gameObject;
            }
            yield return new WaitForSeconds(0.6f);
        }//����2���׻�-21��
        yield return new WaitForSeconds(3.1f);
        UseSkillMask(5);
        yield return new WaitForSeconds(0.5f);
        //����6������
        for (int j = 0; j < 3; j++)
        {
            float[] angles = {30f, 90f, 150f, 210f, 270f ,330f };
            LaserChange = false;
            if (j == 1)
            {
                angles[0] = 15f;
                angles[1] = 75f;
                angles[2] = 135f;
                angles[3] = 195f;
                angles[4] = 255f;
                angles[5] = 315f;
                LaserChange = true;
            }
            for (int i = 0; i < angles.Length; i++)
            {
                //���㼤�����ʼ����յ�
                float angle = angles[i];
                Vector3 startPoint = mapCenter;
                Vector3 endPoint = mapCenter + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                TeraBlastEmpty Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity).GetComponent<TeraBlastEmpty>();
                Terablast.SetEndpoints(startPoint, endPoint, angle);
                Terablast.SetColors(Color.yellow, Color.red);
                Terablast.empty = this;
            }
            yield return new WaitForSeconds(2.5f);
        }
        UseSkillMask(6);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            GameObject playercurse = Instantiate(CursePrefab, player.transform.position, Quaternion.identity);
            playercurse.GetComponent<Curse>().empty = this;
            Destroy(playercurse, 4f);
            for (int j = 0; j < 12; j++)
            {
                Vector2 position = UnityEngine.Random.insideUnitCircle * 20f;
                Vector3 spawnPosition = transform.position + new Vector3(position.x, position.y + 0.5f, 0f);
                GameObject Curse = Instantiate(CursePrefab, spawnPosition, Quaternion.identity);
                Curse.GetComponent<Curse>().empty = this;
                Destroy(Curse, 4f);
            }
            yield return new WaitForSeconds(0.6f);
        }
        UseSkillMask(7);
        yield return new WaitForSeconds(1f);
        float angleIncrement = 360f / 8;
        float rotationSpeed = 30f;
        for (int j = 0; j < 4; j++)
        {
            if (j % 2 == 0)
            {
                rotationSpeed = rotationSpeed * -1;
            }
            for (int i = 0; i < 8; i++)
            {
                float angle = i * angleIncrement;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                MagicalFireEmpty magicalFire = Instantiate(MagicalFirePrefab, transform.position, rotation).GetComponent<MagicalFireEmpty>();
                magicalFire.ps(transform.position, rotationSpeed);
                magicalFire.empty = this;

            }
            yield return new WaitForSeconds(0.8f);
        }
        UseSkillMask(8);
        yield return new WaitForSeconds(1f);
        int icicleCount = 8;
        int realIcicleCount = 8;
        float radius = 7f;
        for (int j = 0; j < 5; j++)
        {
            switch (j)
            {
                case 0:realIcicleCount = 8;icicleCount = 8;radius = 8f;break;
                case 1:realIcicleCount = 18;icicleCount = 24; radius = 12f;break;
                case 2:realIcicleCount = 12;icicleCount = 12;radius = 16f;break;
                case 3:realIcicleCount = 24;icicleCount = 32;radius = 20f;break;
                case 4:realIcicleCount = 16;icicleCount = 16;radius = 24f;break;
            }
            for (int i = 0; i < realIcicleCount; i++)
            {
                float angle = i * (360f / icicleCount);
                Vector2 spawnPosition = player.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius);
                IcicleSpearEmpty IcicleSpear = Instantiate(IcicleSpearPrefab, spawnPosition, Quaternion.identity).GetComponent<IcicleSpearEmpty>();
                IcicleSpear.sf(player.transform.position);
                IcicleSpear.empty = this;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
        GameObject timestopEffect = Instantiate(TimeStopEffect, player.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(timestopEffect);
        Time.timeScale = 1;
        yield return new WaitForSeconds(3f);
        UseSkillMask(9);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 6; j++)
        {
            float angleIncrement2 = 360f / heartStampCount;
            for (int i = 0; i < heartStampCount; i++)
            {
                float angle = i * angleIncrement2;
                Vector3 heartStampPosition = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * HeartStampRadius;

                HeartStampEmpty heartStamp = Instantiate(HeartStampPrefab, heartStampPosition, Quaternion.identity).GetComponent<HeartStampEmpty>();
                heartStamp.empty = this;
                heartStamps.Add(heartStamp.gameObject);
                if (heartStamp != null)
                {
                    heartStamp.SetTarget(player.transform.position);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        UseSkillMask(10);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 5; j++)
        {
            Vector3 randomPoint = (Vector2)player.transform.position + Random.insideUnitCircle.normalized * 3f;
            GameObject reticle = Instantiate(reticlePrefab, randomPoint, Quaternion.identity);
            Destroy(reticle, 2f);
            if (j == 4)
            {
                UseSkillMask(11);
                UseSkill(11);
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < 12; i++)
            {
                Vector3 scaleShotPosition = randomPoint;
                float angleIncrement3 = 360f / 12;
                float angle = i * angleIncrement3;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                GameObject scaleShot = Instantiate(ScaleShotPrefab, scaleShotPosition, rotation);
                scaleShot.GetComponent<ScaleShotEmpty>().empty = this;
            }
        }
        yield return new WaitForSeconds(1.5f);
        UseSkillMask(13);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 60; i++)
        {
            GameObject LeafBlade = Instantiate(LeafBladePrefab, transform.position, Quaternion.identity);
            LeafBlade.GetComponent<LeafBladeEmpty>().empty = this;
            yield return new WaitForSeconds(0.17f);
        }
        UseSkillMask(15);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 7; i++)
        {
            GameObject airSlash = Instantiate(AirSlashPrefab, transform.position, Quaternion.identity);
            airSlash.GetComponent<AirSlashMew>().empty = this;
            yield return new WaitForSeconds(0.6f);
        }
        yield return new WaitForSeconds(0.5f);
        UseSkillMask(16);
        yield return new WaitForSeconds(1f);
        float angleIncrement4 = 7f;
        float angle4 = 0f;
        for (int i = 0; i < 150; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float currentAngle = angle4 + j * 90f;
                Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                MakeItRainEmpty makeitrain = Instantiate(MakeItRainPrefab, transform.position, Quaternion.identity).GetComponent<MakeItRainEmpty>();
                makeitrain.MIRrotate(direction);
                makeitrain.empty = this;
            }
            if (i > 50 && i < 100) 
            {
                angle4 -= angleIncrement4;
            }
            else
            {
                angle4 += angleIncrement4;
            }
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(2.5f);
        UseSkillMask(17);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 25; i++)
        {
            Vector3 randomPosition = mapCenter + new Vector3(Random.Range(-24.0f, 24.0f), Random.Range(-14.0f, 14.0f), 0);
            Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
        }
        UseSkillMask(18);
        yield return new WaitForSeconds(1f);
        float angle5 = 0f;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float currentAngle = angle5 + j * 90f;
                Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                CrossPoisonEmpty crossPoison = Instantiate(CrossPoisonPrefab, transform.position, Quaternion.identity).GetComponent<CrossPoisonEmpty>();
                crossPoison.CProtate(direction);
                crossPoison.empty = this;
            }
            //���ܼ���ȴ�ʱ��
            angle5 += 60f;
            yield return new WaitForSeconds(0.8f);
        }
        yield return new WaitForSeconds(3f);
        UseSkillMask(19);
        yield return new WaitForSeconds(1f);
        int numPoints = 8; // ������ϵĵ���
        radius = 12f; // ����ǵĶ��㵽���ĵľ���
        Vector3[] starVertices = new Vector3[numPoints];
        Vector3[] secredFirePositions = new Vector3[numPoints * 14]; // �洢SecredFire��λ��

        // ��������ǵĶ�������
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * 2f * Mathf.PI / numPoints;
            float x = radius * Mathf.Sin(angle) + player.transform.position.x;
            float y = radius * Mathf.Cos(angle) + player.transform.position.y;
            starVertices[i] = new Vector3(x, y, player.transform.position.z);
        }
        // ����Բ�����ĵ�SecredFire
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 startPoint = starVertices[i];
            Vector3 endPoint = starVertices[(i + 1) % numPoints];

            Vector3 center = (startPoint + endPoint) / 2f;

            GameObject secredFireCenter = Instantiate(SecredFireCentrePrefab, center, Quaternion.identity);
            secredFireCenter.GetComponent<SecredFireEmpryCentre>().empty = this;
            secredFirePositions[numPoints * 12 + i] = center;
        }
        // ��ÿ������Ƕ�������SecredFire
        for (int i = 0; i < numPoints; i++)
        {
            GameObject secredFireVertex = Instantiate(SecredFireVertexPrefab, starVertices[i], Quaternion.identity);
            secredFireVertex.GetComponent<SecredFireEmptyVertex>().empty = this;
            secredFirePositions[numPoints * 12 + numPoints + i] = starVertices[i];
        }

        // ��ÿ�����Ͼ��ȷֲ�����SecredFire
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 startPoint = starVertices[i];
            Vector3 endPoint = starVertices[(i + 3) % numPoints];

            float dist = Vector3.Distance(startPoint, endPoint);
            float step = dist / 12f; // ���ɵ�ľ�����

            Vector3 direction = (endPoint - startPoint).normalized;

            for (int j = 0; j < 12; j++)
            {
                Vector3 secredFirePosition = startPoint + direction * (j * step);
                SecredFireEmpty secredFire = Instantiate(SecredFirePrefab, secredFirePosition, Quaternion.identity).GetComponent<SecredFireEmpty>();
                secredFirePositions[(i * 12) + j] = secredFirePosition;
                secredFire.empty = this;
                secredFire.Initialize(player.transform.position, 2f);
            }
        }
        yield return new WaitForSeconds(4f);
        //���׶�-�ڶ�����
        //������Բ���ͷŻ���������˺��ļ��ģ��ڼ䲻���ͷ�ħ��Ҷ
        StartCoroutine(MagicalLeaf(8));
        yield return new WaitForSeconds(2f);
        for (int j = 0; j < 7; j++)
        {
            float increaseAngle = 9f;
            float angleStep = 360f / 16;
            for (int i = 0; i < 16; i++)
            {
                float angle = j * increaseAngle + i * angleStep;
                Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * 1f;
                FakeLove fakelove = Instantiate(FakeLovePrefab, spawnPos, Quaternion.identity).GetComponent<FakeLove>();
                Vector3 direction = (spawnPos - transform.position).normalized;
                fakelove.Initialize(5f, direction);
                fakelove.mew = gameObject;
            }
            yield return new WaitForSeconds(3f);
        }

        //����ͷŵ����ͷű�������
        StartCoroutine(ElectricBall());
        yield return new WaitForSeconds(4f);
        UseSkillMask(2);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 50; i++)
        {
            float angle = i * 22f;
            GameObject trail = Instantiate(TrailEffect, transform.position, Quaternion.Euler(0, 0, angle));
            StartCoroutine(IceBeam(i));
            Destroy(trail, 2f);
            yield return null;
        }
        yield return new WaitForSeconds(5f);

        //�ͷ�ħ��Ҷ��ͬʱ�ͷų��ּ���ҩ
        StartCoroutine(MagicalLeaf(10));
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomPosition = RandomPosition();
            GameObject fakepotion = Instantiate(FakePotionPrefab, randomPosition, Quaternion.identity);
            Destroy(fakepotion, 15f);
            yield return new WaitForSeconds(1f);
        }

    }

    //�����׶ε���
    private IEnumerator ElectricBall()
    {
        for(int j = 0; j < 20; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                ElectroBallEmpty electricBall = Instantiate(ElectricBallPrefab, transform.position, rotation).GetComponent<ElectroBallEmpty>();
                electricBall.Initialize(transform.position, 15f);
                electricBall.empty = this;

            }
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                ElectroBallEmpty electricBall = Instantiate(ElectricBallPrefab, transform.position, rotation).GetComponent<ElectroBallEmpty>();
                electricBall.Initialize(transform.position, -15f);
                electricBall.empty = this;

            }
            yield return new WaitForSeconds(8f);
        }
    }
    //�����׶εı�������
    private IEnumerator IceBeam(int i)
    {
        yield return new WaitForSeconds(2f);
        float angle = i * 22f;
        GameObject icebeam = Instantiate(IceBeamPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        icebeam.GetComponent<IceBeamEmpty>().empty = this;
    }
    //�����׶ε�ħ��Ҷ
    private IEnumerator MagicalLeaf(int times)
    {
        for(int i = 0; i < times; i++)
        {
            GameObject magicalleaf = Instantiate(magicalLeafPrefab, transform.position, Quaternion.identity);
            magicalleaf.GetComponent<MagicalLeafEmpty>().empty = this;
            Destroy(magicalleaf, 6f);
            yield return new WaitForSeconds(3f);
        }
    }
    //�����׶����Ѱ��λ�ã����λõķ�Χ�ڵ��ǲ�����ҷ�Χ��
    Vector2 RandomPosition()
    {
        float mewRadius = 20f;
        float playerRadius = 4f;
        Vector2 randomPos = transform.position;

        bool isValid = false;
        while (!isValid)
        {
            randomPos = (Vector2)transform.position + Random.insideUnitCircle * mewRadius;

            // ���λ���Ƿ�����Ұ뾶��Χ��
            float distanceToPlayer = Vector2.Distance(randomPos, player.transform.position);
            if (distanceToPlayer > playerRadius)
            {
                isValid = true;
            }
        }

        return randomPos;
    }

    private IEnumerator Phase2Start()
    {
        //������е��ӵ�
        Invincible = true;
        uIHealth.ChangeHpUp();

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }

        //����һ����ɫ�����
        int numPoints = 5; // ������ϵĵ���
        float radius = 12f; // ����ǵĶ��㵽���ĵľ���

        Vector3[] starVertices = new Vector3[numPoints];

        // ��������ǵĶ�������
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
            float step = dist / 12f; // ���ɵ�ľ�����

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

        //�����µķ���
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
            //���λ��ƶ���Empty�Ӷ�����
            transform.SetParent(mewTransform);
        }
        //ɫ��ͷ��������
        cameraAdapt.ActivateVcam();
        cinemachineController = FindObjectOfType<CameraController>();
        cinemachineController.MewCameraFollow();
        cameraAdapt.HideCameraMasks();
        yield return new WaitForSeconds(1.5f);
        turningPhase = false;
        Invincible = false;

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
            case 3: useskillmask.targetColor = new Color(0.2f, 0f, 0.2f); break;//�Ϻ�ɫ
            case 4: useskillmask.targetColor = new Color(1f, 0.75f, 0.8f); break;//��ɫ
            case 5: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); useskillmask.originalColor = Color.red; break;
            case 6: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); break;
            case 7: useskillmask.targetColor = Color.red; break;
            case 8: useskillmask.targetColor = Color.cyan; break;
            case 9: useskillmask.targetColor = new Color(1f, 0f, 0.6f); break;//���ɫ
            case 10: useskillmask.targetColor = new Color(0f, 0f, 0.5f); break;//����ɫ
            case 11: useskillmask.targetColor = Color.black; break;
            case 12: useskillmask.targetColor = new Color(1f, 0.75f, 0.8f); break;
            case 13: useskillmask.targetColor = Color.green; break;
            case 14: useskillmask.targetColor = new Color(0.7f, 0.5f, 0.3f); break;//ǳ��ɫ
            case 15: useskillmask.targetColor = Color.cyan; break;
            case 16: useskillmask.targetColor = Color.grey; break;
            case 17: useskillmask.targetColor = new Color(0.5f, 1f, 0.5f); break;//ǳ��ɫ
            case 18: useskillmask.targetColor = new Color(0.5f, 0f, 0.5f); break;//��ɫ
            case 19: useskillmask.targetColor = Color.red; break;
            case 20: useskillmask.targetColor = new Color(1f, 0.5f, 0f); break;
        }
    }
    void ClearStatusEffects()
    {
        //�������debuff
        EmptyCurseRemove();
        ColdRemove();
        EmptyConfusionRemove();
        EmptyInfatuationRemove();
        EmptyParalysisRemove();
        EmptySleepRemove();
        EmptyBurnRemove();
        BlindRemove();
        FearRemove();
        FrozenRemove();
    }
}
