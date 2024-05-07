using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Misdreavus : Empty
{
    private Vector3 targetPosition;
    private Vector3 direction;
    private string currentState;

    //亡语效果
    public GameObject killEffect;
    public GameObject impoisonEffect;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Ghost;
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
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
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
    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    targetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { targetPosition = SubsititueTarget.transform.position; }
                }
                else { targetPosition = InfatuationForDistanceEmpty().transform.position; }
                if (!isFearDone)
                {
                    direction = (targetPosition - transform.position).normalized;
                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    if(direction.x > 0)
                    {
                        if (direction.y > 0) ChangeAnimationState("MisdreavusMoveNE");
                        else ChangeAnimationState("MisdreavusMoveSE");
                    }
                    else
                    {
                        if(direction.y > 0) ChangeAnimationState("MisdreavusMoveNW");
                        else ChangeAnimationState("MisdreavusMoveSW");
                    }
                }
            }
        }
    }

    void ChangeAnimationState(string newState)
    {   //动画管理
        if (!isHit)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            animator.Play(newState);
        }
    }

    private void OnDestroy()
    {
        GameObject killeffect = Instantiate(killEffect, transform.position, Quaternion.identity);
        Destroy(killeffect, 1f);

        GameObject impoison = Instantiate(impoisonEffect, player.transform.position, Quaternion.identity);
        Destroy(impoison, 20f);
    }
}
