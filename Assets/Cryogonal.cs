using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cryogonal : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;

    bool isInAttackState;

    //bool isAtk = false;
    //bool isIdle = true;
    //bool isMove = false;
    //bool Atking = false;

    //bool lookL = true;
    //bool lookR = false;

    float StateTimer = 0f;
    //public float idleDuration = 2f;
    //public float moveRadius = 5f;

    public CryogonalIceShard CIS;
    private string currentState;

    Vector2 LastPosition;


    int AtkCount = 0;

    float AtkCDTime = 3.0f;
    float MoveTime = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Ice;
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        isInAttackState = false;
        StateTimer = 0;
        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(7) == null)
        {
            TargetPosition = player.transform.position;
            if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
        }
        else { TargetPosition = InfatuationForRangeRayCastEmpty(7).transform.position; }
        Director = new Vector2(((TargetPosition - (Vector2)transform.position).normalized.x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1);
        animator.SetFloat("LookX", Director.x);

        LastPosition = transform.position;
    }





    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isDie && !isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence)
            {

                StateTimer += Time.deltaTime;
                //进入移动模式
                if (StateTimer >= AtkCDTime && isInAttackState)
                {
                    isInAttackState = false;
                    StateTimer = 0;
                    if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(9) == null)
                    {
                        TargetPosition = player.transform.position;
                        if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                    }
                    else { TargetPosition = InfatuationForRangeRayCastEmpty(9).transform.position; }
                    Director = (new Vector2(((TargetPosition - (Vector2)transform.position).normalized.x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1));
                    animator.SetFloat("LookX", Director.x);
                }
                //进入攻击模式
                if (StateTimer >= MoveTime && !isInAttackState)
                {
                    AtkCount++;
                    if (AtkCount > 3) { AtkCount = 0; }
                    SetAtkCDTime();
                    StateTimer = 0; isInAttackState = true;
                    if (!isFearDone) {
                        animator.SetTrigger("Atk"); }
                }
                if (!isInAttackState)
                {
                    //Debug.Log(Time.deltaTime);
                    rigidbody2D.MovePosition(new Vector2(transform.position.x + Director.x * speed * Time.deltaTime * 0.6f * (isFearDone ? -1.3f : 1), transform.position.y + Director.y * speed * Time.deltaTime * 0.6f * (isFearDone ? -1.3f : 1)));
                    if (isFearDone)
                    {
                        animator.SetFloat("LookX", -Director.x);
                    }
                }
                else
                {

                }

            }
            animator.SetFloat("Speed", ((Vector2)transform.position - LastPosition).magnitude);
            LastPosition = transform.position;
        }
    }

   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }

    void SetAtkCDTime()
    {
        switch (AtkCount)
        {
            case 0:
                AtkCDTime = 6.0f;
                MoveTime = 1.0f;
                speed = 3.8f;
                break;
            case 1:
                AtkCDTime = 2.6f;
                MoveTime = 1.0f;
                speed = 4.9f;
                break;
            case 2:
                AtkCDTime = 1.2f;
                MoveTime = 0.7f;
                speed = 6.0f;
                break;
            case 3:
                AtkCDTime = 0.8f;
                MoveTime = 0.0f;
                speed = 0;
                break;
        }
    }

    public void LunchCIS()
    {
        if (!isFearDone)
        {
            Debug.Log(isEmptyConfusionDone);
            if (!isEmptyConfusionDone) {
                for (int i = 0; i < 6; i++)
                {
                    CryogonalIceShard e1 = Instantiate(CIS, transform.position, Quaternion.Euler(0, 0, ((AtkCount % 2 == 0) ? 0 : 90) + i * 60));
                    e1.empty = this;
                } 
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    CryogonalIceShard e1 = Instantiate(CIS, transform.position, Quaternion.Euler(0, 0, ((AtkCount % 2 == 0) ? 0 : 90) + i * 90));
                    e1.empty = this;
                }
            }

        }
    }
}
