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

    public float skillCooldown = 2f; // ������ȴʱ�䣬��λΪ��

    private int currentPhase = 1; // ��ǰ�׶�
    private float skillTimer = 0f; // ���ܼ�ʱ��
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Psychic;//�λõ�����Ϊ����
        EmptyType02 = 0;
        Emptylevel = SetLevel(player.Level, 100);//���޵ȼ�100
        EmptyHpForLevel(Emptylevel);//���ó�ʼѪ��
        
        player = GameObject.FindObjectOfType<PlayerControler>();//��ȡ���
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
        if (EmptyHp == 0)
        {
            currentPhase++;
            Debug.Log("������׶�");
            EmptyHp = CalculateBossHealth();
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
    void Phase2()
    {
        if (EmptyHp == 0)
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
