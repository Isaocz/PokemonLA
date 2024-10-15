using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Misdreavus : Empty
{
    private Vector3 targetPosition;
    private Vector3 direction;
    private string currentState;

    Vector2 LastPosition;




    public bool IsKilledBySkill
    {
        get { return isKilledBySkill; }
        set { isKilledBySkill = value; }
    }
    bool isKilledBySkill;

    //亡语效果
    public GameObject killEffect;
    public GameObject impoisonEffect;

    /// <summary>
    /// 被封印的技能序号
    /// </summary>
    public int ImpoisonSkillIndex;
    public float ImpoisonTime = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;
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

        LastPosition = transform.position;
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
                    if ((targetPosition - transform.position).magnitude <= 1.0f)
                    {
                        speed = 0.5f;
                    }
                    else if ((targetPosition - transform.position).magnitude > 1.0f && (targetPosition - transform.position).magnitude <= 3.0f)
                    {
                        speed = 2.0f;
                    }
                    else
                    {
                        speed = 4.0f;
                    }
                    direction = (targetPosition - transform.position).normalized;
                    direction = (Quaternion.AngleAxis(isEmptyConfusionDone ? 30 : 0, Vector3.forward) * direction).normalized;
                    animator.SetFloat("LookX", (direction.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (direction.y >= 0 ? 1 : -1));
                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));

                }
                else
                {
                    speed = 6.0f;
                    direction = (targetPosition - transform.position).normalized;
                    direction = (Quaternion.AngleAxis(isEmptyConfusionDone ? 150 : 180, Vector3.forward) * direction).normalized;
                    animator.SetFloat("LookX", (direction.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (direction.y >= 0 ? 1 : -1));
                    if ((targetPosition - transform.position).magnitude >= 2.5f)
                    {
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    }
                }

                /*
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
                */
            }
            animator.SetFloat("Speed", ((Vector2)transform.position - LastPosition).magnitude);
            LastPosition = transform.position;
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


    public void Impoison()
    {
        if (IsDeadrattle && IsKilledBySkill) {
            switch (ImpoisonSkillIndex)
            {
                case 1:
                    player.Is01imprison = true;
                    player.imprisonTime01 = ImpoisonTime;
                    break;
                case 2:
                    player.Is02imprison = true;
                    player.imprisonTime02 = ImpoisonTime;
                    break;
                case 3:
                    player.Is03imprison = true;
                    player.imprisonTime03 = ImpoisonTime;
                    break;
                case 4:
                    player.Is04imprison = true;
                    player.imprisonTime04 = ImpoisonTime;
                    break;
            }
            Invoke("BornImpoison", 0.5f);
        }
    }

    void BornImpoison()
    {
        GameObject impoison = Instantiate(impoisonEffect, player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0], Quaternion.identity);
        impoison.GetComponent<impoisonEffect>().player = player;
        Destroy(impoison, 20f);
    }

    public void Killeffect()
    {
        GameObject killeffect = Instantiate(killEffect, transform.position, Quaternion.identity);
        Destroy(killeffect, 1f);
    }
}
