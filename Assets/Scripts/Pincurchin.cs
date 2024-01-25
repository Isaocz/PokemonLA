using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pincurchin : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;
    public bool isMove;
    public PincurchinSpikeMove Spike;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Electric;
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

                    animator.SetTrigger("Move");
                    animator.ResetTrigger("MoveOver");
                    if (!isMove)
                    {
                        Director = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                        {
                            if (rigidbody2D != null && (TargetPosition - (Vector2)transform.position).magnitude <= 3.5f)
                            {
                                Director = ((TargetPosition - (Vector2)transform.position).normalized + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized).normalized;
                            }
                        }
                        if (isEmptyConfusionDone) { Director = Vector2.zero; }

                    }
                    else
                    {
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                        animator.SetFloat("LookX", (Director.x >= 0 ? 1 : -1));
                        animator.SetFloat("LookY", (Director.y >= 0 ? 1 : -1));
                    }
                }
                else
                {
                    animator.SetTrigger("Move");
                    animator.ResetTrigger("MoveOver");
                    if (!isMove)
                    {
                        Director = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                        {
                            if (rigidbody2D != null && (TargetPosition - (Vector2)transform.position).magnitude <= 4.5f)
                            {
                                Director = -(TargetPosition - (Vector2)transform.position).normalized;
                            }
                        }
                        if (isEmptyConfusionDone) { Director = Vector2.zero; }

                    }
                    else
                    {
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                        animator.SetFloat("LookX", (Director.x >= 0 ? 1 : -1));
                        animator.SetFloat("LookY", (Director.y >= 0 ? 1 : -1));
                    }
                }
            }
            else
            {
                animator.SetTrigger("MoveOver");
                animator.ResetTrigger("Move");
            }
            
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
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


    public void Shot()
    {
        if (!isFearDone) {
            if (isEmptyConfusionDone)
            {
                PincurchinSpikeMove s1 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s1.R = 330; s1.StartPosition = transform.position; s1.empty = this;
                PincurchinSpikeMove s2 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s2.R = 90; s2.StartPosition = transform.position; s2.empty = this;
                PincurchinSpikeMove s3 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s3.R = 210; s3.StartPosition = transform.position; s3.empty = this;
            }
            else
            {
                PincurchinSpikeMove s1 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s1.R = 0; s1.StartPosition = transform.position; s1.empty = this;
                PincurchinSpikeMove s2 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s2.R = 90; s2.StartPosition = transform.position; s2.empty = this;
                PincurchinSpikeMove s3 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s3.R = 180; s3.StartPosition = transform.position; s3.empty = this;
                PincurchinSpikeMove s4 = Instantiate(Spike, transform.position, Quaternion.identity, transform.parent);
                s4.R = 270; s4.StartPosition = transform.position; s4.empty = this;
            }
        }
    }


}
