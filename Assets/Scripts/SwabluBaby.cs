using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwabluBaby : FollowBaby
{
    // Start is called before the first frame update
    Vector3 MoveDir;
    SwabluCollider Colision;
    GameObject Shadow01;
    GameObject Shadow02;

    float MoveSpeed;


    public enum State
    {
        idle,
        Move,
        Return,
    }
    public State NowState;


    void Start()
    {
        FollowBabyStart();
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
        Colision = transform.GetChild(0).GetComponent<SwabluCollider>();
        Shadow01 = transform.GetChild(1).gameObject;
        Shadow02 = transform.GetChild(2).gameObject;
        animator = GetComponent<Animator>();
        MoveSpeed = 12.0f;
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
        switch (NowState)
        {
            case State.Move:
                if (MoveDir.x > 0) { animator.SetFloat("LookX", 1); }
                else { animator.SetFloat("LookX", -1); }
                Colision.transform.position += Time.deltaTime * MoveSpeed * MoveDir;
                Shadow01.transform.position += Time.deltaTime * MoveSpeed * MoveDir;
                Shadow02.transform.position += Time.deltaTime * MoveSpeed * MoveDir;
                MoveSpeed -= 6.0f * Time.deltaTime;
                if (MoveSpeed <= 0)
                {
                    Return();
                }
                break;
            case State.Return:
                if (-Colision.transform.localPosition.normalized.x > 0) { animator.SetFloat("LookX", 1); }
                else { animator.SetFloat("LookX", -1); }
                Colision.transform.position += Time.deltaTime * MoveSpeed * -Colision.transform.localPosition.normalized;
                Shadow01.transform.position += Time.deltaTime * MoveSpeed * -Colision.transform.localPosition.normalized;
                Shadow02.transform.position += Time.deltaTime * MoveSpeed * -Colision.transform.localPosition.normalized;
                MoveSpeed = Mathf.Clamp( MoveSpeed - 6.0f * Time.deltaTime , 1.0f , 12.0f);
                if (Colision.transform.localPosition.magnitude <= 0.05f)
                {
                    Colision.transform.localPosition = Vector3.zero;
                    Shadow01.transform.localPosition = new Vector3(0, -0.81f, 0);
                    Shadow02.transform.localPosition = new Vector3(0, -0.7632998f, 0);
                    NowState = State.idle;
                    MoveSpeed = 12.0f;
                }
                break;
        }

    }

    public override void FollowBabyShot(Vector2Int Dir)
    {
        if (NowState == State.idle)
        {
            base.FollowBabyShot(Dir);
            NowState = State.Move;
            MoveDir = new Vector3(Dir.x, Dir.y, 0);
        }
    }


    public void Return()
    {
        if ((NowState != State.Return)) {
            NowState = State.Return;
            MoveSpeed = 12.0f;
        }
    }


    public override void InANewRoomEvent()
    {
        base.InANewRoomEvent();
        Colision.transform.localPosition = Vector3.zero;
        Shadow01.transform.localPosition = new Vector3(0, -0.81f, 0);
        Shadow02.transform.localPosition = new Vector3(0, -0.7632998f, 0);
        NowState = State.idle;
        MoveSpeed = 12.0f;
    }

}
