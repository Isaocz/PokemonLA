using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySlamCollider : MonoBehaviour
{
    Skill ParentBodySlam;

    private void Start()
    {
        ParentBodySlam = transform.parent.GetComponent<Skill>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentBodySlam.HitAndKo(e);
                if (ParentBodySlam.SkillFrom == 2)
                {
                    if (Random.Range(0.0f, 1.0f) + ((float)ParentBodySlam.player.LuckPoint / 30) > 0.5f)
                    {
                        e.EmptyParalysisDone(1, 10);
                    }
                }
                else
                {
                    if (Random.Range(0.0f, 1.0f) + ((float)ParentBodySlam.player.LuckPoint / 30) > 0.7f)
                    {
                        e.EmptyParalysisDone(1, 10);
                    }
                }

            }
        }
    }
}
