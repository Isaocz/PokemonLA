using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masquerain : Empty
{
    public enum FACE_TO
    {
        FL,
        FR,
        BL,
        BR,
    }
    
    public enum AI_STATE
    {
        IDLE,
        PATROL,
        CHASE,
        ATK,
    }


    public AI_STATE aiState;
    private float preBodyScale;
    private IEnumerator timerAi = null;
    private MasquerainPathFinder _pathFinder;
    private GameObject chasingObj;
    private Vector3 lastChasePos;
    private float lastAtkTime;  // 上次攻击距今
    
    public FACE_TO faceTo;
    //[Label("观察半径")]
    public float foundRadius = 12;
    //[Label("攻击半径")]
    public float atkRadius = 7;
    //[Label("巡逻选择半径")]
    public float patrolChooseRadius = 8;
    //[Label("最大维持追踪距�?")]
    public float disKeepChase = 15;
    //[Label("巡逻速度")]
    public float patrolSpeed = 2.2f;
    //[Label("追逐速度")]
    public float chaseSpeed = 5f;
    //[Label("追逐速cd�?")]
    public float cdSkill = 3f;
    public Transform body;
    public GameObject skill;

    GameObject AtkTarget;
    
    void Start()
    {
        speed = 0f;
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

        preBodyScale = body.localScale.x;
        _pathFinder = GetComponent<MasquerainPathFinder>();
        aiState = AI_STATE.IDLE;
        lastAtkTime = 0;

        // Timer.Start(this, 5, () =>
        // {
        //     animator.SetTrigger("Hit");
        // });
        //
        // Timer.Start(this, 8, () =>
        // {
        //     animator.SetTrigger("Die");
        // });

        // ChangeFaceTo(FACE_TO.BL);
    }

    void Update()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            if (!isFearDone) {
                if (_pathFinder.Walking)
                {
                    HandleFaceTo(_pathFinder.faceDir);
                }
            }
            EmptyDie();
            StateMaterialChange();
            UpdateEmptyChangeHP();
        }

        if (!isDie && !isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone)
        {
            lastAtkTime += Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyBeKnock();
            if (isFearDone)
            {
                if (FindAtkTarget(foundRadius) != null) {
                    Vector2 D = (transform.position - FindAtkTarget(foundRadius).transform.position).normalized;
                    rigidbody2D.position = rigidbody2D.position + D * 5 * Time.deltaTime;
                    HandleFaceTo(D);
                }
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }

    private void HandleFaceTo(Vector2 dir)
    {
        if (dir.x > 0)
        {
            if (dir.y > 0.1)
            {
                changeFaceTo(FACE_TO.BR);
            }
            else
            {
                changeFaceTo(FACE_TO.FR);
            }
        }
        else
        {
            if (dir.y > 0.1)
            {
                changeFaceTo(FACE_TO.BL);
            }
            else
            {
                changeFaceTo(FACE_TO.FL);
            }
        }
    }
    
    private void changeFaceTo(FACE_TO ft)
    {
        faceTo = ft;
        Vector3 scale = body.localScale;
        if (ft == FACE_TO.FL || ft == FACE_TO.FR)
        {
            animator.SetBool("bFaceFront", true);
            scale.x = (ft == FACE_TO.FL ? 1 : -1) * preBodyScale;
        }else if (ft == FACE_TO.BL || ft == FACE_TO.BR)
        {
            animator.SetBool("bFaceFront", false);
            scale.x = (ft == FACE_TO.BR ? 1 : -1) * preBodyScale;
        }
        body.localScale = scale;
    }
    
    void OnAniBornFinsih()
    {
        ResetPlayer();
        StartAiTimer();
    }
    
    private void StartAiTimer()
    {
        float aiDuration = 0.02f;
        timerAi = Timer.Start(this, 0f, () =>
        {
            if (isDie)
            {
                StopCoroutine(timerAi);
                return 0;
            }
            if (isHit)
            {
                return aiDuration;
            }

            if (aiState == AI_STATE.IDLE)
            {
                if (!isSilence && !isSleepDone && !isCanNotMoveWhenParalysis)
                {
                    GameObject target = FindAtkTarget(foundRadius);
                    if (target)
                    {
                        Vector2 dis = target.transform.position - transform.position;
                        if (dis.magnitude <= atkRadius)
                        {
                            changeToAtk(target);
                        }
                        else
                        {
                            // 进入追踪
                            changeToChase(target);
                        }
                    }
                    else
                    {
                        // 选择巡逻点开始巡�?
                        Vector2 patrolDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        patrolDir.Normalize();
                        Vector2 patrolPos = patrolDir * Random.Range(5, patrolChooseRadius) + rigidbody2D.position;
                        // 如果超出房间中心范围，规范为其边�?
                        Vector2 roomMid = transform.parent.position;
                        patrolPos.x = Mathf.Clamp(patrolPos.x, roomMid.x - ConstantRoom.ROOM_INNER_WIDTH / 2, roomMid.x + ConstantRoom.ROOM_INNER_WIDTH / 2);
                        patrolPos.y = Mathf.Clamp(patrolPos.y, roomMid.y - ConstantRoom.ROOM_INNER_HIGHT / 2, roomMid.y + ConstantRoom.ROOM_INNER_HIGHT / 2);
                        speed = patrolSpeed;
                        _pathFinder.SetTargetPos(patrolPos);
                        aiState = AI_STATE.PATROL;
                    }
                }
            }
            else if (aiState == AI_STATE.PATROL)
            {
                GameObject target = FindAtkTarget(foundRadius);
                if (target && !isSilence && !isSleepDone && !isCanNotMoveWhenParalysis)
                {
                    Vector2 dis = target.transform.position - transform.position;
                    if (dis.magnitude <= atkRadius)
                    {
                        changeToAtk(target);
                    }
                    else
                    {
                        // 进入追踪
                        changeToChase(target);
                    }
                }
                else if (!_pathFinder.Walking)
                {
                    aiState = AI_STATE.IDLE;
                }
            }
            else if (aiState == AI_STATE.CHASE)
            {
                float dis = (chasingObj.transform.position - transform.position).magnitude;
                if ( dis > disKeepChase)
                {
                    _pathFinder.Stop();
                    aiState = AI_STATE.IDLE;
                }
                else if (dis <= atkRadius && !isSilence && !isSleepDone && !isCanNotMoveWhenParalysis && !isFearDone)
                {
                    changeToAtk(chasingObj);
                }
                else
                {
                    float disToLastChasePos = (lastChasePos - transform.position).magnitude;
                    if (isFearDone)
                    {
                        if (disToLastChasePos < 5)
                        {
                            Vector3 pos = transform.position - (chasingObj.transform.position - transform.position);
                            _pathFinder.SetTargetPos(pos.normalized * 5);
                        }
                    }
                    else
                    {
                        if (disToLastChasePos > 3 || !_pathFinder.Walking)
                        {
                            // 更新追踪位置
                            lastChasePos = chasingObj.transform.position;
                            _pathFinder.SetTargetPos(lastChasePos);
                        }
                    }
                }
            }
            else if (aiState == AI_STATE.ATK)
            {
                if (lastAtkTime > cdSkill)
                {
                    aiState = AI_STATE.IDLE;
                }
            }
            
            return aiDuration;
        });
    }

    private void changeToChase(GameObject target)
    {
        chasingObj = target;
        lastChasePos = target.transform.position;
        speed = chaseSpeed;

        if (isFearDone)
        {
            Vector3 pos = transform.position - (chasingObj.transform.position - transform.position);
            _pathFinder.SetTargetPos(pos.normalized * 5);
        }
        else
        {
            _pathFinder.SetTargetPos(lastChasePos);
        }
        aiState = AI_STATE.CHASE;
    }
    
    private void changeToAtk(GameObject target)
    {
        if (!isDie && !isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone)
        {

            int ConfusionLunchOffsetRotation = Random.Range(-30, 30);
            Vector2 dir = (Quaternion.AngleAxis((isEmptyConfusionDone ? ConfusionLunchOffsetRotation : 0), Vector3.forward) * (target.transform.position - transform.position).normalized).normalized;
            HandleFaceTo(dir);

            animator.SetTrigger("Atk");
            _pathFinder.Stop(); 
            aiState = AI_STATE.ATK;
            lastAtkTime = 0;

            Timer.Start(this, 0.5f, () =>
            {
                int ConfusionLunchOffsetRotation = Random.Range(-30, 30);
                Vector2 dir = (Quaternion.AngleAxis((isEmptyConfusionDone ? ConfusionLunchOffsetRotation : 0), Vector3.forward) * (target.transform.position - transform.position).normalized).normalized;
                HandleFaceTo(dir);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                GameObject skillObj = Instantiate(skill, transform.position + new Vector3(dir.x, dir.y) * 1, Quaternion.Euler(0f, 0f, angle));
                AirSlash airSlash = skillObj.GetComponent<AirSlash>();
                airSlash.empty = this;
                Timer.Start(this, 0.15f, () =>
                {
                    if (skillObj)
                    {

                        airSlash.LaunchNotForce(dir, 10);
                        if (rigidbody2D !=null) { rigidbody2D.AddForce(-dir * 1600); }
                    }
                });
            });
        }
    }


}
