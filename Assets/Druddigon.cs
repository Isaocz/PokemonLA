using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druddigon : Empty
{
    MyAstarAI AI;

    Vector2 Director;
    Vector3 LastPosition;
    float DirChangeTimer;


    public DragonClawDruddigon DragonClaw;
    float DragonClawTimer;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Dragon;
        EmptyType02 = Type.TypeEnum.No;
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
        AI = transform.GetComponent<MyAstarAI>();
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
            if (!isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence)
            {
                if (!AI.isCanNotMove) {
                    animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                    DirChangeTimer += Time.deltaTime;
                    if (DirChangeTimer >= 0.3f) {
                        DirChangeTimer = 0;
                        Director = (transform.position - LastPosition).normalized;
                        if (Director.x >= 0) { animator.SetFloat("LookX", 1); }
                        else { animator.SetFloat("LookX", -1); }
                        if (Director.y >= 0) { animator.SetFloat("LookY", 1); }
                        else { animator.SetFloat("LookY", -1); }
                    }
                    LastPosition = transform.position;
                }
                if (DragonClawTimer > 0) { DragonClawTimer -= Time.deltaTime; if (DragonClawTimer <= 0) { DragonClawTimer = 0; } }
                if (DragonClawTimer == 0 && (AI.targetPosition.position - transform.position).magnitude <= 3.0f && !AI.isCanNotMove && !isFearDone)
                {
                    DragonClawTimer = 4.0f;
                    AI.isCanNotMove = true;
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                animator.SetFloat("Speed", 0 );
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

    public void CallCanMove()
    {
        //AI.canMove = true;
        animator.SetFloat("Speed", 1.0f);
    }

    public void Claw()
    {
        Vector2 Dir = new Vector2(animator.GetFloat("LookX") , animator.GetFloat("LookY"));
        DragonClawDruddigon dc = Instantiate(DragonClaw , transform.position + Vector3.up * 0.8f + Vector3.right * 1.8f * Dir.x , Quaternion.identity);
        dc.empty = this;
        dc.GetComponent<Animator>().SetFloat("LookX", Dir.x);
        dc.GetComponent<Animator>().SetFloat("LookY", Dir.y);
    }

    public void AttackOver()
    {
        AI.isCanNotMove = false;
    }

}
