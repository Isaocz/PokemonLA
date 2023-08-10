using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mew : Empty
{
    public GameObject magicalLeafPrefab;//����1
    public float delayBetweenLeaves = 0.3f; // ÿƬMagicalLeaf֮����ӳ�ʱ��
    int Leafnum;//ħ��Ҷ����
    public GameObject blizzardPrefab;//����2
    public GameObject WillOWispPrefab;//����3
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
    public int icicleCount = 8;
    public float delayBetweenExecutions = 1f;
    public int numExecutions = 3;
    public GameObject HeartStampPrefab;//����9
    public float HeartStampRadius=1.5f;
    public int heartStampCount = 8;
    public GameObject reticlePrefab; //ReticleԤ����
    public GameObject ScaleShotPrefab; //����10
    public int scaleShotCount;
    public GameObject MeanLookPrefab;//����11
    public GameObject DazzlingGleamPrefab;//����12

    float skillCooldown;
    //Audio
    public BackGroundMusic bgmScript;
    //�׶�
    private int currentPhase = 1; // ��ǰ�׶�
    private float skillTimer = 0f; // ���ܼ�ʱ��
    private List<GameObject> heartStamps = new List<GameObject>();//��HeartStamp���д洢
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
            Debug.Log("������׶�");
            EmptyHp = CalculateBossHealth();
            ClearStatusEffects();
        }
        else if (skillTimer <= 0f)
        {
            // ���ѡ��һ�������ͷ�
            RamdomTeleport();
            int randomSkillIndex = Random.Range(1, 13);
            UseSkill(randomSkillIndex);
            SkillTimerUpdate(randomSkillIndex, 1);
        }

        // ���ܼ�ʱ���ݼ�
            skillTimer -= Time.deltaTime;
    }
    void Phase2()
    {
        if (EmptyHp <= 0)
        {
            currentPhase++;
            Debug.Log("�������׶�");
            EmptyHp = 1;
            ClearStatusEffects();
        }
        else if (skillTimer <= 0f)
        {
            // ���ѡ��һ�������ͷ�
            int randomSkillIndex = Random.Range(1, 25);
            UseSkill(randomSkillIndex);

            // ���ü��ܼ�ʱ��
            skillTimer = skillCooldown;
        }

        // ���ܼ�ʱ���ݼ�
        skillTimer -= Time.deltaTime;
    }
    void Phase3()
    {
        //���λ��޵У���ʱ�޷�����
        Invincible = true;
    }
    void UseSkill(int skillIndex)
    {
        // �������д�ͷż��ܵĴ���
        Debug.Log("Boss used skill: " + skillIndex);
        switch (skillIndex)
        {
            case 1:
                //����1��ħ��Ҷ
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
                        // ��Mewλ��ʵ����ħ��Ҷ
                        GameObject magicalLeaf = Instantiate(magicalLeafPrefab, transform.position, Quaternion.identity);
                        Destroy(magicalLeaf, 6f); // 6�������ħ��Ҷ����
                        yield return new WaitForSeconds(delayBetweenLeaves);
                    }
                }
                break;
            case 2:
                //����2������ѩ
                GameObject blizzard = Instantiate(blizzardPrefab, transform.position, Quaternion.identity);
                Destroy(blizzard, 6f);//6������ٱ���ѩ����
                break;
            case 3://����3���׻�
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float angleStep = 360f / numWillOWisp; // ����ÿ��WillOWisp֮��ĽǶȼ��
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = i * angleStep; // ���㵱ǰWillOWisp�ĽǶ�
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * WillOWispRadius; // ���㵱ǰWillOWisp������λ��
                            GameObject willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity);
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.GetComponent<WillOWispEmpty>().Initialize(moveSpeed, direction); // ����WillOWisp���ƶ��ٶ�
                            Destroy(willOWisp, 4f);
                        }
                        yield return new WaitForSeconds(1f);
                    }
                }
                break;
            case 4://����4�������ദ
                GameObject PlayNice = Instantiate(PlayNicePrefab, transform.position, Quaternion.identity);
                Destroy(PlayNice, 5f);
                break;
            case 5://����5��̫������
                //�ͷ�3�����⣬�ֱ�λ��90�ȡ�210�ȡ�330�ȵ�λ��
                transform.position = transform.parent.position;
                //������������
                float[] angles = { 90f, 210f, 330f };
                for (int i = 0; i < angles.Length; i++)
                {
                    //���㼤�����ʼ����յ�
                    float angle = angles[i];
                    Vector3 startPoint = transform.parent.position;
                    Vector3 endPoint = new Vector3(15f * Mathf.Cos(Mathf.Deg2Rad * angle), 15f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                    GameObject Terablast = Instantiate(TeraBlastPrefab, startPoint, Quaternion.identity);
                    TeraBlastEmpty terablast = Terablast.GetComponent<TeraBlastEmpty>();
                    terablast.SetEndpoints(startPoint, endPoint, angle);
                    terablast.SetColors(Color.yellow, Color.red);
                }
                break;
            case 6://����6������
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
            case 7://����7��ħ������
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

            case 8://����8����׶
                StartCoroutine(SummonIcicleSpears());
                break;
            case 9://����9������ӡ��
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
            case 10://����10������
                // ����ScaleShot
                StartCoroutine(ReleaseScaleShoot());
                IEnumerator ReleaseScaleShoot()
                {
                    for (int j = 0; j < 3; j++)
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

                            // ����ScaleShot���ɺ���ͬ��������Ҫ��ScaleShot���һ�������ƶ��Ľű�
                            ScaleShotEmpty scaleShotMovement = scaleShot.GetComponent<ScaleShotEmpty>();
                        }
                    }
                }
                break;
                case 11: //����11����ɫĿ��
                StartCoroutine(ReleaseMeanLook());
                IEnumerator ReleaseMeanLook()
                {
                    yield return new WaitForSeconds(1f);
                    GameObject blackCircle = Instantiate(MeanLookPrefab, player.transform.position, Quaternion.identity);

                    // ��ȡ��ɫԲ�İ뾶
                    float circleRadius = blackCircle.GetComponent<CircleCollider2D>().radius;

                    // �ں�ɫԲ��Χ��������ҵ��ƶ�
                    while (blackCircle != null)
                    {
                        // ����������ɫԲ�ľ���
                        float distance = Vector2.Distance(player.transform.position, blackCircle.transform.position);

                        if (distance > circleRadius)
                        {
                            // �����Ҿ����ɫԲ�ľ������Բ�뾶��������ƶ��غ�ɫԲ�ķ�Χ��
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
                        }
                    }
                }
                break;

        }
    }
    void SkillTimerUpdate(int skillindex, int stage)//��������ʱ�����
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
        // �ڷ��������ѡ��һ��λ��
        Vector3 randomPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);

        // �����"Wall"��"Environment"��ǩ�Ķ����Ƿ���ײ
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
            //�����ײ����Ѱ��λ��
            RamdomTeleport();
        }
        else
        {
            //��֮����˲���ƶ�
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
        // �������д����쳣״̬�Ĵ���
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
