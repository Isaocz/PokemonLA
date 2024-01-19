using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippopBaby : FollowBaby
{
    // Start is called before the first frame update
    void Start()
    {
        FollowBabyStart();
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
        InANewRoomEvent();
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
    }

    public override void InANewRoomEvent()
    {
        base.InANewRoomEvent();
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
        switch (Random.Range(0,3))
        {
            case 0:
                transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 1:
                transform.GetChild(4).gameObject.SetActive(true);
                break;
            case 2:
                transform.GetChild(5).gameObject.SetActive(true);
                break;
        }
    }
}
