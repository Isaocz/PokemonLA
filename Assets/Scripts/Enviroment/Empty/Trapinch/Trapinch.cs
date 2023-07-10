using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * editor ��䣺
 * �쳣״̬����
 * player ui state
 * ui health
 * ���ԣ�����ֵ
 * base exp ��������
 * HWP ȡ��Ŭ��ֵ
 * skinRenderers body��Ⱦ�����飨���ܣ�
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

    [Label("�۲�뾶")]
    public float foundRadius = 8;
    [Label("����ҧҧ�İ뾶")]
    public float biteRadius = 3;
    [Label("��չ��ɳ��������С�뾶")]
    public float growSandsRadius = 5;
    [Label("��չ��ɳ�������뿪ʼ����Сʱ��")]
    public float growSandsDuration = 2;
    [Label("ҧҧcd")]
    public float cdBite = 2;
    [Label("��ɳ����cd")]
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
        // ƽ�� 5 ֡��Ӧһ��
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
                        //Ŀ�겻�ڷ�Χ�ڣ���չ
                        //TODO ����Ƿ��Ѿ���չ
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
