using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmpleMove : Empty
{
    bool run = false;
    bool launch = false;
    float runCDTimer = 0;
    float launchTimer = 0;
    Vector2 move;
    Vector2 look;
    Vector2 position;
    Vector2 PlayerPosition;
    public Projectile ProjectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.4f;
        EmptyType01 = 7;
        EmptyType02 = 0;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level,20);
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
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            if (!isDie && !isHit && !run && !isSilence)
            {

                position = rigidbody2D.position;
                PlayerPosition = player.GetComponent<Rigidbody2D>().position;
                if (position.x - PlayerPosition.x >= 0) { look.x = 1; } else { look.x = -1; }
                if (position.y - PlayerPosition.y >= 0) { look.y = 1; } else { look.y = -1; }
                RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room","PlayerFly"));
                if (Vector2.Distance(position, PlayerPosition) >= 2.1f && Vector2.Distance(position, PlayerPosition) < 6.0f && SearchPlayer.collider != null && SearchPlayer.transform.tag == "Player")
                {
                    launch = true;
                }
                if (launch && !isSilence)
                {

                    if (launchTimer == 0)
                    {

                        animator.SetTrigger("Launch");
                    }
                    launchTimer += Time.deltaTime;
                    if (launchTimer >= 3.0f) { launch = false; launchTimer = 0; }
                }
            }

            if (isToxicDone){EmptyToxic();}
        }
        InvincibleUpdate();
    }


    


    private void FixedUpdate()
    {
        if (!isBorn)
        {
            if (!isDie && !isHit && !isSilence)
            {
                //获取当前刚体坐标，
                position = rigidbody2D.position;
                PlayerPosition = player.GetComponent<Rigidbody2D>().position;
                if (position.x - PlayerPosition.x >= 0) { look.x = 1; } else { look.x = -1; }
                if (position.y - PlayerPosition.y >= 0) { look.y = 1; } else { look.y = -1; }
                if (Vector2.Distance(position, PlayerPosition) < 2.1f)
                {
                    run = true;
                }
                Vector2 NowPosition = position;
                if (run && runCDTimer == 0)
                {


                    animator.SetFloat("LookDirectionX", look.x);
                    animator.SetFloat("LookDirectionY", look.y);
                    position.x += (position.x - PlayerPosition.x) * speed * Time.deltaTime;
                    position.y += (position.y - PlayerPosition.y) * speed * Time.deltaTime;

                    rigidbody2D.position = position;
                    if (Vector2.Distance(position, PlayerPosition) >= Random.Range(7, 13)) { run = false; runCDTimer = 1; }
                }
                if (runCDTimer != 0)
                {
                    runCDTimer += Time.deltaTime;
                    if (runCDTimer > 2.2f) { runCDTimer = 0; }
                }
                if (!run && Vector2.Distance(position, PlayerPosition) < 5.0f)
                {
                    animator.SetFloat("LookDirectionX", -look.x);
                    animator.SetFloat("LookDirectionY", -look.y);
                }



                move = new Vector2(position.x - NowPosition.x, position.y - NowPosition.y);

                animator.SetFloat("Speed", move.magnitude);
            }


            EmptyBeKnock();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if(other.transform.tag == ("Enviroment"))
        {
            run = false;
            runCDTimer = 1;
        }
    }

    public void LaunchStringShot()
    {
        if ( !isFearDone) {
            Projectile ProjectileObject;

            if (PlayerPosition.y - position.y >= 0)
            {
                ProjectileObject = Instantiate(ProjectilePrefab, rigidbody2D.position + new Vector2(0.04f - look.x * 0.5f, 0.375f), Quaternion.Euler(0, 0, Vector2.Angle(PlayerPosition - position, new Vector2(1, 0))), gameObject.transform);

            }
            else
            {
                ProjectileObject = Instantiate(ProjectilePrefab, rigidbody2D.position + new Vector2(0.04f - look.x * 0.5f, 0.375f), Quaternion.Euler(0, 0, -Vector2.Angle(PlayerPosition - position, new Vector2(1, 0))), gameObject.transform);

            }
            ProjectileObject.Launch(PlayerPosition - position, 110);
        }
    }
}
