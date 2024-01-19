using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogepiBaby : FollowBaby
{
    float ProjectleCD;
    bool isProjectleLunch;
    public Projectile WaterGun;
    public Projectile Confusion;
    public Projectile Thunder;
    public Projectile Fire;
    public Projectile Spike;
    public Projectile ToxicSpike;
    public Projectile Seed;
    public Projectile Rock;
    public Projectile IceSpike;
    public Projectile Scale;

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
        if (isProjectleLunch)
        {
            ProjectleCD += Time.deltaTime;
            if (ProjectleCD >= 1.2f) { ProjectleCD = 0; isProjectleLunch = false; }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isProjectleLunch)
        {

            if (other.gameObject.tag == "Empty")
            {
                if (other.gameObject.tag == "Empty")
                {
                    Vector2 d = (new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y)).normalized;
                    Projectile p = Instantiate(RandomPro(), transform.position, Quaternion.identity);
                    p.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360Y(d, Vector3.right), Vector3.forward);
                    p.LaunchNotForce(d, 10);
                    p.Baby = this;
                    p.BabyLevel = BabyLevel();
                    isProjectleLunch = true;
                }
            }
        }
    }

    Projectile RandomPro()
    {
        Projectile OutPut = WaterGun;
        switch (Random.Range(0,10))
        {
            case 0:
                OutPut = WaterGun;
                break;
            case 1:
                OutPut = Fire;
                break;
            case 2:
                OutPut = Seed;
                break;
            case 3:
                OutPut = Scale;
                break;
            case 4:
                OutPut = Spike;
                break;

            case 5:
                OutPut = ToxicSpike;
                break;
            case 6:
                OutPut = IceSpike;
                break;
            case 7:
                OutPut = Thunder;
                break;
            case 8:
                OutPut = Rock;
                break;
            case 9:
                OutPut = Confusion;
                break;
        }
        return OutPut;
    }

    public override void FollowBabyShot(Vector2Int Dir) { }
}
