using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kricketune : Empty
{
    Vector2 Director;
    Vector2 MoveDirector;
    Vector2 TargetPosition;

    float MoveDRefreshTimer;
    int isMoveDFlip;
    public KrickeyunrMoveShadow MoveShadow;
    float MoveShadowTimer;

    float XCutTimer;
    bool isXCutStart;
    public bool isXcutMove;
    Collider2D KricketuneCollider;



    bool isSingSong;
    float SingSongPer;
    float SingsongJudegTimer;
    public GameObject SingPS;
    public KricketunrBufBuzz BugBuzz;
    bool isBugBuzzBorn;



    public Animator WingAnimator;
    public Kricketot BabyKricketot;
    Room ParentRoom;
    public int ChildCount;


    float ConfusionTimer = 1.5f;
    int ConfusionTurnRotation = 15;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Bug;
        EmptyType02 = Type.TypeEnum.No;
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
        KricketuneCollider = GetComponent<Collider2D>();
        ParentRoom = transform.parent.parent.GetComponent<Room>();
        isMoveDFlip = 1;
        SingSongPer = 1;
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isDie && !isBorn)
        {
            if (XCutTimer <= 1) { XCutTimer += Time.deltaTime; }
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {

                if (animator.speed == 0) { animator.speed = 1; WingAnimator.speed = 1; }
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }

                if (!isSingSong)
                {
                    if (!isXcutMove) {
                        Director = (TargetPosition - (Vector2)transform.position).normalized;
                        float Angle = _mTool.Angle_360Y(Director, Vector2.right);
                        if (Angle >= 45 && Angle < 135) { MoveDirector.x = 0; MoveDirector.y = 1; }
                        else if (Angle >= 135 && Angle < 225) { MoveDirector.x = -1; MoveDirector.y = 0; }
                        else if (Angle >= 225 && Angle < 315) { MoveDirector.x = 0; MoveDirector.y = -1; }
                        else { MoveDirector.x = 1; MoveDirector.y = 0; }
                        if (isFearDone) { MoveDirector = -MoveDirector; }
                        SingsongJudegTimer += Time.deltaTime;
                        SingSongPer -= 0.005f * Time.deltaTime;
                        if (SingsongJudegTimer >= 1.5f)
                        {
                            SingsongJudegTimer = 0;
                            if (Random.Range(0.0f, 1.0f) > SingSongPer + 0.3f && !isXCutStart && !isFearDone) { SingSongPer = 1; isSingSong = true; animator.SetTrigger("Sing"); }
                        }
                        if (isEmptyConfusionDone) {
                            ConfusionTimer -= Time.deltaTime;
                            MoveDirector = (Quaternion.AngleAxis(ConfusionTurnRotation, Vector3.forward) * MoveDirector).normalized;
                            if (ConfusionTimer <= 0)
                            {
                                ConfusionTimer = 1.5f;
                                ConfusionTurnRotation = Random.Range(-30, 30);
                            }
                        }
                        Director = MoveDirector;
                    }
                    if (!isXCutStart)
                    {
                        Vector2 XCutDirectior = CheckXCutTarget();
                        if (XCutDirectior != Vector2.zero && XCutTimer > 1 && !isFearDone) { isXCutStart = true; animator.SetTrigger("XCut"); SingSongPer -= 0.2f; }
                        animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);
                        MoveDRefreshTimer += Time.deltaTime;
                        if (MoveDRefreshTimer >= 1f) {
                            MoveDRefreshTimer = 0;
                            if (Random.Range(0.0f, 1.0f) > 0.5f) { isMoveDFlip = -1; } else { isMoveDFlip = 1; }
                        }
                        MoveDirector = Quaternion.AngleAxis(45 * isMoveDFlip, Vector3.forward) * MoveDirector;
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)MoveDirector.x * Time.deltaTime * speed * (isFearDone ? 1.4f : 1), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)MoveDirector.y * Time.deltaTime * speed * (isFearDone ? 1.4f : 1), -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    }

                    if (isXcutMove)
                    {
                        MoveShadowTimer += Time.deltaTime;
                        if (MoveShadowTimer >= 0.07f) {
                            MoveShadowTimer = 0;
                            KrickeyunrMoveShadow S = Instantiate(MoveShadow, transform.position + 0.6f * Vector3.down, Quaternion.identity);
                            S.sprite01.sprite = skinRenderers[0].sprite;
                            S.sprite02.sprite = skinRenderers[1].sprite;
                        }
                        Debug.Log(MoveDirector);
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)MoveDirector.x * Time.deltaTime * 7.25f * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)MoveDirector.y * Time.deltaTime * 7.25f * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    }
                    WingAnimator.SetFloat("LookX", animator.GetFloat("LookX"));
                    WingAnimator.SetFloat("LookY", animator.GetFloat("LookY"));
                }
                else if (isSingSong)
                {
                    WingAnimator.SetFloat("LookX", 0);
                    WingAnimator.SetFloat("LookY", -1);
                }
            }
            else if (isCanNotMoveWhenParalysis)
            {
                animator.speed = 0;
                WingAnimator.speed = 0;
            }

            if (isSilence)
            {
                animator.SetFloat("LookX", 0);
                animator.SetFloat("LookY", -1);
                WingAnimator.SetFloat("LookX", 0);
                WingAnimator.SetFloat("LookY", -1);
            }

            if (((isSleepDone && isSingSong)) || ((isFearDone && isSingSong)))
            {
                animator.SetTrigger("Sleep");
                animator.SetFloat("LookX",0);
                animator.SetFloat("LookY",-1);
                CallSingStartFalse();
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
        if (other.transform.tag == ("Player"))
        {
            if (isXcutMove)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 75, 0, 0, Type.TypeEnum.Bug);
            }
            else
            {
                EmptyTouchHit(other.gameObject);
            }
        }
    }

    public void CallXCutStartFalse()
    {
        isXCutStart = false;
    }

    public void CallSingStartFalse()
    {
        isSingSong = false;
        isBugBuzzBorn = false;
    }

    public void BornBaby()
    {
        if (ChildCount < 4 && Mathf.Abs(transform.position.x - transform.parent.position.x) < 9.4f)
        {
            if (isThisPointEmpty(transform.position + Vector3.right * 3) &&  isThisPointInRoom(transform.position + Vector3.right * 3 - transform.parent.position)   )
            {
                ParentRoom.isClear++;
                Instantiate(BabyKricketot, transform.position + Vector3.right * 3, Quaternion.identity, transform.parent).isBeCall = true;
                ChildCount++;
            }
            if (isThisPointEmpty(transform.position - Vector3.right * 3) && isThisPointInRoom(transform.position - Vector3.right * 3 - transform.parent.position))
            {
                ParentRoom.isClear++;
                Instantiate(BabyKricketot, transform.position - Vector3.right * 3, Quaternion.identity, transform.parent).isBeCall = true;
                ChildCount++;
            }
        }
    }

    Vector2 CheckXCutTarget()
    {
        
        RaycastHit2D SearchPlayerL01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y),Vector3.left , 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL02 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f),Vector3.left , 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL03 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.50f),Vector3.left , 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL04 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.75f),Vector3.left , 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL05 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector3.left, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL06 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.50f), Vector3.left, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL07 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.75f), Vector3.left, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (   SearchPlayerL01.collider != null && SearchPlayerL01.transform.tag == "Player" && ( ((isSubsititue && SubsititueTarget != null) && (SearchPlayerL01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL01.collider.gameObject == player.gameObject))
            || SearchPlayerL02.collider != null && SearchPlayerL02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL02.collider.gameObject == player.gameObject))
            || SearchPlayerL03.collider != null && SearchPlayerL03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL03.collider.gameObject == player.gameObject))
            || SearchPlayerL04.collider != null && SearchPlayerL04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL04.collider.gameObject == player.gameObject))
            || SearchPlayerL05.collider != null && SearchPlayerL05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL05.collider.gameObject == player.gameObject))
            || SearchPlayerL06.collider != null && SearchPlayerL06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL06.collider.gameObject == player.gameObject))
            || SearchPlayerL07.collider != null && SearchPlayerL07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL07.collider.gameObject == player.gameObject)))
        {


            return Vector2.left;
        }

        RaycastHit2D SearchPlayerR01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR02 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR03 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.50f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR04 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.75f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR05 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR06 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.50f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR07 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.75f), Vector3.right, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (   SearchPlayerR01.collider != null && SearchPlayerR01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR01.collider.gameObject == player.gameObject))
            || SearchPlayerR02.collider != null && SearchPlayerR02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR02.collider.gameObject == player.gameObject))
            || SearchPlayerR03.collider != null && SearchPlayerR03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR03.collider.gameObject == player.gameObject))
            || SearchPlayerR04.collider != null && SearchPlayerR04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR04.collider.gameObject == player.gameObject))
            || SearchPlayerR05.collider != null && SearchPlayerR05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR05.collider.gameObject == player.gameObject))
            || SearchPlayerR06.collider != null && SearchPlayerR06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR06.collider.gameObject == player.gameObject))
            || SearchPlayerR07.collider != null && SearchPlayerR07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR07.collider.gameObject == player.gameObject)))
        {
            return Vector2.right;
        }

        RaycastHit2D SearchPlayerU01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU02 = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU03 = Physics2D.Raycast(new Vector2(transform.position.x + 0.50f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU04 = Physics2D.Raycast(new Vector2(transform.position.x + 0.75f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU05 = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU06 = Physics2D.Raycast(new Vector2(transform.position.x - 0.50f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU07 = Physics2D.Raycast(new Vector2(transform.position.x - 0.75f, transform.position.y), Vector3.up, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (   SearchPlayerU01.collider != null && SearchPlayerU01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU01.collider.gameObject == player.gameObject))
            || SearchPlayerU02.collider != null && SearchPlayerU02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU02.collider.gameObject == player.gameObject))
            || SearchPlayerU03.collider != null && SearchPlayerU03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU03.collider.gameObject == player.gameObject))
            || SearchPlayerU04.collider != null && SearchPlayerU04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU04.collider.gameObject == player.gameObject))
            || SearchPlayerU05.collider != null && SearchPlayerU05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU05.collider.gameObject == player.gameObject))
            || SearchPlayerU06.collider != null && SearchPlayerU06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU06.collider.gameObject == player.gameObject))
            || SearchPlayerU07.collider != null && SearchPlayerU07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU07.collider.gameObject == player.gameObject)))
        {
            return Vector2.up;
        }

        RaycastHit2D SearchPlayerD01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD02 = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD03 = Physics2D.Raycast(new Vector2(transform.position.x + 0.50f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD04 = Physics2D.Raycast(new Vector2(transform.position.x + 0.75f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD05 = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD06 = Physics2D.Raycast(new Vector2(transform.position.x - 0.50f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD07 = Physics2D.Raycast(new Vector2(transform.position.x - 0.75f, transform.position.y), Vector3.down, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (   SearchPlayerD01.collider != null && SearchPlayerD01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD01.collider.gameObject == player.gameObject))
            || SearchPlayerD02.collider != null && SearchPlayerD02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD02.collider.gameObject == player.gameObject))
            || SearchPlayerD03.collider != null && SearchPlayerD03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD03.collider.gameObject == player.gameObject))
            || SearchPlayerD04.collider != null && SearchPlayerD04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD04.collider.gameObject == player.gameObject))
            || SearchPlayerD05.collider != null && SearchPlayerD05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD05.collider.gameObject == player.gameObject))
            || SearchPlayerD06.collider != null && SearchPlayerD06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD06.collider.gameObject == player.gameObject))
            || SearchPlayerD07.collider != null && SearchPlayerD07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD07.collider.gameObject == player.gameObject)))
        {
            return Vector2.down;
        }


        return Vector2.zero;
    }

    public void StartSing()
    {
        if (!isBugBuzzBorn) {
            isBugBuzzBorn = true;
            Instantiate(SingPS, transform.position + 1.2f * Vector3.down, Quaternion.identity, transform);
            Instantiate(BugBuzz, transform.position, Quaternion.identity, transform).ParentEmpty = this;
        }
    }



}
