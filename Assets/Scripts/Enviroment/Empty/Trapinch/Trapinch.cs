using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * editor ��䣺
 * �쳣״̬����
 * player ui state
 * ui health
 * ���ԣ�����ֵ
 * base exp ��������
 * HWP ȡ��Ŭ��ֵ
 * skinRenderers body��Ⱦ�����飨���ܣ�
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
            // ƽ�� 5 ֡��Ӧһ��
            return 0.3f;
        });
    }
}
