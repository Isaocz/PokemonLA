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
        EmptyType01 = 8;
        EmptyType02 = 4;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level, 30);
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
        if (!isBorn)
        {
            EmptyDie();
            if (isToxicDone) { EmptyToxic(); }
        }
        InvincibleUpdate();
    }

    private void FixedUpdate()
    {
        if (!isBorn && !isInvisible)
        {
            EmptyBeKnock();
            StateMaterialChange();
            if (!isAtack)
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
        if (isInvisible)
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
            WOWObj1.empty = transform.GetComponent<Empty>();
            Projectile WOWObj2 = Instantiate(WillOWisp, transform.position, Quaternion.Euler(-90, 0, 0), transform.parent);
            WOWObj2.Launch(-(Quaternion.AngleAxis(15, Vector3.forward) * Direction), 220);
            WOWObj2.empty = transform.GetComponent<Empty>();
            Projectile WOWObj3 = Instantiate(WillOWisp, transform.position, Quaternion.Euler(-90, 0, 0), transform.parent);
            WOWObj3.Launch(-(Quaternion.AngleAxis(-15, Vector3.forward) * Direction), 220);
            WOWObj3.empty = transform.GetComponent<Empty>();
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
    }

    void MoveForPlayer()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                if (!isFearDone)
                {
                    if (player.transform.position.x + 3 <= transform.parent.position.x + 12)
                    {
                        transform.position = new Vector3(player.transform.position.x + 3 + Random.Range(-0.5f, 0.0f), player.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
                        animator.SetFloat("LookX", 1);
                        animator.SetFloat("LookY", 0);
                        Direction = Vector2.right;
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (player.transform.position.x + 5 <= transform.parent.position.x + 12)
                    {
                        transform.position = new Vector3(player.transform.position.x + 5 + Random.Range(-0.5f, 0.0f), player.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
                        animator.SetFloat("LookX", -1);
                        animator.SetFloat("LookY", 0);
                        Direction = Vector2.right;
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
                    if (player.transform.position.x - 3 >= transform.parent.position.x - 12)
                    {
                        transform.position = new Vector3(player.transform.position.x - 3 - Random.Range(-0.5f, 0.0f), player.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
                        animator.SetFloat("LookX", -1);
                        animator.SetFloat("LookY", 0);
                        Direction = Vector2.left;
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (player.transform.position.x - 5 >= transform.parent.position.x - 12)
                    {
                        transform.position = new Vector3(player.transform.position.x - 5 - Random.Range(-0.5f, 0.0f), player.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
                        animator.SetFloat("LookX", 1);
                        animator.SetFloat("LookY", 0);
                        Direction = Vector2.left;
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
                    if (player.transform.position.y + 3 <= transform.parent.position.y + 7)
                    {
                        transform.position = new Vector3(player.transform.position.x + Random.Range(-1.5f, 1.5f), player.transform.position.y + 3 + Random.Range(-0.5f, 0.0f), 0);
                        animator.SetFloat("LookY", 1);
                        animator.SetFloat("LookX", 0);
                        Direction = Vector2.up;
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (player.transform.position.y + 5 <= transform.parent.position.y + 7)
                    {
                        transform.position = new Vector3(player.transform.position.x + Random.Range(-1.5f, 1.5f), player.transform.position.y + 5 + Random.Range(-0.5f, 0.0f), 0);
                        animator.SetFloat("LookY", -1);
                        animator.SetFloat("LookX", 0);
                        Direction = Vector2.up;
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
                    if (player.transform.position.y - 3 >= transform.parent.position.x - 7)
                    {
                        transform.position = new Vector3(player.transform.position.x + Random.Range(-1.5f, 1.5f), player.transform.position.y - 3 - Random.Range(-0.5f, 0.0f), 0);
                        animator.SetFloat("LookY", -1);
                        animator.SetFloat("LookX", 0);
                        Direction = Vector2.down;
                    }
                    else
                    {
                        MoveForPlayer();
                    }
                }
                else
                {
                    if (player.transform.position.y - 5 >= transform.parent.position.x - 7)
                    {
                        transform.position = new Vector3(player.transform.position.x + Random.Range(-1.5f, 1.5f), player.transform.position.y - 5 - Random.Range(-0.5f, 0.0f), 0);
                        animator.SetFloat("LookY", 1);
                        animator.SetFloat("LookX", 0);
                        Direction = Vector2.down;
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
