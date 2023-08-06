using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isTestHitDone;

    [Header("�Ƿ����")]
    public bool isFroze;
    public float FrozeTime;

    [Header("�Ƿ�����")]
    public bool isBurn;

    [Header("�Ƿ����")]
    public bool isParalysis;

    [Header("�Ƿ��ж�")]
    public bool isToxic;

    [Header("�Ƿ�˯��")]
    public bool isSleep;

    [Header("�Ƿ�η��")]
    public bool isFear;
    public float FearTime;

    [Header("�Ƿ���ä")]
    public bool isBlind;
    public float BlindTime;

    [Header("�Ƿ����")]
    public bool isConfusion;

    [Header("�Ƿ�����")]
    public bool isInfatuation;
    public float InfatuationTime;

    [Header("�Ƿ���")]
    public bool isCold;
    public float ColdTime;

    [Header("�Ƿ�����")]
    public bool isCurse;
    public float CurseTime;

    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
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
            Empty target = other.GetComponent<Empty>();
            isCanNotMove = true;
            isTestHitDone = true;
            if (isFroze) { target.Frozen(FrozeTime, 1, 1); }
            if (isBurn) { target.BurnFloatPlus(1) ; }
            if (isParalysis) { target.ParalysisFloatPlus(1) ; }
            if (isToxic) { target.ToxicFloatPlus(1) ; }
            if (isSleep) { target.SleepFloatPlus(1) ; }
            if (isFear) { target.Fear(FearTime,1) ; }
            if (isBlind) { target.Blind(BlindTime,1) ; }
            if (isConfusion) { target.ConfusionFloatPlus() ; }
            if (isInfatuation) { target.EmptyInfatuation(InfatuationTime , 1) ; }
            if (isCold) { target.Cold(ColdTime) ; }
            if (isCurse) { target.EmptyCurse(CurseTime,1) ; }
        }
        else if (other.tag == "Room" )
        {
            isCanNotMove = true;
            isTestHitDone = true;
        }
    }
}
