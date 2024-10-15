using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castform : Empty
{
    Vector2 TargetPosition;

    int AnimatorStateChageIndex;
    bool isInChangeAnimation;
    bool isAngry;

    float StateTimer;
    float WeatherChangePer;
    float WeatherChangeTimer;
    float LunchCDTimer;
    float LunchCD;
    float SkillCDTimer;
    float SkillPer;


    bool isInSkill;
    bool isNormalState;
    public WeatherBall WeatherBallNormal;


    bool isSunnyState;
    public WeatherBall WeatherBallFire;
    public SolarBeamManger SolarBeam;
    float SolarBeamRotation01;
    float SolarBeamRotation02;
    float SolarBeamRotation03;
    public FireBlastEmptyEmpty FireBlast;


    bool isRainState;
    public WeatherBall WeatherBallWater;
    public HydroPumpEmptyManger HydroPump;
    public ThunderCastformManger Thunder01;
    public ThunderCastform3LineManger Thunder02;
    public ThunderCastformCircleManger Thunder03;


    bool isHailState;
    public WeatherBall WeatherBallIce;
    public BlizzardCastform Blizzard;
    public CastformShadow Shadow;
    CastformShadow ShadowRight;
    CastformShadow ShadowLeft;




    Vector2 Director;
    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Normal;
        EmptyType02 = PokemonType.TypeEnum.No;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint) * BossAtkBouns;
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint) * BossAtkBouns;
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * BossDefBouns;
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * BossDefBouns;
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator.SetFloat("isHail", -1);
        animator.SetFloat("isRain", 0);
        isNormalState = true;
        AnimatorStateChageIndex = 0;
        SkillPer = 0f;


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

            if ((isSleepDone || isFearDone) && isInSkill)
            {
                isInSkill = false;
                animator.SetTrigger("AttackOver");
                animator.SetTrigger("Sleep");
            }

            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis )
            {
                //获取目标
                if (!isSilence) {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }

                //没有释放技能时计时器增加，并且检查当前天气
                if (!isInSkill)
                {
                    StateTimer += Time.deltaTime;
                    WeatherChangeTimer += Time.deltaTime;
                    LunchCDTimer += Time.deltaTime;
                    CheckWeather();
                }

                if (!isFearDone) {
                    //进入二阶段
                    
                    if (!isAngry && EmptyHp <= maxHP * 0.5f && !isEmptyFrozenDone && !isFearDone)
                    {
                        GameObject AngryEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(2);
                        Instantiate(AngryEffect, transform.position + Vector3.up * 1.5f, Quaternion.identity).SetActive(true);
                        isAngry = true;
                        if (SkillPer < 0) { SkillPer = 0.25f; }
                    }

                    //改变天气
                    if (!isNormalState) {
                        if (StateTimer >= 10) { WeatherChangePer = (1 - Mathf.Sqrt(1.5f - (StateTimer) / 20.0f)) * (isAngry ? 2 : 1); }
                        else { WeatherChangePer = 0; }
                    }
                    else
                    {
                        if (isAngry)
                        {
                            WeatherChangePer = 0.8f;
                        }
                        else
                        {
                            if (StateTimer >= 2) { WeatherChangePer = (StateTimer - 2) * 0.4f; }
                            else { WeatherChangePer = 0; }
                        }
                    }
                    if (WeatherChangeTimer > 1)
                    {
                        WeatherChangeTimer = 0;
                        if (!isInChangeAnimation && !isInSkill && Random.Range(0.0f, 1.0f) < WeatherChangePer)
                        {
                            isInSkill = true;
                            StateTimer = 0;
                            ChangeWeather();
                        }
                    }


                    //释放技能
                    if (!isInChangeAnimation) {
                        //释放天气球
                        if (!isSilence) {
                            if ((TargetPosition - (Vector2)transform.position).magnitude > ((isAngry) ? (0) : (5f)))
                            {
                                LunchCD = Mathf.Clamp(1.8f - ((TargetPosition - (Vector2)transform.position).magnitude - 5.5f) / 10, 0.6f, 1.8f);
                                if (LunchCDTimer > LunchCD)
                                {
                                    LunchCDTimer = 0;
                                    LunchWeatherBall((TargetPosition - (Vector2)transform.position).normalized * 0.6f + Vector2.up * 0.5f, (TargetPosition - (Vector2)transform.position).normalized, (Mathf.Clamp((TargetPosition - (Vector2)transform.position).magnitude * 1.2f, 6.5f, 11.5f)) * (isInSkill ? 0.75f : 1.0f), ((TargetPosition - (Vector2)transform.position).magnitude > (isAngry? 9f : 10f)));
                                }
                            }
                        }
                        //释放其他技能
                        if (!isInSkill && !isNormalState) {
                            SkillCDTimer += Time.deltaTime;
                            if (SkillCDTimer > ((isAngry) ? 1f : 1.5f))
                            {
                                SkillCDTimer = 0;
                                if (Random.Range(0.0f, 1.0f) < SkillPer)
                                {
                                    isInSkill = true;
                                    if (isSunnyState)
                                    {
                                        if (isAngry) { Invoke("DecideSolarBeamRotation", 10); }
                                        SkillPer = -0.25f;
                                        animator.SetTrigger("Attack");
                                        Invoke("LunchFireBlast", 0.4f);
                                        Invoke("AttackAnimationOver", 15);
                                        StateTimer += 5.0f;
                                    }
                                    else if (isRainState)
                                    {
                                        if (isAngry) { Invoke("LunchThunder", 4); }
                                        SkillPer = 0;
                                        animator.SetTrigger("Attack");
                                        Invoke("LunchHydroPump", 0.4f);
                                        Invoke("AttackAnimationOver", 6);
                                        StateTimer += 5.0f;
                                    }
                                    else if (isHailState)
                                    {
                                        SkillPer = -0.25f;
                                        StateTimer += 10.0f;
                                        if (!isAngry) {
                                            animator.SetTrigger("Attack");
                                            if (!isSleepDone && !isFearDone)
                                            {
                                                Instantiate(Blizzard, transform.position, Quaternion.identity).empty = this;
                                            }
                                            Invoke("AttackAnimationOver", 15);
                                        }
                                        else
                                        {
                                            animator.SetTrigger("DoubleTeam");
                                            Invoke("Lunch3Blizzard", 5f);
                                            Invoke("AttackAnimationOver", 17);
                                        }
                                    }
                                }
                                else { SkillPer += ((isAngry) ? 0.35f : 0.25f); }
                            }
                        }
                    }
                }
            }
        }
        if (isDie && !Weather.GlobalWeather.isNormal) { Weather.GlobalWeather.ChangeWeatherNormal();  }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn)
        {
            
            

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
    }

    

    void CheckWeather()
    {
        if (!isInChangeAnimation) {
            if ((Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus))
            {
                if (!isSunnyState)
                {
                    animator.SetTrigger("ChangeState");
                    AnimatorStateChageIndex = 1;
                    isInChangeAnimation = true;
                }
            }
            else if ((Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus))
            {
                if (!isRainState)
                {
                    animator.SetTrigger("ChangeState");
                    AnimatorStateChageIndex = 2;
                    isInChangeAnimation = true;
                }
            }
            else if ((Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus))
            {
                if (!isHailState)
                {
                    animator.SetTrigger("ChangeState");
                    AnimatorStateChageIndex = 3;
                    isInChangeAnimation = true;
                }
            }
            else
            {
                if (!isNormalState)
                {
                    animator.SetTrigger("ChangeState");
                    AnimatorStateChageIndex = 0;
                    isInChangeAnimation = true;
                }
            }
        }
    }

    void LunchWeatherBall(Vector3 LunchStartPosition , Vector3 LunchDirection , float LunchSpeed , bool is3)
    {
        if (!isSleepDone && !isFearDone && !isSilence)
        {
            if (isEmptyConfusionDone) { LunchDirection = Quaternion.AngleAxis( Random.Range(-30,30) , Vector3.forward) * LunchDirection; }
            WeatherBall LunchBall = null;
            if (isNormalState) { LunchBall = WeatherBallNormal; }
            else if (isSunnyState) { LunchBall = WeatherBallFire; }
            else if (isRainState) { LunchBall = WeatherBallWater; }
            else if (isHailState) { LunchBall = WeatherBallIce; }
            WeatherBall b = Instantiate(LunchBall, LunchDirection * 0.6f + Vector3.up * 0.5f + transform.position , Quaternion.identity);
            b.empty = this;
            b.LaunchNotForce(LunchDirection, LunchSpeed);

            if (is3)
            {
                WeatherBall b1 = Instantiate(LunchBall, Quaternion.AngleAxis(25, Vector3.forward) * LunchDirection * 0.6f + Vector3.up * 0.5f + transform.position, Quaternion.identity);
                b1.empty = this;
                b1.LaunchNotForce(Quaternion.AngleAxis(18, Vector3.forward) * LunchDirection, LunchSpeed);
                WeatherBall b2 = Instantiate(LunchBall, Quaternion.AngleAxis(-25, Vector3.forward) * LunchDirection * 0.6f + Vector3.up * 0.5f + transform.position, Quaternion.identity);
                b2.empty = this;
                b2.LaunchNotForce(Quaternion.AngleAxis(-18, Vector3.forward) * LunchDirection, LunchSpeed);
            }
        }
    }

    void ChangeWeather()
    {
        isInSkill = true;
        int r = Random.Range(1, 4);
        while (AnimatorStateChageIndex == r) { r = Random.Range(1, 4); }
        AnimatorStateChageIndex = r;
        animator.SetTrigger("Attack");
        switch (AnimatorStateChageIndex)
        {
            case 1:
                Weather.GlobalWeather.ChangeWeatherSunshine(100, false);
                break;
            case 2:
                Weather.GlobalWeather.ChangeWeatherRain(100, false);
                break;
            case 3:
                Weather.GlobalWeather.ChangeWeatherHail(100, false);
                break;
        }
        Invoke("AttackAnimationOver", 1.0f);
    }

    public void AttackAnimationOver()
    {
        animator.SetTrigger("AttackOver");
        isInSkill = false;
    }

    public void ChangeState()
    {
        switch (AnimatorStateChageIndex)
        {
            case 0:
                ChangeToNormalState();
                break;
            case 1:
                ChangeToSunnyState();
                break;
            case 2:
                ChangeToRainState();
                break;
            case 3:
                ChangeToHailState();
                break;
        }

    }

    public void CallChangeAnimationOver()
    {
        isInChangeAnimation = false;
        WeatherChangeTimer = 0;
        StateTimer = 0;
    }

    void ChangeToNormalState()
    {
        isNormalState = true;
        isSunnyState = false;
        isRainState = false;
        isHailState = false;

        EmptyType01 = PokemonType.TypeEnum.Normal;
        EmptyType02 = PokemonType.TypeEnum.No;

        isBurnDef = false;
        isFrozenDef = false;

        animator.SetFloat("isHail", -1);
        animator.SetFloat("isRain", 0);
    }

    void ChangeToSunnyState()
    {
        isNormalState = false;
        isSunnyState = true;
        isRainState = false;
        isHailState = false;

        EmptyType01 = PokemonType.TypeEnum.Fire;
        EmptyType02 = PokemonType.TypeEnum.No;
        isBurnDef = true;
        isFrozenDef = false;

        animator.SetFloat("isHail", 0);
        animator.SetFloat("isRain", -1);

        BurnRemove();
    }

    void ChangeToRainState()
    {
        isNormalState = false;
        isSunnyState = false;
        isRainState = true;
        isHailState = false;

        EmptyType01 = PokemonType.TypeEnum.Water;
        EmptyType02 = PokemonType.TypeEnum.No;

        isBurnDef = false;
        isFrozenDef = false;

        animator.SetFloat("isHail", 0);
        animator.SetFloat("isRain", 1);
    }

    void ChangeToHailState()
    {
        isNormalState = false;
        isSunnyState = false;
        isRainState = false;
        isHailState = true;

        EmptyType01 = PokemonType.TypeEnum.Ice;
        EmptyType02 = PokemonType.TypeEnum.No;
        isBurnDef = false;
        isFrozenDef = true;

        animator.SetFloat("isHail", 1);
        animator.SetFloat("isRain", 0);

        FrozenRemove();
    }

    public void BornShadow01()
    {
        ShadowRight = Instantiate(Shadow , transform.position + Vector3.right * 7 , Quaternion.identity);
        ShadowRight.ParentEmpty = this;
    }

    public void BornShadow02()
    {
        ShadowLeft = Instantiate(Shadow, transform.position - Vector3.right * 7, Quaternion.identity);
        ShadowLeft.ParentEmpty = this;
    }

    void DecideSolarBeamRotation()
    {
        SolarBeamRotation01 = _mTool.Angle_360Y(((Vector3)TargetPosition - transform.position).normalized, Vector3.right);
        Invoke("LunchSolarBeam" , 0.2f);
    }

    void LunchSolarBeam()
    {
        SolarBeamRotation02 = _mTool.Angle_360Y(((Vector3)TargetPosition - transform.position).normalized, Vector3.right);
        SolarBeamRotation03 = SolarBeamRotation02 + (SolarBeamRotation02 - SolarBeamRotation01);
        if (isSilence) { SolarBeamRotation03 = 0; }
        if (isEmptyConfusionDone) { SolarBeamRotation03 = Random.Range(0,360); }
        if (!isSleepDone && !isFearDone) {
            SolarBeamManger s = Instantiate(SolarBeam, transform.position, Quaternion.identity);
            s.Rotation = SolarBeamRotation03;
            s.ParentEmpty = this;
        }
    }

    void LunchFireBlast()
    {
        if (!isSleepDone && !isFearDone)
        {
            Instantiate(FireBlast, transform.position, Quaternion.identity).ParentEmpty = this;
        }
    }

    void LunchHydroPump()
    {
        if (!isSleepDone && !isFearDone)
        {
            HydroPumpEmptyManger h = Instantiate(HydroPump, transform.position, Quaternion.identity);
            h.ParentEmpty = this;
            h.RotationSpeed = (isAngry ? 18 : 30);
        }
    }

    void LunchThunder()
    {
        if (!isSleepDone && !isFearDone) {
            switch (Random.Range(0, 3))
            {
                case 0:
                    Instantiate(Thunder01, transform.position, Quaternion.identity).ParentEmpty = this;
                    break;
                case 1:
                    Instantiate(Thunder02, transform.position, Quaternion.identity).ParentEmpty = this;
                    break;
                case 2:
                    Instantiate(Thunder03, transform.position, Quaternion.identity).ParentEmpty = this;
                    break;
            }
        }
    }

    void Lunch3Blizzard()
    {
        if (!isSleepDone && !isFearDone)
        {
            BlizzardCastform b = Instantiate(Blizzard, transform.position, Quaternion.identity);
            b.empty = this;
            b.isDoubleTeam = true;
            if (ShadowRight != null)
            {
                BlizzardCastform bR = Instantiate(Blizzard, transform.position + 7 * Vector3.right, Quaternion.identity);
                bR.empty = this;
                bR.isDoubleTeam = true;
                ShadowRight.Blizzard = bR;
            }
            if (ShadowLeft != null)
            {
                BlizzardCastform bL = Instantiate(Blizzard, transform.position + 7 * Vector3.left, Quaternion.identity);
                bL.empty = this;
                bL.isDoubleTeam = true;
                ShadowLeft.Blizzard = bL;
            }
        }
    }



    public void CallInvisibleTrue()
    {
        Invincible = true;
    }

    public void CallInvisibleFalse()
    {
        Invincible = false;
    }


}