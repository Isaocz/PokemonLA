using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * editor 填充：
 * 异常状态材质
 * player ui state
 * ui health
 * 属性，种族值
 * base exp 基础经验
 * HWP 取得努力值
 * skinRenderers body渲染器数组（可能）
 * 
 * code part:
 * base part 1,2,3,4
 * 
 * ani:
 * param: Die, Hit
 * clip: Die(EmptyDestroy), Hit
 * 
 * 异常状态：
 * 着迷（FindAtkTarget()处理），混乱（isEmptyConfusionDone），致盲（isSilence）
 * 畏缩（isFearDone），沉睡（isSleepDone），麻痹（isCanNotMoveWhenParalysis）
 * 
 * 替身（FindAtkTarget）
*/

public class Trapinch : Empty
{

    //[Label("观察半径")]
    public float foundRadius = 8;
    //[Label("发动咬咬的半径")]
    public float biteRadius = 1.2f;
    //[Label("扩展流沙地狱的最小半径")]
    public float growSandsRadius = 5;
    //[Label("扩展流沙地狱距离开始的最小时间")]
    public float growSandsDuration = 2;
    //[Label("咬咬cd")]
    public float cdBite = 2;
    //[Label("流沙地狱cd")]
    public float cdSands = 14;
    //[Label("麻痹不能动概率")]
    public float proParalysis = 0.33f;
    //[Label("prefab咬咬")]
    public GameObject biteObj;
    //[Label("prefab流沙地狱")]
    public GameObject sandsObj;

    private enum AI_STATE
    {
        IDLE,
        ATK_BITE,
        ATK_SANDS,
        ATK_SANDS_GROW,
    }

    private float lastUseBite = 0;
    private float lastUseSands = 0;
    private GameObject sandsHolding = null;

    private AI_STATE aiState;
    private IEnumerator timerAi = null;

    // Start is called before the first frame update
    void Start()
    {
        // * base part 1
        speed = 0f;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, 30);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        lastUseBite = -cdBite;
        lastUseSands = -cdSands;
    }

    // Update is called once per frame
    void Update()
    {
        
        // * base part 2
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {
        // * base part 3
        ResetPlayer();
        if (!isBorn)
        {
            EmptyBeKnock();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // * base part 4
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }

    

    private void StartAiTimer()
    {
        // 反应频率20帧
        float aiDuration = 0.05f;
        timerAi = Timer.Start(this, 0f, () =>
        {
            if (isDie)
            {
                StopCoroutine(timerAi);
                return 0;
            }
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateinfo.IsName("TrapinchLeftBeHit") || stateinfo.IsName("TrapinchRightBeHit") || isSilence || isFearDone || isSleepDone)
            {
                return aiDuration;
            }
            if (stateinfo.IsName("TrapinchLeftIdle") || stateinfo.IsName("TrapinchRightIdle"))
            {
                aiState = AI_STATE.IDLE;
                if (sandsHolding)
                {
                    aiState = AI_STATE.ATK_SANDS;
                    if (sandsHolding.GetComponent<TrapinchSandTomb>().IsGrowUp)
                    {
                        aiState = AI_STATE.ATK_SANDS_GROW;
                    }
                }
            }
            if (aiState == AI_STATE.IDLE)
            {
                GameObject target = FindAtkTarget(foundRadius);
                //target = null;
                if (target)
                {
                    Vector2 dis = target.transform.position - transform.position;
                    animator.SetFloat("Direction", dis.x);
                    if (Vector2.Distance(transform.position, target.transform.position) <= biteRadius && Time.time - lastUseBite > cdBite)
                    {
                        triggerBite();
                    }
                    else if (Vector2.Distance(transform.position, target.transform.position) <= foundRadius && Time.time - lastUseSands > cdSands)
                    {
                        if (!isCanNotMoveWhenParalysis || Random.Range(0.0f, 1.0f) > proParalysis)
                        {
                            animator.SetTrigger("Sands");
                            aiState = AI_STATE.ATK_SANDS;
                        }
                        lastUseSands = Time.time;
                    }
                }
            }
            else if (aiState == AI_STATE.ATK_SANDS)
            {
                GameObject target = FindAtkTarget(growSandsRadius);
                if (target && Vector2.Distance(transform.position, target.transform.position) <= biteRadius && Time.time - lastUseBite > cdBite)
                {
                    triggerBite();
                }
                else if (Time.time - lastUseSands > growSandsDuration)
                {
                    if (!target)
                    {
                        //不在范围内，尝试扩展流沙地狱
                        if (sandsHolding && sandsHolding.GetComponent<TrapinchSandTomb>().IsCanGrowUp())
                        {
                            animator.SetTrigger("SandsGrow");
                            aiState = AI_STATE.ATK_SANDS_GROW;
                        }
                    }
                }
            }
            else if (aiState == AI_STATE.ATK_SANDS_GROW)
            {
                GameObject target = FindAtkTarget(foundRadius);
                if (target && Vector2.Distance(transform.position, target.transform.position) <= biteRadius && Time.time - lastUseBite > cdBite)
                {
                    triggerBite();
                }
            }
            return aiDuration;
        });
    }

    private void triggerBite()
    {
        if (!isCanNotMoveWhenParalysis || Random.Range(0.0f, 1.0f) > proParalysis)
        {
            animator.SetTrigger("Bite");
            aiState = AI_STATE.ATK_BITE;
        }
        lastUseBite = Time.time;
    }

    void OnAniBornFinsih()
    {
        StartAiTimer();
    }

    void OnAniTriggerBite()
    {
        if (isEmptyConfusionDone)
        {
            // 混乱只有50%可以放出咬咬
            if (Random.Range(0.0f, 1.0f) > 0.5)
            {
                return;
            }
        }
        //再次检查目标
        GameObject target = FindAtkTarget(foundRadius);
        if (target && Vector2.Distance(transform.position, target.transform.position) <= biteRadius)
        {
            Instantiate(biteObj, target.transform.position, Quaternion.identity);
            if (isEmptyInfatuationDone && target.tag == "Player")
            {
                // 魅惑时不对player造成伤害
                return;
            }
            Timer.Start(this, 0.1f, () =>
            {
                Pokemon.PokemonHpChange(gameObject, target, 60, 0, 0, Type.TypeEnum.Dark);

                PlayerControler playerControler = target.GetComponent<PlayerControler>();
                if (playerControler != null && target.tag == "Player")
                {
                    // 击退主角
                    playerControler.KnockOutPoint = 15;
                    playerControler.KnockOutDirection = (target.transform.position - transform.position).normalized;
                }
            });
        }
    }

    void OnAniTriggerSands()
    {
        GameObject sands = Instantiate(sandsObj, transform.position, Quaternion.identity, transform);
        sands.GetComponent<TrapinchSandTomb>().SetOwner(this);
        sandsHolding = sands;
    }

    void OnAniTriggerSandsGrow()
    {
        if (sandsHolding && sandsHolding.GetComponent<TrapinchSandTomb>().IsCanGrowUp())
        {
            sandsHolding.GetComponent<TrapinchSandTomb>().GrowUp();
        }
    }
}
