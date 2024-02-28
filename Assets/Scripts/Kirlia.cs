using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirlia : Empty
{
    Vector2 Director;
    Vector2 JumpDirector;
    Vector2 TargetPosition;


    public bool isTP;
    float TPTimer;

    public bool isJump;
    public bool isInJump;
    int JumpCount;
    public KirliaConfusionManger JumpConfusion;


    public bool isDG;
    public KirliaDGSix DG;

    bool isFearTP;



    // Start is called before the first frame update
    void Start()
    {
        
        EmptyType01 = Type.TypeEnum.Psychic;
        EmptyType02 = Type.TypeEnum.Fairy;
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
        animator.SetFloat("LookX", -1);
        animator.SetFloat("LookY", -1);
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
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();

            
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position; Debug.Log(TargetPosition); }


                if (!isFearDone)
                {
                    isFearTP = false;
                    if (!isDG && !isTP)
                    {
                        if (JumpCount < 3)
                        {
                            isJump = true;
                            isDG = false;
                        }
                        else if (JumpCount >= 3)
                        {
                            JumpCount = 0;
                            isJump = false;
                            isDG = true;
                        }
                    }
                    if (isJump)
                    {

                        if (!isInJump)
                        {
                            Director = (TargetPosition - (Vector2)transform.position).normalized;
                            JumpDirector = (Quaternion.AngleAxis(((JumpCount % 2 == 0 ? -1 : 1)) * (isEmptyConfusionDone ? Random.Range(25, 100) : Random.Range(12, 25)), Vector3.forward) * Director);
                            if (isEmptyConfusionDone) { JumpDirector += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); JumpDirector = JumpDirector.normalized; }
                        }
                        else
                        {
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)JumpDirector.x * Time.deltaTime * speed * (Mathf.Pow(1.2f, Mathf.Clamp(JumpCount, 0, 2))), -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)JumpDirector.y * Time.deltaTime * speed * (Mathf.Pow(1.2f, Mathf.Clamp(JumpCount, 0, 2))), -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                            animator.SetFloat("LookX", ((JumpCount == 2 ? Director.x : JumpDirector.x) >= 0 ? 1 : -1));
                            animator.SetFloat("LookY", ((JumpCount == 2 ? Director.y : JumpDirector.y) >= 0 ? 1 : -1));
                        }
                        animator.SetTrigger("Jump");
                        animator.ResetTrigger("Idle");
                        animator.ResetTrigger("TP");
                        animator.ResetTrigger("Attack");
                    }
                    else if (isDG && !isJump && !isTP)
                    {
                        animator.SetTrigger("Attack");
                        animator.ResetTrigger("Idle");
                        animator.ResetTrigger("Jump");
                        animator.ResetTrigger("TP");
                    }
                    else if (isTP && !isDG && !isJump)
                    {
                        TPTimer += Time.deltaTime;
                        if (TPTimer > 6.5f)
                        {
                            animator.SetTrigger("TP");
                            TPTimer = 0;
                        }
                        animator.ResetTrigger("Idle");
                        animator.ResetTrigger("Jump");
                        animator.ResetTrigger("Attack");
                    }
                }
                else
                {
                    if (!isFearTP) {
                        isFearTP = true;
                        isTP = true;
                        TPTimer = 4.1f;
                        isDG = false;
                        isJump = false;
                    }
                    if (!isDG && !isTP)
                    {
                        isJump = true;
                        isDG = false;
                    }
                    if (isJump)
                    {

                        if (!isInJump)
                        {
                            Director = (TargetPosition - (Vector2)transform.position).normalized;
                            JumpDirector = (Quaternion.AngleAxis(((JumpCount % 2 == 0 ? -1 : 1)) * Random.Range(70, 90), Vector3.forward) * -Director);
                            if (isEmptyConfusionDone) { JumpDirector += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); JumpDirector = JumpDirector.normalized; }
                        }
                        else
                        {
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)JumpDirector.x * Time.deltaTime * speed * (Mathf.Pow(1.2f, Mathf.Clamp(JumpCount, 0, 2))), -12f + transform.parent.position.x, 12f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)JumpDirector.y * Time.deltaTime * speed * (Mathf.Pow(1.2f, Mathf.Clamp(JumpCount, 0, 2))), -7.5f + transform.parent.position.y, 7.5f + transform.parent.position.y));
                            animator.SetFloat("LookX", ((JumpDirector.x) >= 0 ? 1 : -1));
                            animator.SetFloat("LookY", ((JumpDirector.y) >= 0 ? 1 : -1));
                        }
                        animator.SetTrigger("Jump");
                        animator.ResetTrigger("Idle");
                        animator.ResetTrigger("TP");
                        animator.ResetTrigger("Attack");
                    }
                    else if (isTP && !isDG && !isJump)
                    {
                        TPTimer += Time.deltaTime;
                        if (TPTimer > 4.2f)
                        {
                            animator.SetTrigger("FearTP");
                            TPTimer = 0;
                        }
                        animator.ResetTrigger("Idle");
                        animator.ResetTrigger("Jump");
                        animator.ResetTrigger("Attack");
                    }
                }


            }
            else if (isSilence)
            {
                animator.SetTrigger("Idle");
                animator.ResetTrigger("Jump");
                animator.ResetTrigger("TP");
                animator.ResetTrigger("Attack");
            }

           
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
        TPPosition = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(4.5f, 9f);
        TPPosition = new Vector3( Mathf.Clamp(TPPosition.x , transform.parent.position.x-12.0f , transform.parent.position.x+12.0f), Mathf.Clamp(TPPosition.y, transform.parent.position.y - 7.0f, transform.parent.position.y + 7.0f));
        int TPCount = 0;
        while (!isThisPointEmpty(TPPosition) || Mathf.Abs(TPPosition.x - transform.parent.position.x) > 12 || Mathf.Abs(TPPosition.y - transform.parent.position.y) > 7)
        {
            TPPosition = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(4.5f, 7.5f);
            TPPosition = new Vector3(Mathf.Clamp(TPPosition.x, transform.parent.position.x - 12.0f, transform.parent.position.x + 12.0f), Mathf.Clamp(TPPosition.y, transform.parent.position.y - 7.0f, transform.parent.position.y + 7.0f));
            Debug.Log(TPPosition);
            Debug.Log(!isThisPointEmpty(TPPosition) || Mathf.Abs(TPPosition.x) >= 12 || Mathf.Abs(TPPosition.y) >= 7);
            TPCount++;
            if(TPCount > 100)
            {
                TPPosition = transform.position;
                break;
            }
        }

        transform.position = TPPosition;
    }

    public void JumpCountPlus()
    {
        if (!isFearDone) {
            JumpCount++;
            if ( JumpCount != 3) Instantiate(JumpConfusion, transform.position, Quaternion.identity).ParentKirlia = this;
        }
    }

    public void CallisTPFalse()
    {
        isTP = false;
        animator.SetFloat("LookX", (-1));
        animator.SetFloat("LookY", (-1));
    }

    public void LunchDG()
    {
        Instantiate(DG, transform.position, Quaternion.identity).ParentKirlia = this;
        isTP = true;
        isDG = false;
        isJump = false;
    }
}
