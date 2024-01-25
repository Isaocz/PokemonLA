using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlimmetBaby : FollowBaby
{

    public bool isDropDone;

    // Start is called before the first frame update
    void Start()
    {
        FollowBabyStart();
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
    }

    public override void InANewRoomEvent()
    {
        base.InANewRoomEvent();
        isDropDone = false;
    }

}
