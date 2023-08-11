using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combee : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;

    bool isRush;
    bool isRushDmage;
    float RushTimer;
    Vector2 RushDirection;
    public GameObject Ps;
    bool isPSBorn;




    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Fairy;
        EmptyType02 = Type.TypeEnum.Normal;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, 30);
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
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {
            EmptyBeKnock();
            StateMaterialChange();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; }

                if (!isFearDone) {
                    if ((TargetPosition - (Vector2)transform.position).magnitude >= 2.2f && !isRush)
                    {
                        isRushDmage = false;
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                        Director = (TargetPosition - (Vector2)transform.position).normalized;
                        animator.SetFloat("LookX", Director.x);
                        animator.SetFloat("LookY", Director.y);
                    }
                    else
                    {
                        isRush = true;
                        RushTimer += Time.deltaTime;
                        if (RushTimer <= 0.5f)
                        {
                            isRushDmage = false;
                            RushDirection = Director;
                            if (isEmptyConfusionDone) { RushDirection = new Vector2(Director.x + Random.Range(-1.0f , 1.0f) , Director.y + Random.Range(-1.0f, 1.0f)).normalized; }
                            Director = (TargetPosition - (Vector2)transform.position ).normalized;
                            animator.SetFloat("LookX", Director.x);
                            animator.SetFloat("LookY", Director.y);
                        }
                        else if (RushTimer > 0.5f && RushTimer <= 4f)
                        {
                            if (!isPSBorn)
                            {
                                Instantiate(Ps, transform.position + Vector3.up, Quaternion.identity, transform);
                                isPSBorn = true;
                            }
                            animator.SetTrigger("Rush");
                            animator.ResetTrigger("RushOver");
                            isRushDmage = true;
                            rigidbody2D.position += RushDirection * 9.5f * Time.deltaTime;
                            if (rigidbody2D.position.x - transform.parent.position.x >= 15.0f || rigidbody2D.position.x - transform.parent.position.x <= -15.0f || rigidbody2D.position.y - transform.parent.position.y >= 10.0f || rigidbody2D.position.y - transform.parent.position.y <= -10.0f)
                            {
                                RushTimer = 4.0f;
                            }
                        }
                        else if (RushTimer > 4.0f)
                        {
                            isPSBorn = false;
                            isRushDmage = false;
                            animator.SetTrigger("RushOver");
                            animator.ResetTrigger("Rush");
                            Director = (TargetPosition - (Vector2)transform.position).normalized;
                            animator.SetFloat("LookX", Director.x);
                            animator.SetFloat("LookY", Director.y);
                            if (RushTimer > 5.5f) { isRush = false; RushTimer = 0; }
                        }
                    }
                }
                else
                {
                    RushTimer = 0;
                    isRush = false;
                    isRushDmage = false;
                    animator.SetTrigger("RushOver");
                    animator.ResetTrigger("Rush");
                    Director = -(TargetPosition - (Vector2)transform.position).normalized;
                    animator.SetFloat("LookX", Director.x);
                    animator.SetFloat("LookY", Director.y);
                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                }



            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (isRush && other.transform.tag == ("Room"))
        {
            RushTimer = 4.0f;
        }
        if (!isRushDmage) {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                EmptyTouchHit(other.gameObject);

            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                InfatuationEmptyTouchHit(other.gameObject);
            }
        }
        else
        {
            if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
            {
                EmptyTouchHit(other.gameObject);

            }
            if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
            {
                InfatuationEmptyTouchHit(other.gameObject);
            }
            RushTimer = 4.0f;
        }

    }
}
