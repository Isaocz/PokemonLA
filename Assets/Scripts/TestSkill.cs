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

    [Header("�Ƿ����")]
    public bool isFroze;
    public float FrozeTime;

    [Header("�Ƿ�����")]
    public bool isBurn;
    public float BurnTime;

    [Header("�Ƿ����")]
    public bool isParalysis;
    public float ParalysisTime;

    [Header("�Ƿ��ж�")]
    public bool isToxic;
    public float ToxicTime;

    [Header("�Ƿ�˯��")]
    public bool isSleep;
    public float SleepTime;

    [Header("�Ƿ�η��")]
    public bool isFear;
    public float FearTime;

    [Header("�Ƿ���ä")]
    public bool isBlind;
    public float BlindTime;

    [Header("�Ƿ����")]
    public bool isConfusion;
    public float ConfusionTime;

    [Header("�Ƿ�����")]
    public bool isInfatuation;
    public float InfatuationTime;

    [Header("�Ƿ���")]
    public bool isCold;
    public float ColdTime;

    [Header("�Ƿ�����")]
    public bool isCurse;
    public float CurseTime;

    [Header("�Ƿ񱻹����½�")]
    public bool isAtkDown;
    public float AtkDownTime;

    [Header("�Ƿ񱻷����½�")]
    public bool isDefDown;
    public float DefDownTime;

    [Header("�Ƿ��ع��½�")]
    public bool isSpADown;
    public float SpADownTime;

    [Header("�Ƿ��ط��½�")]
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
