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
    
    private enum AI_STATE
    {
        IDLE,
        PATROL,
        CHASE,
        ATK,
    }


    private AI_STATE aiState;
    private float preBodyScale;
    private IEnumerator timerAi = null;
    private Rigidbody2D rigi;
    private MasquerainPathFinder _pathFinder;
    
    public FACE_TO faceTo;
    //[Label("观察半径")]
    public float foundRadius = 8;
    //[Label("巡逻选择半径")]
    public float patrolChooseRadius = 5;
    //[Label("巡逻速度")]
    public float patrolSpeed = 5;
    public Transform body;
    
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
        rigi = GetComponent<Rigidbody2D>();
        aiState = AI_STATE.IDLE;

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
            HandleFaceTo();
            EmptyDie();
            StateMaterialChange();
            UpdateEmptyChangeHP();
        }
    }
    
    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyBeKnock();
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

    private void HandleFaceTo()
    {
        Vector2 dir = _pathFinder.faceDir;
        if (dir.x > 0)
        {
            if (dir.y > 0)
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
            if (dir.y > 0)
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

            if (aiState == AI_STATE.IDLE)
            {
                // 选择巡逻点开始巡逻
                Vector2 patrolDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                patrolDir.Normalize();
                Vector2 patrolPos = patrolDir * Random.Range(2, patrolChooseRadius) + rigi.position;
                // 如果超出房间中心范围，规范为其边界
                Vector2 roomMid = transform.parent.position;
                patrolPos.x = Mathf.Clamp(patrolPos.x, roomMid.x - ConstantRoom.ROOM_INNER_WIDTH / 2, roomMid.x + ConstantRoom.ROOM_INNER_WIDTH / 2);
                patrolPos.y = Mathf.Clamp(patrolPos.y, roomMid.y - ConstantRoom.ROOM_INNER_HIGHT / 2, roomMid.y + ConstantRoom.ROOM_INNER_HIGHT / 2);
                speed = patrolSpeed;
                _pathFinder.SetTargetPos(patrolPos, ()=>{});
                aiState = AI_STATE.PATROL;
            } else if (aiState == AI_STATE.PATROL)
            {
                if (!_pathFinder.Walking)
                {
                    aiState = AI_STATE.IDLE;
                }
            }
            
            return aiDuration;
        });
    }

}
