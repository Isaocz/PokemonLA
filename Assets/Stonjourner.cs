using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonjourner : Empty
{
    Vector2 Director;
    public StonjournerPowerPoint PowerPoint;
    public StonjournerStompingTantrum ST;
    float STTimer;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Normal;
        EmptyType02 = Type.TypeEnum.Normal;
        player = GameObject.FindObjectOfType<PlayerControler>();
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

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone)
            {
                STTimer += Time.deltaTime;
                if (STTimer >= (isParalysisDone?8f:5f))
                {
                    STTimer = 0;
                    animator.SetTrigger("Jump");
                }
            }
        }
        else if (isDie && !PowerPoint.isSpriteFadeOut)
        {
            PowerPoint.EndPS();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
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


    public void Jump()
    {
        Instantiate(ST, transform.position, Quaternion.identity, transform.parent).empty = this;

    }


}
