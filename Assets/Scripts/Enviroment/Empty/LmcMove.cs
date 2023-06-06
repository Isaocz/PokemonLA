using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LmcMove : Empty
{



    //����2�����ͱ�������ʾ�������������Ϊ1��������Ϊ-1����һ����ʾ��ʼ��x����
    float distance;
    int isLeft = -1;
    float FirstX;


    // Start is called before the first frame update
    void Start()
    {
        //��ʼ����ë�����ֵ
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


        //��ȡ����Ŀ�� ����������Ŀ�� ���ø���ĳ�ʼx�������FirstX��
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
                //��ȡ��ǰ�������꣬����ǰx����ͳ�ʼx�������С���趨�õ��ƶ�����ʱ�������ƶ� ������ʱ�����ó�ʼλ�ã�����ת
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
