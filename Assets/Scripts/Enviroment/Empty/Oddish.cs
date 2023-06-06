using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oddish : Empty
{
    AIPath AI;
    float LookX = 0;
    int KaCheaker;
    bool isKa;
    Vector2 KaCheakVector;
    Vector2 Direction;
    Vector3 position;



    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = 4;
        EmptyType02 = 12;
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
        KaCheakVector = rigidbody2D.position;

        transform.GetChild(3).DetachChildren();



    }

    // Update is called once per frame
    void Update()
    {
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            if (isToxicDone) { EmptyToxic(); }
        }
        InvincibleUpdate();
    }

    private void FixedUpdate()
    {
        if (!isBorn)
        {
            EmptyBeKnock();
            animator.SetFloat("Speed" , Mathf.Abs(transform.position.x - LookX));
            if (transform.position.x - LookX >= 0) { animator.SetFloat("LookX", 1); }
            else { animator.SetFloat("LookX", 0); }
            LookX = transform.position.x;
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);
        }
    }

    public void CallCanMove()
    {
        //AI.canMove = true;
        animator.SetFloat("Speed", 1.0f);
    }
}
