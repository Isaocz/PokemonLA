using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * editor 填充：
 * 异常状态材质
 * player ui state
 * ui health
 * 属性，种族值
 * base exp 基础经验
 * HWP 取得努力值
 * skinRenderers body渲染器数组（可能）
 * 
 * code part:
 * base part 1
 * base part 2
 * 
 * ani:
 * param(Die, Hit)
*/

public class Trapinch : Empty
{
    private enum AI_STATE
    {
        IDLE,
        READY_ATK,
        ATKING,
    }

    private IEnumerator timerAi = null;

    // Start is called before the first frame update
    void Start()
    {
        // * base part 1
        speed = 0f;
        player = GameObject.FindObjectOfType<PlayerControler>();
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
    }

    // Update is called once per frame
    void Update()
    {
        // * base part 2
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            UpdateEmptyChangeHP();
        }
    }

    void OnAniBornFinsih()
    {
        int now = 1;
        print(now);
        timerAi = Timer.Start(this, 0f, () =>
        {
            print(now);
            now++;
            if (now > 10)
            {
                StopCoroutine(timerAi);
            }
            // 平均 5 帧反应一次
            return 0.3f;
        });
    }
}
