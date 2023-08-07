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

    public float skillCooldown = 2f; // 技能冷却时间，单位为秒

    private int currentPhase = 1; // 当前阶段
    private float skillTimer = 0f; // 技能计时器
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Psychic;//梦幻的属性为超能
        EmptyType02 = 0;
        Emptylevel = SetLevel(player.Level, 100);//上限等级100
        EmptyHpForLevel(Emptylevel);//设置初始血量
        
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//能力等级
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;//经验值

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
            Debug.Log("进入二阶段");
            EmptyHp = CalculateBossHealth();
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
    void Phase2()
    {
        if (EmptyHp == 0)
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
