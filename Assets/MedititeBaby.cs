using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedititeBaby : FollowBaby
{
    public TogepiWaterGun Confusion;

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

    public override void FollowBabyShot(Vector2Int Dir)
    {
        base.FollowBabyShot(Dir);

        Projectile p = Instantiate(Confusion, transform.position, Quaternion.identity);
        p.LaunchNotForce(Dir, 8.0f);
        p.transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(new Vector2(Dir.x, Dir.y), Vector2.right));
        p.Baby = this;
        p.BabyLevel = BabyLevel();



    }
}
