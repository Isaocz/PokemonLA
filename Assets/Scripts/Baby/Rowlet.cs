using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rowlet : FollowBaby
{
    float LeafCD;
    bool isLeafLunch;
    public Projectile Leaf;


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
        if (isLeafLunch)
        {
            LeafCD += Time.deltaTime;
            if (LeafCD >= 0.6f) { LeafCD = 0; isLeafLunch = false; }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isLeafLunch)
        {

            if (other.gameObject.tag == "Empty")
            {
                if (other.gameObject.tag == "Empty")
                {
                    Vector2 d = (new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y)).normalized;
                    Projectile p = Instantiate(Leaf, transform.position, Quaternion.identity);
                    p.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360(d, Vector3.up), Vector3.forward);
                    p.LaunchNotForce(d, 7);
                    p.Baby = this;
                    p.BabyLevel = BabyLevel();
                    isLeafLunch = true;
                }
            }
        }
    }

}
