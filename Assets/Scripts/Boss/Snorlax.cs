using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snorlax : Empty
{
    public Yamn yamn;
    public bool isEmptyU = true;
    public bool isEmptyD = true;
    public bool isEmptyL = true;
    public bool isEmptyR = true;
    Vector2 position;
    Vector2Int direction;
    bool isTurn;
    float TurnTimer = 2.0f;

    public bool isSlam;
    bool isSlamFull;
    public bool isSlameMove;
    float SlamTimer = 2.0f;
    Vector2 SlamPostion;
    Vector2 SlamStartPostion;
    float SlamPer = 0.3f;

    bool isYamn;
    public bool isYamnMove;
    float YamnTimer = 3f;

    bool isImpact;
    public bool isImpactMove;
    float ImpactTimer = 2.0f;


    bool isAngry;

    public bool isGigaImpact;
    public bool isGigaImpactMove;
    Vector2 GigaImpactPostion1;

    public SnorlaxBerry[] BerryList;
    Vector3 TargetPosition;

    


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Normal;
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint) * BossAtkBouns;
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint) * BossAtkBouns;
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * BossDefBouns;
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * BossDefBouns;
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;


        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = new Vector2Int(0, -1);
        animator.SetFloat("LookX" , direction.x);
        animator.SetFloat("LookY" , direction.y);
        SlamPer = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            UpdateEmptyChangeHP();
            EmptyDie();


            if (!isSleepDone && !isCanNotMoveWhenParalysis)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }


                if (!isAngry && EmptyHp <= maxHP * 0.65f && !isEmptyFrozenDone && !isFearDone)
                {
                    isAngry = true;
                    isGigaImpact = true;
                    animator.SetTrigger("GigaImpact");
                    GameObject AngryEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(2);
                    Instantiate(AngryEffect, transform.position + Vector3.up * 2.5f, Quaternion.identity).SetActive(true);
                }



                if (!isSlam && !isGigaImpact && !isDie && !isEmptyFrozenDone)
                {
                    CheckPlayer();
                    if (!isTurn && !isYamn && !isImpact)
                    {
                        CheckTurn();
                    }
                }
                if (isTurn)
                {
                    TurnTimer -= Time.deltaTime;
                }
                if (TurnTimer <= 0.0f)
                {
                    isTurn = false;
                    TurnTimer = 2.0f;
                }

                if (!isSlam && !isSlamFull && !isEmptyFrozenDone)
                {
                    if ((transform.position - TargetPosition).magnitude < (isAngry ? 15 : 6))
                    {
                        if (Random.Range(0.0f, 1.0f) > SlamPer)
                        {
                            isSlamFull = true;
                            SlamPer += 0.1f;
                        }
                        else
                        {

                            if (!isFearDone) { SlamPostion = TargetPosition; }
                            else { SlamPostion = 2 * transform.position - TargetPosition; }
                            SlamStartPostion = transform.position;
                            isSlam = true;
                            animator.SetTrigger("Slam");
                            if (Mathf.Abs(transform.position.x - TargetPosition.x) > Mathf.Abs(transform.position.y - TargetPosition.y))
                            {
                                direction = new Vector2Int(((transform.position.x - TargetPosition.x) > 0 ? -1 : 1), 0);
                            }
                            else
                            {
                                direction = new Vector2Int(0, ((transform.position.y - TargetPosition.y) > 0 ? -1 : 1));
                            }
                            if (isFearDone) { direction = new Vector2Int(-direction.x, -direction.y); }
                            SlamPer = 0.3f;
                        }
                    }
                }

                if ((isSlam || isSlamFull))
                {
                    SlamTimer -= Time.deltaTime;
                }
                if (SlamTimer <= 0.0f)
                {
                    isSlam = false;
                    isSlamFull = false;
                    SlamTimer = 2.0f;
                }


                if (isYamn && !isEmptyFrozenDone)
                {
                    YamnTimer -= Time.deltaTime;
                }
                if (YamnTimer <= 0.0f)
                {
                    isYamn = false;
                    YamnTimer = isImpact ? 7f : 3f;
                }
                if (isImpact)
                {
                    ImpactTimer -= Time.deltaTime;
                }
                if (ImpactTimer <= 0.0f)
                {
                    isImpact = false;
                    ImpactTimer = 3f;
                }

                if (!isGigaImpact && player.isSleepDone && isAngry && !isEmptyFrozenDone && !isFearDone)
                {
                    RaycastHit2D PlayerChecker = Physics2D.Raycast(new Vector2(transform.position.x + 0.7f, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x + 0.7f, TargetPosition.y - transform.position.y + 0.5f), 15f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
                    if (PlayerChecker.collider != null && (PlayerChecker.collider.gameObject.layer == 8 || PlayerChecker.collider.gameObject.layer == 17))
                    {
                        isGigaImpact = true;
                        animator.SetTrigger("GigaImpact");
                    }
                }
            }
            animator.SetFloat("LookX", direction.x);
            animator.SetFloat("LookY", direction.y);


        }
    }
    
    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn)
        {
            Vector2 NowPosition = position;
            if (!isDie && !isHit  && !isEmptyFrozenDone)
            {
                if (!isSleepDone && !isCanNotMoveWhenParalysis)
                {
                    if (!isSlam && !isYamnMove && !isImpactMove & !isGigaImpact)
                    {
                        //获取当前刚体坐标，当当前x坐标和初始x坐标距离小于设定好的移动距离时，缓慢移动 当大于时，重置初始位置，方向反转
                        position = rigidbody2D.position;
                        position.x = Mathf.Clamp(position.x + speed * direction.x * Time.deltaTime, transform.parent.position.x - 13, transform.parent.position.x + 13);
                        position.y = Mathf.Clamp(position.y + speed * direction.y * Time.deltaTime, transform.parent.position.y - 10f, transform.parent.position.y + 10f);
                        rigidbody2D.position = position;
                    }
                    if (isSlameMove)
                    {
                        position = rigidbody2D.position;
                        if (isEmptyConfusionDone) { SlamStartPostion = SlamStartPostion + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)); }
                        if (!isFearDone && !isSilence)
                        {
                            position.x = Mathf.Clamp(position.x + ((TargetPosition.x - SlamStartPostion.x) / 15f), transform.parent.position.x - 11, transform.parent.position.x + 11);
                            position.y = Mathf.Clamp(position.y + ((TargetPosition.y - SlamStartPostion.y) / 15f), transform.parent.position.y - 6.2f, transform.parent.position.y + 6.2f);
                        }
                        else
                        {
                            position.x = Mathf.Clamp(position.x + ((SlamPostion.x - SlamStartPostion.x) / 15f), transform.parent.position.x - 11, transform.parent.position.x + 11);
                            position.y = Mathf.Clamp(position.y + ((SlamPostion.y - SlamStartPostion.y) / 15f), transform.parent.position.y - 6.2f, transform.parent.position.y + 6.2f);
                        }
                        rigidbody2D.position = position;
                    }
                    if (isImpactMove)
                    {
                        position = rigidbody2D.position;
                        position.x = Mathf.Clamp(position.x + (isEmptyConfusionDone ? Random.Range(2, 6) : 4) * speed * direction.x * Time.deltaTime, transform.parent.position.x - 13, transform.parent.position.x + 13);
                        position.y = Mathf.Clamp(position.y + (isEmptyConfusionDone ? Random.Range(2, 6) : 4) * speed * direction.y * Time.deltaTime, transform.parent.position.y - 10, transform.parent.position.y + 10);
                        rigidbody2D.position = position;
                    }
                    if (isGigaImpactMove)
                    {
                        if (isFearDone) { GigaImpactPostion1 = new Vector2(-GigaImpactPostion1.x, -GigaImpactPostion1.y); }
                        if (Mathf.Abs(GigaImpactPostion1.x) >= Mathf.Abs(GigaImpactPostion1.y))
                        {
                            direction.x = (GigaImpactPostion1.x >= 0 ? 1 : -1);
                            direction.y = 0;
                        }
                        else
                        {
                            direction.x = 0;
                            direction.y = (GigaImpactPostion1.y >= 0 ? 1 : -1);
                        }
                        if (isEmptyConfusionDone) { GigaImpactPostion1 = (GigaImpactPostion1 + new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f))).normalized; }
                        position = rigidbody2D.position;
                        position.x = Mathf.Clamp(position.x + 5.5f * speed * GigaImpactPostion1.x * Time.deltaTime, transform.parent.position.x - 13, transform.parent.position.x + 13);
                        position.y = Mathf.Clamp(position.y + 5.5f * speed * GigaImpactPostion1.y * Time.deltaTime, transform.parent.position.y - 10, transform.parent.position.y + 10);
                        rigidbody2D.position = position;
                    }
                }
            }
            Vector2 move = new Vector2(position.x - NowPosition.x, position.y - NowPosition.y);
            animator.SetFloat("Speed", move.magnitude);
            EmptyBeKnock();
            StateMaterialChange();
        }
    }

    public void EatBerry(int BerryIndex)
    {
        switch (BerryIndex)
        {
            case 1:
                Debug.Log(EmptyHp);
                Debug.Log(-maxHP / 8);
                PokemonHpChange(null , this.gameObject , 0 ,0 , maxHP / 8,PokemonType.TypeEnum.IgnoreType);
                Debug.Log(EmptyHp);
                break;
            case 2:
                speed = 3;
                animator.speed = 1.2f;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            if (!isSlam && !isImpact && !isGigaImpact)
            {
                EmptyTouchHit(other.gameObject);
            }else if (isSlam)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 80, 0, 0, PokemonType.TypeEnum.Normal);
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(-(80 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250), 0, 1);
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.ParalysisFloatPlus(0.15f);
                }
            }
            else if (isImpact)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 50, 0, 0, PokemonType.TypeEnum.Normal);
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(-(50 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250), 0, 1);
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            else if (isGigaImpact)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 120, 0, 0, PokemonType.TypeEnum.Normal);
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(-(120 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250), 0, 1);
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }

        }
        if(isGigaImpactMove && ( (other.transform.tag == ("Player")) || (other.transform.tag == ("Room")) || (other.transform.tag == ("Enviroment")) ) )
        {
            AnimatorReSet(); animator.SetTrigger("GigaImpactOver");
        }
    }


    public void CallGigaImpactPostion()
    {
        GigaImpactPostion1 = TargetPosition;
    }

    public void DropBerry()
    {
        SnorlaxBerry BerryObj = Instantiate(BerryList[(Random.Range(0.0f, 1.0f) > 0.3f ? 0 : 1)], transform.position + new Vector3(0.7f, 0.721f, 0), Quaternion.identity, transform.parent);
        DestoryEvent += BerryObj.BerryDestoryAnimator;
        BerryObj = Instantiate(BerryList[(Random.Range(0.0f, 1.0f) > 0.3f ? 0 : 1)], transform.position + new Vector3(-0.7f, 0.721f, 0), Quaternion.identity, transform.parent);
        DestoryEvent += BerryObj.BerryDestoryAnimator;
    }

    public void CallisImpact()
    {
        isImpact = true;
    }

    public void CallisGigaImpactFalse()
    {
        isGigaImpact = false;
    }

    void AnimatorReSet()
    {
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Slam");
        animator.ResetTrigger("Yamn");
        animator.ResetTrigger("Impact");
        animator.ResetTrigger("GigaImpact");
        animator.ResetTrigger("GigaImpactOver");
    }

    public void MakeGigaImpactDirector()
    {
        if (!isSilence)
        {
            GigaImpactPostion1 = (new Vector2(5 * TargetPosition.x - 4 * GigaImpactPostion1.x - transform.position.x, 5 * TargetPosition.y - 4 * GigaImpactPostion1.y - transform.position.y));
        }
        else
        {
            GigaImpactPostion1 = (new Vector2(GigaImpactPostion1.x - transform.position.x, GigaImpactPostion1.y - transform.position.y));
        }
        if (GigaImpactPostion1 == Vector2.zero) { GigaImpactPostion1 = direction; }
        else { GigaImpactPostion1 = GigaImpactPostion1.normalized; }
    }

    public void LunchYawn()
    {
        Yamn yamnObj = Instantiate(yamn, transform.transform.position + Vector3.up*1.7f, Quaternion.identity, transform.parent);
        if (isEmptyConfusionDone) { yamnObj.Launch((direction + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))).normalized, 220); }
        else { yamnObj.Launch(direction, 220); }
        yamnObj.empty = transform.GetComponent<Empty>();
    }


    void CheckPlayer()
    {
        RaycastHit2D hitU1 = Physics2D.Raycast(new Vector2(transform.position.x + 0.7f, transform.position.y + 0.5f), Vector2.up, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitU2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.7f, transform.position.y + 0.5f), Vector2.up, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitD1 = Physics2D.Raycast(new Vector2(transform.position.x + 0.7f, transform.position.y + 0.5f), Vector2.down, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitD2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.7f, transform.position.y + 0.5f), Vector2.down, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitR = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, 4f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        if (!isAngry || isImpact)
        {
            if (!isFearDone && !isYamn)
            {
                if (hitU1.collider != null)
                {
                    if (hitU1.collider.gameObject.layer == 8 || hitU1.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, 1);
                        }
                    }
                }
                if (hitU2.collider != null)
                {
                    if (hitU2.collider.gameObject.layer == 8 || hitU2.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, 1);
                        }
                    }
                }
                if (hitD1.collider != null)
                {
                    if (hitD1.collider.gameObject.layer == 8 || hitD1.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, -1);
                        }
                    }
                }
                if (hitD2.collider != null)
                {
                    if (hitD2.collider.gameObject.layer == 8 || hitD2.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, -1);
                        }
                    }
                }
                if (hitR.collider != null)
                {
                    if (hitR.collider.gameObject.layer == 8 || hitR.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(1, 0);
                        }
                    }
                }
                if (hitL.collider != null)
                {
                    if (hitL.collider.gameObject.layer == 8 || hitL.collider.gameObject.layer == 17)
                    {
                        isYamn = true; AnimatorReSet(); animator.SetTrigger("Yamn");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(-1, 0);
                        }
                    }
                }
            }
        }
        else
        {
            if (!isFearDone && !isImpact)
            {
                if (hitU1.collider != null)
                {
                    if (hitU1.collider.gameObject.layer == 8 || hitU1.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, 1);
                        }
                    }
                }
                if (hitU2.collider != null)
                {
                    if (hitU2.collider.gameObject.layer == 8 || hitU2.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, 1);
                        }
                    }
                }
                if (hitD1.collider != null)
                {
                    if (hitD1.collider.gameObject.layer == 8 || hitD1.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, -1);
                        }
                    }
                }
                if (hitD2.collider != null)
                {
                    if (hitD2.collider.gameObject.layer == 8 || hitD2.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(0, -1);
                        }
                    }
                }
                if (hitR.collider != null)
                {
                    if (hitR.collider.gameObject.layer == 8 || hitR.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(1, 0);
                        }
                    }
                }
                if (hitL.collider != null)
                {
                    if (hitL.collider.gameObject.layer == 8 || hitL.collider.gameObject.layer == 17)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                        if (!isSilence)
                        {
                            direction = new Vector2Int(-1, 0);
                        }
                    }
                }
            }
        }

        if (isFearDone && !isSilence)
        {
            if (hitU1.collider != null)
            {
                if (hitU1.collider.gameObject.layer == 8 || hitU1.collider.gameObject.layer == 17)
                {
                    if (isAngry) { AnimatorReSet(); animator.SetTrigger("Impact"); }
                    direction = new Vector2Int(0, -1);
                }
            }
            if (hitU2.collider != null)
            {
                if (hitU2.collider.gameObject.layer == 8 || hitU2.collider.gameObject.layer == 17)
                {
                    if (isAngry) { AnimatorReSet(); animator.SetTrigger("Impact"); }
                    direction = new Vector2Int(0, -1);
                }
            }
            if (hitD1.collider != null)
            {
                if (hitD1.collider.gameObject.layer == 8 || hitD1.collider.gameObject.layer == 17)
                {
                    if (isAngry)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                    }
                    direction = new Vector2Int(0, 1);
                }
            }
            if (hitD2.collider != null)
            {
                if (hitD2.collider.gameObject.layer == 8 || hitD2.collider.gameObject.layer == 17)
                {
                    if (isAngry)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                    }
                    direction = new Vector2Int(0, 1);
                }
            }
            if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.layer == 8 || hitR.collider.gameObject.layer == 17)
                {
                    if (isAngry)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                    }
                    direction = new Vector2Int(-1, 0);
                }
            }
            if (hitL.collider != null)
            {
                if (hitL.collider.gameObject.layer == 8 || hitL.collider.gameObject.layer == 17)
                {
                    if (isAngry)
                    {
                        AnimatorReSet(); animator.SetTrigger("Impact");
                    }
                    direction = new Vector2Int(1, 0);
                }
            }
        }
    }



    void CheckTurn()
    {
        if (direction.x == 0 && (isEmptyU && isEmptyD) && Random.Range(0.0f, 1.0f) <= 0.3f)
        {
            switch (Random.Range(1, 3))
            {
                case 1:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    break;
            }
        }
        else if (direction.x == 0 && !isEmptyU)
        {
            switch (Random.Range(1, 4))
            {

                case 1:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 3:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
            }
        }
        else if (direction.x == 0 && !isEmptyD)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
            }
        }
        else if (direction.y == 0 && (isEmptyL && isEmptyR) && Random.Range(0.0f, 1.0f) <= 0.3f)
        {

            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    break;
            }
        }
        else if (direction.y == 0 && !isEmptyL)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
            }
        }
        else if (direction.y == 0 && !isEmptyR)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 2:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; TurnTimer = 0.5f; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; TurnTimer = 0.5f; }
                    break;
            }
        }

    }
}
