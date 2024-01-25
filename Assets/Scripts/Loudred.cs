using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loudred : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;

    public LoudredInhale Inhale;
    LoudredInhale NowInhale;

    public LoudredHyperVoice HyperVoice;
    LoudredHyperVoice NowHyperVoice;

    float StateTimer;

    enum State
    {
        Idle,
        Inhale,
        HyperVoice,
    }
    State NowState;

    bool isPSPlaying;
    bool isSleepPause;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Normal;
        EmptyType02 = Type.TypeEnum.No;
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
        if (!isDie && !isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();

            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence && !isFearDone )
            {
                if (!isPSPlaying) { isPSPlaying = true; PlayInhale(); PlayHyperVoice(); }
                if (isSleepPause) { isSleepPause = false; }
                if (NowState == State.Idle)
                {
                    if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(7) == null)
                    {
                        TargetPosition = player.transform.position;
                        if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                    }
                    else { TargetPosition = InfatuationForRangeRayCastEmpty(7).transform.position; }
                    Director = new Vector2(((TargetPosition - (Vector2)transform.position).normalized.x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1);
                    animator.SetFloat("LookX", Director.x);
                    animator.SetFloat("LookY", Director.y);
                    
                }
                StateTimer += Time.deltaTime;
                if ( StateTimer >= 6.0f && NowState == State.Idle )
                {
                    NowState = State.Inhale;
                    animator.SetTrigger("Inhale");
                    NowInhale = Instantiate(Inhale, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director,Vector2.right) - 45 ),transform);
                    NowInhale.ParentEmpty = this;
                }
                if (StateTimer >= 10.3f && NowState == State.Inhale)
                {
                    if (NowInhale) { Destroy(NowInhale.gameObject); }
                    NowState = State.HyperVoice;

                    if (!isEmptyConfusionDone) {
                        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(7) == null)
                        {
                            TargetPosition = player.transform.position;
                            if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                        }
                        else { TargetPosition = InfatuationForRangeRayCastEmpty(7).transform.position; }
                        Director = new Vector2(((TargetPosition - (Vector2)transform.position).normalized.x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1);
                        animator.SetFloat("LookX", Director.x);
                        animator.SetFloat("LookY", Director.y);
                    }
                    
                    animator.SetTrigger("Roar");
                    NowHyperVoice = Instantiate(HyperVoice, transform.position + Vector3.up * 0.3f, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Director, Vector2.right) - 45), transform);
                    NowHyperVoice.empty = this;
                    
                }
                if (StateTimer >= 12.70f && NowState == State.HyperVoice)
                {
                    if (NowHyperVoice) { Destroy(NowHyperVoice.gameObject); }
                    StateTimer = 0.0f;
                    NowState = State.Idle;
                    NowHyperVoice = null;
                    NowInhale = null;
                }
            }
            if (isCanNotMoveWhenParalysis || isEmptyFrozenDone || isSilence) { if (isPSPlaying) { isPSPlaying = false; PauseInhale(); PauseHyperVoice(); } }
            if ((isFearDone || isSleepDone) && !isSleepPause)
            {
                isSleepPause = true;
                if (NowHyperVoice) { Destroy(NowHyperVoice.gameObject); }
                if (NowInhale) { Destroy(NowInhale.gameObject); }
                StateTimer = 0.0f;
                NowState = State.Idle;
                animator.SetTrigger("Sleep");
            }
        }
        if (isDie)
        {
            if (NowHyperVoice) { Destroy(NowHyperVoice.gameObject); }
            if (NowInhale) { Destroy(NowInhale.gameObject); }
        }

    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();


        }
    }


    void PauseInhale()
    {
        if (NowInhale != null)
        {
            isPSPlaying = false;
            NowInhale.GetComponent<PolygonCollider2D>().enabled = false;

            for (int i = 0; i < NowInhale.transform.childCount; i++)
            {
                ParticleSystem p = NowInhale.transform.GetChild(i).GetComponent<ParticleSystem>();
                var e = p.emission;
                
                float t = p.main.startLifetime.constantMax;
                Timer.Start(this, t  , () => { isPSPlaying = false;if (p) { p.Pause(); } } );
                e.enabled = false;
            }
        }
    }

    void PlayInhale()
    {
        if (NowInhale != null)
        {
            isPSPlaying = true;
            NowInhale.GetComponent<PolygonCollider2D>().enabled = true;
            for (int i = 0; i < NowInhale.transform.childCount; i++)
            {
                var e = NowInhale.transform.GetChild(i).GetComponent<ParticleSystem>().emission;
                NowInhale.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                e.enabled = true;
            }
        }
    }

    void PauseHyperVoice()
    {
        if (NowHyperVoice != null)
        {
            isPSPlaying = false;
            for (int i = 0; i < NowHyperVoice.transform.childCount; i++)
            {
                ParticleSystem p = NowHyperVoice.transform.GetChild(i).GetComponent<ParticleSystem>();
                var e = p.emission;
                float t = p.main.startLifetime.constantMax;
                Timer.Start(this, t, () => { isPSPlaying = false;if (p) { p.Pause(); } });
                e.enabled = false;
            }
        }
    }

    void PlayHyperVoice()
    {
        if (NowHyperVoice != null)
        {
            isPSPlaying = true;
            for (int i = 0; i < NowHyperVoice.transform.childCount; i++)
            {
                var e = NowHyperVoice.transform.GetChild(i).GetComponent<ParticleSystem>().emission;
                NowHyperVoice.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                e.enabled = true;
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
}
