using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkBoardSet : TownChair
{
    public GameObject ZButtonObj;
    public bool isZEnable;
    public TownParkBoard ParentBoard;

    public bool isR;

    private void Start()
    {
        ChairsStart();
        ZButtonObj = transform.GetChild(0).gameObject;
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        ChairsJumpUp();
        ChairsSetDown(); 
        ChairsJumpDown();


        if (ChairTarget != null)
        {
            if (State == ChairState.SetDown)
            {
                if (!ChairTarget.transform.parent) { 
                    ChairTarget.transform.parent = transform; ChairTarget.transform.localRotation = Quaternion.identity;
                    if (isR) { ParentBoard.RWeight += ChairTarget.Weight; }
                    else { ParentBoard.LWeight += ChairTarget.Weight; }
                }

                if (!ChairTarget.isInZ)
                {
                    isZEnable = true;
                    ChairTarget.isInZ = true;
                }
            }
            else
            {
                if (ChairTarget.transform.parent) {
                    ChairTarget.transform.parent = null; ChairTarget.transform.rotation = Quaternion.identity;
                    if (isR) { ParentBoard.RWeight -= ChairTarget.Weight; }
                    else { ParentBoard.LWeight -= ChairTarget.Weight; }
                }

                if (isZEnable)
                {
                    ChairTarget.isInZ = false;
                    isZEnable = false;
                }
            }

            if (isZEnable && ZButton.Z.IsZButtonDown)
            {
                ParentBoard.Jump();
            }
        }
    }
}
