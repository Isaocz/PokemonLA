using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gastly : Empty
{

    public bool isInvisible;
    bool isAtack;
    int AtackTimer;
    public Projectile WillOWisp;
    Vector2 Direction;


    // Start is called before the first frame update
    void Start()
    {
        speed = 0f;
        EmptyType01 = Type.TypeEnum.Ghost;
        EmptyType02 = Type.TypeEnum.Poison;
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

        animator.SetFloat("LookX" , 0);
        animator.SetFloat("LookY" , 1);
        Direction = Vector2.up;
        AtackTimer = 80;

        

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
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn && !isInvisible)
        {
            EmptyBeKnock();
            StateMaterialChange();
            if (!isAtack && !isSleepDone && !isCanNotMoveWhenParalysis)
            {
                AtackTimer += Random.Range(0.0f, 1.0f) >= 0.15f ? 1 : 0;
                if (AtackTimer >= 100)
                {
                    animator.SetTrigger("Invisible");
                    animator.ResetTrigger("EnInvisible");
                    AtackTimer = 0;
                }
                
            }
        }
        if (isInvisible && !isSleepDone && !isCanNotMoveWhenParalysis)
        {
            AtackTimer += Random.Range(0.0f, 1.0f) >= 0.15f ? 1 : 0;
            if(AtackTimer >= 160) {
               
                MoveForPlayer();
                animator.SetTrigger("EnInvisible");
                animator.ResetTrigger("Invisible");
                isAtack = true;
                isInvisible = false;
                AtackTimer = 0;
            }
        }
    }

    public void CallAtack()
    {
        isAtack = false;
        if (!isFearDone && !isSilence)
        {
            Projectile WOWObj1 = Instantiate(WillOWisp, transform.position, Quaternion.Euler(-90, 0, 0), transform.parent);
            WOWObj1.Launch(-Direction, 220);
            WOWObj1.empty = this;
            Projectile WOWObj2 = Instantiate(WillOWisp, transform.position, Quaternion.Euler(-90, 0, 0), transform.parent);
            WOWObj2.Launch(-(Quaternion.AngleAxis(15, Vector3.forward) * Direction), 220);
            WOWObj2.empty = this;
            Projectile WOWObj3 = Instantiate(WillOWisp, transform.position, Quaternion.Euler(-90, 0, 0), transform.parent);
            WOWObj3.Launch(-(Quaternion.AngleAxis(-15, Vector3.forward) * Direction), 220);
            WOWObj3.empty = this;
        }
    }

    public void CallInvisible()
    {
        isInvisible = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player") && !isInvisible)
        {
            EmptyTouchHit(other.gameObject);
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }

    void MoveForPlayer()
    {
        Vector3 TargetPosition = player.transform.position;
        //如果该敌人处于被替身吸引状态，且替身目标不为空
        if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
        if ( isEmptyInfatuationDone && InfatuationForDistanceEmpty() != null )
        {
            TargetPosition = InfatuationForDistanceEmpty().transform.position;
        }
        switch (Random.Range(1, 5))
        {
            case 1:
                if (!isFearDone)
                {
                    if (TargetPosition.x + 3 <= transform.parent.position.x + 12)
                    {
                        transform.position = new Vector3(TargetPosition.x + 3 + Random.Range(-0.5f, 0.0f), TargetPosition.y + Random.Range(-1.5f, 1.5f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookX", 1);
                            animator.SetFloat("LookY", 0);
                            Direction = Vector2.right;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; } }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (TargetPosition.x + 5 <= transform.parent.position.x + 12)
                    {
                        transform.position = new Vector3(TargetPosition.x + 5 + Random.Range(-0.5f, 0.0f), TargetPosition.y + Random.Range(-1.5f, 1.5f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookX", -1);
                            animator.SetFloat("LookY", 0);
                            Direction = Vector2.right;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; } }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                break;
            case 2:
                if (!isFearDone)
                {
                    if (TargetPosition.x - 3 >= transform.parent.position.x - 12)
                    {
                        transform.position = new Vector3(TargetPosition.x - 3 - Random.Range(-0.5f, 0.0f), TargetPosition.y + Random.Range(-1.5f, 1.5f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookX", -1);
                            animator.SetFloat("LookY", 0);
                            Direction = Vector2.left;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; } }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (TargetPosition.x - 5 >= transform.parent.position.x - 12)
                    {
                        transform.position = new Vector3(TargetPosition.x - 5 - Random.Range(-0.5f, 0.0f), TargetPosition.y + Random.Range(-1.5f, 1.5f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookX", 1);
                            animator.SetFloat("LookY", 0);
                            Direction = Vector2.left;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; }}
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                break;
            case 3:
                if (!isFearDone)
                {
                    if (TargetPosition.y + 3 <= transform.parent.position.y + 7)
                    {
                        transform.position = new Vector3(TargetPosition.x + Random.Range(-1.5f, 1.5f), TargetPosition.y + 3 + Random.Range(-0.5f, 0.0f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookY", 1);
                            animator.SetFloat("LookX", 0);
                            Direction = Vector2.up;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; } }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (TargetPosition.y + 5 <= transform.parent.position.y + 7)
                    {
                        transform.position = new Vector3(TargetPosition.x + Random.Range(-1.5f, 1.5f), TargetPosition.y + 5 + Random.Range(-0.5f, 0.0f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookY", -1);
                            animator.SetFloat("LookX", 0);
                            Direction = Vector2.up;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; } }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                break;
            case 4:
                if (!isFearDone)
                {
                    if (TargetPosition.y - 3 >= transform.parent.position.x - 7)
                    {
                        transform.position = new Vector3(TargetPosition.x + Random.Range(-1.5f, 1.5f), TargetPosition.y - 3 - Random.Range(-0.5f, 0.0f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookY", -1);
                            animator.SetFloat("LookX", 0);
                            Direction = Vector2.down;
                        }
                        else { switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; }  }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (TargetPosition.y - 5 >= transform.parent.position.x - 7)
                    {
                        transform.position = new Vector3(TargetPosition.x + Random.Range(-1.5f, 1.5f), TargetPosition.y - 5 - Random.Range(-0.5f, 0.0f), 0);
                        if (!isEmptyConfusionDone)
                        {
                            animator.SetFloat("LookY", 1);
                            animator.SetFloat("LookX", 0);
                            Direction = Vector2.down;
                        }
                        else {
                            switch (Random.Range(0, 4)) { case 0: Direction = Vector2.right; break; case 1: Direction = Vector2.left; break; case 2: Direction = Vector2.up; break; case 3: Direction = Vector2.down; break; }
                        }
                        animator.SetFloat("LookX", Direction.x);
                        animator.SetFloat("LookY", Direction.y);
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                break;

        }
    }
    
}
