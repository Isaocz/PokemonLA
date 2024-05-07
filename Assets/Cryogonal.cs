using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cryogonal : Empty
{
    Vector2 TargetPosition;
    bool isAtk = false;
    bool isIdle = true;
    bool isMove = false;
    bool Atking = false;

    bool lookL = true;
    bool lookR = false;

    float StateTimer = 0f;
    public float idleDuration = 2f;
    public float moveRadius = 5f;

    public CryogonalIceShard CIS;
    private string currentState;
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

        //Debug.Log("idle:" + isIdle + " move:" + isMove + " atk:" + isAtk + " TargetPosition:" + TargetPosition + " StateTimer:" + StateTimer);
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence && !isFearDone)
            {

                StateTimer += Time.deltaTime;
                if (isIdle && !isMove && !isAtk)
                {   // Idle阶段
                    if (lookL && !lookR)
                    { ChangeAnimationState("CryogonalIdleL"); }
                    else if (!lookL && lookR)
                    { ChangeAnimationState("CryogonalIdleR");}
                }
                
                if(StateTimer > idleDuration && isIdle || isMove)//移动状态
                {
                    if (isIdle)
                    {   // 设置Move状态基本参数（移动到的位置、状态布尔值）
                        SetRandomTargetPosition();
                        isIdle = false;
                        isMove = true;
                        Atking = false;
                        StateTimer = 0f;
                    }
                    if (Vector2.Distance(transform.position, TargetPosition) < 0.1f || StateTimer > 3f)
                    {   // 转移至下一阶段
                        isMove = false;
                        isAtk = true;
                        StateTimer = 0f;
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, 0.1f);
                        if (transform.position.x < TargetPosition.x)
                        {
                            // 往右移动
                            ChangeAnimationState("CryogonalMoveR");
                            lookR = true;
                            lookL = false;
                        }
                        else if (transform.position.x > TargetPosition.x)
                        {
                            // 往左移动
                            ChangeAnimationState("CryogonalMoveL");
                            lookL = true;
                            lookR = false;
                        }
                    }
                }

                if(!isMove && isAtk)//攻击状态
                {
                    if (transform.position.x < TargetPosition.x)
                    {
                        // 往右
                        ChangeAnimationState("CrygonalAtkR");
                        lookR = true;
                        lookL = false;
                    }
                    else if (transform.position.x > TargetPosition.x)
                    {
                        // 往左
                        ChangeAnimationState("CrygonalAtkL");
                        lookL = true;
                        lookR = false;
                    }
                    if (!Atking)
                    {   // 进行攻击
                        LunchCIS();
                        Atking = true;
                    }
                    if(StateTimer > 1f)
                    {   // 转移至Idle阶段
                        isAtk = false;
                        isIdle = true;
                        StateTimer = 0f;
                    }
                }
            }
        }
    }

    void ChangeAnimationState(string newState)
    {   //动画管理
        if (!isHit)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            animator.Play(newState);
        }
    }

    void SetRandomTargetPosition()//设置随机位置
    {
        Vector2 newTargetPosition;
        bool isValidPosition = false;

        while (!isValidPosition)
        {
            newTargetPosition = (Vector2)transform.position + Random.insideUnitCircle * moveRadius;

            // 检测newTargetPosition是否与Environment和Room标签的物体相撞
            Collider2D[] colliders = Physics2D.OverlapCircleAll(newTargetPosition, 0.5f);
            bool isColliding = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enviroment") || collider.CompareTag("Room"))
                {
                    isColliding = true;
                    break;
                }
            }

            // 如果没有相撞,则设置为有效位置
            if (!isColliding)
            {
                isValidPosition = true;
                TargetPosition = newTargetPosition;
            }
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

    public void LunchCIS()
    {
        if (!isFearDone)
        {
            for(int i = 0; i < 6; i++)
            {
                CryogonalIceShard e1 = Instantiate(CIS, transform.position, Quaternion.Euler(0, 0, i * 60));
                e1.empty = this;
            }
        }
    }
}
