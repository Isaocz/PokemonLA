using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bellibolt : Empty
{

    public Projectile BelliboltThunder;
    public SpriteRenderer ThunderMask;
    public bool isInJump;
    Animator MaskAnimator;
    public int ChargeCount;
    bool isChargeCountPlus;
    float ChargeTimer;



    Vector2 Director;
    Vector2 TargetPosition;




    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Electric;
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
        MaskAnimator = ThunderMask.transform.parent.GetComponent<Animator>();
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
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();

            if (isEmptyFrozenDone)
            {
                ThunderMask.gameObject.SetActive(false);
                MaskAnimator.speed = 0;
            }
            else if(!isEmptyFrozenDone && !ThunderMask.gameObject.activeInHierarchy)
            {
                ThunderMask.gameObject.SetActive(true);
                MaskAnimator.speed = animator.speed;
            }



            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(8) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(8).transform.position; }

                Charge();

                if (!isHit)
                {
                    if (isChargeCountPlus) { isChargeCountPlus = false; }
                    if (!isFearDone)
                    {
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                        {
                            if (((Vector2)transform.position - TargetPosition).magnitude >= 0.5f && rigidbody2D != null)
                            {
                                if (ChargeCount >= 3 && ((Vector2)transform.position - TargetPosition).magnitude <= 3.25f)
                                {
                                    animator.SetTrigger("Thunder");
                                    ChargeCount = 0;
                                }
                                else
                                {
                                    if (!isInJump)
                                    {
                                        Director = (TargetPosition - (Vector2)transform.position).normalized;
                                        if (isEmptyConfusionDone) { Director += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); Director = Director.normalized; }
                                    }
                                    else
                                    {
                                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                                    }
                                    animator.SetTrigger("Jump");
                                    animator.ResetTrigger("Idle");
                                }
                            }
                            else
                            {
                                animator.SetTrigger("Idle");
                                animator.ResetTrigger("Jump");
                            }
                        }
                        else
                        {
                            animator.SetTrigger("Idle");
                            animator.ResetTrigger("Jump");
                        }
                    }
                    else
                    {
                        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                        if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                        if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                        {
                            Director = -(TargetPosition - (Vector2)transform.position).normalized;
                            if (((Vector2)transform.position - TargetPosition).magnitude >= 0.5f && rigidbody2D != null)
                            {
                                if (isInJump)
                                {
                                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                                }
                                animator.SetTrigger("Jump");
                                animator.ResetTrigger("Idle");
                            }
                            else
                            {
                                animator.SetTrigger("Idle");
                                animator.ResetTrigger("Jump");
                            }
                        }
                        else
                        {
                            animator.SetTrigger("Idle");
                            animator.ResetTrigger("Jump");
                        }
                    }
                }

            }
            else
            {
                animator.SetTrigger("Idle");
                animator.ResetTrigger("Jump");
                if ( isSilence ) { Charge();                                                                                                                                                                                                                }
            }

        }
    }


    void Charge()
    {
        ChargeTimer += Time.deltaTime;
        if ((isHit && !isChargeCountPlus) || (ChargeTimer >= 4))
        {
            ChargeCount++;
            isChargeCountPlus = true;
            if (ChargeCount == 0) { MaskAnimator.SetTrigger("ChargeOver"); MaskAnimator.ResetTrigger("Charge"); ThunderMask.color = new Color(1, 1, 1, 0.6f); speed = 3; }
            if (ChargeCount == 1) { MaskAnimator.SetTrigger("Charge"); MaskAnimator.ResetTrigger("ChargeOver"); ThunderMask.color = new Color(1, 1, 1, 0.6f); speed = 4.5f; }
            if (ChargeCount == 2) { MaskAnimator.SetTrigger("Charge"); MaskAnimator.ResetTrigger("ChargeOver"); ThunderMask.color = new Color(1, 1, 1, 0.4f); speed = 6f; }
            if (ChargeCount >= 3) { MaskAnimator.SetTrigger("Charge"); MaskAnimator.ResetTrigger("ChargeOver"); ThunderMask.color = new Color(1, 1, 1, 0f); speed = 8f; }
            ChargeTimer = 0;
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


    public void Thunder()
    {
        Projectile ThunderOdj = Instantiate(BelliboltThunder, transform.position + 0.3f * Vector3.up, Quaternion.identity);
        ThunderOdj.empty = this;
        MaskAnimator.SetTrigger("ChargeOver"); MaskAnimator.ResetTrigger("Charge"); ThunderMask.color = new Color(1, 1, 1, 0.6f); speed = 3; ChargeCount = 0;
    }

}
