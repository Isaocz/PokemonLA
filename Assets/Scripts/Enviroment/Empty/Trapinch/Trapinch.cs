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
 * base part 1
 * base part 2
 * 
 * ani:
 * param: Die, Hit
*/

public class Trapinch : Empty
{

    [Label("观察半径")]
    public float foundRadius = 8;
    [Label("发动咬咬的半径")]
    public float biteRadius = 3;
    [Label("扩展流沙地狱的最小半径")]
    public float growSandsRadius = 5;
    [Label("扩展流沙地狱距离开始的最小时间")]
    public float growSandsDuration = 2;
    [Label("咬咬cd")]
    public float cdBite = 2;
    [Label("流沙地狱cd")]
    public float cdSands = 14;

    private enum AI_STATE
    {
        IDLE,
        ATK_BITE,
        ATK_SANDS,
        ATK_SANDS_GROW,
    }

    private float lastUseBite = 0;
    private float lastUseSands = 0;
    //private GameObject sandsObj = null;

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

    private void StartAiTimer()
    {
        // 平均 5 帧反应一次
        float aiDuration = 0.3f;
        timerAi = Timer.Start(this, 0f, () =>
        {
            if (isDie)
            {
                StopCoroutine(timerAi);
                return 0;
            }
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateinfo.IsName("TrapinchLeftBeHit") || stateinfo.IsName("TrapinchRightBeHit"))
            {
                return aiDuration;
            }
            if (stateinfo.IsName("TrapinchLeftIdle") || stateinfo.IsName("TrapinchRightIdle"))
            {
                aiState = AI_STATE.IDLE;
            }
            if (aiState == AI_STATE.IDLE)
            {
                GameObject target = FindAtkTarget(foundRadius);
                if (target)
                {
                    Vector2 dis = target.transform.position - transform.position;
                    animator.SetFloat("Direction", dis.x);
                    if (Vector2.Distance(transform.position, target.transform.position) <= biteRadius && Time.time - lastUseBite > cdBite)
                    {
                        triggerBite();
                    }
                    else if (Time.time - lastUseSands > cdSands)
                    {
                        animator.SetTrigger("Sands");
                        aiState = AI_STATE.ATK_SANDS;
                        lastUseSands = Time.time;
                    }
                }
            }
            if (aiState == AI_STATE.ATK_SANDS)
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
                        //目标不在范围内，扩展
                        //TODO 检查是否已经扩展
                        animator.SetTrigger("SandsGrow");
                        aiState = AI_STATE.ATK_SANDS_GROW;
                    }
                }
            }
            if (aiState == AI_STATE.ATK_SANDS_GROW)
            {
                GameObject target = FindAtkTarget(growSandsRadius);
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
        animator.SetTrigger("Bite");
        aiState = AI_STATE.ATK_BITE;
        lastUseBite = Time.time;
    }

    void OnAniBornFinsih()
    {
        StartAiTimer();
    }

    void OnAniTriggerBite()
    {

    }

    void OnAniTriggerSands()
    {

    }
}
