using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazzlingGleam : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null) {
                HitAndKo(target);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.5f)
                {
                    player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.蓝色增加特攻型);
                }
                else
                {
                    player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型);
                }
            }
        }
    }
}
