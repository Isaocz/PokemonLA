using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bronzong : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;
    bool isAngry;



    //状态
    public enum State
    {
        Normal,
        GyroBall,
        Idle,
        ST,
    }
    public State NowState;


    //眼睛相关
    public SpriteRenderer EyesSprite;
    bool isEyesLightUp;
    bool isEyesLightDown;

    //陀螺球的运动粒子
    ParticleSystem PS1;
    ParticleSystem PS2;
    bool isPSPlaying;


    //挡路相关
    public BlockBronzong BlockUD;
    public BlockBronzong BlockR;
    public BlockBronzong BlockL;
    public float BlockCDTime;
    float BlockCDTimer;


    //陀螺球相关
    bool isGyroBallInstan;
    float GyroBallTimer;
    Vector3 GyroBallStartPosition;
    float GyroBallRidus;
    float GyroBallRSpeed;
    float RDir;


    //跺脚相关
    public STBronzong ST;
    public float STCDTime;
    public bool isINSTMove;
    bool isINST;
    float STCDTimer;
    Vector2 STDir;
    public int STAtkUpCount;
    public MetalSoundBronzong MetalSound;


    //念力相关
    public float ConfusionCDTime;
    float ConfusionCDTimer;
    public ConfusionBronzong Confusion;
    public ConfusionBronzong ConfusionTrace;
    bool isSameTimeBlock;


    //Idle状态
    float IdleTimer;


    public GrassyTerrain PT;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Steel;
        EmptyType02 = Type.TypeEnum.Psychic;
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
        animator.SetFloat("LookX" , -1);
        animator.SetFloat("LookY" , -1);
        NowState = State.Normal;
        BlockCDTimer = BlockCDTime;
        ConfusionCDTimer = ConfusionCDTime;
        STCDTimer = STCDTime;
        STDir = Vector2.zero;
        isSameTimeBlock = false;
        IdleTimer = 0;

        PS1 = transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        PS2 = transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1).GetComponent<ParticleSystem>();

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
            if ( isEyesLightUp && EyesSprite.enabled ) { EyesSprite.color += new Color(0, 0, 0, 3.0f * Time.deltaTime); if (EyesSprite.color.a >= 1) { EyesSprite.color = new Color(1, 1, 1, 1); isEyesLightUp = false; } }
            if ( isEyesLightDown && EyesSprite.enabled ) { EyesSprite.color -= new Color(0, 0, 0, 3.0f * Time.deltaTime); if (EyesSprite.color.a <= 0) { EyesSprite.color = new Color(1, 1, 1, 0); isEyesLightDown = false; } }


            if (!isEmptyFrozenDone)
            {
                if (!isPSPlaying) { PlayAllPS(); }
                if (!isCanNotMoveWhenParalysis /* && !isSilence */) {
                    
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                    switch (NowState)
                    {
                        case State.Normal:
                            if (!isSleepDone) {
                                //进入跺脚
                                if (STCDTimer > 0 && !isSilence)
                                {
                                    STCDTimer -= Time.deltaTime;
                                    if (STCDTimer <= 0) { STCDTimer = 0; }
                                }
                                if (!isINST && STCDTimer == 0 && ((Vector2)transform.position - TargetPosition).magnitude <= 4.5f)
                                {
                                    isINST = true;
                                    animator.SetTrigger("Bulldoze");
                                }
                                //跺脚期间移动
                                if (isINSTMove)
                                {
                                    if (STDir == Vector2.zero) { STDir = (isFearDone ? -1.0f : 1.0f) * (Quaternion.AngleAxis( (isEmptyConfusionDone)? Random.Range(-30.0f , 30.0f) : 0 , Vector3.forward) * (TargetPosition - (Vector2)(transform.position + Vector3.down * 1.3f)).normalized); }
                                    Debug.Log("xxx");
                                    rigidbody2D.MovePosition(new Vector2(Mathf.Clamp(transform.position.x + STDir.x * Time.deltaTime * speed * 5.5f, transform.parent.position.x - 12.5f, transform.parent.position.x + 12.5f), Mathf.Clamp(transform.position.y + STDir.y * Time.deltaTime * speed * 5.5f, transform.parent.position.y - 6.5f, transform.parent.position.y + 6.5f)));
                                }
                                //仅在跺脚冷却期间能使用挡路念力
                                if (!isINST) {
                                    //移动
                                    if (!isSilence) {
                                        Vector2 Dir = (isFearDone ? -1.5f : 1.0f) * ((TargetPosition - (Vector2)transform.position).normalized);
                                        animator.SetFloat("LookX", (Dir.x > 0) ? 1 : -1);
                                        animator.SetFloat("LookY", (Dir.y > 0) ? 1 : -1);
                                        rigidbody2D.MovePosition(new Vector2(transform.position.x + Dir.x * Time.deltaTime * speed * (isAngry ? 1.2f : 0.6f), transform.position.y + Dir.y * Time.deltaTime * speed * (isAngry ? 1.2f : 0.6f)));
                                    }

                                    if (!isFearDone) {
                                        //挡路
                                        if (BlockCDTimer > 0 && !isSilence)
                                        {
                                            BlockCDTimer -= Time.deltaTime;
                                            if (BlockCDTimer <= 0) { BlockCDTimer = 0; }
                                        }
                                        if (BlockCDTimer == 0)
                                        {
                                            InstantiateBlock();
                                        }

                                        //念力
                                        if (ConfusionCDTimer > 0 && !isSilence)
                                        {
                                            ConfusionCDTimer -= Time.deltaTime;
                                            if (ConfusionCDTimer <= 0) { ConfusionCDTimer = 0; }
                                        }
                                        if (ConfusionCDTimer == 0)
                                        {
                                            ConfusionCDTimer = ConfusionCDTime;
                                            int CR = (int)((isEmptyConfusionDone) ? Random.Range(-30.0f, 30.0f) : 0);
                                            if (!isSameTimeBlock) {
                                                ConfusionBronzong c1 = Instantiate(Confusion, transform.position, Quaternion.Euler(0, 0, CR + _mTool.Angle_360Y(TargetPosition - (Vector2)transform.position, new Vector2(1, 0))));
                                                c1.LaunchNotForce(Quaternion.AngleAxis(CR, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized, 0);
                                                c1.empty = this;
                                                ConfusionBronzong c2 = Instantiate(Confusion, transform.position, Quaternion.Euler(0, 0, CR + -20 + _mTool.Angle_360Y(TargetPosition - (Vector2)transform.position, new Vector2(1, 0))));
                                                c2.LaunchNotForce(Quaternion.AngleAxis(CR - 20, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized, 0);
                                                c2.empty = this;
                                                ConfusionBronzong c3 = Instantiate(Confusion, transform.position, Quaternion.Euler(0, 0, CR + 20 + _mTool.Angle_360Y(TargetPosition - (Vector2)transform.position, new Vector2(1, 0))));
                                                c3.LaunchNotForce(Quaternion.AngleAxis(CR + 20, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized, 0);
                                                c3.empty = this;
                                            }
                                            else
                                            {
                                                ConfusionBronzong c1 = Instantiate(ConfusionTrace, transform.position, Quaternion.Euler(0, 0, CR + _mTool.Angle_360Y(TargetPosition - (Vector2)transform.position, new Vector2(1, 0))));
                                                c1.LaunchNotForce(Quaternion.AngleAxis(CR, Vector3.forward) * (TargetPosition - (Vector2)transform.position).normalized, 0);
                                                c1.empty = this;
                                            }
                                            if (isSameTimeBlock) { isSameTimeBlock = false; }
                                            else { isSameTimeBlock = true; }
                                        };
                                    }
                                }
                                if (!isAngry && EmptyHp <= maxHP * 0.95f)
                                {
                                    isAngry = true;
                                    Instantiate(PT, transform.position - 1.3f * Vector3.down, Quaternion.identity);
                                }
                            }
                            break;
                        case State.GyroBall:
                            if (!isSleepDone) {
                                if (!isGyroBallInstan)
                                {
                                    isGyroBallInstan = true;
                                    animator.SetTrigger("GyroBall");
                                    GyroBallStartPosition = transform.position;
                                    GyroBallRidus = 0;
                                    GyroBallRSpeed = 360.0f ;
                                    RDir = (Random.Range(0.0f, 1.0f) > 0.5f) ? -1 : 1;
                                }
                                GyroBallTimer += RDir * GyroBallRSpeed * (isEmptyConfusionDone ? 0.5f : 1.0f) * Time.deltaTime;
                                GyroBallRidus += 1.8f * Time.deltaTime;
                                animator.SetFloat("LookX", (Mathf.Cos(Mathf.Deg2Rad * (GyroBallTimer + 90)) > 0) ? 1 : -1);
                                animator.SetFloat("LookY", (Mathf.Sin(Mathf.Deg2Rad * (GyroBallTimer + 90)) > 0) ? 1 : -1);
                                rigidbody2D.MovePosition(new Vector2(Mathf.Clamp(GyroBallStartPosition.x + GyroBallRidus * Mathf.Cos(Mathf.Deg2Rad * (GyroBallTimer)), transform.parent.position.x - 11.35f, transform.parent.position.x + 11.35f), Mathf.Clamp(GyroBallStartPosition.y + GyroBallRidus * Mathf.Sin(Mathf.Deg2Rad * (GyroBallTimer)), transform.parent.position.y - 5.5f, transform.parent.position.y + 7.5f)));
                                if (GyroBallRidus >= 18.0f) { GyroBallRSpeed -= Time.deltaTime * 180; }
                                if (GyroBallRSpeed <= 5) {
                                    NowState = State.Idle;
                                    IdleTimer = 4.0f;
                                    isGyroBallInstan = false;
                                    animator.SetTrigger("GyroBallOver");
                                }
                            }
                            else
                            {
                                NowState = State.Normal;
                                isGyroBallInstan = false;
                                animator.SetTrigger("GyroBallOver");
                            }
                            break;
                        case State.Idle:
                            if (isSleepDone) {
                                IdleTimer -= Time.deltaTime;
                                if (IdleTimer <= 0 && !isSilence) {
                                    IdleTimer = 0;
                                    BlockCDTimer = BlockCDTime;
                                    ConfusionCDTimer = ConfusionCDTime;
                                    STCDTimer = STCDTime;
                                    isINST = false;
                                    isINSTMove = false;
                                    STDir = Vector2.zero;
                                    isSameTimeBlock = false;
                                    NowState = State.Normal;
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if (isPSPlaying) { PauseAllPS(); }
            }
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            TargetPosition = player.transform.position;



            


        }
    }


    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            if (NowState == State.GyroBall)
            {
                PlayerControler p = other.gameObject.GetComponent<PlayerControler>();
                if ( p != null) {
                    Debug.Log(SpeedAbilityPoint.ToString() + "+" + p.SpeedAbilityPoint.ToString());
                    Pokemon.PokemonHpChange( this.gameObject , p.gameObject , Mathf.Clamp(((25 * p.SpeedAbilityPoint) / SpeedAbilityPoint) , 1 , 150) , 0 , 0 , Type.TypeEnum.Steel );
                    p.KnockOutPoint = Knock;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                }
                else 
                {
                    if (other.gameObject.GetComponent<Substitute>() != null) Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 120 , 0, 0, Type.TypeEnum.Steel);
                }

            }
            else
            {
                EmptyTouchHit(other.gameObject);
            }
        }
    }


    /// <summary>
    /// 生成拦路墙
    /// </summary>
    void InstantiateBlock()
    {
        BlockCDTimer = BlockCDTime;
        EyesLightUp();

        Vector2 p1 = TargetPosition;
        Timer.Start(this , 0.1f , () => {
            Vector2 p = (TargetPosition-p1).normalized;
            float a = _mTool.Angle_360Y(p, Vector3.right);
            int BlockCase = Random.Range(0, 3);
            p = p + ((isEmptyConfusionDone) ? new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f)) : Vector2.zero);
            switch (BlockCase)
            {
                case 0: //大墙型
                    if (a >= 45 && a <= 135)       { Instantiate(BlockUD, TargetPosition + p  + Vector2.right*1.5f, Quaternion.identity);    Instantiate(BlockUD, TargetPosition + p - Vector2.right * 1.5f, Quaternion.identity); }  //玩家向上移动时
                    else if (a >= 135 && a <= 215) { Instantiate(BlockL, TargetPosition + p + Vector2.up * 1.5f, Quaternion.identity);      Instantiate(BlockL, TargetPosition + p - Vector2.up * 1.5f, Quaternion.identity); }   //玩家向左移动时
                    else if (a >= 215 && a <= 305) { Instantiate(BlockUD, TargetPosition + p + Vector2.right * 1.5f, Quaternion.identity);  Instantiate(BlockUD, TargetPosition + p - Vector2.right * 1.5f, Quaternion.identity); }  //玩家向下移动时
                    else                           { Instantiate(BlockR, TargetPosition + p + Vector2.up * 1.5f, Quaternion.identity);      Instantiate(BlockR, TargetPosition + p - Vector2.up * 1.5f, Quaternion.identity); }   //玩家向右移动时
                    break;
                case 1: //口袋型
                    if (a >= 45 && a <= 135)       { Instantiate(BlockUD, TargetPosition + p, Quaternion.identity);  Instantiate(BlockL, TargetPosition + p + Vector2.left * 1 + Vector2.down * 1, Quaternion.identity);    Instantiate(BlockR, TargetPosition + p + Vector2.right * 1 + Vector2.down * 1, Quaternion.identity); }  //玩家向上移动时
                    else if (a >= 135 && a <= 215) { Instantiate(BlockL, TargetPosition + p, Quaternion.identity);   Instantiate(BlockUD, TargetPosition + p + Vector2.right * 1f + Vector2.down * 1, Quaternion.identity);  Instantiate(BlockUD, TargetPosition + p + Vector2.right * 1 + Vector2.up * 1, Quaternion.identity); }   //玩家向左移动时
                    else if (a >= 215 && a <= 305) { Instantiate(BlockUD, TargetPosition + p, Quaternion.identity);  Instantiate(BlockL, TargetPosition + p + Vector2.left * 1 + Vector2.up * 1, Quaternion.identity);      Instantiate(BlockR, TargetPosition + p + Vector2.right * 1 + Vector2.up * 1, Quaternion.identity); }  //玩家向下移动时
                    else                           { Instantiate(BlockR, TargetPosition + p, Quaternion.identity);   Instantiate(BlockUD, TargetPosition + p + Vector2.left * 1 + Vector2.down * 1, Quaternion.identity);   Instantiate(BlockUD, TargetPosition + p + Vector2.left * 1 + Vector2.up * 1, Quaternion.identity); }   //玩家向右移动时
                    break;
                case 2: //夹击型
                    if (a >= 45 && a <= 135)       { Instantiate(BlockUD, TargetPosition + p, Quaternion.identity);  Instantiate(BlockUD, TargetPosition + p + Vector2.down * 2, Quaternion.identity); }  //玩家向上移动时
                    else if (a >= 135 && a <= 215) { Instantiate(BlockL, TargetPosition + p, Quaternion.identity);   Instantiate(BlockR, TargetPosition + p + Vector2.right * 2, Quaternion.identity); }   //玩家向左移动时
                    else if (a >= 215 && a <= 305) { Instantiate(BlockUD, TargetPosition + p, Quaternion.identity);  Instantiate(BlockUD, TargetPosition + p + Vector2.up * 2, Quaternion.identity); }  //玩家向下移动时
                    else                           { Instantiate(BlockR, TargetPosition + p, Quaternion.identity);   Instantiate(BlockL, TargetPosition + p + Vector2.left * 2, Quaternion.identity); }   //玩家向右移动时
                    break;
            }  
        });
        Timer.Start(this, 2.0f, () => { EyesLightDown(); });
    }


    /// <summary>
    /// 睁眼
    /// </summary>
    void EyesLightUp()
    {
        if (!isEyesLightUp && !isEyesLightDown && EyesSprite.color.a == 0.0f )
        {
            isEyesLightUp = true;
        }
    }


    /// <summary>
    /// 闭眼
    /// </summary>
    void EyesLightDown()
    {
        if (!isEyesLightUp && !isEyesLightDown && EyesSprite.color.a == 1.0f)
        {
            isEyesLightDown = true;
        }
    }


    public void BornSTPS()
    {
        Instantiate(ST , transform.position + Vector3.down * 1.3f , Quaternion.identity).empty = this;
        if (isAngry)
        {
            Instantiate(MetalSound, transform.position + Vector3.down * 1.3f, Quaternion.identity);
        }
    } 

    public void STOver()
    {
        BlockCDTimer = BlockCDTime;
        ConfusionCDTimer = ConfusionCDTime;
        STCDTimer = STCDTime;
        isINST = false;
        isINSTMove = false;
        STDir = Vector2.zero;
        isSameTimeBlock = false;

        if (STAtkUpCount >= 2 && !isSilence && !isFearDone)
        {
            NowState = State.GyroBall;
        }
    }


    /// <summary>
    /// Play所有运动粒子
    /// </summary>
    public void PlayAllPS()
    {
        PS1.Play();
        PS2.Play();
        isPSPlaying = true;
        var E1 = PS1.emission;
        E1.enabled = true;
        var E2 = PS2.emission;
        E2.enabled = true;
    }

    /// <summary>
    /// 暂停发射所有运动粒子
    /// </summary>
    public void PauseAllPS()
    {
        isPSPlaying = false;
        var E1 = PS1.emission;
        E1.enabled = false;
        var E2 = PS2.emission;
        E2.enabled = false;
    }


}
