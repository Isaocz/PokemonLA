using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ralts : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;
    Vector2 LastPosition;
    public bool isTP;

    public Projectile RaltsConfusion;
    float ConfusionCDTimer;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Fairy;
        EmptyType02 = Type.TypeEnum.Normal;
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
        LastPosition = transform.position;
        ConfusionCDTimer = 0;
        animator.SetFloat("LookX", -1);
        animator.SetFloat("LookY", -1);
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            animator.SetFloat("LookX", (Director.x >= 0 ? 1 : -1));
            animator.SetFloat("LookY", (Director.y >= 0 ? 1 : -1));
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyBeKnock();
            if (!isDie && !isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(7) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(7).transform.position; }

                if (isHit && !isTP)
                {
                    isTP = true;
                    animator.SetTrigger("TP");
                }

                if (!isHit && !isTP)
                {
                    if (!isFearDone) {
                        ConfusionCDTimer += Time.deltaTime;
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player" ) || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")) )
                        {
                            Director = (TargetPosition - (Vector2)transform.position).normalized;
                            if (((Vector2)transform.position - TargetPosition).magnitude >= 2.5f && rigidbody2D != null) {
                                rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                            }
                             
                            if (ConfusionCDTimer >= 1.5f)
                            {
                                ConfusionCDTimer = 0;
                                Lunch();
                            }
                        }
                    }
                    else
                    {
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                        {
                            Director = -(TargetPosition - (Vector2)transform.position).normalized;
                            if (((Vector2)transform.position - TargetPosition).magnitude <= 8.5f && rigidbody2D != null)
                            {
                                rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * 3 * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * 3 * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                            }

                            if (ConfusionCDTimer >= 1.5f)
                            {
                                ConfusionCDTimer = 0;
                            }
                        }
                    }
                }

            }
            animator.SetFloat("Speed", ((Vector2)transform.position - LastPosition).magnitude);
            LastPosition = transform.position;

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


    public void MoveToOtherPoint()
    {
        Vector3 TPPosition;
        if (!isFearDone)
        {
            TPPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);
        }
        else
        {
            TPPosition = transform.parent.position + new Vector3((Random.Range(0.0f, 1.0f) > 0.5 ? (Random.Range(-12.0f, -10.0f)) : (Random.Range(10.0f, 12.0f))), (Random.Range(0.0f, 1.0f) > 0.5 ? (Random.Range(-7.0f, -5.50f)) : (Random.Range(5.5f, 7f))), 0);
        }
        while (!isThisPointEmpty(TPPosition))
        {
            TPPosition = transform.parent.position + new Vector3(Random.Range(-12.0f, 12.0f), Random.Range(-7.0f, 7.0f), 0);
        }

        transform.position = TPPosition;
    }


    public void CallisTPFalse()
    {
        isTP = false;
    }


    public void Lunch()
    {
        if (!isDie)
        {
            Projectile ProjectileObject;
            ProjectileObject = Instantiate(RaltsConfusion, rigidbody2D.position + new Vector2(Director.x * 0.375f, 0.375f), Quaternion.identity /* Quaternion.Euler(0, 0, (TargetPosition.x - transform.position.x >= 0 ? -1 : 1) * Vector2.Angle(TargetPosition - (Vector2)transform.position, new Vector2(0, 1))) */  );
            Vector2 p = new Vector2((TargetPosition.x - transform.position.x), (TargetPosition.y - transform.position.y)).normalized;
            if (isEmptyConfusionDone) {
                p += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); p = p.normalized;
                ProjectileObject.GetComponent<EmptyTrace>().distance = 2.25f;
            }
            ProjectileObject.transform.rotation = Quaternion.Euler(0, 0, (TargetPosition.y - transform.position.y <= 0 ? -1 : 1) * Vector2.Angle(p, new Vector2(1, 0)));
            ProjectileObject.empty = this;
        }
    }

}
