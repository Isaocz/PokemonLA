using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Exeggcute : Empty
{
    //[Label("观察半径")]
    public float foundRadius = 5;
    //[Label("攻击时间间隔")]
    public float atkInterval = 0.3F;
    //[Label("落地范围半径")]
    public float atkDownRadius = 0.8F;
    //[Label("飞行高度")]
    public float throwHigh = 5F;
    //[Label("飞行时间")]
    public float throwTime = 0.8F;
    //[Label("cd（技能不能正常发出时有用）")]
    public float cdBomb = 3F;
    //[Label("发现标志")]
    public Transform transFound;
    //[Tooltip("对象列表")]
    public List<GameObject> eggList;
    public GameObject ui;
    //[Label("蛋蛋影子")]
    public GameObject oneEggShadow;
    //[Label("预判框")]
    public GameObject skillRange;

    private enum AI_STATE
    {
        IDLE,
        READY_ATK,
        ATKING,
    }

    // temp
    public GameObject effectExplosion;

    // 附属对象，死亡时去除
    private List<GameObject> objects;

    private AI_STATE aIState = AI_STATE.IDLE;
    private float lastUseBomb = 0;
    private Vector3 lastThrowPos;

    // Start is called before the first frame update
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

        transFound.gameObject.SetActive(false);
        objects = new List<GameObject>();

        lastUseBomb = -cdBomb;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            // TODO
            runFSM();
            UpdateEmptyChangeHP();
        }
        //InvincibleUpdate();
    }

    private void runFSM()
    {
        if (isDie)
        {
            return;
        }
        if(aIState == AI_STATE.IDLE)
        {
            if (Time.time - lastUseBomb < cdBomb)
            {
                return;
            }
            if (isCanNotMoveWhenParalysis)
            {
                // 麻痹 1/3 概率进入cd
                if (Random.Range(0.0f, 1.0f) < 0.33)
                {
                    lastUseBomb = Time.time;
                    return;
                }
            }
            if (isEmptyInfatuationDone)
            {
                if (transform.parent.childCount > 1 && InfatuationForRangeRayCastEmpty(foundRadius) != null)
                {
                    turnToReadyAtk();
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= foundRadius)
                {
                    turnToReadyAtk();
                }
                else if(isSubsititue && SubsititueTarget != null && Vector2.Distance(transform.position, SubsititueTarget.transform.position) <= foundRadius)
                {
                    turnToReadyAtk();
                }
            }
            return;
        }
        if (aIState == AI_STATE.READY_ATK)
        {
            // just wait
            return;
        }
        //transform.DOJump(new Vector3(0, 0, 0), 1, 1, 1);
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyBeKnock();
        }
    }

    private void turnToReadyAtk()
    {
        transFound.gameObject.SetActive(true);
        animator.SetTrigger("ReadyAtk");
        aIState = AI_STATE.READY_ATK;
    }

    public void RunEggActionByIdx(int i)
    {
        GameObject eggobj = eggList[i];
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(atkInterval * i);

        GameObject range = null;
        Vector3 jumpPos = transform.position;
        seq.AppendCallback(() =>
        {
            Vector3 playerPos = transform.position;
            // 起跳时，取目标的位置
            if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(foundRadius) == null)
            {
                if (isSubsititue && SubsititueTarget != null) 
                { 
                    playerPos = SubsititueTarget.transform.position; 
                }
                else if (player)
                {
                    playerPos = player.transform.position;
                    if (i == eggList.Count - 1)
                    {
                        // 大蛋根据玩家的移动方向做预判
                        var speed = player.GetSpeed();
                        playerPos = playerPos + new Vector3(speed.x, speed.y) * (throwTime + 0.3f);
                    }
                }
            }
            else 
            {
                playerPos = InfatuationForRangeRayCastEmpty(foundRadius).transform.position;
            }

            var downRadius = atkDownRadius;
            if (isEmptyConfusionDone)
            {
                // 混乱会有更不确定的范围
                downRadius = downRadius + 1;
            }
            jumpPos = playerPos + new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * downRadius;

            if (isFearDone)
            {
                // 恐惧时向相反方向发射
                jumpPos.x = -jumpPos.x;
                jumpPos.y = -jumpPos.y;
            }

            if (isSilence)
            {
                // 致盲向上次的方向发射
                jumpPos = lastThrowPos;
            }

            if (isSleepDone)
            {
                jumpPos = transform.position;
            }

            // 如果超出房间中心范围，规范为其边界
            Vector2 roomMid = transform.parent.position;
            jumpPos.x = Mathf.Clamp(jumpPos.x, roomMid.x - ConstantRoom.ROOM_INNER_WIDTH / 2, roomMid.x + ConstantRoom.ROOM_INNER_WIDTH / 2);
            jumpPos.y = Mathf.Clamp(jumpPos.y, roomMid.y - ConstantRoom.ROOM_INNER_HIGHT / 2, roomMid.y + ConstantRoom.ROOM_INNER_HIGHT / 2);
            lastThrowPos = jumpPos;

            range = Instantiate(skillRange, jumpPos, Quaternion.identity);
            range.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            range.GetComponent<SpriteRenderer>().DOFade(1, 1f);
            objects.Add(range);
        });

        Vector3 curScale = eggobj.transform.localScale;
        seq.Append(eggobj.transform.DOScaleY(curScale.y * 0.8F, 0.15F));
        seq.Append(eggobj.transform.DOScaleY(curScale.y, 0.15F));

        GameObject shadow = null;
        seq.AppendCallback(() =>
        {
            shadow = Instantiate(oneEggShadow, eggobj.transform.position, Quaternion.identity);
            shadow.transform.DOMove(jumpPos, throwTime);
            eggobj.transform.DOJump(jumpPos, throwHigh, 1, throwTime);
            eggobj.transform.DOLocalRotate(new Vector3(0, 0, 360F), throwTime, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
        });
        // wait jump finish
        seq.AppendInterval(throwTime);

        seq.AppendCallback(() =>
        {
            Destroy(range);
            objects.Remove(range);

            GameObject effect = Instantiate(effectExplosion, eggobj.transform.position, Quaternion.identity);
            ExeggcuteExploreCB exploreCB = effect.transform.GetChild(0).GetChild(0).GetComponent<ExeggcuteExploreCB>();
            exploreCB.SetEmptyInfo(this);
            exploreCB.SetAimTag(isEmptyInfatuationDone ? "Empty" : "Player");
            eggobj.SetActive(false);
            shadow.SetActive(false);
            objects.Add(effect);
            objects.Add(shadow);
        });

        if (i == eggList.Count - 1)
        {
            // 全部爆炸完成后的处理
            seq.AppendInterval(1.5F);
            seq.AppendCallback(() =>
            {
                // 触发死亡
                EmptyHp = 0;
            });
            seq.AppendInterval(0.1F);
            seq.AppendCallback(() =>
            {
                FinishDie();
            });
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

    // call by ani ReadyAtk
    public void OnAniTriggerAtk()
    {
        if (isDie)
        {
            return;
        }
        Invincible = true;
        // 后续的动作由 DOTween 执行
        animator.enabled = false;
        ui.SetActive(false);
        //GetComponent<Collider2D>().enabled = false;
        lastThrowPos = transform.position;

        for (int i = 0; i < eggList.Count; i++)
        {
            eggList[i].GetComponent<SpriteRenderer>().sortingOrder = 11;
            RunEggActionByIdx(i);
        }
    }

    public void FinishDie()
    {
        foreach (var item in objects)
        {
            Destroy(item);
        }
        EmptyDestroy();
    }
}
