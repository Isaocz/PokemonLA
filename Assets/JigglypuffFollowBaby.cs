using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglypuffFollowBaby : FollowBaby
{
    PlayerControler player;
    float SweetKissCD;
    bool isSweetKissLunch;
    public Projectile SweetKiss;


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
        if (isSweetKissLunch)
        {
            SweetKissCD += Time.deltaTime;
            if (SweetKissCD >= 1.8f) { SweetKissCD = 0; isSweetKissLunch = false; }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isSweetKissLunch)
        {

            if(other.gameObject.tag == "Empty")
            {
                if (other.gameObject.tag == "Empty")
                {
                    Projectile p = Instantiate(SweetKiss, transform.position, Quaternion.identity);
                    p.LaunchNotForce((new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y).normalized), 5);
                    isSweetKissLunch = true;
                }
            }
        }
    }

}
