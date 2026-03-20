using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    //测试种类
    public enum StateTestType
    {
        Froze,
        Burn,
        Paralysis,
        Toxic,
        Sleep,
        Fear,
        Blind,
        Confusion,
        Infatuation,
        Cold,
        Curse,
        SpeDown,
        AtkDown,
        DefDown,
        SpADown,
        SpDDown,
    }


    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isTestHitDone;

    public bool ProjectileMode;

    [Header("是否冰冻")]
    public bool isFroze;
    public float FrozeTime;
    public float FrozePoint;

    [Header("是否烧伤")]
    public bool isBurn;
    public float BurnTime;
    public float BurnPoint;

    [Header("是否麻痹")]
    public bool isParalysis;
    public float ParalysisTime;
    public float ParalysisPoint;

    [Header("是否中毒")]
    public bool isToxic;
    public float ToxicTime;
    public float ToxicPoint;

    [Header("是否睡眠")]
    public bool isSleep;
    public float SleepTime;
    public float SleepPoint;

    [Header("是否畏缩")]
    public bool isFear;
    public float FearTime;
    public float FearPoint;

    [Header("是否致盲")]
    public bool isBlind;
    public float BlindTime;
    public float BlindPoint;

    [Header("是否混乱")]
    public bool isConfusion;
    public float ConfusionTime;
    public float ConfusionPoint;

    [Header("是否着迷")]
    public bool isInfatuation;
    public float InfatuationTime;
    public float InfatuationPoint;

    [Header("是否寒冷")]
    public bool isCold;
    public float ColdTime;

    [Header("是否被诅咒")]
    public bool isCurse;
    public float CurseTime;

    [Header("是否减速")]
    public bool isSpeDown;
    public float SpeDownTime;

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
            Debug.Log("xxxxsa");
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
        if (isFroze) { target.Frozen(FrozeTime, FrozePoint, 1); }
        if (isBurn) { target.EmptyBurnDone(BurnPoint, BurnTime, 1); }
        if (isParalysis) { target.EmptyParalysisDone(ParalysisPoint, ParalysisTime, 1); }
        if (isToxic) { target.EmptyToxicDone(ToxicPoint, ToxicTime, 1); }
        if (isSleep) { target.EmptySleepDone(SleepPoint, SleepTime, 1); }
        if (isFear) { target.Fear(FearTime, FearPoint); }
        if (isBlind) { target.Blind(BlindTime, BlindPoint); }
        if (isConfusion) {  target.EmptyConfusion(ConfusionTime, ConfusionPoint);}
        if (isInfatuation) { target.EmptyInfatuation(InfatuationTime, InfatuationPoint ); }
        if (isCold) { target.Cold(ColdTime); }
        if (isCurse) { target.EmptyCurse(CurseTime, 0.5f); }
        if (isAtkDown) { target.AtkChange(-1, AtkDownTime); }
        if (isDefDown) { target.DefChange(-1, DefDownTime); }
        if (isSpADown) { target.SpAChange(-1, SpADownTime); }
        if (isSpDDown) { target.SpDChange(-1, SpDDownTime); }
        if (isSpeDown) { target.SpeedChange(); target.SpeedRemove01(SpeDownTime * target.OtherStateResistance); }
    }
}
