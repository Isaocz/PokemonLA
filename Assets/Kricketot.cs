using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kricketot : Empty
{

    Vector2 Director;
    Vector2 TargetPosition;

    float LunchTimer;
    public KricketotStruggleBug KSB;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Bug;
        EmptyType02 = Type.TypeEnum.No;
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
        if (!isDie && !isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(15) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(15).transform.position; }

                if (!isFearDone)
                {
                    Director = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                    RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 13.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                    if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 13.5f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                    if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                    {
                        LunchTimer += Time.deltaTime;
                        if (LunchTimer >= 3.5f)
                        {
                            LunchTimer = 0;
                            animator.SetTrigger("Attack");
                            Lunch();
                        }
                    }
                }

                animator.SetFloat("LookX" , ((TargetPosition.x - transform.position.x > 0) ? 1 : -1 ));
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

    void Lunch()
    {
        Vector2 LunchDirector1 = Quaternion.AngleAxis( (isEmptyConfusionDone ? 45 : 15) , Vector3.forward) * (TargetPosition - (Vector2)transform.position);
        Vector2 LunchDirector2 = Quaternion.AngleAxis(-(isEmptyConfusionDone ? 45 : 15), Vector3.forward) * (TargetPosition - (Vector2)transform.position);

        KricketotStruggleBug p1 = Instantiate(KSB, transform.position, Quaternion.identity, transform.parent);
        p1.LaunchNotForce(LunchDirector1, 8.5f);
        p1.empty = this;
        p1.transform.rotation = Quaternion.AngleAxis( _mTool.Angle_360(  (Vector3)LunchDirector1 , Vector3.up)  , Vector3.forward) * transform.rotation;

        KricketotStruggleBug p2 = Instantiate(KSB, transform.position, Quaternion.identity, transform.parent);
        p2.LaunchNotForce(LunchDirector2, 9.5f);
        p2.empty = this;
        p2.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360(  (Vector3)LunchDirector2, Vector3.up), Vector3.forward) * transform.rotation;
    }

}
