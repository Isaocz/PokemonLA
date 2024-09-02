using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinecoBaby : FollowBaby
{
    // Start is called before the first frame update
    Vector3 MoveDir;
    PinecoBabySprite Colision;
    GameObject Shadow01;
    GameObject Shadow02;
    bool isDieEventDone;

    public GameObject BlastPrefabs;
    GameObject BlastObj;

    public enum State
    {
        idle,
        Move,
        Return,
        Blast,
        Die,
    }
    public State NowState;


    void Start()
    {
        FollowBabyStart();
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
        Colision = transform.GetChild(0).GetComponent<PinecoBabySprite>();
        Shadow01 = transform.GetChild(1).gameObject;
        Shadow02 = transform.GetChild(2).gameObject;
        animator = GetComponent<Animator>();
        Colision.transform.localPosition = Vector3.zero;
        Shadow01.transform.localPosition = new Vector3(0, -0.5867003f, 0);
        Shadow02.transform.localPosition = new Vector3(0, -0.54f, 0);
        NowState = State.idle;
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
                Colision.transform.position += Time.deltaTime * 12.0f * MoveDir;
                Shadow01.transform.position += Time.deltaTime * 12.0f * MoveDir;
                Shadow02.transform.position += Time.deltaTime * 12.0f * MoveDir;
                break;
            case State.Return:
                if (-Colision.transform.localPosition.normalized.x > 0) { animator.SetFloat("LookX" , 1); }
                else { animator.SetFloat("LookX", -1); }
                Colision.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                Shadow01.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                Shadow02.transform.position += Time.deltaTime * 3.5f * -Colision.transform.localPosition.normalized;
                if (Colision.transform.localPosition.magnitude <= 0.05f ) {
                    Colision.transform.localPosition = Vector3.zero;
                    Shadow01.transform.localPosition = new Vector3( 0 , -0.5867003f , 0);
                    Shadow02.transform.localPosition = new Vector3(0, -0.54f, 0);
                    NowState = State.idle; }
                break;
            case State.Blast:
                Colision.transform.position = BlastObj.transform.position;
                Shadow01.transform.position = BlastObj.transform.position;
                Shadow02.transform.position = BlastObj.transform.position;
                if (!isDieEventDone) {
                    isDieEventDone = true;
                    Timer.Start(this, 1.2f, () =>
                    {
                        NowState = State.Die;
                        Colision.transform.localPosition = Vector3.zero;
                        Shadow01.transform.localPosition = new Vector3(0, -0.5867003f, 0);
                        Shadow02.transform.localPosition = new Vector3(0, -0.54f, 0);
                        animator.SetTrigger("Die");
                    });
                }
                break;
        }
        
    }

    public override void FollowBabyShot(Vector2Int Dir)
    {
        if (NowState == State.idle) {
            base.FollowBabyShot(Dir);
            NowState = State.Move;
            MoveDir = new Vector3(Dir.x, Dir.y, 0);
        }
    }

    public override void InANewRoomEvent()
    {
        base.InANewRoomEvent();
        isDieEventDone = false;
        if (NowState == State.Die)
        {
            NowState = State.idle;
            animator.SetTrigger("DieOver");
        }
        Colision.transform.localPosition = Vector3.zero;
        Shadow01.transform.localPosition = new Vector3(0, -0.5867003f, 0);
        Shadow02.transform.localPosition = new Vector3(0, -0.54f, 0);
        NowState = State.idle;
    }

    public void Return()
    {
        NowState = State.Return;
    }

    public void Blast()
    {
        BlastObj = Instantiate(BlastPrefabs, Colision.transform.position , Quaternion.identity);
        PinecoBlast p = BlastObj.transform.GetChild(0).GetComponent<PinecoBlast>();
        p.Baby = this;
        p.BabyLevel = BabyLevel();
        NowState = State.Blast;
    }

}
