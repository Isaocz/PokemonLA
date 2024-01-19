using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilceryBaby : FollowBaby
{
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
    public override void FollowBabyShot(Vector2Int Dir) { }
}
