using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaKick : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 2.0f)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                if (SkillFrom == 2 && ExistenceTime > 0.5f) { ExistenceTime = 0.4f; }
            }
        }
    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;


    }
}
