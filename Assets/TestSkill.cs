using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    bool isTestHitDone;

    [Header("ÊÇ·ñ±ù¶³")]
    public bool isFroze;
    public float FrozeTime;

    [Header("ÊÇ·ñÉÕÉË")]
    public bool isBurn;
    public float BurnTime;

    [Header("ÊÇ·ñÂé±Ô")]
    public bool isParalysis;
    public float ParalysisTime;

    [Header("ÊÇ·ñÖÐ¶¾")]
    public bool isToxic;
    public float ToxicTime;

    [Header("ÊÇ·ñË¯Ãß")]
    public bool isSleep;
    public float SleepTime;

    [Header("ÊÇ·ñÎ·Ëõ")]
    public bool isFear;
    public float FearTime;

    [Header("ÊÇ·ñÖÂÃ¤")]
    public bool isBlind;
    public float BlindTime;

    [Header("ÊÇ·ñ»ìÂÒ")]
    public bool isConfusion;
    public float ConfusionTime;

    [Header("ÊÇ·ñ×ÅÃÔ")]
    public bool isInfatuation;
    public float InfatuationTime;

    [Header("ÊÇ·ñº®Àä")]
    public bool isCold;
    public float ColdTime;

    [Header("ÊÇ·ñ±»×çÖä")]
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
            if (isBurn) { target.EmptyBurnDone(1,BurnTime,1) ; }
            if (isParalysis) { target.EmptyParalysisDone(1 , ParalysisTime, 1) ; }
            if (isToxic) { target.EmptyToxicDone(1, ToxicTime , 1) ; }
            if (isSleep) { target.EmptySleepDone(1, SleepTime , 1) ; }
            if (isFear) { target.Fear(FearTime,1) ; }
            if (isBlind) { target.Blind(BlindTime,1) ; }
            if (isConfusion) { target.EmptyConfusion(ConfusionTime , 1) ; }
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
