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
                    e.EmptyParalysisDone(1, 10, 0.5f - ((float)ParentBodySlam.player.LuckPoint / 30));
                }
                else
                {
                    e.EmptyParalysisDone(1, 10, 0.3f - ((float)ParentBodySlam.player.LuckPoint / 30));
                }

            }
        }
    }
}
