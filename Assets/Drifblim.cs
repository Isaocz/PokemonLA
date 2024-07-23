using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifblim : Empty
{
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
        EmptyType01 = Type.TypeEnum.Ghost;
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
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = InitialDirection;
        ShadowBallCD = 0f;
        IsShadowBall = false;
        IsReflect = true;
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
            GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
            var cb = boom.transform.GetChild(0).GetComponent<ExeggcuteExploreCB>();
            cb.SetEmptyInfo(this);
            cb.SetAimTag(isEmptyInfatuationDone ? "Empty" : "Player");
            cb.SetType(Type.TypeEnum.Normal);
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

}
