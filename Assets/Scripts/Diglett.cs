using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diglett : Empty
{
    public GameObject MudShot;

    Vector2 Director;
    Vector2 TargetPosition;
    bool isUnderGround;
    float UpGroundTimer;
    float UnderGroundTimer;
    bool isLunchMudShot;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Fairy;
        EmptyType02 = PokemonType.TypeEnum.Normal;
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
        if (!isBorn)
        {

            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();


            if (isSleepDone) { animator.speed = 0; }
            else { animator.speed = 1; }

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis) {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(12) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(12).transform.position; }
                if (!isUnderGround)
                {
                    UpGroundTimer += Time.deltaTime;
                    if (!isLunchMudShot && UpGroundTimer >= 2 && UpGroundTimer <= 3.5f)
                    {

                        LunchMudShot();
                    }
                    if (UpGroundTimer >= 6)
                    {
                        isUnderGround = true;
                        UpGroundTimer = 0;
                        animator.SetTrigger("Down");
                        animator.ResetTrigger("Up");
                    }
                }
                else
                {
                    UnderGroundTimer += Time.deltaTime;
                    if (UnderGroundTimer >= 3)
                    {
                        isLunchMudShot = false;
                        isUnderGround = false;
                        UnderGroundTimer = 0;
                        animator.SetTrigger("Up");
                        animator.ResetTrigger("Down");
                    }
                }

                Director = (TargetPosition - (Vector2)transform.position);
                animator.SetFloat("LookX", Director.x);
            }
        }
    }


    void LunchMudShot()
    {
        if (!isEmptyInfatuationDone)
        {
            RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 12f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
            if (SearchPlayer.collider != null && SearchPlayer.transform.tag == "Player")
            {
                Vector2 d = new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y).normalized;
                if (isEmptyConfusionDone) { d += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); d = d.normalized; }
                if (!isFearDone && !isSilence)
                {
                    Instantiate(MudShot, transform.position + 0.25f * Vector3.up + (Vector3)d, Quaternion.Euler(new Vector3(0, 0, _mTool.Angle_360Y(d, Vector2.right))), transform.parent).transform.GetChild(0).GetComponent<DiglettMudShot>().EmptyDiglett = this;
                }
                isLunchMudShot = true;
            }
        }
        else
        {
            RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 12f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly"));
            if (SearchPlayer.collider != null && SearchPlayer.transform.tag == "Empty")
            {
                Vector2 d = new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y).normalized;
                if (isEmptyConfusionDone) { d += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); d = d.normalized; }
                if (!isFearDone && !isSilence)
                {
                    Instantiate(MudShot, transform.position + 0.25f * Vector3.up + (Vector3)d, Quaternion.Euler(new Vector3(0, 0, _mTool.Angle_360Y(d, Vector2.right))), transform.parent).transform.GetChild(0).GetComponent<DiglettMudShot>().EmptyDiglett = this;
                }
                isLunchMudShot = true;
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


    bool isSubsititueUpground;
    GameObject SubsititueTargetUpGround;
    public void GetSubsititueInformation()
    {
        isSubsititueUpground = isSubsititue;
        SubsititueTargetUpGround = SubsititueTarget;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
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
        if (!isFearDone) {
            if ((Random.Range(0.0f, 1.0f) > 0.65f || isEmptyInfatuationDone || isSubsititueUpground) && !isSilence)
            {
                if (SubsititueTargetUpGround != null && isSubsititueUpground) { transform.position = SubsititueTargetUpGround.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0); }
                else { transform.position = (Vector3)TargetPosition + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0); }
            }
            else
            {
                transform.position = transform.parent.position + new Vector3(Random.Range(ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.RoomSize[3]), Random.Range(ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.RoomSize[0]), 0);
            }

        }
        else
        {
            transform.position = transform.parent.position + new Vector3( (Random.Range(0.0f,1.0f) > 0.5 ? (Random.Range(ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.RoomSize[2]+2.0f)) : (Random.Range(ParentPokemonRoom.RoomSize[3]-2.0f, ParentPokemonRoom.RoomSize[3])) )   , (Random.Range(0.0f, 1.0f) > 0.5 ? (Random.Range(ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.RoomSize[1]+1.5f)) : (Random.Range(ParentPokemonRoom.RoomSize[0]-1.5f, ParentPokemonRoom.RoomSize[0]))),    0);
        }
        while (!isThisPointEmpty(transform.position))
        {
            transform.position = transform.parent.position + new Vector3(Random.Range(ParentPokemonRoom.RoomSize[2], ParentPokemonRoom.RoomSize[3]), Random.Range(ParentPokemonRoom.RoomSize[1], ParentPokemonRoom.RoomSize[0]), 0);
            Debug.Log(transform.position);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, transform.parent.position.x + ParentPokemonRoom.RoomSize[2] , transform.parent.position.x  + ParentPokemonRoom.RoomSize[3]), Mathf.Clamp(transform.position.y, transform.parent.position.y + ParentPokemonRoom.RoomSize[1], transform.parent.position.y + ParentPokemonRoom.RoomSize[0]), 0);
    }


}
