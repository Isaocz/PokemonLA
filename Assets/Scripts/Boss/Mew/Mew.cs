using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics.Eventing.Reader;

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

    //�л�����ʱ�ȴ�
    private float MeanLookTimer = 0f;
    private bool UsedMeanLook = false;
    private bool turningPhase = false;

    //Audio
    public BackGroundMusic bgmScript;
    //�׶�
    public int currentPhase = 1; // ��ǰ�׶�
    private float skillTimer = 0f; // ���ܼ�ʱ��
    private List<GameObject> heartStamps = new List<GameObject>();//��HeartStamp���д洢
    private float MoreAttackTimer;//���׶θ��༼�ܼ�ʱ��
    private int[] MoreAttackSkill = { 1, 8, 9, 10, 11, 13, 15, 17, 18, 20 };
    private int MoreAttackSkillIndex;
    private bool isMasked = false;//���༼���ͷ�����

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

    // Start is called before the first frame update
    void Start()
    {
        //Audio
        bgmScript = BackGroundMusic.StaticBGM;

        //�λõĻ�������
        EmptyType01 = Type.TypeEnum.Psychic;//�λõ�����Ϊ����
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, 100);//���޵ȼ�100
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
        cinemachineController = FindObjectOfType<CameraController>();
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
        if (!UsedMeanLook && EmptyHp < maxHP)//��Ҫ���ԣ����Ժ��޸�
        {
            if (!roomCreated && currentPhase == 1)
            {
                turningPhase = true;
                StopAllCoroutines();//ֹͣ���м����ͷ�
                EmptyHp = maxHP;
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
            currentPhase++;
            Debug.Log("�������׶�");
            EmptyHp = maxHP;
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
        //���λ��޵У���ʱ�޷�����
        Invincible = true;
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
                    //�ͷż��ܺ�ȴ���ʱ��
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
                    float radius = 12f; // ����ǵĶ��㵽���ĵľ���

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
        RamdomTeleport();

        //���ѡ�����ͷ�
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
        //������е��ӵ�
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
