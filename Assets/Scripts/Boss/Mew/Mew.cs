using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mew : Empty
{
    public bool isEmptyU = true;
    public bool isEmptyD = true;
    public bool isEmptyL = true;
    public bool isEmptyR = true;
    Vector2 position;
    Vector2Int direction;

    public GameObject magicalLeafPrefab;//����1
    public float delayBetweenLeaves = 0.3f; // ÿƬMagicalLeaf֮����ӳ�ʱ��
    int Leafnum;//ħ��Ҷ����
    public GameObject blizzardPrefab;//����2
    public GameObject WillOWispPrefab;//����3
    public int numWillOWisp = 12; // WillOWisp������
    public float radius = 2f; // WillOWisp�����ɰ뾶
    public float moveSpeed = 4f; // WillOWisp���ƶ��ٶ�

    public float skillCooldown = 4f; // ������ȴʱ�䣬��λΪ��

    private int currentPhase = 1; // ��ǰ�׶�
    private float skillTimer = 0f; // ���ܼ�ʱ��
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Psychic;//�λõ�����Ϊ����
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
        Emptylevel = SetLevel(player.Level, 100);//���޵ȼ�100
        EmptyHpForLevel(Emptylevel);//���ó�ʼѪ��
        

        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//�����ȼ�
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;//����ֵ

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = new Vector2Int(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            UpdateEmptyChangeHP();
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
            int randomSkillIndex = Random.Range(1, 5);
            UseSkill(randomSkillIndex);

            // ���ü��ܼ�ʱ��
            skillTimer = skillCooldown;
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
            case 3:
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float angleStep = 360f / numWillOWisp; // ����ÿ��WillOWisp֮��ĽǶȼ��
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = i * angleStep; // ���㵱ǰWillOWisp�ĽǶ�
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * radius; // ���㵱ǰWillOWisp������λ��
                            GameObject willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity);
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.GetComponent<WillOWispEmpty>().Initialize(moveSpeed, direction); // ����WillOWisp���ƶ��ٶ�
                            Destroy(willOWisp, 4f);
                        }
                        yield return new WaitForSeconds(1f);
                    }
                }
                break;
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
