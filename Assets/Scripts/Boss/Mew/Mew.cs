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

    public GameObject magicalLeafPrefab;//技能1
    public float delayBetweenLeaves = 0.3f; // 每片MagicalLeaf之间的延迟时间
    int Leafnum;//魔法叶数量
    public GameObject blizzardPrefab;//技能2
    public GameObject WillOWispPrefab;//技能3
    public int numWillOWisp = 12; // WillOWisp的数量
    public float radius = 2f; // WillOWisp的生成半径
    public float moveSpeed = 4f; // WillOWisp的移动速度

    public float skillCooldown = 4f; // 技能冷却时间，单位为秒

    private int currentPhase = 1; // 当前阶段
    private float skillTimer = 0f; // 技能计时器
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Psychic;//梦幻的属性为超能
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, 100);//上限等级100
        EmptyHpForLevel(Emptylevel);//设置初始血量
        

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
            int randomSkillIndex = Random.Range(1, 5);
            UseSkill(randomSkillIndex);

            // 重置技能计时器
            skillTimer = skillCooldown;
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
            case 3:
                StartCoroutine(ReleaseWillOWisp());
                IEnumerator ReleaseWillOWisp()
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float angleStep = 360f / numWillOWisp; // 计算每个WillOWisp之间的角度间隔
                        for (int i = 0; i < numWillOWisp; i++)
                        {
                            float angle = i * angleStep; // 计算当前WillOWisp的角度
                            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * radius; // 计算当前WillOWisp的生成位置
                            GameObject willOWisp = Instantiate(WillOWispPrefab, spawnPos, Quaternion.identity);
                            Vector3 direction = (spawnPos - transform.position).normalized;
                            willOWisp.GetComponent<WillOWispEmpty>().Initialize(moveSpeed, direction); // 设置WillOWisp的移动速度
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
