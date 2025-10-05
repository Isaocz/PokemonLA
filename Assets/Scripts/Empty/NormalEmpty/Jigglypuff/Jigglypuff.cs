using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jigglypuff : Empty
{
    
    public bool TurnToU;
    public bool TurnToD;
    public bool TurnToL;
    public bool TurnToR;


    bool isRoll;
    bool isRollStart;
    bool isTurn;
    bool isSing;
    public int TurnCount;
    int RollTimer;
    public int SingTimer;
    Vector2 Director;

    public GameObject Sing;
    GameObject SingObj;

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
        animator.SetFloat("LookX", 0);
        animator.SetFloat("LookY", 1);

        RollTimer = 350;



        StartOverEvent();
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
        if (!isBorn && !isDie && !isHit && isRoll && !isSleepDone && !isCanNotMoveWhenParalysis)
        {
           CheckTurn();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            if (!isSleepDone && !isCanNotMoveWhenParalysis)
            {

                animator.ResetTrigger("Sleep");
                if (!isDie && !isHit)
                {
                    if (!isRoll)
                    {
                        if (RollTimer != 0)
                        {
                            RollTimer += Random.Range(0.0f, 1.0f) > 0.2 ? 1 : 0;
                        }
                        else if (!isRollStart)
                        {
                            animator.SetTrigger("Sing");
                            animator.ResetTrigger("SingOver");
                            SingTimer += isSing ? 1 : 0;
                            if (SingTimer == 0) { SingObj = Instantiate(Sing, transform.position, Quaternion.identity, transform); }
                            isSing = true;
                            if (SingTimer >= 400 || isSilence || isEmptyFrozenDone)
                            {
                                SingTimer = 0;
                                RollTimer = 1;
                                isSing = false;
                                animator.SetTrigger("SingOver");
                                animator.ResetTrigger("Sing");
                            }

                        }
                        if (RollTimer >= 400)
                        {
                            RollTimer = 0;
                            isRollStart = true;
                            animator.SetTrigger("Roll");
                            animator.ResetTrigger("RollOver");
                            Director = new Vector2Int(Random.Range(0.0f, 1.0f) > 0.5 ? -1 : 1, Random.Range(0.0f, 1.0f) > 0.5 ? -1 : 1);
                            animator.SetFloat("LookX", Director.x);
                            animator.SetFloat("LookY", Director.y);
                        }
                    }
                    else
                    {
                        transform.position += new Vector3(Director.x * speed * Time.deltaTime, Director.y * speed * Time.deltaTime, 0);
                    }
                }
                Vector3 TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                if (isEmptyInfatuationDone && InfatuationForDistanceEmpty() != null)
                {
                    TargetPosition = InfatuationForDistanceEmpty().transform.position;
                }
                if (isSilence || isEmptyFrozenDone || ((transform.position - TargetPosition).magnitude <= 5 && TurnCount >= 2))
                {
                    if ((isSilence || isEmptyFrozenDone) && SingObj != null) { Destroy(SingObj); }
                    RollOver();
                }
                if (isHit && isSing) { SingTimer += 10; }
            }
            if (isSleepDone)
            {
                animator.ResetTrigger("Sing");
                animator.ResetTrigger("SingOver");
                animator.ResetTrigger("Roll");
                animator.ResetTrigger("RollOver");
                animator.SetTrigger("Sleep");
                RollTimer = 0;
                isRoll = false;
                isRollStart = false;
                SingTimer = 0;
                isSing = false;
                TurnCount = 0;
                Destroy(SingObj);
            }
            EmptyBeKnock();
            StateMaterialChange();

        }
    }



    void CheckTurn()
    {
        if (!isTurn)
        {
            if (TurnToU) {  Director.y *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToD) {  Director.y *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToR) {  Director.x *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToL) {  Director.x *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            animator.SetFloat("LookX", Director.x);
            animator.SetFloat("LookY", Director.y);
            if(TurnCount >= (isEmptyConfusionDone?(Random.Range(5,12)):7))
            {
                //RollOver();
                Invoke("RollOver", 0.7f);
            }
        }
    }

    void RollOver()
    {
        isRoll = false;
        isRollStart = false;
        TurnCount = 0;
        animator.SetTrigger("RollOver");
        animator.ResetTrigger("Roll");
    }

    void CallisTurnFalse()
    {
        isTurn = false;
    }

    public void CallisRoll()
    {
        isRoll = true;
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
}
