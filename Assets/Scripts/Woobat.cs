using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woobat : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;

    float TurnTimer;

    CharmWoobat Charm;
    bool isCharmPlaying;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Psychic;
        EmptyType02 = Type.TypeEnum.Flying;
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
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
        Charm = transform.GetChild(4).GetComponent<CharmWoobat>();
        Charm.ParentEmpty = this;
        isCharmPlaying = true;
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
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence)
            {
                if (!isCharmPlaying) { PlayCharm(); }
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(15) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(15).transform.position; }
                Director = (new Vector2(((TargetPosition - (Vector2)transform.position).x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1));
                Director = Director.normalized;



                
                //获取玩家的移动方向
                Vector2 PlayerMoveSpeed = Vector2.zero;
                {

                    PlayerMoveSpeed = new Vector2(player.PlayerMoveHorizontal, player.PlayerMoveHorizontal);
                    
                    if (PlayerMoveSpeed != Vector2.zero)
                    {
                        PlayerMoveSpeed = ((PlayerMoveSpeed.normalized * Mathf.Max(Mathf.Abs(PlayerMoveSpeed.x), Mathf.Abs(PlayerMoveSpeed.y))).normalized) * 1.5f;
                    }
                }
                Director = (isEmptyConfusionDone || isEmptyInfatuationDone)? Director.normalized :( 1.5f*PlayerMoveSpeed + Director.normalized) / 2.5f;
                

                rigidbody2D.MovePosition(new Vector2(transform.position.x + (isFearDone ? -1.0f : 1.0f) * Mathf.Clamp( Director.x * speed , -3.2f, 3.2f) * Time.deltaTime, transform.position.y + (isFearDone ? -1.0f : 1.0f) * Mathf.Clamp(Director.y * speed, -3.2f, 3.2f) * Time.deltaTime));
                TurnTimer += Time.deltaTime;
                Director = (new Vector2((Director.x > 0) ? 1 : -1, (Director.normalized.y > 0) ? 1 : -1));
                if (TurnTimer >= 0.5f)
                {
                    TurnTimer = 0;
                    animator.SetFloat("LookX", (isFearDone ? -1.0f : 1.0f) * Director.x);
                    animator.SetFloat("LookY", (isFearDone ? -1.0f : 1.0f) * Director.y);
                }
            }
            else
            {
                if (isCharmPlaying) { PauseCharm(); }
            }

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


    void PauseCharm()
    {
        if (isCharmPlaying && Charm != null) {
            isCharmPlaying = false;
            Charm.GetComponent<Collider2D>().enabled = false;
            Charm.GetComponent<SpriteRenderer>().enabled = false;
            var e = Charm.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            e.enabled = false;
        }
    }

    void PlayCharm()
    {
        if (!isCharmPlaying && Charm != null)
        {
            isCharmPlaying = true;
            Charm.GetComponent<Collider2D>().enabled = true;
            Charm.GetComponent<SpriteRenderer>().enabled = true;
            var e = Charm.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            e.enabled = true;
        }
    }


}
