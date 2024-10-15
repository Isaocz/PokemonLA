using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloyster : Empty
{
    Vector2 Director;
    public Vector2 TargetPosition;

    bool isClose;
    bool isOpenTimePlus;
    float OpenTimer;

    public bool isBeam;
    float BeamTimer;
    public CloysterAuroraBeamManger AuroraBeam;

    float MustOpenTimer;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Water;
        EmptyType02 = PokemonType.TypeEnum.Ice;
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
        animator.SetFloat("isClosing" , 1);
        DefAbilityPoint /= 2;
        DefChange(-1, 0.0f);
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

            if (isSleepDone && isClose)
            {
                OpenTimer = 0;
                animator.SetTrigger("Open");
                isClose = false;
            }

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position;  }

                if (isClose)
                {
                    MustOpenTimer += Time.deltaTime;
                    BeamTimer = 0;
                    isBeam = false;
                    if (isHit && !isOpenTimePlus)
                    {
                        OpenTimer = Mathf.Clamp(OpenTimer + Random.Range(0.25f,0.5f), 0, 3.0f);
                        isOpenTimePlus = true;
                    }
                    else if (!isHit && isOpenTimePlus)
                    {
                        isOpenTimePlus = false;
                    }
                }
                RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 20f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 20f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                if (!isClose && !isBeam)
                {
                    BeamTimer += Time.deltaTime;
                    if (SearchPlayer.collider != null && (  (!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player")   ||    (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")   )   &&   !isFearDone)
                    {
                        if (BeamTimer >= 0.75f)
                        {
                            BeamTimer = 0;
                            isBeam = true;
                            Instantiate(AuroraBeam, transform.position, Quaternion.identity,transform).ParentCloyster = this;
                        }
                    }
                }

                if (MustOpenTimer >= 20.0f)
                {
                    OpenTimer = 0;
                    MustOpenTimer = 0;
                    animator.SetTrigger("Open");
                    isClose = false;
                }

                if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                {
                    if (!isBeam)
                    {
                        if ((TargetPosition - (Vector2)transform.position).magnitude <= 6.5f && !isClose)
                        {
                            animator.SetTrigger("Close");
                            isClose = true;
                        }
                        else if ((TargetPosition - (Vector2)transform.position).magnitude > 6.5f && isClose)
                        {
                            if (OpenTimer > 0)
                            {
                                OpenTimer -= Time.deltaTime;
                            }
                            else
                            {
                                MustOpenTimer = 0;
                                OpenTimer = 0;
                                animator.SetTrigger("Open");
                                isClose = false;
                            }
                        }
                    }
                }
                else
                {
                    if (!isBeam)
                    {
                        if ((TargetPosition - (Vector2)transform.position).magnitude <= 6.5f && !isClose)
                        {
                            animator.SetTrigger("Close");
                            isClose = true;
                        }
                    }
                }
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


    public void Close()
    {
        animator.SetFloat("isClosing", -1);
        DefChange(1, 0.0f);
        Invincible = true;
    }

    public void Open()
    {
        animator.SetFloat("isClosing", 1);
        DefChange(-1,0.0f);
        Invincible = false;
    }


    public void BeamClose()
    {
        animator.SetTrigger("Close");
        isClose = true;
        OpenTimer = 1.2f;
    }

}
