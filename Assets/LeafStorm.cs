using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafStorm : Skill
{
    public float zAngle;
    private bool isDown;
    // Start is called before the first frame update
    void Start()
    {
        isDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (!isDown)
                {
                    if (SkillFrom == 0)
                    {
                        player.playerData.SpABounsJustOneRoom -= 2;
                    }
                    else if (SkillFrom == 2)
                    {
                        player.playerData.SpABounsJustOneRoom -= 1;
                    }
                    isDown = true;
                }
            }
        }
    }
}
