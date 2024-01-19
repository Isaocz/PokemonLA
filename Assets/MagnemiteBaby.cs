using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnemiteBaby : FollowBaby
{
    public Projectile MagnemiteThunder;

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
        Projectile p = Instantiate(MagnemiteThunder, transform.position, Quaternion.identity);
        p.LaunchNotForce(Dir, 12.5f);
        p.transform.rotation = Quaternion.Euler(0,0,0);
        p.Baby = this;
        p.BabyLevel = BabyLevel();
    }
}
