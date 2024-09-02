using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreepy : FollowBaby
{
    // Start is called before the first frame update
    Vector3 MoveDir;
    DreepyCollider Colision;
    GameObject Shadow01;
    GameObject Shadow02;



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
        Colision = transform.GetChild(0).GetComponent<DreepyCollider>();
        Shadow01 = transform.GetChild(1).gameObject;
        Shadow02 = transform.GetChild(2).gameObject;
        animator = GetComponent<Animator>();
        Colision.transform.localPosition = Vector3.zero;
        Shadow01.transform.localPosition = new Vector3(0, -0.985f, 0);
        Shadow02.transform.localPosition = new Vector3(0, -0.9382998f, 0);
        NowState = State.idle;

    }

    // Update is called once per frame
    void Update()
    {
        
        switch (NowState)
        {
            case State.Move:
                if (MoveDir.x > 0) { animator.SetFloat("LookX", 1); }
                else { animator.SetFloat("LookX", -1); }
                Colision.transform.position += Time.deltaTime * 10.0f * MoveDir;
                Shadow01.transform.position += Time.deltaTime * 10.0f * MoveDir;
                Shadow02.transform.position += Time.deltaTime * 10.0f * MoveDir;
                animator.SetFloat("LookX" , MoveDir.x);
                animator.SetFloat("LookY" , MoveDir.y);
                break;
            case State.Return:
                FollowBabyUpdate();
                if (-Colision.transform.localPosition.normalized.x > 0) { animator.SetFloat("LookX", 1); }
                else { animator.SetFloat("LookX", -1); }
                Colision.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                Shadow01.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                Shadow02.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                if (Colision.transform.localPosition.magnitude <= 0.05f)
                {
                    Colision.transform.localPosition = Vector3.zero;
                    Shadow01.transform.localPosition = new Vector3(0, -0.985f, 0);
                    Shadow02.transform.localPosition = new Vector3(0, -0.9382998f, 0);
                    NowState = State.idle;
                }
                break;
            case State.idle:
                FollowBabyUpdate();
                break;
        }

    }

    public override void FollowBabyShot(Vector2Int Dir)
    {
        if (NowState == State.idle)
        {
            base.FollowBabyShot(Dir);
            NowState = State.Move;
            animator.SetTrigger("Rush");
            MoveDir = new Vector3(Dir.x, Dir.y, 0);
        }
    }


    public void Return()
    {
        if ((NowState != State.Return))
        {
            animator.SetTrigger("RushOver");
            NowState = State.Return;
        }
    }




    public override void InANewRoomEvent()
    {
        base.InANewRoomEvent();
        Colision.transform.localPosition = Vector3.zero;
        Shadow01.transform.localPosition = new Vector3(0, -0.985f, 0);
        Shadow02.transform.localPosition = new Vector3(0, -0.9382998f, 0);
        NowState = State.idle;
    }

}
