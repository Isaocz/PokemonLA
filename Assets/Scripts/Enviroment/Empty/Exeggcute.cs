using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum AI_STATE {
    IDLE,
    READY_ATK,
    ATKING,
}

public class Exeggcute : Empty
{
    [Label("观察半径")]
    public float foundRadius = 5;
    [Label("攻击时间间隔")]
    public float atkInterval = 0.3F;
    [Label("落地范围半径")]
    public float atkDownRadius = 0.8F;
    [Label("飞行高度")]
    public float throwHigh = 5F;
    [Label("飞行时间")]
    public float throwTime = 0.8F;
    [Label("发现标志")]
    public Transform transFound;
    [Tooltip("对象列表")]
    public List<GameObject> eggList;
    public GameObject ui;
    public GameObject oneEggShadow;

    // temp
    public GameObject effectExplosion;

    private AI_STATE aIState = AI_STATE.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0f;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level, 30);
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
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
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
            if (Vector2.Distance(transform.position, player.transform.position) <= foundRadius)
            {
                transFound.gameObject.SetActive(true);
                animator.SetTrigger("ReadyAtk");
                aIState = AI_STATE.READY_ATK;
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

    public void RunEggActionByIdx(int i)
    {
        GameObject eggobj = eggList[i];
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(atkInterval * i);

        Vector3 playerPos = player.transform.position;
        seq.AppendCallback(() =>
        {
            // 以起跳时的位置为准
            playerPos = player.transform.position;
            if (i == eggList.Count - 1)
            {
                // 大蛋根据玩家的移动方向做预判
                var speed = player.GetSpeed();
                playerPos = playerPos + new Vector3(speed.x, speed.y) * throwTime;
            }
            var downRadius = atkDownRadius;
            if (isEmptyConfusionDone)
            {
                downRadius = downRadius + 1;
            }
            playerPos = playerPos + new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * downRadius;
            // TODO: 如果超出房间范围，规范为边界
        });

        Vector3 curScale = eggobj.transform.localScale;
        seq.Append(eggobj.transform.DOScaleY(curScale.y * 0.8F, 0.15F));
        seq.Append(eggobj.transform.DOScaleY(curScale.y, 0.15F));

        GameObject shadow = null;
        seq.AppendCallback(() =>
        {
            shadow = Instantiate(oneEggShadow, eggobj.transform.position, Quaternion.identity);
            shadow.transform.DOMove(playerPos, throwTime);
            eggobj.transform.DOJump(playerPos, throwHigh, 1, throwTime);
        });
        // wait jump finish
        seq.AppendInterval(throwTime);

        seq.AppendCallback(() =>
        {
            Instantiate(effectExplosion, eggobj.transform.position, Quaternion.identity);
            eggobj.SetActive(false);
            shadow.SetActive(false);
        });

        if (i == eggList.Count - 1)
        {
            // 全部爆炸后的处理
            seq.AppendCallback(() =>
            {
                // 触发死亡
                EmptyHp = 0;
            });
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);
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
        GetComponent<Collider2D>().enabled = false;

        for (int i = 0; i < eggList.Count; i++)
        {
            RunEggActionByIdx(i);
        }
    }
}
