using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isTestHitDone;

    public bool ProjectileMode;

    [Header("是否冰冻")]
    public bool isFroze;
    public float FrozeTime;

    [Header("是否烧伤")]
    public bool isBurn;
    public float BurnTime;

    [Header("是否麻痹")]
    public bool isParalysis;
    public float ParalysisTime;

    [Header("是否中毒")]
    public bool isToxic;
    public float ToxicTime;

    [Header("是否睡眠")]
    public bool isSleep;
    public float SleepTime;

    [Header("是否畏缩")]
    public bool isFear;
    public float FearTime;

    [Header("是否致盲")]
    public bool isBlind;
    public float BlindTime;

    [Header("是否混乱")]
    public bool isConfusion;
    public float ConfusionTime;

    [Header("是否着迷")]
    public bool isInfatuation;
    public float InfatuationTime;

    [Header("是否寒冷")]
    public bool isCold;
    public float ColdTime;

    [Header("是否被诅咒")]
    public bool isCurse;
    public float CurseTime;

    [Header("是否被攻击下降")]
    public bool isAtkDown;
    public float AtkDownTime;

    [Header("是否被防御下降")]
    public bool isDefDown;
    public float DefDownTime;

    [Header("是否被特攻下降")]
    public bool isSpADown;
    public float SpADownTime;

    [Header("是否被特防下降")]
    public bool isSpDDown;
    public float SpDDownTime;

    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        if (!ProjectileMode)
        {
            GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
            for (int i = 0; i < EmptyParent.transform.childCount; i++)
            {
                Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
                if ( e != null ) { StateDone(e); }
            }
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTestHitDone)
        {
            StartExistenceTimer();
        }
    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 7.5f * Time.deltaTime;
            postion.y += direction.y * 7.5f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isCanNotMove = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            if (ProjectileMode)
            {
                Empty target = other.GetComponent<Empty>();
                //isCanNotMove = true;
                //isTestHitDone = true;
                StateDone(target);
            }
        }
        else if (other.tag == "Room")
        {
            isCanNotMove = true;
            isTestHitDone = true;
        }

    }

    void StateDone( Empty target )
    {
        if (isFroze) { target.Frozen(FrozeTime, 50f, 1); }
        if (isBurn) { target.EmptyBurnDone(0.5f, BurnTime, 1); }
        if (isParalysis) { target.EmptyParalysisDone(0.5f, ParalysisTime, 1); }
        if (isToxic) { target.EmptyToxicDone(0.5f, ToxicTime, 1); }
        if (isSleep) { target.EmptySleepDone(50f, SleepTime, 1); }
        if (isFear) { target.Fear(FearTime, 50f); }
        if (isBlind) { target.Blind(BlindTime, 50f); }
        if (isConfusion) {  target.EmptyConfusion(ConfusionTime, 50f);}
        if (isInfatuation) { target.EmptyInfatuation(InfatuationTime, 0.5f ); }
        if (isCold) { target.Cold(ColdTime); }
        if (isCurse) { target.EmptyCurse(CurseTime, 0.5f); }
        if (isAtkDown) { target.AtkChange(-1, AtkDownTime); }
        if (isDefDown) { target.DefChange(-1, DefDownTime); }
        if (isSpADown) { target.SpAChange(-1, SpADownTime); }
        if (isSpDDown) { target.SpDChange(-1, SpDDownTime); }
    }
}
