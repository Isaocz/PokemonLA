using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bronzor : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;

    bool isInAttackState;
    float StateTimer;

    public BronzorExtrasensory Extrasensory;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Steel;
        EmptyType02 = Type.TypeEnum.Psychic;
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
        animator.SetFloat("LookY", Director.y);
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
                if (StateTimer >= 1.5f && isInAttackState)
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
                    animator.SetFloat("LookY", Director.y);
                }
                //进入攻击模式
                if (StateTimer >= 1 && !isInAttackState)
                {
                    StateTimer = 0; isInAttackState = true; animator.SetTrigger("Attack");
                }
                if (!isInAttackState)
                {
                    Debug.Log(Time.deltaTime);
                    rigidbody2D.MovePosition(new Vector2(transform.position.x + Director.x * speed * Time.deltaTime * 0.6f * (isFearDone ? -1.3f : 1), transform.position.y + Director.y * speed * Time.deltaTime * 0.6f * (isFearDone ? -1.3f : 1)));
                    if (isFearDone)
                    {
                        animator.SetFloat("LookX", -Director.x);
                        animator.SetFloat("LookY", -Director.y);
                    }
                }
                else
                {

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


    public void LunchExtrasensory()
    {
        if (!isFearDone) {
            BronzorExtrasensory e1 = Instantiate(Extrasensory, transform.position + Vector3.right * 0.5f, Quaternion.Euler(0, 0, 0));
            e1.empty = this; e1.LaunchNotForce(Vector3.right, (isEmptyConfusionDone ? 7.5f : 13.0f) );
            BronzorExtrasensory e2 = Instantiate(Extrasensory, transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, 0, 90));
            e2.empty = this; e2.LaunchNotForce(Vector3.up, (isEmptyConfusionDone ? 7.5f : 13.0f));
            BronzorExtrasensory e3 = Instantiate(Extrasensory, transform.position + Vector3.left * 0.5f, Quaternion.Euler(0, 0, 180));
            e3.empty = this; e3.LaunchNotForce(Vector3.left, (isEmptyConfusionDone ? 7.5f : 13.0f));
            BronzorExtrasensory e4 = Instantiate(Extrasensory, transform.position + Vector3.down * 0.5f, Quaternion.Euler(0, 0, 270));
            e4.empty = this; e4.LaunchNotForce(Vector3.down, (isEmptyConfusionDone ? 7.5f : 13.0f));
        }
    }

}
