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

        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
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
                {   // Idle�׶�
                    if (lookL && !lookR)
                    { ChangeAnimationState("CryogonalIdleL"); }
                    else if (!lookL && lookR)
                    { ChangeAnimationState("CryogonalIdleR");}
                }
                
                if(StateTimer > idleDuration && isIdle || isMove)//�ƶ�״̬
                {
                    if (isIdle)
                    {   // ����Move״̬�����������ƶ�����λ�á�״̬����ֵ��
                        SetRandomTargetPosition();
                        isIdle = false;
                        isMove = true;
                        Atking = false;
                        StateTimer = 0f;
                    }
                    if (Vector2.Distance(transform.position, TargetPosition) < 0.1f || StateTimer > 3f)
                    {   // ת������һ�׶�
                        isMove = false;
                        isAtk = true;
                        StateTimer = 0f;
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, 0.1f);
                        if (transform.position.x < TargetPosition.x)
                        {
                            // �����ƶ�
                            ChangeAnimationState("CryogonalMoveR");
                            lookR = true;
                            lookL = false;
                        }
                        else if (transform.position.x > TargetPosition.x)
                        {
                            // �����ƶ�
                            ChangeAnimationState("CryogonalMoveL");
                            lookL = true;
                            lookR = false;
                        }
                    }
                }

                if(!isMove && isAtk)//����״̬
                {
                    if (transform.position.x < TargetPosition.x)
                    {
                        // ����
                        ChangeAnimationState("CrygonalAtkR");
                        lookR = true;
                        lookL = false;
                    }
                    else if (transform.position.x > TargetPosition.x)
                    {
                        // ����
                        ChangeAnimationState("CrygonalAtkL");
                        lookL = true;
                        lookR = false;
                    }
                    if (!Atking)
                    {   // ���й���
                        LunchCIS();
                        Atking = true;
                    }
                    if(StateTimer > 1f)
                    {   // ת����Idle�׶�
                        isAtk = false;
                        isIdle = true;
                        StateTimer = 0f;
                    }
                }
            }
        }
    }

    void ChangeAnimationState(string newState)
    {   //��������
        if (!isHit)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            animator.Play(newState);
        }
    }

    void SetRandomTargetPosition()//�������λ��
    {
        Vector2 newTargetPosition;
        bool isValidPosition = false;

        while (!isValidPosition)
        {
            newTargetPosition = (Vector2)transform.position + Random.insideUnitCircle * moveRadius;

            // ���newTargetPosition�Ƿ���Environment��Room��ǩ��������ײ
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

            // ���û����ײ,������Ϊ��Чλ��
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
