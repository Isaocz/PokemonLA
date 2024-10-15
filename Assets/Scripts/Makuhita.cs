using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makuhita : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;


    enum State
    {
        NormalMove,
        Jump,
        Punch,
        Idle,
    }

    State NowState;
    float SkillChangeTimer;
    float SkillCDTimer;


    //普通移动
    float MoveTurnTimer;
    Vector2 MoveDir;


    //跳
    bool isJumpMove;
    Vector2 JumpDir;
    public StompingTanturmMakuhita ST;


    //拳击
    bool isPuchMove;
    Vector2 PunchDir;
    public DrainPunchMakuhita DrainPunch;
    float PunchMoveTime;
    public KrickeyunrMoveShadow MoveShadow;



    bool isSleepIns;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Fighting;
        EmptyType02 = PokemonType.TypeEnum.No;
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
        NowState = State.NormalMove;
        SkillCDTimer = 5.0f;
        MoveDir = RandomDir();
        animator.SetFloat("LookX", MoveDir.x);
        animator.SetFloat("LookY", MoveDir.y);
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


            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence)
            {
                if (isSleepIns) { isSleepIns = false; }
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(15) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(15).transform.position; }



                if (NowState == State.NormalMove)
                {
                    MoveTurnTimer += Time.deltaTime;
                    if (MoveTurnTimer > 2.0f)
                    {
                        MoveDir = RandomDir();
                        animator.SetFloat("LookX" , MoveDir.x);
                        animator.SetFloat("LookY" , MoveDir.y);
                        MoveTurnTimer = 0; 
                    }
                    if (SkillCDTimer > 0) { SkillCDTimer -= Time.deltaTime; if (SkillCDTimer <= 0 ) { SkillCDTimer = 0; } }
                    if (SkillCDTimer == 0 && (TargetPosition-(Vector2)transform.position).magnitude < 6.5f )
                    {
                        NowState = State.Jump;
                        SkillCDTimer = 11;
                        animator.SetTrigger("Attack");
                        JumpDir = (Quaternion.AngleAxis((isEmptyConfusionDone ? Random.Range(-45, 45) : 0), Vector3.forward)) * (TargetPosition - (Vector2)transform.position).normalized;
                        animator.SetFloat("LookX", ((JumpDir.x > 0)? 1 : -1 ) );
                        animator.SetFloat("LookY", ((JumpDir.y > 0) ? 1 : -1));
                    }
                }
                else
                {
                    SkillChangeTimer += Time.deltaTime;
                    if (SkillChangeTimer > 1.0f && NowState == State.Jump)
                    {
                        JumpDir = Vector2.zero;
                        PunchDir = (Quaternion.AngleAxis((isEmptyConfusionDone ? Random.Range(-45,45) : 0 ), Vector3.forward  )) * (TargetPosition - (Vector2)transform.position).normalized;
                        NowState = State.Punch;
                        animator.SetFloat("LookX", ((PunchDir.x > 0) ? 1 : -1));
                        animator.SetFloat("LookY", ((PunchDir.y > 0) ? 1 : -1));
                        isPuchMove = true;
                        Instantiate(DrainPunch , transform.position + (Vector3)PunchDir , Quaternion.identity , transform).empty = this;
                    }
                    if (SkillChangeTimer > 1.35f && NowState == State.Punch)
                    {
                        animator.SetFloat("LookX", ((PunchDir.x > 0) ? 1 : -1));
                        animator.SetFloat("LookY", ((PunchDir.y > 0) ? 1 : -1));
                        isPuchMove = false;
                        PunchDir = Vector2.zero;
                        NowState = State.Idle;
                    }
                    if (SkillChangeTimer > 6.35f && NowState == State.Idle)
                    {
                        NowState = State.NormalMove;
                        SkillChangeTimer = 0;
                        MoveDir = RandomDir();
                        animator.SetFloat("LookX", MoveDir.x);
                        animator.SetFloat("LookY", MoveDir.y);
                    }


                }
               
            }

            if (isSleepDone && !isSleepIns)
            {
                isSleepIns = true;
                NowState = State.NormalMove;
                SkillChangeTimer = 0;
                SkillCDTimer = 5;
                MoveTurnTimer = 0;
                MoveDir = Vector2.zero;
                JumpDir = Vector2.zero;
                PunchDir = Vector2.zero;
                isJumpMove = false;
                isPuchMove = false;
                PunchMoveTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone )
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(15) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(15).transform.position; }



                if (NowState == State.NormalMove)
                {
                    rigidbody2D.MovePosition(new Vector2(transform.position.x + speed * Time.deltaTime * MoveDir.x, transform.position.y + speed * Time.deltaTime * MoveDir.y));
                }
                if (isJumpMove)
                {
                    rigidbody2D.MovePosition(new Vector2(    Mathf.Clamp(transform.position.x + (isFearDone ? -1 : 1) * speed * Time.deltaTime * JumpDir.x * 8.0f , transform.parent.position.x - 12.0f , transform.parent.position.x + 12.0f), Mathf.Clamp((transform.position.y + (isFearDone ? -1 : 1) * speed * Time.deltaTime * JumpDir.y * 8.0f), transform.parent.position.y - 7.0f, transform.parent.position.y + 7.0f)  ));

                    if (isFearDone)
                    {
                        animator.SetFloat("LookX", ((JumpDir.x > 0) ? -1 : 1));
                        animator.SetFloat("LookY", ((JumpDir.y > 0) ? -1 : 1));
                    }
                    else
                    {
                        animator.SetFloat("LookX", ((JumpDir.x > 0) ? 1 : -1));
                        animator.SetFloat("LookY", ((JumpDir.y > 0) ? 1 : -1));
                    }
                }
                if (isPuchMove)
                {
                    PunchMoveTime += Time.deltaTime;
                    if (PunchMoveTime > 0.03f) {
                        PunchMoveTime = 0;
                        KrickeyunrMoveShadow m = Instantiate(MoveShadow , transform.position , Quaternion.identity);
                        m.GetComponent<SpriteRenderer>().sprite = transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        m.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 0.6f);
                        m.transform.localScale *= 1.5f; 
                    }

                    rigidbody2D.MovePosition(new Vector2(transform.position.x + (isFearDone ? -1 : 1) * speed * Time.deltaTime * PunchDir.x * 15.0f, transform.position.y + (isFearDone ? -1 : 1) * speed * Time.deltaTime * PunchDir.y * 15.0f));
                    if (isFearDone)
                    {
                        animator.SetFloat("LookX", ((PunchDir.x > 0) ? -1 : 1));
                        animator.SetFloat("LookY", ((PunchDir.y > 0) ? -1 : 1));
                    }
                    else
                    {
                        animator.SetFloat("LookX", ((PunchDir.x > 0) ? 1 : -1));
                        animator.SetFloat("LookY", ((PunchDir.y > 0) ? 1 : -1));
                    }
                }



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

    public void ColliderJumpMode()
    {
        gameObject.layer = 20;
        isJumpMove = true;
    }

    public void ColliderMoveMode()
    {
        gameObject.layer = 9;
        if (isJumpMove)
        {
            Instantiate(ST, transform.position, Quaternion.identity).empty = this;
        }
        isJumpMove = false;
    }
    
    Vector2 RandomDir()
    {
        Vector2 OutPut = Vector2.up + Vector2.right;
        switch (Random.Range(0,4))
        {
            case 0: OutPut = Vector2.up + Vector2.right; break;
            case 1: OutPut = Vector2.up + Vector2.left; break;
            case 2: OutPut = Vector2.down + Vector2.right; break;
            case 3: OutPut = Vector2.down + Vector2.left; break;
        }
        return OutPut;
    }

}
