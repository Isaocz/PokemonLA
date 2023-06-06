using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LmcMove : Empty
{



    //声明2个整型变量，表示朝向，如果朝向右为1，朝向左为-1，另一个表示初始的x坐标
    float distance;
    int isLeft = -1;
    float FirstX;


    // Start is called before the first frame update
    void Start()
    {
        //初始化绿毛虫的数值
        distance = 5.0f;
        speed = 2.0f;
        EmptyType01 = 7;
        EmptyType02 = 0;

        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level,15);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint= AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp*Emptylevel/7;


        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        FirstX = rigidbody2D.position.x;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
        }
        InvincibleUpdate();
    }

    private void FixedUpdate()
    {
        if (!isBorn)
        {
            if (!isDie && !isHit)
            {
                //获取当前刚体坐标，当当前x坐标和初始x坐标距离小于设定好的移动距离时，缓慢移动 当大于时，重置初始位置，方向反转
                Vector2 position = rigidbody2D.position;
                if (Mathf.Abs(position.x - FirstX) < distance)
                {
                    position.x = position.x + speed * isLeft * Time.deltaTime;
                    rigidbody2D.position = position;

                }
                else
                {
                    FirstX = position.x;
                    isLeft = -isLeft;
                    animator.SetTrigger("turn");
                }
            }

            EmptyBeKnock();
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }
}
