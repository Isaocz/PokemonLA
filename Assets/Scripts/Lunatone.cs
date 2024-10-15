using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunatone : Empty
{


    Vector2 Director;
    Vector2 TargetPosition;
    float MoveTurnTimer;

    enum State
    {
        Normal,
        Attack,
    }
    State NowState;

    int AttackCount;
    public float AttackCDTime;
    float AttackCDTimer;

    public PsychicLunatoneManger Psychic;



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Rock;
        EmptyType02 = PokemonType.TypeEnum.Psychic;
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

        Director = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right;
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

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                switch (NowState) {
                    case State.Normal:
                        TargetPosition = player.transform.position;
                        if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                        MoveTurnTimer += Time.deltaTime;
                        if (MoveTurnTimer >= 2.0f)
                        {
                            AttackCount++;
                            if (AttackCount > 0)
                            {
                                AttackCount = 0;
                                NowState = State.Attack;
                                animator.SetTrigger("Attack");
                                if (!isFearDone) { Instantiate(Psychic, transform.position, Quaternion.identity).ParentEmpty = this; }
                            }
                            MoveTurnTimer = 0.0f;
                            Director = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right;
                            if (isFearDone) { Director = (Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.forward) * ((Vector2)transform.position - TargetPosition).normalized); }
                            animator.SetFloat("LookX", (Director.x > 0) ? 1 : -1);
                        }
                        if (MoveTurnTimer <= 1.2f)
                        {
                            rigidbody2D.MovePosition(new Vector2(rigidbody2D.position.x + Time.deltaTime * speed * Director.x * (isFearDone?2:1), rigidbody2D.position.y + Time.deltaTime * speed * Director.y * (isFearDone ? 2 : 1)));
                        }
                        break;
                    case State.Attack:
                        AttackCDTimer += Time.deltaTime;
                        if (AttackCDTimer >= AttackCDTime) { AttackCDTimer = 0; NowState = State.Normal; }
                        break;
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
}
