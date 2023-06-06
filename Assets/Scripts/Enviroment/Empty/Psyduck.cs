using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psyduck : Empty
{

    bool isAttak;
    float AttackTimer=0;
    float direction;
    public Projectile WaterGun;
    public Projectile Confusion;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0f;
        EmptyType01 = 11;
        EmptyType02 = 0;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level,30);
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
            if (!isDie && !isSilence && !isAttak && !isFearDone)
            {
                Attak();
            }
            if (isAttak && !isSilence && !isFearDone)
            {
                AttackTimer += Time.deltaTime;
            }
            if (AttackTimer >= 3.0f)
            {
                isAttak = false;
                AttackTimer = 0;
            }

            if (isToxicDone) { EmptyToxic(); }
        }
        InvincibleUpdate();
    }

    private void FixedUpdate()
    {
        if (!isBorn)
        {
            EmptyBeKnock();
        }
    }

    public void Lunch()
    {
        Projectile ProjectileObject;
        Projectile Shot;
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
        {
            Shot = WaterGun;
        }
        else
        {
            Shot = Confusion;
        }
        ProjectileObject = Instantiate(Shot, rigidbody2D.position + new Vector2(direction*0.375f, 0.375f), Quaternion.Euler(0, 0, -direction * Vector2.Angle(player.transform.position - transform.position, new Vector2(0, 1))));
        ProjectileObject.Launch(new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y), 55);
        ProjectileObject.empty = gameObject.GetComponent<Empty>();

    }

    void Attak()
    {

        RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room","PlayerFly"));
        if (SearchPlayer.collider != null && SearchPlayer.transform.tag == "Player") {
            Turn();
            animator.SetTrigger("Attak");
            isAttak = true;
        }
    }

    void Turn()
    {
        if(player.transform.position.x - transform.position.x >= 0) { animator.SetFloat("LookX", 1);direction = 1; }
        else{ animator.SetFloat("LookX", 0);direction = -1; }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }
}
