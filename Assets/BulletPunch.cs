using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPunch : Skill
{
    Empty target;
    public GameObject TackleBlast;
    BulletPunch ParentBulletPunch;

    private void Start()
    {
        ParentBulletPunch = GetComponent<BulletPunch>();
    }

    // Start is called before the first frame update

    void Update()
    {

        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentBulletPunch != null) { ParentBulletPunch.HitAndKo(target); Instantiate(TackleBlast, other.transform.position, Quaternion.identity); }
        }
        else if (other.tag == "Projectel")
        {
            Destroy(other.gameObject);
            Instantiate(TackleBlast, other.transform.position, Quaternion.identity);
            if (SkillFrom == 2 && player.playerData.DefBounsJustOneRoom <= 8) { player.playerData.DefBounsJustOneRoom++; }
        }
    }
}
