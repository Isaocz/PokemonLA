using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuppetBaby : FollowBaby
{

    public Projectile ShadowBall;

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

    public override void FollowBabyShot(Vector2Int Dir)
    {
        base.FollowBabyShot(Dir);

        Projectile p = Instantiate(ShadowBall, transform.position, Quaternion.identity);
        p.LaunchNotForce(Dir, 7.5f);
        p.transform.rotation = Quaternion.Euler(0, 0, 0);
        p.Baby = this;
        p.BabyLevel = BabyLevel();



    }
}
