using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Mew : Empty
{
    [Header("����")]
    public GameObject magicalLeafPrefab;//����1
    public GameObject blizzardPrefab;//����2
    public GameObject WillOWispPrefab;//����3
    public float WillOWispRadius = 2f; // WillOWisp�����ɰ뾶
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
    public GameObject TeleportEndPrefab;
    public GameObject EdgePar;//���Խ���߽��ʱ����ʾ������
    public GameObject Phase3OrbRotate;
    public GameObject ElectricBallPrefab;//����21
    public GameObject ElectricBallp2;
    public GameObject TimeStopEffect;
    public GameObject TrailEffect;
    public GameObject TrailEffect2;
    public GameObject TrailEffect3;
    public GameObject IceBeamPrefab;//����22

    public GameObject FakePotionPrefab;//����ҩ
    public GameObject FakeAntidote;//�ٽ���ҩ
    public GameObject FakeBurnHeal;//������ҩ
    public GameObject FakeAwakening;//�ٽ���ҩ
    public GameObject FakeIceHeal;//�ٽⶳҩ
    public GameObject FakeParalyzeHeal;//�ٽ���ҩ
    public GameObject FakeLovePrefab;//����

    //�������ܵ���ȴʱ��
    [System.Serializable]
    public struct DictionaryData
    {
        public int skillIndex;
        public int stage;
        public float skillTimer;
    }
    public List<DictionaryData> DictionaryDataList;

    public Dictionary<int, Dictionary<int, float>> SkillTimer = new Dictionary<int, Dictionary<int, float>>();
    //���ֵ��intΪ����index��С�ֵ��intΪstage����ǰ�׶Σ���С�ֵ��floatΪ��Ӧ��skillTimer

    //�սἼ
    public GameObject Swords;
    public GameObject Meanlookfinal;

    [Header("Ѫ��UI����")]
    public GameObject timeBar1;
    public GameObject timeBar2;
    public GameObject timeBar3;
    public GameObject timeBar4;
    public Sprite TimeBar1;
    public Sprite TimeBar2;
    public Sprite TimeBar3;
    public Sprite TimeBar4;

    //�л�����
    private float MeanLookTimer = 0f;
    private bool UsedMeanLook = false;
    private bool turningPhase = false;

    //Audio
    public BackGroundMusic bgmScript;

    [Header("����")]
    public int currentPhase = 1; // ��ǰ�׶�
    public bool LaserChange = false;
    private float skillTimer = 0f; // ���ܼ�ʱ��
    private bool isReset;
    private bool isTeleport;
    public float teleportTime;
    private float teleportTimer = 0f;
    Vector3 targetPosition;
    Vector3 currentPosition;
    private float MoreAttackTimer = -4f;//���׶θ��༼�ܼ�ʱ��
    private bool isPhase3 = false;
    private int[] Phase3Skills = new int[]
    {
        1,3,5,6,7,8,9,10,13,15,16,17,18,19
    };
    private bool isSkillFin;
    private bool isDying = false;//����
    private bool isFinal;

    //�������
    private int teleportAttempts = 0;//������ͼ�����

    //����
    public GameObject MewBossRoomPrefab;
    public Vector3 MewBossRoomPosition = new Vector3(60f, 60f, 0f);
    private GameObject Camera;
    private bool roomCreated = false;
    private Vector3Int GetnowRoom;
    private Vector3 GetPlayerPosition;
    private Vector3 GetCameraPostion;

    //�������
    private CameraController cinemachineController;
    private CameraAdapt cameraAdapt;
    private GameObject AtkTarget;

    //ʱ��Ѫ��
    private float HpTimer = 191f;
    private float HpTiming = 191f;
    
    //�����ж�
    public static bool MewBossKilled = false;

    //��ɫ
    private Color[] colors = new Color[]
{
        new Color(0.7176471f, 0.6901961f, 0.6666667f, 0.5882353f), // Normal
        new Color(1, 0.75f, 0.175f, 0.7803922f), // Fighting
        new Color(0.678f,0.643f,0.91f,0.5882353f), // Flying
        new Color(0.875f,0.385f,0.733f,0.5882353f), // Poison
        new Color(0.81f,0.702f,0.51f,0.7058824f), // Ground
        new Color(0.68f, 0.54f,0.2636614f,0.702f), // Rock
        new Color(0.9f,0.945f,0.506f,0.5882353f), // Bug
        new Color(0.92f,0.31f,0.43f,0.627451f), // Ghost
        new Color(0.765f,0.64f,0.5f,0.53f), // Steel
        new Color(1,0.95f,0,0.6862745f), // Fire
        new Color(0.54f,0.75f,0.8903922f,0.5882353f), // Water
        new Color(0.69f,0.87f,0.54f,0.5882353f), // Grass
        new Color(0.96f,0.93f,0.63f,0.5882353f), // Electric
        new Color(0.86f,0.50f,0.655f,0.5882353f), // Psychic
        new Color(0.495f,0.765f,0.895f,0.5882353f), // Ice
        new Color(0.845f,0.553f,0.5137255f,0.682353f), // Dragon
        new Color(1,0.498f,0.53104f,0.8313726f), // Dark
        new Color(0.823f,0.87f,0.95f,0.5882353f), // Fairy
};
    public Type.TypeEnum SkillType;
    public Material src1;
    public Material src2;
    public Material src3;
    public float intensity = 1f;
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
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        mapCenter = transform.parent.position;
        GetnowRoom = player.NowRoom;
        GetPlayerPosition = player.transform.position;
        GetCameraPostion = Camera.transform.position;
        transform.parent.parent.GetComponent<Room>().isClear += 1;

        //�볡
        ClearProjectile();
        isReset = false;

        //���б��еļ�����ȴʱ���趨���ֵ���
        foreach (DictionaryData data in DictionaryDataList)
        {
            if (!SkillTimer.ContainsKey(data.skillIndex))
            {
                SkillTimer[data.skillIndex] = new Dictionary<int, float>();
            }
            SkillTimer[data.skillIndex][data.stage] = data.skillTimer;
        }

        //ɾ�����л�������
        Transform grandParent = transform.parent.parent;
        Transform enviroment = grandParent.Find("Enviroment");

        if (enviroment != null)
        {
            for (int i = 0; i < enviroment.childCount; i++)
            {
                if (enviroment.tag == "Grass")
                {
                    NormalGress grass = enviroment.GetComponent<NormalGress>();
                    if (grass != null)
                    {
                        grass.GrassDie();
                    }
                }
                else
                {
                    Destroy(enviroment.GetChild(i).gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn && !isDying)
        {
            if (currentPhase == 3)//���׶��ж�
            {
                Phase3();
                HpTiming -= Time.deltaTime;
                uIHealth.Per = HpTiming / HpTimer;
                uIHealth.ChangeHpDown();
                UISkillButton.Instance.isEscEnable = false;
                //������ҵ��ƶ��뾶
                float distance = Vector2.Distance(player.transform.position, transform.position);
                if (!isFinal)
                {
                    if (distance > 19f)
                    {
                        Vector3 direction = (player.transform.position - transform.position).normalized;
                        Vector3 targetPosition = transform.position + direction * 19f;
                        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 7f * Time.deltaTime);
                        GameObject edgePar = Instantiate(EdgePar, player.transform.position, Quaternion.identity);
                        Destroy(edgePar, 0.4f);
                    }
                }
                else
                {
                    if (distance > 14f)
                    {
                        Vector3 direction = (player.transform.position - transform.position).normalized;
                        Vector3 targetPosition = transform.position + direction * 14f;
                        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 7f * Time.deltaTime);
                        GameObject edgePar = Instantiate(EdgePar, player.transform.position, Quaternion.identity);
                        Destroy(edgePar, 0.4f);
                    }
                }
                if (HpTiming <= 0f)
                {
                    if (!isDying)
                    {
                        isDying = true;
                        ClearProjectile();
                        StartCoroutine(Phase3End());
                    }
                }
            }
            else
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
                                Invincible = true;
                                StopAllCoroutines();
                                if (!UsedMeanLook)
                                {
                                    if (!roomCreated && currentPhase == 1)
                                    {
                                        isReset = false;
                                        turningPhase = true;
                                        ClearStatusEffects();
                                        EmptyHp = maxHP;
                                        player.Hp = player.maxHp;
                                        uIHealth.Per = EmptyHp / maxHP;
                                        StartCoroutine(Phase2Start());
                                    }
                                }
                            }
                            else if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
                            {
                                //��ʵ�����������Ҫת�׶λ���û���쳣״̬�����м��ܼ�ʱ��������һ����ʼ���޷�ֹͣ���쳣״ֻ̬��ֹͣ���ܼ�ʱ��
                                Phase1();
                            }
                            break;
                        case 2:
                            if (EmptyHp <= 0 && currentPhase == 2)
                            {
                                isReset = false;
                                ClearStatusEffects();
                                StopAllCoroutines();
                                player.Hp = (player.Hp < player.maxHp / 2) ? (player.Hp + player.maxHp / 2) : player.maxHp;
                                EmptyHp = maxHP;
                                uIHealth.Per = EmptyHp / maxHP;
                                uIHealth.ChangeHpUp();
                                currentPhase++;
                                isPhase3 = true;
                            }
                            else if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
                            {
                                Phase2();
                            }
                            MoreAttack();
                            break;
                    }
                }
            }
            ChangeColor();
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

    #region �׶�ui
    void Phase1()
    {
        ResetSkillTimer();
        if (skillTimer <= 0f)
        {
            int randomSkillIndex = Random.Range(1, 19);
            StartCoroutine(Phase1Skill(randomSkillIndex));
            SkillTimerUpdate(randomSkillIndex, 1);
        }
        // ���ܼ�ʱ���ݼ�
        skillTimer -= Time.deltaTime;
    }
    void Phase2()
    {
        ResetSkillTimer();
        if (skillTimer <= 0f)
        {
            // ���ѡ��һ�������ͷ�
            int randomSkillIndex = Random.Range(1, 21);
            StartCoroutine(Phase2Skill(randomSkillIndex));
            targetPosition = RamdomTeleport();
            currentPosition = transform.position;
            SkillTimerUpdate(randomSkillIndex, 2);
        }
        if (isTeleport)
        {
            teleportTimer += Time.deltaTime;
            float t = Mathf.Clamp01(Mathf.Sin(teleportTimer / teleportTime * Mathf.PI * 0.5f));
            transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
            if(teleportTimer >= teleportTime)
            {
                isTeleport = false;
                teleportTimer = 0f;
            }
        }

        // ���ܼ�ʱ���ݼ�
        skillTimer -= Time.deltaTime;
    }
    private void MoreAttack()//���׶εĶ��⹥��ģʽ
    {
        List<Vector3> spawnedPositions = new List<Vector3>();

        MoreAttackTimer += Time.deltaTime;
        if (MoreAttackTimer > 2.5f)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector3 position;
                bool validPosition = false;

                // �������λ�ã�ֱ���ҵ�һ�����ʵ�λ��
                while (!validPosition)
                {
                    position = Random.insideUnitCircle * 20f;
                    Vector3 spawnPosition = player.transform.position + new Vector3(position.x, position.y + 0.5f, 0f);
                    float distanceToPlayer = Vector2.Distance(position, player.transform.position);

                    // �����λ���Ƿ��������ɵ�λ�ù���
                    validPosition = true;
                    foreach (Vector3 spawnedPos in spawnedPositions)
                    {
                        if (Vector3.Distance(spawnPosition, spawnedPos) < 4f && distanceToPlayer < 4f)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    if (validPosition)
                    {
                        spawnedPositions.Add(spawnPosition);
                        GameObject ebp2 = ObjectPoolManager.SpawnObject(ElectricBallp2, spawnPosition, Quaternion.identity);
                        ebp2.GetComponent<ElectroBallp2>().empty = this;
                    }
                }
            }
            MoreAttackTimer = 0f;
        }
    }
    void Phase3()
    {
        ResetSkillTimer();

        if (isPhase3)
        {
            bgmScript.ChangeBGMToMew(3);
            Invincible = true;
            isPhase3 = false;
            StartCoroutine(Phase3Start());
        }

        if (!isSkillFin)
        {
            if (skillTimer <= 0f)
            {
                if (Phase3Skills.Length == 0)
                {
                    isSkillFin = true;
                    StartCoroutine(Phase3Middle());
                }
                else
                {
                    int randomIndex = Random.Range(0, Phase3Skills.Length);
                    int randomNumber = Phase3Skills[randomIndex];
                    ChangeType(randomNumber);
                    TeleportEnd();
                    UseSkill(randomNumber);
                    //ʹ�ü��ܺ��Ƴ������ڵļ���
                    int[] LeftSkills = new int[Phase3Skills.Length - 1];
                    for (int i = 0, j = 0; i < Phase3Skills.Length; i++)
                    {
                        if (i != randomIndex)
                        {
                            LeftSkills[j] = Phase3Skills[i];
                            j++;
                        }
                    }
                    Phase3Skills = LeftSkills;
                    SkillTimerUpdate(randomNumber, 3);
                }
            }
            skillTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region �������

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
                    for (int i = 0; i < (currentPhase == 3 ? 2 : 1); i++) 
                    {//ħ��Ҷ����
                        for (int j = 0; j < (currentPhase == 1 ? 3 : currentPhase == 2 ? 5 : 8); j++)
                        {
                            Vector2 spawnPosition;
                            //ʵ����ħ��Ҷ
                            if (currentPhase == 3)
                            {
                                float angle = j * (360f / 8);
                                spawnPosition = transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * 2f);
                            }
                            else
                            {
                                spawnPosition = transform.position;
                            }
                            GameObject magicalLeaf = ObjectPoolManager.SpawnObject(magicalLeafPrefab, spawnPosition, Quaternion.identity);
                            magicalLeaf.GetComponent<MagicalLeafEmpty>().SetTarget(AtkTarget);
                            magicalLeaf.GetComponent<MagicalLeafEmpty>().empty = this;
                            ObjectPoolManager.ReturnObjectToPool(magicalLeaf, 6f); // 6�������ħ��Ҷ����
                            yield return new WaitForSeconds(currentPhase == 3? 0f: 0.3f);//ÿƬħ��Ҷ֮����ӳ�
                        }
                        yield return new WaitForSeconds(currentPhase == 3 ? 2f : 0f);//�ظ��ͷ�ħ��Ҷ���ӳ�
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
                float WaitingWillOWisp;
                float numWillOWisp;
                float WillOWispDegree;
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    if(currentPhase == 1)
                    {
                        numWillOWisp = 16;
                        WillOWispDegree = 2;
                        WaitingWillOWisp = 1f;
                    }
                    else if (currentPhase == 2)
                    {
                        numWillOWisp = 20;
                        WillOWispDegree = 5;
                        WaitingWillOWisp = 0.8f;
                    }
                    else
                    {
                        numWillOWisp = 20;
                        WillOWispDegree = 10;
                        WaitingWillOWisp = 0.4f;
                    }
                    for (int j = 0; j < WillOWispDegree; j++)
                    {
                        float increaseAngle = (currentPhase == 3 ? (Random.Range(0f, 14f)): 10f);
                        float angleStep = 360f / numWillOWisp; // ����ÿ��WillOWisp֮��ĽǶȼ��
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = j * increaseAngle + i * angleStep; // ���㵱ǰWillOWisp�ĽǶ�
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius; // ���㵱ǰWillOWisp������λ��
                            WillOWispEmpty willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity).GetComponent<WillOWispEmpty>();
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.Initialize(currentPhase == 3 ? 8f : 4f, direction); // ����WillOWisp���ƶ��ٶ�
                            willOWisp.empty = this;
                        }
                        yield return new WaitForSeconds(WaitingWillOWisp);
                    }
                }
                break;
            case 4://����4�������ദ
                GameObject PlayNice = ObjectPoolManager.SpawnObject(PlayNicePrefab, transform.position, Quaternion.identity);
                ClearStatusEffects();
                ObjectPoolManager.ReturnObjectToPool(PlayNice, 5f);
                break;
            case 5://����5��̫������
                //�ͷ�3�����⣬�ֱ�λ��90�ȡ�210�ȡ�330�ȵ�λ��
                StartCoroutine(ReleaseTeraBlast());
                IEnumerator ReleaseTeraBlast()
                {
                    if (currentPhase != 3)
                    {
                        float[] angles = { 90f, 210f, 330f };
                        for (int i = 0; i < angles.Length; i++)
                        {
                            //���㼤�����ʼ����յ�
                            float angle = angles[i];
                            Vector3 startPoint = transform.position;
                            Vector3 endPoint = transform.position + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                            TeraBlastEmpty Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity).GetComponent<TeraBlastEmpty>();
                            Terablast.SetEndpoints(startPoint, endPoint, angle);
                            Terablast.SetColors(Color.yellow, Color.red);
                            Terablast.empty = this;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            float[] angles = new float[6];
                            LaserChange = j == 1 ? true : false;
                            float randomAngle = Random.Range(0f, 30f);
                            for (int i = 0; i < angles.Length; i++)
                            {
                                angles[i] = randomAngle + 60f * i;
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
                    }
                    yield return null;
                }
                break;
            case 6://����6������
                if(currentPhase == 2)
                {
                    return;
                }
                StartCoroutine(ReleaseCurse());
                IEnumerator ReleaseCurse()
                {
                    if (currentPhase == 1)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            GameObject Curse = ObjectPoolManager.SpawnObject(CursePrefab, AtkTarget.transform.position, Quaternion.identity);
                            Curse.GetComponent<Curse>().empty = this;
                            ObjectPoolManager.ReturnObjectToPool(Curse, 4f);
                            yield return new WaitForSeconds(0.6f);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            GameObject playercurse = ObjectPoolManager.SpawnObject(CursePrefab, player.transform.position, Quaternion.identity);
                            playercurse.GetComponent<Curse>().empty = this;
                            Destroy(playercurse, 4f);
                            for (int j = 0; j < 12; j++)
                            {
                                Vector2 position = UnityEngine.Random.insideUnitCircle * 20f;
                                Vector3 spawnPosition = transform.position + new Vector3(position.x, position.y + 0.5f, 0f);
                                GameObject Curse = ObjectPoolManager.SpawnObject(CursePrefab, spawnPosition, Quaternion.identity);
                                Curse.GetComponent<Curse>().empty = this;
                                ObjectPoolManager.ReturnObjectToPool(Curse, 4f);
                            }
                            yield return new WaitForSeconds(0.6f);
                        }
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
                    for (int j = 0; j < Times; j++) 
                    {
                        if (j % 2 == 0 && currentPhase == 3)
                        {
                            rotationSpeed = rotationSpeed * -1;
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            float angle = i * angleIncrement;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                            MagicalFireEmpty magicalFire = ObjectPoolManager.SpawnObject(MagicalFirePrefab, transform.position, rotation).GetComponent<MagicalFireEmpty>();
                            magicalFire.ps(transform.position, rotationSpeed, currentPhase);
                            magicalFire.empty = this;
                            ObjectPoolManager.ReturnObjectToPool(magicalFire.gameObject, 7f);

                        }
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;

            case 8://����8����׶
                StartCoroutine(SummonIcicleSpears());
                IEnumerator SummonIcicleSpears()
                {
                    if (currentPhase != 3)
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
                                Vector2 spawnPosition = AtkTarget.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * summonRadius);
                                IcicleSpearEmpty IcicleSpear = Instantiate(IcicleSpearPrefab, spawnPosition, Quaternion.identity).GetComponent<IcicleSpearEmpty>();
                                IcicleSpear.sf(AtkTarget.transform.position);
                                IcicleSpear.empty = this;
                            }
                            yield return new WaitForSeconds(delayBetweenExecutions);
                        }
                    }
                    else
                    {
                        Time.timeScale = 0;
                        GameObject timestopEffect = ObjectPoolManager.SpawnObject(TimeStopEffect, player.transform.position, Quaternion.identity);
                        int icicleCount = 8;
                        int realIcicleCount = 8;
                        float radius = 7f;
                        for (int j = 0; j < 5; j++)
                        {
                            switch (j)
                            {
                                case 0: realIcicleCount = 8; icicleCount = 8; radius = 8f; break;
                                case 1: realIcicleCount = 24; icicleCount = 32; radius = 14f; break;
                                case 2: realIcicleCount = 12; icicleCount = 12; radius = 20f; break;
                                case 3: realIcicleCount = 36; icicleCount = 48; radius = 26f; break;
                                case 4: realIcicleCount = 16; icicleCount = 16; radius = 32f; break;
                            }
                            int randomangle = Random.Range(0, 360);
                            for (int i = 0; i < realIcicleCount; i++)
                            {
                                float angle = i * (360f / icicleCount);
                                switch (j)
                                {
                                    case 0: case 2: case 4: break;
                                    case 1: case 3: angle = angle + randomangle; break;
                                }
                                Vector2 spawnPosition = player.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius);
                                IcicleSpearEmpty IcicleSpear = Instantiate(IcicleSpearPrefab, spawnPosition, Quaternion.identity).GetComponent<IcicleSpearEmpty>();
                                IcicleSpear.sf(player.transform.position);
                                IcicleSpear.empty = this;
                            }
                            yield return null;
                        }
                        yield return new WaitForSecondsRealtime(3f);
                        Destroy(timestopEffect);
                        Time.timeScale = 1;
                        yield return new WaitForSeconds(4f);
                    }
                }
                break;
            case 9://����9������ӡ��
                SkillType = Type.TypeEnum.Fairy;
                StartCoroutine(ReleaseHeartStamp());
                IEnumerator ReleaseHeartStamp()
                {
                    float intervalTime = 1.5f;
                    int Times = 4;
                    if (currentPhase == 2)
                    {
                        intervalTime = 1.2f;
                        Times = 5;
                    }

                    for (int i = 0; i < Times; i++) {
                        float angleIncrement = 360f / heartStampCount;
                        for (int j = 0; j < heartStampCount; j++)
                        {
                            float angle = j * angleIncrement;
                            Vector3 heartStampPosition = transform.position;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                            HeartStampEmpty heartStamp = ObjectPoolManager.SpawnObject(HeartStampPrefab, heartStampPosition, rotation).GetComponent<HeartStampEmpty>();
                            heartStamp.phrase = 2;
                            heartStamp.empty = this;
                        }

                        if(currentPhase == 3)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                float angle = j * 360f / 16;
                                Vector3 heartStampPosition = transform.position;
                                Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                                HeartStampEmpty heartStamp = ObjectPoolManager.SpawnObject(HeartStampPrefab, heartStampPosition, rotation).GetComponent<HeartStampEmpty>();
                                heartStamp.phrase = 3;
                                heartStamp.empty = this;
                            }
                        }

                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 10://����10������
                SkillType = Type.TypeEnum.Dragon;
                StartCoroutine(ReleaseScaleShoot());
                IEnumerator ReleaseScaleShoot()
                {
                    if (currentPhase != 3)
                    {
                        int Times = 3;
                        if (currentPhase == 2)
                        {
                            Times = 5;
                        }
                        for (int j = 0; j < Times; j++)
                        {
                            Vector3 randomPoint = (Vector2)AtkTarget.transform.position + Random.insideUnitCircle.normalized * 3f;
                            // ����Reticle������λ��
                            GameObject reticle = ObjectPoolManager.SpawnObject(reticlePrefab, randomPoint, Quaternion.identity);
                            ObjectPoolManager.ReturnObjectToPool(reticle, 2f);
                            yield return new WaitForSeconds(1.5f);
                            for (int k = 0; k < scaleShotCount; k++)
                            {
                                // ����ScaleShot���ɵ�λ�úͷ���
                                Vector3 scaleShotPosition = randomPoint;
                                float angleIncrement = 360f / scaleShotCount;
                                float angle = k * angleIncrement;
                                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                                // ����ScaleShot
                                GameObject scaleShot = ObjectPoolManager.SpawnObject(ScaleShotPrefab, scaleShotPosition, rotation);
                                GameObject trail3 = ObjectPoolManager.SpawnObject(TrailEffect3, scaleShotPosition, Quaternion.Euler(0, 0, angle));
                                ObjectPoolManager.ReturnObjectToPool(trail3, 1f);
                                scaleShot.GetComponent<ScaleShotEmpty>().empty = this;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++) 
                        {
                            Vector3[] SpawnPoints;
                            float offset = 7f;
                            if (i % 2 == 1)
                            {
                                SpawnPoints = new Vector3[]
                                {
                                    mapCenter,
                                    new Vector3(mapCenter.x + offset, mapCenter.y + offset),
                                    new Vector3(mapCenter.x + offset, mapCenter.y - offset),
                                    new Vector3(mapCenter.x - offset, mapCenter.y + offset),
                                    new Vector3(mapCenter.x - offset, mapCenter.y - offset),
                                    new Vector3(mapCenter.x, mapCenter.y + 2 * offset),
                                    new Vector3(mapCenter.x, mapCenter.y - 2 * offset),
                                    new Vector3(mapCenter.x + 2 * offset, mapCenter.y),
                                    new Vector3(mapCenter.x + 2 * offset, mapCenter.y),
                                };
                            }
                            else
                            {
                                SpawnPoints = new Vector3[]
                                {
                                    new Vector3(mapCenter.x, mapCenter.y + offset),
                                    new Vector3(mapCenter.x, mapCenter.y - offset),
                                    new Vector3(mapCenter.x + offset, mapCenter.y),
                                    new Vector3(mapCenter.x - offset, mapCenter.y),
                                    new Vector3(mapCenter.x + 2 * offset, mapCenter.y + offset),
                                    new Vector3(mapCenter.x - 2 * offset, mapCenter.y + offset),
                                    new Vector3(mapCenter.x + 2 * offset, mapCenter.y - offset),
                                    new Vector3(mapCenter.x - 2 * offset, mapCenter.y - offset),
                                    new Vector3(mapCenter.x + offset, mapCenter.y + 2 * offset),
                                    new Vector3(mapCenter.x + offset, mapCenter.y - 2 * offset),
                                    new Vector3(mapCenter.x - offset, mapCenter.y + 2 * offset),
                                    new Vector3(mapCenter.x - offset, mapCenter.y - 2 * offset),
                                };
                            }
                            for (int j = 0; j < SpawnPoints.Length; j++)
                            {
                                Vector3 scaleShotPosition = SpawnPoints[j];
                                GameObject reticle = ObjectPoolManager.SpawnObject(reticlePrefab, SpawnPoints[j], Quaternion.identity);
                                ObjectPoolManager.ReturnObjectToPool(reticle, 2f);
                                Timer.Start(this, 1.5f, () =>
                                {
                                    for (int k = 0; k < scaleShotCount; k++)
                                    {
                                        float angleIncrement = 360f / scaleShotCount;
                                        float angle = k * angleIncrement;
                                        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                                        // ����ScaleShot
                                        GameObject scaleShot = ObjectPoolManager.SpawnObject(ScaleShotPrefab, scaleShotPosition, rotation);
                                        GameObject trail3 = ObjectPoolManager.SpawnObject(TrailEffect3, scaleShotPosition, Quaternion.Euler(0, 0, angle));
                                        ObjectPoolManager.ReturnObjectToPool(trail3, 1f);
                                        scaleShot.GetComponent<ScaleShotEmpty>().empty = this;
                                    }
                                });
                            }
                            yield return new WaitForSeconds(1.75f);
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
                SkillType = Type.TypeEnum.Dark;
                GameObject MeanLookse = Instantiate(MeanLookSE, transform.position, Quaternion.identity);
                Destroy(MeanLookse, 1f);
                StartCoroutine(ReleaseMeanLook());
                IEnumerator ReleaseMeanLook()
                {
                    yield return new WaitForSeconds(1f);
                    GameObject blackCircle = Instantiate(MeanLookPrefab, player.transform.position, Quaternion.identity);
                    UsedMeanLook = true;
                    
                }
                break;
            case 12://����12��ħ����ҫ
                SkillType = Type.TypeEnum.Fairy;
                GameObject dazzlingGleam = ObjectPoolManager.SpawnObject(DazzlingGleamPrefab, transform.position, Quaternion.identity);
                ObjectPoolManager.ReturnObjectToPool(dazzlingGleam, 5f);
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
                            Pokemon.PokemonHpChange(this.gameObject, collider.gameObject, 0, 120, 0, Type.TypeEnum.Fairy);
                            playerinside.KnockOutPoint = 5f;
                            playerinside.KnockOutDirection = (playerinside.transform.position - transform.position).normalized;
                        }
                    }
                }
                break;
            case 13://����13��Ҷ��
                SkillType = Type.TypeEnum.Grass;
                StartCoroutine(ReleaseLeafBlade());
                IEnumerator ReleaseLeafBlade()
                {
                    int shootCount = 10; // �������
                    float shootInterval = 0.3f; // ������
                    if(currentPhase == 2)
                    {
                        shootCount = 15;
                        shootInterval = 0.2f;
                    }
                    else if(currentPhase == 3)
                    {
                        shootCount = 80;
                        shootInterval = 0.1f;
                    }
                    for (int i = 0; i < shootCount; i++)
                    {

                        // ʵ����LeafBlade
                        GameObject LeafBlade = ObjectPoolManager.SpawnObject(LeafBladePrefab, transform.position, Quaternion.identity);
                        LeafBlade.GetComponent<LeafBladeEmpty>().Initialize(AtkTarget.transform, currentPhase, currentPhase == 3 ? Random.Range(0, 3) : 0);
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
                SkillType = Type.TypeEnum.Rock;
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
                SkillType = Type.TypeEnum.Flying;
                StartCoroutine(ReleaseAirSlash());
                IEnumerator ReleaseAirSlash()
                {
                    float intervalTime = 1.3f;
                    int Times = 3;
                    if (currentPhase == 2)
                    {
                        intervalTime = 0.8f;
                        Times = 5;
                    }
                    else if(currentPhase ==3)
                    {
                        intervalTime = 0.3f;
                        Times = 13;
                    }
                    for(int i = 0;i< Times;i++)
                    {
                        GameObject airSlash = Instantiate(AirSlashPrefab, transform.position, Quaternion.identity);
                        airSlash.GetComponent<AirSlashMew>().empty = this;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 16://����16���Խ�
                SkillType = Type.TypeEnum.Steel;
                StartCoroutine(ReleaseMakeItRain());
                IEnumerator ReleaseMakeItRain()
                {
                    float angle = 0f;
                    float angleIncrement = currentPhase == 3 ? 10f : 8f;
                    int Degree = 80;
                    int Times = 3;
                    if(currentPhase == 3)
                    {
                        Degree = 180;
                        Times = 6;
                    }
                    for (int i = 0; i < Degree; i++) 
                    {
                        for (int j = 0; j < Times; j++)
                        {
                            float currentAngle = angle + j * (360f / Times);
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            MakeItRainEmpty makeitrain = ObjectPoolManager.SpawnObject(MakeItRainPrefab, transform.position, Quaternion.identity).GetComponent<MakeItRainEmpty>();
                            makeitrain.MIRrotate(direction);
                            makeitrain.empty = this;
                        }
                        if (currentPhase == 3) 
                        {
                            float decreasedAngle = (i - 50) / 100f * Mathf.PI;
                            angleIncrement = Random.Range(15f, 30f) * Mathf.Cos(decreasedAngle);
                        }
                        angle += angleIncrement;
                        yield return new WaitForSeconds(0.07f);
                    }
                }
                break;
            case 17://����17������
                SkillType = Type.TypeEnum.Bug;
                if (currentPhase == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector3 randomPosition = mapCenter + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);
                        Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
                    }
                }
                else
                {
                    for (int i = 0; i < (currentPhase == 3 ? 25 : 14); i++) 
                    {
                        Vector3 randomPosition = mapCenter + new Vector3(Random.Range(-24.0f, 24.0f), Random.Range(-14.0f, 14.0f), 0);
                        Instantiate(StickyWebPrefab, randomPosition, Quaternion.identity);
                    }
                }
                break;
            case 18://����18��ʮ�ֶ���
                SkillType = Type.TypeEnum.Poison;
                StartCoroutine(ReleaseCrossPoison());
                IEnumerator ReleaseCrossPoison()
                {
                    float angle = 0f;
                    float angleIncrement = 45f;
                    int Degree = 4;
                    int Times = 4;
                    float intervalTime = 1f;
                    if (currentPhase == 2)
                    {
                        Degree = 5;
                        Times = 4;
                        angleIncrement = 60f;
                        intervalTime = 0.8f;
                    }
                    else if (currentPhase == 3)
                    {
                        Degree = 5;
                        Times = 8;
                        angleIncrement = 0f;
                        intervalTime = 0.8f;
                    }
                    for (int i= 0; i < Degree; i++)
                    {
                        for (int j = 0; j < Times; j++)
                        {
                            float currentAngle = angle + j * (360f / Times);
                            Vector3 direction = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.up;
                            CrossPoisonEmpty crossPoison = Instantiate(CrossPoisonPrefab, transform.position, Quaternion.identity).GetComponent<CrossPoisonEmpty>();
                            crossPoison.Initialize(direction, currentPhase);
                            crossPoison.empty = this;
                        }
                        //���ܼ���ȴ�ʱ��
                        angle += angleIncrement;
                        yield return new WaitForSeconds(intervalTime);
                    }
                }
                break;
            case 19://����19����ʥ֮��
                SkillType = Type.TypeEnum.Fire;
                StartCoroutine(ReleaseScaredFire());
                IEnumerator ReleaseScaredFire()
                {
                    if (currentPhase != 3)
                    {
                        int numPoints = 6; // ���ϵĵ���
                        float radius = 15f; // �ǵĶ��㵽���ĵľ���

                        Vector3[] starVertices = new Vector3[numPoints];

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

                            GameObject secredFireCenter = ObjectPoolManager.SpawnObject(SecredFireCentrePrefab, center, Quaternion.identity);
                            secredFireCenter.GetComponent<SecredFireEmpryCentre>().empty = this;
                        }
                        // ��ÿ������Ƕ�������SecredFire
                        for (int i = 0; i < numPoints; i++)
                        {
                            GameObject secredFireVertex = ObjectPoolManager.SpawnObject(SecredFireVertexPrefab, starVertices[i], Quaternion.identity);
                            secredFireVertex.GetComponent<SecredFireEmptyVertex>().empty = this;
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
                                SecredFireEmpty secredFire = ObjectPoolManager.SpawnObject(SecredFirePrefab, secredFirePosition, Quaternion.identity).GetComponent<SecredFireEmpty>();
                                secredFire.empty = this;
                                secredFire.Initialize(player.transform.position, 3f);
                            }
                        }
                        yield return null;
                    }
                    else
                    {
                        int numPoints = 8; // ������ϵĵ���
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
                        // ����Բ�����ĵ�SecredFire
                        for (int i = 0; i < numPoints; i++)
                        {
                            Vector3 startPoint = starVertices[i];
                            Vector3 endPoint = starVertices[(i + 1) % numPoints];

                            Vector3 center = (startPoint + endPoint) / 2f;

                            GameObject secredFireCenter = ObjectPoolManager.SpawnObject(SecredFireCentrePrefab, center, Quaternion.identity);
                            secredFireCenter.GetComponent<SecredFireEmpryCentre>().empty = this;
                        }
                        // ��ÿ������Ƕ�������SecredFire
                        for (int i = 0; i < numPoints; i++)
                        {
                            GameObject secredFireVertex = ObjectPoolManager.SpawnObject(SecredFireVertexPrefab, starVertices[i], Quaternion.identity);
                            secredFireVertex.GetComponent<SecredFireEmptyVertex>().empty = this;
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
                                SecredFireEmpty secredFire = ObjectPoolManager.SpawnObject(SecredFirePrefab, secredFirePosition, Quaternion.identity).GetComponent<SecredFireEmpty>();
                                secredFire.empty = this;
                                secredFire.Initialize(player.transform.position, 3f);
                            }
                        }
                    }
                }
                break;
            case 20://����20��ʥ��
                SkillType = Type.TypeEnum.Fighting;
                int Times;
                if(currentPhase == 3)
                {
                    Times = 4;
                }
                else
                {
                    Times = 3;
                }
                StartCoroutine(ReleaseSecredSword());
                IEnumerator ReleaseSecredSword()
                {   
                    for(int i = 0;i<Times; i++)  
                    {
                        Instantiate(reticle2Prefab, AtkTarget.transform.position, Quaternion.identity);
                        float randomAngle = Random.Range(180f, 480f);
                        for(int j = 0; j < 6; j++)
                        {
                            float angle = j * 60;
                            float radius = 10f;
                            Vector3 spawnPos = AtkTarget.transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
                            SecredSwordEmpty secredSword = Instantiate(SecredSwordPrefab, spawnPos, Quaternion.identity).GetComponent<SecredSwordEmpty>();
                            secredSword.empty = this;
                            secredSword.Initialize(angle, radius, AtkTarget, randomAngle);
                            
                        }
                        yield return new WaitForSeconds(3f);
                    }

                }
                break;
        }
        //���׶��ܹ��Ƴ��ļ��ܣ�����ѩ��2����������6���ͼ�ʯ������14��
    }
    void SkillTimerUpdate(int skillindex, int stage)//��������ʱ�����
    {
        skillTimer = SkillTimer[skillindex][stage];
    }
    void ResetSkillTimer()
    {//Ϊ�����볡��ת�׶�û��ôͻȻ
        if (!isReset)
        {
            skillTimer = 3f;
            isReset = true;
        }
    }

    #endregion

    #region ������ɫ
    void ChangeColor()
    {
        float factor = Mathf.Pow(2, intensity);
        if (!isFinal)
        {
            Material[] srcs = new Material[]
            {
                src1,
                src2,
                src3
            };
            switch (SkillType)
            {
                case Type.TypeEnum.Normal:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[0].r * factor, colors[0].g * factor, colors[0].b * factor, 0));
                    }
                    break;
                case Type.TypeEnum.Fighting:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[1].r * factor, colors[1].g * factor, colors[1].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Flying:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[2].r * factor, colors[2].g * factor, colors[2].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Poison:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[3].r * factor, colors[3].g * factor, colors[3].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Ground:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[4].r * factor, colors[4].g * factor, colors[4].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Rock:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[5].r * factor, colors[5].g * factor, colors[5].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Bug:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[6].r * factor, colors[6].g * factor, colors[6].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Ghost:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[7].r * factor, colors[7].g * factor, colors[7].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Steel:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[8].r * factor, colors[8].g * factor, colors[8].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Fire:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[9].r * factor, colors[9].g * factor, colors[9].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Water:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[10].r * factor, colors[10].g * factor, colors[10].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Grass:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[11].r * factor, colors[11].g * factor, colors[11].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Electric:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[12].r * factor, colors[12].g * factor, colors[12].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Psychic:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[13].r * factor, colors[13].g * factor, colors[13].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Ice:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[14].r * factor, colors[14].g * factor, colors[14].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Dragon:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[15].r * factor, colors[15].g * factor, colors[15].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Dark:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[16].r * factor, colors[16].g * factor, colors[16].b * factor, 1));
                    }
                    break;
                case Type.TypeEnum.Fairy:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", new Color(colors[17].r * factor, colors[17].g * factor, colors[17].b * factor, 1));
                    }
                    break;
                default:
                    for (int i = 0; i < srcs.Length; i++)
                    {
                        srcs[i].SetColor("_Color", Color.white);
                    }
                    break;
            }
        }
        else
        {
            src1.SetColor("_Color", new Color(Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time + 0.5f, 1), Mathf.PingPong(Time.time + 1f, 1)));
            src2.SetColor("_Color", src1.color);
            src3.SetColor("_Color", src1.color);
        }
        
    }

    #endregion

    Vector3 RamdomTeleport()
    {
        bool collided = false;
        Vector3 randomPosition;
        teleportAttempts++;
        if (teleportAttempts > 150)
        {
            // ������Դ�������150�Σ���ȡ���������
            Debug.LogWarning("û�гɹ��ҵ����ʵĴ���λ��");
            return Vector3.zero;
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
                return RamdomTeleport();
            }
            teleportAttempts = 0;
            return randomPosition;
        }
        //���׶θ����ƶ��������Χ��������Ӱ�죩
        else if (currentPhase == 2)
        {
            float minDistance = 5f;
            float maxDistance = 10f;
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minDistance, maxDistance);
            randomPosition = AtkTarget.transform.position + randomDirection * randomDistance;
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
                return RamdomTeleport();
            }
            teleportAttempts = 0;
            //��֮���и����ƶ�
            return randomPosition;
        }
        return Vector3.zero;
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

    #region ��һ�׶�
    private IEnumerator Phase1Skill(int randomSkillIndex)
    {
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);

        //���ѡ�����ͷ�
        if(randomSkillIndex == 5||randomSkillIndex == 16)
        {
            transform.position = mapCenter;//������Ϊ�Խ𳱻���̫������ʱ���͵����м�
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
    #endregion

    #region �ڶ��׶�
    private IEnumerator Phase2Skill(int randomSkillIndex)
    {
        if (randomSkillIndex == 2 || randomSkillIndex == 6 || randomSkillIndex == 14)
        {
            yield break;
        }
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
        //������е��ӵ�
        uIHealth.ChangeHpUp();
        ClearProjectile();

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
        transform.parent.parent.GetComponent<Room>().isClear = 0;
        player.NowRoom = new Vector3Int(100, 100, 0);
        player.InANewRoom = true;
        player.NewRoomTimer = 0f;
        currentPhase++;

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

        //�ĸ�Χ���ŵ���
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);
        turningPhase = false;
        Invincible = false;
    }

    #endregion

    #region �����׶�
    private IEnumerator Phase3Start()
    {//�����׶β�������Ӱ�죡
        animator.SetTrigger("Teleport");
        uIHealth.Fade(1f, false);
        yield return new WaitForSeconds(1f);
        transform.position = mapCenter;
        Transform uitext = player.transform.GetChild(2).GetChild(3);
        if (uitext)
        {
            uitext.GetComponent<PlayerUIText>().SetText("��ֹʹ�õ���\n��ֹ�򿪲˵�");
        }
        player.CanNotUseSpaceItem = true;//���������ʹ����������
        MewOrbRotate mewOrbRotate = Phase3OrbRotate.GetComponent<MewOrbRotate>();
        StartCoroutine(mewOrbRotate.ActivatePhase3Effect(1, 30, 20f));
        //�޸�ui
        Image timebar1 = timeBar1.GetComponent<Image>();
        timebar1.sprite = TimeBar1;
        Image timebar2 = timeBar2.GetComponent<Image>();
        timebar2.sprite = TimeBar2;
        Image timebar3 = timeBar3.GetComponent<Image>();
        timebar3.sprite = TimeBar3;
        Image timebar4 = timeBar4.GetComponent<Image>();
        timebar4.sprite = TimeBar4;
        uIHealth.Fade(1f, true);
        //����ӵ�
        ClearProjectile();
    }

    //���׶�-�ڶ�����
    private IEnumerator Phase3Middle() 
    {
        //������Բ���ͷŻ���������˺��ļ���
        SkillType = Type.TypeEnum.Fighting;
        TeleportEnd();
        UseSkill(20);
        for (int j = 0; j < 7; j++)
        {
            float increaseAngle = 9f;
            float angleStep = 360f / 16;
            for (int i = 0; i < 32; i++)
            {
                float angle = j * increaseAngle + i * angleStep;
                Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * 1f;
                FakeLove fakelove = Instantiate(FakeLovePrefab, spawnPos, Quaternion.identity).GetComponent<FakeLove>();
                Vector3 direction = (spawnPos - transform.position).normalized;
                fakelove.Initialize(4f, direction);
                fakelove.mew = gameObject;
            }
            yield return new WaitForSeconds(1.5f);
        }

        //����ͷŵ����ͷű�������
        StartCoroutine(ElectricBall());
        yield return new WaitForSeconds(4f);
        //�ͷż�ҩ
        for (int i = 0; i < 50; i++)
        {
            Vector2 randomPosition = RandomPosition();
            int randomIndex = Random.Range(1, 7);
            GameObject RandomFake = null;
            switch (randomIndex)
            {
                case 1: RandomFake = FakePotionPrefab; break;
                case 2: RandomFake = FakeAntidote; break;
                case 3: RandomFake = FakeAwakening; break;
                case 4: RandomFake = FakeBurnHeal; break;
                case 5: RandomFake = FakeIceHeal; break;
                case 6: RandomFake = FakeParalyzeHeal; break;
            }
            GameObject fakepotion = Instantiate(RandomFake, randomPosition, Quaternion.identity);
            Destroy(fakepotion, 20f);
        }
        SkillType = Type.TypeEnum.Ice;
        TeleportEnd();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 560; i++)
        {
            float angle = i * 11f;
            GameObject trail = ObjectPoolManager.SpawnObject(TrailEffect, transform.position, Quaternion.Euler(0, 0, angle));
            StartCoroutine(IceBeam(i));
            ObjectPoolManager.ReturnObjectToPool(trail, 2f);
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(2f);
        //���ռ���
        GameObject MeanLookse = ObjectPoolManager.SpawnObject(MeanLookSE, transform.position, Quaternion.identity);
        ObjectPoolManager.ReturnObjectToPool(MeanLookse, 1f);
        yield return new WaitForSeconds(1f);
        isFinal = true;
        GameObject meanlookfinal = Instantiate(Meanlookfinal, transform.position, Quaternion.identity);
        StartCoroutine(ShootSwords(324, 3, 0.1f, false));
        yield return new WaitForSeconds(17f);
        StartCoroutine(ShootSwords(6, 8, 3f, true));
        yield return new WaitForSeconds(19f);
        Destroy(meanlookfinal, 1f);
    }

    //�����׶ε���
    private IEnumerator ElectricBall()
    {
        for(int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                float angle = 10f * i + j * 30f;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                float speed = (j % 2 == 0) ? 15f : -15f;
                ElectroBallEmpty electricBall = Instantiate(ElectricBallPrefab, transform.position, rotation).GetComponent<ElectroBallEmpty>();
                electricBall.Initialize(transform.position, speed);
                electricBall.empty = this;
            }
            yield return new WaitForSeconds(4f);
        }
    }
    //�����׶εı�������
    private IEnumerator IceBeam(int i)
    {
        yield return new WaitForSeconds(2f);
        float angle = i * 11f;
        GameObject icebeam = ObjectPoolManager.SpawnObject(IceBeamPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        icebeam.GetComponent<IceBeamEmpty>().empty = this;
    }

    private IEnumerator ShootSwords(int shootTimes, int shootAmounts, float intervalTime, bool needTrail)
    {
        float angleIncreasement = 6f;
        float angle = 0f;
        for (int i = 0; i < shootTimes; i++)
        {
            for (int j = 0; j < shootAmounts; j++)
            {
                float currentAngle = j * 360 / shootAmounts + angle;
                Vector2 spawnPosition = mapCenter + (Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right * 20f);
                if (needTrail)
                {
                    GameObject trail = ObjectPoolManager.SpawnObject(TrailEffect2, transform.position, Quaternion.Euler(0, 0, currentAngle));
                    ObjectPoolManager.ReturnObjectToPool(trail, 1f);
                }
                GameObject swords = Instantiate(Swords, spawnPosition, Quaternion.identity);
                SwordsMew swordsmew = swords.GetComponent<SwordsMew>();
                swordsmew.Initialize(mapCenter, i % 18, i);
                swordsmew.empty = this;
                
            }
            angle += angleIncreasement;
            yield return new WaitForSeconds(intervalTime);
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

    private IEnumerator Phase3End()
    {
        animator.SetTrigger("Die");
        CameraShake(1.5f, 1f, true);
        yield return new WaitForSeconds(0.4f);
        isFinal = false;
        MewBossKilled = true;
        //����Ҵ��ͻ�ԭ������
        GameObject mask = Instantiate(Phase2Mask, transform.position, Quaternion.identity);
        Destroy(mask, 2.2f);
        yield return new WaitForSeconds(1.1f);
        player.NowRoom = GetnowRoom;
        player.transform.position = GetPlayerPosition;
        player.InANewRoom = true;
        player.NewRoomTimer = 0f;

        //ɫ��ͷ���رգ�
        cameraAdapt.DeactivateVcam();
        cameraAdapt.ShowCameraMasks();
        Camera.transform.position = GetCameraPostion;
        UISkillButton.Instance.isEscEnable = true;

        //�������������ж���
        ObjectPoolManager.DestoryObjectInPool(true);

        //�λøµ�
        player.CanNotUseSpaceItem = false;
        Invincible = false;
        EmptyHp = 0;
        EmptyDie();
    }

    #endregion

    private void ChangeType(int randomIndex)
    {
        switch(randomIndex)
        {
            case 1: SkillType = Type.TypeEnum.Grass; break;
            case 2: SkillType = Type.TypeEnum.Ice; break;
            case 3: SkillType = Type.TypeEnum.Fire; break;
            case 4: SkillType = Type.TypeEnum.Normal; break;
            case 5: SkillType = Type.TypeEnum.Normal; break;
            case 6: SkillType = Type.TypeEnum.Ghost; break;
            case 7: SkillType = Type.TypeEnum.Fire; break;
            case 8: SkillType = Type.TypeEnum.Ice; break;
            case 9: SkillType = Type.TypeEnum.Fairy; break;
            case 10: SkillType = Type.TypeEnum.Dragon;break;
            case 11: SkillType = Type.TypeEnum.Dark;break;
            case 12: SkillType = Type.TypeEnum.Fairy;break;
            case 13: SkillType = Type.TypeEnum.Grass; break;
            case 14: SkillType = Type.TypeEnum.Rock; break;
            case 15: SkillType = Type.TypeEnum.Flying; break;
            case 16: SkillType = Type.TypeEnum.Steel; break;
            case 17: SkillType = Type.TypeEnum.Bug; break;
            case 18: SkillType = Type.TypeEnum.Poison;break;
            case 19: SkillType = Type.TypeEnum.Fire;break;
            case 20: SkillType = Type.TypeEnum.Fighting;break;
        }
    }

    private void TeleportEnd()
    {
        GameObject useskillprefab = Instantiate(TeleportEndPrefab, transform.position, Quaternion.identity);
        ParAnimation useskillmask = useskillprefab.GetComponent<ParAnimation>();
        switch (SkillType)
        {
            case Type.TypeEnum.Normal: useskillmask.startColor = colors[0]; break;
            case Type.TypeEnum.Fighting: useskillmask.startColor = colors[1]; break;
            case Type.TypeEnum.Flying: useskillmask.startColor = colors[2]; break;
            case Type.TypeEnum.Poison: useskillmask.startColor = colors[3]; break;
            case Type.TypeEnum.Ground: useskillmask.startColor = colors[4]; break;
            case Type.TypeEnum.Rock: useskillmask.startColor = colors[5]; break;
            case Type.TypeEnum.Bug: useskillmask.startColor = colors[6]; break;
            case Type.TypeEnum.Ghost: useskillmask.startColor = colors[7]; break;
            case Type.TypeEnum.Steel: useskillmask.startColor = colors[8]; break;
            case Type.TypeEnum.Fire: useskillmask.startColor = colors[9]; break;
            case Type.TypeEnum.Water: useskillmask.startColor = colors[10]; break;
            case Type.TypeEnum.Grass: useskillmask.startColor = colors[11]; break;
            case Type.TypeEnum.Electric: useskillmask.startColor = colors[12]; break;
            case Type.TypeEnum.Psychic: useskillmask.startColor = colors[13]; break;
            case Type.TypeEnum.Ice: useskillmask.startColor = colors[14]; break;
            case Type.TypeEnum.Dragon: useskillmask.startColor = colors[15]; break;
            case Type.TypeEnum.Dark: useskillmask.startColor = colors[16]; break;
            case Type.TypeEnum.Fairy: useskillmask.startColor = colors[17]; break;
            default: useskillmask.startColor = Color.white; break;
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

    void ClearProjectile()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectel");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }
}
