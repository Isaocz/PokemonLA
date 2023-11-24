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

    [Header(" «∑Ò±˘∂≥")]
    public bool isFroze;
    public float FrozeTime;

    [Header(" «∑Ò…’…À")]
    public bool isBurn;
    public float BurnTime;

    [Header(" «∑Ò¬È±‘")]
    public bool isParalysis;
    public float ParalysisTime;

    [Header(" «∑Ò÷–∂æ")]
    public bool isToxic;
    public float ToxicTime;

    [Header(" «∑ÒÀØ√ﬂ")]
    public bool isSleep;
    public float SleepTime;

    [Header(" «∑ÒŒ∑Àı")]
    public bool isFear;
    public float FearTime;

    [Header(" «∑Ò÷¬√§")]
    public bool isBlind;
    public float BlindTime;

    [Header(" «∑ÒªÏ¬“")]
    public bool isConfusion;
    public float ConfusionTime;

    [Header(" «∑Ò◊≈√‘")]
    public bool isInfatuation;
    public float InfatuationTime;

    [Header(" «∑Ò∫Æ¿‰")]
    public bool isCold;
    public float ColdTime;

    [Header(" «∑Ò±ª◊Á÷‰")]
    public bool isCurse;
    public float CurseTime;

    [Header(" «∑Ò±ªπ•ª˜œ¬Ωµ")]
    public bool isAtkDown;
    public float AtkDownTime;

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
                isCanNotMove = true;
                isTestHitDone = true;
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
        if (isFroze) { target.Frozen(FrozeTime, 10, 1); }
        if (isBurn) { target.EmptyBurnDone(1, BurnTime, 1); }
        if (isParalysis) { target.EmptyParalysisDone(10, ParalysisTime, 1); }
        if (isToxic) { target.EmptyToxicDone(10, ToxicTime, 1); }
        if (isSleep) { target.EmptySleepDone(10, SleepTime, 1); }
        if (isFear) { target.Fear(FearTime, 10); }
        if (isBlind) { target.Blind(BlindTime, 10); }
        if (isConfusion) { target.EmptyConfusion(ConfusionTime, 10); }
        if (isInfatuation) { target.EmptyInfatuation(InfatuationTime, 10); }
        if (isCold) { target.Cold(ColdTime); }
        if (isCurse) { target.EmptyCurse(CurseTime, 10); }
        if (isAtkDown) { target.AtkChange(-1, 20); }
    }
}
