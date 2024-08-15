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

    private void FixedUpdate()
    {
        SpikeTimer += Time.deltaTime;
        SwitchState();

        if (SpikeState == State.N2I) 
        {
            for (int i = 0; i < SpikeList.Count; i++)
            {
                float f = ((SpikeTimer - ((CycleTimer / 2.0f) - ToTimer)) / ToTimer) * 0.8f;
                SpikeList[i].color = new Color(1, 1, 1, Mathf.Clamp(1.0f - f, 0.2f, 1.0f));
                ShadowList[i].color = new Color(1, 1, 1, Mathf.Clamp(1.0f - f, 0.2f, 1.0f));
                Debug.Log(SpikeList[i].gameObject.name);
            }
        }
        if (SpikeState == State.I2N) 
        {
            for (int i = 0; i < SpikeList.Count; i++)
            {
                float f = ((SpikeTimer - (CycleTimer - ToTimer)) / ToTimer) * 0.8f;
                if (SpikeTimer >= CycleTimer || SpikeTimer < CycleTimer - ToTimer) { f = 0.8f; }
                SpikeList[i].color = new Color(1, 1, 1, Mathf.Clamp( 0.2f + f , 0.2f ,1.0f ));
                ShadowList[i].color = new Color(1, 1, 1, Mathf.Clamp(0.2f + f, 0.2f, 1.0f));
                Debug.Log(SpikeList[i].gameObject.name);
            }
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


    //根据时间切换状态
    void SwitchState()
    {
        if (SpikeTimer >= CycleTimer) { SpikeTimer = 0; }
        if (SpikeTimer > 0.0f && SpikeTimer <= CycleTimer / 2.0f - ToTimer && SpikeState != State.Normal)
        {
            SpikeCollider2D.enabled = true;
            SpikeState = State.Normal;
            for (int i = 0; i < SpikeList.Count; i++)
            {
                SpikeList[i].color = new Color(1, 1, 1, 1.0f);
                ShadowList[i].color = new Color(1, 1, 1, 1.0f);
                SetAnimtor();
            }
        }
        else if (SpikeTimer > CycleTimer / 2.0f - ToTimer && SpikeTimer <= CycleTimer/2.0f && SpikeState != State.N2I)
        {
            SpikeState = State.N2I;
        }
        else if (SpikeTimer > CycleTimer / 2.0f && SpikeTimer <= CycleTimer - ToTimer && SpikeState != State.Inviciable)
        {
            SpikeCollider2D.enabled = false;
            SpikeState = State.Inviciable;
            for (int i = 0; i < SpikeList.Count; i++)
            {
                SpikeList[i].color = new Color(1 , 1 , 1 , 0.2f);
                ShadowList[i].color = new Color(1 , 1 , 1 , 0.2f);
            }
            Debug.Log(SpikeList.Count);
        }
        else if (SpikeTimer > CycleTimer-ToTimer && SpikeTimer <= CycleTimer && SpikeState != State.I2N)
        {
            SpikeState = State.I2N;
        }
        
    }











    //普通刺的伤害
    // Start is called before the first frame update
    private void Update()
    {
        SpikesUpdate();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInviciable) {
            SpikeOnTriggerStay2D(other);
        }
    }
}
