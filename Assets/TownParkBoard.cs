using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownParkBoard : MonoBehaviour
{
    Animator animator;


    enum BoardState
    {
        LDown,
        RDown,
        L2R,
        R2L,
    }

    public float RWeight
    {
        get { return rweight; }
        set { rweight = value; }
    }
    public float rweight = 0.0f;


    public float LWeight
    {
        get { return lweight; }
        set { lweight = value; }
    }
    public float lweight = 0.1f;



    BoardState State;

    public ParkBoardSet LSet;
    public ParkBoardSet RSet;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        State = BoardState.LDown;
        LSet.enabled = true;
        RSet.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //跷跷板被踢起
    public void Jump()
    {
        if (State == BoardState.LDown)
        {
            animator.SetTrigger("(2)");
            State = BoardState.L2R;
            LSet.enabled = false;
            RSet.enabled = false;
        }
        else if (State == BoardState.RDown)
        {
            animator.SetTrigger(")2(");
            State = BoardState.R2L;
            LSet.enabled = false;
            RSet.enabled = false;
        }
    }

    //跷跷板检测当前重量
    public void JudgeWeirht()
    {
        if (State == BoardState.LDown && RWeight > LWeight)
        {
            animator.SetTrigger("(2)");
            State = BoardState.L2R;
            LSet.enabled = false;
            RSet.enabled = false;
        }
        else if (State == BoardState.RDown && RWeight < LWeight)
        {
            animator.SetTrigger(")2(");
            State = BoardState.R2L;
            LSet.enabled = false;
            RSet.enabled = false;
        }
    }

    public void StateLDown()
    {
        State = BoardState.LDown;
        LSet.enabled = true;
        RSet.enabled = false;
    }

    public void StateRDown()
    {
        State = BoardState.RDown;
        LSet.enabled = false;
        RSet.enabled = true;
    }

}
