using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mawile : Empty
{
    public Vector2 Director;
    public Vector2 TargetPosition;

    public SweetScentMaeileManger SweetScent;
    float SweetScentTimer;
    public float SweetScentTime;

    public BiteMawile Bite;
    float BiteTimer;
    public float BiteTime;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Steel;
        EmptyType02 = PokemonType.TypeEnum.Fairy;
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
        animator.SetFloat("LookX" , Director.x);
        animator.SetFloat("LookY" , Director.y);
        SweetScentTimer = 0;



        StartOverEvent();
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

            if (!isEmptyFrozenDone && !isCanNotMoveWhenParalysis && !isSleepDone && !isSilence)
            {

                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(9) == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForRangeRayCastEmpty(9).transform.position; }


                if (SweetScentTimer > 0) { SweetScentTimer -= Time.deltaTime; if (SweetScentTimer <= 0) { SweetScentTimer = 0; } }
                if (SweetScentTimer == 0 && (TargetPosition - (Vector2)transform.position).magnitude <= 6.2f)
                {
                    SweetScentTimer = SweetScentTime;
                    Instantiate(SweetScent, transform.position, Quaternion.identity).ParentEmpty = this;
                }

                if (BiteTimer > 0) { BiteTimer -= Time.deltaTime; if (BiteTimer <= 0) { BiteTimer = 0; } }
                if (BiteTimer == 0 && (TargetPosition - (Vector2)transform.position).magnitude <= 3.3f && Vector2.Angle((TargetPosition - (Vector2)transform.position).normalized , Director) <= (isEmptyConfusionDone? 85.0f : 52.0f) && !isFearDone )
                {
                    BiteTimer = BiteTime;
                    animator.SetTrigger("Bite");
                    Instantiate(Bite, transform.position + (Vector3)Director * 1.75f, Quaternion.identity).empty = this;
                    //Instantiate(SweetScent, transform.position, Quaternion.identity).ParentEmpty = this;
                }
            }

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

    public void ResetDir()
    {
        Director = (new Vector2(((TargetPosition - (Vector2)transform.position).normalized.x > 0) ? 1 : -1, ((TargetPosition - (Vector2)transform.position).normalized.y > 0) ? 1 : -1));
        animator.SetFloat("LookX", Director.x);
        animator.SetFloat("LookY", Director.y);
    }
}
