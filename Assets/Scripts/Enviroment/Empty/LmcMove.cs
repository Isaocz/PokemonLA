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
        //（仅绿毛虫）初始化绿毛虫的移动距离
        distance = 5.0f;

        //---（对于所有敌人子类）---，初始化敌人移动速度，属性
        speed = 2.0f;
        EmptyType01 = Type.TypeEnum.Bug;
        EmptyType02 = 0;

        //---（对于所有敌人子类）---，获取玩家
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();

        //---（对于所有敌人子类）---，设置等级
        Emptylevel = SetLevel(player.Level,15);
        //---（对于所有敌人子类）---，初始化能力值和可获得经验
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint= AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp*Emptylevel/7;


        //---（对于所有敌人子类）---，获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //（仅绿毛虫）绿毛虫的初始位置
        FirstX = rigidbody2D.position.x;
        

    }

    // Update is called once per frame
    void Update()
    {
        //---（对于所有敌人子类）---，如果玩家进化，重新获取玩家
        ResetPlayer();

        //---（对于所有敌人子类）---，当敌人生成动画完毕后
        if (!isBorn)
        {
            //每帧检测敌人是否死亡
            EmptyDie();
            //每帧检测敌人材质是否变更
            StateMaterialChange();
            //每帧检测敌人是否中毒
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {
        //---（对于所有敌人子类）---，如果玩家进化，重新获取玩家
        ResetPlayer();
        if (!isBorn)
        {
            //（仅绿毛虫）绿毛虫的移动方式
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
            //---（对于所有敌人子类）---，检测敌人是否被击退
            EmptyBeKnock();
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //---（对于所有敌人子类）---，触碰到玩家后造成碰撞伤害
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }
}
