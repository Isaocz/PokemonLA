using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesStealthRock : Spike
{

    //隐形岩是否隐形
    bool isInviciable = false;

    List<SpriteRenderer> SpikeList = new List<SpriteRenderer> { };
    List<SpriteRenderer> ShadowList = new List<SpriteRenderer> { };

    enum State
    {
        Normal,       //普通状态
        Inviciable,   //隐形状态
        N2I,          //普2隐
        I2N,          //隐2普
    }
    State SpikeState;
    float SpikeTimer;


    public float StartDelay;

    Collider2D SpikeCollider2D;

    //周期
    public float CycleTimer = 12.0f;
    //切换状态的时间
    public float ToTimer = 0.3f;

    bool forceN2I = false;   // Normal 时强制进入 N2I
    bool freezeState = false; // SpikeOver 后冻结状态，不再切换

    /// <summary>
    /// 刺碰撞是否有效
    /// </summary>
    bool isColliderEnable;



    private void Start()
    {
        SpikeCollider2D = transform.GetComponent<Collider2D>();
        for (int i = 1; i < transform.childCount; i++)
        {
            SpikeList.Add(transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>());
            ShadowList.Add(transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>());
            
        }
        SpikeTimer += StartDelay;
        if (SpikeTimer == 0.0f) { SetAnimtor(); }
        SwitchState();
    }

    void FixedUpdate()
    {
        SpikeTimer += Time.deltaTime;
        SwitchState();

        float half = CycleTimer * 0.5f;

        if (SpikeState == State.N2I)
        {
            float t;

            if (forceN2I)
            {
                // 强制渐隐：SpikeTimer 从 0 → ToTimer
                t = Mathf.Clamp01(SpikeTimer / ToTimer);
            }
            else
            {
                // 原本周期渐隐
                t = Mathf.InverseLerp(half - ToTimer, half, SpikeTimer);
            }

            float alpha = Mathf.Lerp(1f, 0.2f, t);
            SetAlpha(alpha);


            // 渐隐完成后锁定隐形状态
            if (t >= 1f && forceN2I)
            {
                SpikeState = State.Inviciable;
                SetAlpha(0.2f);
                SpikeCollider2D.enabled = false;
            }
        }
        else if (SpikeState == State.I2N)
        {
            float t = Mathf.InverseLerp(CycleTimer - ToTimer, CycleTimer, SpikeTimer);
            SetAlpha(Mathf.Lerp(0.2f, 1f, t));
        }
    }


    void SetAnimtor()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Animator a = transform.GetChild(i).GetComponent<Animator>();
            Timer.Start(this , 0.25f * (i+1) , ()=> { a.SetTrigger("Idle"); });
            //transform.GetChild(i).GetComponent<Animator>().SetTrigger("Idle" );
        }
    }


    void SwitchState()
    {
        // SpikeOver 后冻结状态，不再切换
        if (freezeState)
        {
            return;
        }



        float half = CycleTimer * 0.5f;

        if (SpikeTimer >= CycleTimer)
        {
            SpikeTimer = 0;
        }

        // Normal
        if (SpikeTimer < half - ToTimer)
        {
            EnterNormal();
            SpikeState = State.Normal;
        }
        // Normal → Invisible (fade out)
        else if (SpikeTimer < half)
        {
            SpikeState = State.N2I;
        }
        // Invisible
        else if (SpikeTimer < CycleTimer - ToTimer)
        {
            EnterInvisible();
            SpikeState = State.Inviciable;
        }
        // Invisible → Normal (fade in)
        else
        {
            SpikeState = State.I2N;
        }
    }






    void SetAlpha(float a)
    {
        foreach (var s in SpikeList)
            s.color = new Color(1, 1, 1, a);

        foreach (var s in ShadowList)
            s.color = new Color(1, 1, 1, a);
    }

    void EnterNormal()
    {
        isColliderEnable = true;
        SetAlpha(1f);

        if (SpikeState != State.Normal)
            SetAnimtor();   // 只在进入 Normal 时触发一次
    }

    void EnterInvisible()
    {
        isColliderEnable = false;
        SetAlpha(0.2f);
    }







    //普通刺的伤害
    // Start is called before the first frame update
    private void Update()
    {
        SpikesUpdate();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInviciable && isColliderEnable) {
            SpikeOnTriggerStay2D(other);
        }
    }

    public override void SpikeOver()
    {
        base.SpikeOver();

        freezeState = true; // 后续不再切换状态

        if (SpikeState == State.Normal)
        {
            // Normal 时强制进入 N2I
            forceN2I = true;
            SpikeState = State.N2I;
            SpikeTimer = 0; // 从头开始渐隐
            SpikeCollider2D.enabled = false;
        }
        // 其他状态：不进入 N2I，只冻结状态
    }

}
