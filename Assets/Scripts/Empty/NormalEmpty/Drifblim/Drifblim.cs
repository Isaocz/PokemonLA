using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifblim : Empty
{
    /// <summary>
    /// 目标位置
    /// </summary>
    private Vector3 TargetPosition;

    public Vector3 InitialDirection;
    public DrifloonExplosion ExplosionMega;
    public DrifloonExplosion ExplosionSmall;
    public DrifblimShadowBall2 ShadowBallPrefab;


    private Vector3 direction;
    //private string currentState;

    //private Vector3 currentPosition;
    //private float timer;
    //private bool IsReflect;

    bool isAngry = false;
    bool isExploson = false;
    bool isExplosonOBJBorn = false;

    public static float SPEEDALPHA_ANGRY = 2.7f;

    public static float DISTENCE_EXPLOSON = 2.6f;

    public static float RADIUS_EXPLOSION_MEGA = 5.0f;
    public static float RADIUS_EXPLOSION_SMALL = 1.3f;


    //影子球速度
    static float SHADOWBALL_SPEED = 9.0f;
    static float TIME_CD_SHADOW = 1.0f;
    float Timer_CD_ShadowBall = 0.0f;


    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;
        EmptyType02 = PokemonType.TypeEnum.Flying;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = InitialDirection;

        //IsReflect = true;



        StartOverEvent();
    }



    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();


            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (Timer_CD_ShadowBall < TIME_CD_SHADOW)
                {
                    Timer_CD_ShadowBall += Time.deltaTime;
                    if (Timer_CD_ShadowBall >= TIME_CD_SHADOW) { Timer_CD_ShadowBall = TIME_CD_SHADOW; }
                }

                //生气加速
                if (!isAngry && EmptyHp <= maxHP / 2)
                {
                    isAngry = true;
                    AngryEffect(new Vector2(0.0f, 1.5f), this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), true);

                }

                if (!isFearDone)
                {
                    //Debug.Log(Vector2.Distance(TargetPosition, transform.position));
                    //近距离自爆
                    if (!isExploson && Vector2.Distance(TargetPosition, transform.position) <= DISTENCE_EXPLOSON)
                    {
                        EmptyEcplosionEvent();
                    }
                }
            }

            /*
            timer += Time.deltaTime;
            if(timer > 0.6f)
            {   //检测是否被卡住
                if(!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone && !isHit && !IsReflect && (!(Mathf.Abs(currentPosition.x - transform.position.x) > 0.8f && Mathf.Abs(currentPosition.y - transform.position.y) > 0.8f)))
                {
                    RaycastHit2D raycastHit2Da = Physics2D.Raycast(transform.position, new Vector2(1, 1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Db = Physics2D.Raycast(transform.position, new Vector2(1, -1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Dc = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Dd = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 20f, LayerMask.GetMask("Room"));
                    float maxDistance = Mathf.Max(raycastHit2Da.distance, raycastHit2Db.distance, raycastHit2Dc.distance, raycastHit2Dd.distance);
                    if (maxDistance == raycastHit2Da.distance)
                    {
                        direction = new Vector2(1, 1);
                    }
                    else if (maxDistance == raycastHit2Db.distance)
                    {
                        direction = new Vector2(1, -1);
                    }
                    else if (maxDistance == raycastHit2Dc.distance)
                    {
                        direction = new Vector2(-1, 1);
                    }
                    else
                    {
                        direction = new Vector2(-1, -1);
                    }
                }
                timer = 0f;
                IsReflect = false;
                currentPosition = transform.position;
            }
            */
        }

    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {

            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone)
            { animator.SetBool("Exploson", true); }
            else
            { animator.SetBool("Exploson", false); }
            //根据魅惑情况确实目标位置
            Transform InfatuationTarget = InfatuationForDistanceEmpty();
            if (!isEmptyInfatuationDone || (ParentPokemonRoom.GetEmptyList().Count + ParentPokemonRoom.GetEmptyCloneList().Count) <= 1 || InfatuationTarget == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationTarget.transform.position; }



            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isExploson)
            {


                //移动
                rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed * (isAngry ? SPEEDALPHA_ANGRY : 1.0f) * (isFearDone ? 1.6f : 1.0f), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed * (isAngry ? SPEEDALPHA_ANGRY : 1.0f) * (isFearDone ? 1.6f : 1.0f), -10f + transform.parent.position.y, 10f + transform.parent.position.y));

                /*
                if (direction.x > 0)
                {
                    if (direction.y > 0) ChangeAnimationState("DrifloonMoveNE");
                    else ChangeAnimationState("DrifloonMoveSE");
                }
                else
                {
                    if (direction.y > 0) ChangeAnimationState("DrifloonMoveNW");
                    else ChangeAnimationState("DrifloonMoveSW");
                }
                */
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && !isExploson && other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if (isEmptyInfatuationDone && !isExploson && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }

        /*
        if(other.transform.tag == "Room")
        {
            //反弹操作
            Vector2 newDirection = Vector2.Reflect(direction, other.GetContact(0).normal);
            Vector2[] comparisonValues = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1)
            };

            float minDistance = Mathf.Infinity;
            int closestIndex = -1;
            for (int i = 0; i < comparisonValues.Length; i++)
            {
                float distance = Vector2.SqrMagnitude(newDirection - comparisonValues[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            direction = comparisonValues[closestIndex];
            IsReflect = true;
        }
        */
    }


    //=================================碰壁反弹=======================================


    private void OnCollisionStay2D(Collision2D other)
    {
        Rebound_BeLunch();
    }


    /// <summary>
    /// 反弹限制计时器的时间上限
    /// </summary>
    static float TIME_REBOUND_CD = 0.1f;

    /// <summary>
    /// 反弹限制计时器 防止在一次碰撞中多次反弹
    /// </summary>
    float ReboundTimer;

    /// <summary>
    /// 被发射时发生反弹
    /// </summary>
    bool Rebound_BeLunch()
    {
        //当前时间
        float now = Time.time;

        if (now - ReboundTimer < TIME_REBOUND_CD) { return false; } // 防抖：0.1s 内忽略

        ReboundTimer = now;

        //碰撞方向
        Vector2 HitVector = Vector2.right;
        {
            Vector2 EmptyCenter = (Vector2)transform.position + GetComponent<Collider2D>().offset;
            float[] DistenceList = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };
            RaycastHit2D RPRay = Physics2D.Raycast(EmptyCenter, Vector2.right, LayerMask.GetMask("Room"));
            RaycastHit2D UPRay = Physics2D.Raycast(EmptyCenter, Vector2.up, LayerMask.GetMask("Room"));
            RaycastHit2D LPRay = Physics2D.Raycast(EmptyCenter, Vector2.left, LayerMask.GetMask("Room"));
            RaycastHit2D DPRay = Physics2D.Raycast(EmptyCenter, Vector2.down, LayerMask.GetMask("Room"));
            if (RPRay) { DistenceList[0] = RPRay.distance; Debug.DrawLine(EmptyCenter, RPRay.point, Color.red, 0.1f); }
            if (UPRay) { DistenceList[1] = UPRay.distance; Debug.DrawLine(EmptyCenter, UPRay.point, Color.red, 0.1f); }
            if (LPRay) { DistenceList[2] = LPRay.distance; Debug.DrawLine(EmptyCenter, LPRay.point, Color.red, 0.1f); }
            if (DPRay) { DistenceList[3] = DPRay.distance; Debug.DrawLine(EmptyCenter, DPRay.point, Color.red, 0.1f); }
            float MinDistence = DistenceList[0];
            int MinIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (MinDistence > DistenceList[i])
                {
                    MinDistence = DistenceList[i];
                    MinIndex = i;
                }
            }
            Debug.Log("R" + DistenceList[0] + "U" + DistenceList[1] + "L" + DistenceList[2] + "D" + DistenceList[3] + "+" + MinDistence);
            HitVector = _mTool.MainVector2(Quaternion.AngleAxis(MinIndex * 90.0f, Vector3.forward) * HitVector);
        }

        direction = new Vector2(direction.x * (HitVector.x == 0 ? 1 : -1), direction.y * (HitVector.y == 0 ? 1 : -1));
        if (isEmptyConfusionDone) { direction = ConfusionDir(direction, 30.0f); }
        SetDirector(direction);
        if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone)
        {
            LaunchShadowBall();
        }
        return true;

    }



    void LaunchShadowBall()
    {
        if (Timer_CD_ShadowBall >= TIME_CD_SHADOW ) {
            Timer_CD_ShadowBall = 0;
            float startp = 1.5f;
            List<Vector2> dir = new List<Vector2> { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1), };
            DrifblimShadowBall2 s0 = Instantiate(ShadowBallPrefab, ParentPokemonRoom.EnsurePointReachesRoom((Vector2)transform.position + startp * dir[0]), Quaternion.identity);
            DrifblimShadowBall2 s1 = Instantiate(ShadowBallPrefab, ParentPokemonRoom.EnsurePointReachesRoom((Vector2)transform.position + startp * dir[1]), Quaternion.identity);
            DrifblimShadowBall2 s2 = Instantiate(ShadowBallPrefab, ParentPokemonRoom.EnsurePointReachesRoom((Vector2)transform.position + startp * dir[2]), Quaternion.identity);
            DrifblimShadowBall2 s3 = Instantiate(ShadowBallPrefab, ParentPokemonRoom.EnsurePointReachesRoom((Vector2)transform.position + startp * dir[3]), Quaternion.identity);
            s0.empty = this; s0.LaunchNotForce(dir[0], SHADOWBALL_SPEED);
            s1.empty = this; s1.LaunchNotForce(dir[1], SHADOWBALL_SPEED);
            s2.empty = this; s2.LaunchNotForce(dir[2], SHADOWBALL_SPEED);
            s3.empty = this; s3.LaunchNotForce(dir[3], SHADOWBALL_SPEED);
        }
    }

    //=================================碰壁反弹=======================================


    /// <summary>
    /// 设置敌人的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        direction = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }


    public void Explosion()
    {
        if (IsDeadrattle && !isExplosonOBJBorn)
        {
            isExplosonOBJBorn = true;
            DrifloonExplosion boom = Instantiate(ExplosionMega, transform.position, Quaternion.identity);
            boom.empty = this;
            Destroy(boom.gameObject, 5f);
            StartCoroutine(InstantiateSmallExplosionManger(transform.position));

        }
    }


    IEnumerator InstantiateSmallExplosionManger(Vector2 center)
    {
        List<Vector2> dirs = new List<Vector2>
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

        // 每个方向的当前点
        List<Vector2> starts = new List<Vector2>();

        // 初始化四个方向的起点
        for (int i = 0; i < dirs.Count; i++)
        {
            Vector2 start = center + dirs[i] * (RADIUS_EXPLOSION_MEGA + RADIUS_EXPLOSION_SMALL);
            starts.Add(start);
        }

        while (true)
        {
            bool anyValid = false;

            // 四个方向同时生成一轮
            for (int i = 0; i < dirs.Count; i++)
            {
                if (ParentPokemonRoom.isPointInRoon(starts[i], 0f))
                {
                    anyValid = true;
                    InstantiateSmallExplosionSingel(starts[i]);

                    // 推进到下一层
                    starts[i] += dirs[i] * RADIUS_EXPLOSION_SMALL * 2;
                }
            }

            // 如果四个方向都越界了，结束
            if (!anyValid)
                break;

            yield return new WaitForSeconds(0.2f);
        }
    }


    void InstantiateSmallExplosionSingel( Vector2 p )
    {
        DrifloonExplosion boom = Instantiate(ExplosionSmall, p, Quaternion.identity);
        //爆炸音效
        if (AudioManager.Instance != null) { AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonBasicSFXList.Explosion, transform.position); }
        
        boom.empty = this;
        Destroy(boom.gameObject, 5f);
    }


    /**
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
    **/

    //爆炸
    public override void EmptyEcplosionEvent()
    {
        base.EmptyEcplosionEvent();
        animator.SetTrigger("Exploson");
        Pokemon.PokemonHpChange(null, this.gameObject, this.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType);
        ParentPokemonRoom.CameraShake(0.6f, 3.0f, true);
        isExploson = true;
    }


    /// <summary>
    /// 混乱时角度偏转
    /// </summary>
    Vector2 ConfusionDir(Vector2 dir, float Alpha)
    {
        if (isEmptyConfusionDone)
        {
            return Quaternion.AngleAxis(Random.Range(-Alpha, Alpha), Vector3.forward) * dir;
        }
        return dir;
    }
    /**
    public Vector3 InitialDirection;
    public GameObject explosion;
    public GameObject DSB;//DrifblimShadowBall
    private Vector3 direction;
    private string currentState;

    private Vector3 currentPosition;
    private float timer;
    private bool IsReflect;
    private float ShadowBallCD;
    private bool IsShadowBall;


    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;
        EmptyType02 = PokemonType.TypeEnum.Flying;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = InitialDirection;
        ShadowBallCD = 0f;
        IsShadowBall = false;
        IsReflect = true;



        StartOverEvent();
    }

    void Update()
    {
        ResetPlayer();
        if (!isBorn && !isDie)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            timer += Time.deltaTime;
            if (timer > 0.6f)
            {   //检测是否被卡住
                if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone && !isHit && !IsReflect && (!(Mathf.Abs(currentPosition.x - transform.position.x) > 0.6f && Mathf.Abs(currentPosition.y - transform.position.y) > 0.6f)))
                {
                    RaycastHit2D raycastHit2Da = Physics2D.Raycast(transform.position, new Vector2(1, 1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Db = Physics2D.Raycast(transform.position, new Vector2(1, -1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Dc = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 20f, LayerMask.GetMask("Room"));
                    RaycastHit2D raycastHit2Dd = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 20f, LayerMask.GetMask("Room"));
                    float maxDistance = Mathf.Max(raycastHit2Da.distance, raycastHit2Db.distance, raycastHit2Dc.distance, raycastHit2Dd.distance);//检测斜向的射线距离
                    if (maxDistance == raycastHit2Da.distance)
                    {
                        DrifblimShadowBall();
                        direction = new Vector2(1, 1);
                    }
                    else if (maxDistance == raycastHit2Db.distance)
                    {
                        DrifblimShadowBall();
                        direction = new Vector2(1, -1);
                    }
                    else if (maxDistance == raycastHit2Dc.distance)
                    {
                        DrifblimShadowBall();
                        direction = new Vector2(-1, 1);
                    }
                    else
                    {
                        DrifblimShadowBall();
                        direction = new Vector2(-1, -1);
                    }
                }
                timer = 0f;
                IsReflect = false;
                currentPosition = transform.position;
            }
        }
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone)
            {
                rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                if (direction.x > 0)
                {
                    if (direction.y > 0) ChangeAnimationState("DrifblimMoveNE");
                    else ChangeAnimationState("DrifblimMoveSE");
                }
                else
                {
                    if (direction.y > 0) ChangeAnimationState("DrifblimMoveNW");
                    else ChangeAnimationState("DrifblimMoveSW");
                }
            }
        }

        if (IsShadowBall)
        {
            ShadowBallCD += Time.deltaTime;
            if(ShadowBallCD > 1f)
            {
                IsShadowBall = false;
                ShadowBallCD = 0f;
            }
        }//ShadowBallCD
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

        if (other.transform.tag == "Room")
        {
            //反弹操作
            Vector2 newDirection = Vector2.Reflect(direction, other.GetContact(0).normal);
            Vector2[] comparisonValues = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1)
            };

            float minDistance = Mathf.Infinity;
            int closestIndex = -1;
            for (int i = 0; i < comparisonValues.Length; i++)
            {
                float distance = Vector2.SqrMagnitude(newDirection - comparisonValues[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }
            DrifblimShadowBall();
            direction = comparisonValues[closestIndex];
            IsReflect = true;
        }
    }


    public void Explosion()
    {
        if (IsDeadrattle)
        {
            Debug.Log("Boom");
            GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
            var cb = boom.transform.GetChild(0).GetComponent<ExeggcuteExploreCB>();
            cb.SetEmptyInfo(this);
            cb.SetAimTag(isEmptyInfatuationDone ? "Empty" : "Player");
            cb.SetType(PokemonType.TypeEnum.Normal);
            cb.Dmage = 250;
            Destroy(boom, 5f);
        }
    }


    void DrifblimShadowBall()
    {
        if (!IsShadowBall && !isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone)
        {
            IsShadowBall = true;
            var shadowBall = Instantiate(DSB, transform.position, Quaternion.identity).GetComponent<DrifblimShadowBall>();
            shadowBall.empty = this;
            shadowBall.direction = -(Quaternion.AngleAxis((isEmptyConfusionDone ? (Random.Range(-45,45)) : 0) , Vector3.forward ) * direction  );
        }
    }//发射ShadowBall

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
    **/
}
