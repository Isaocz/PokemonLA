using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlidePS : MonoBehaviour
{
    RockSlideSkill ParentRS;
    List<Empty> TargetList = new List<Empty> {  };

    // Start is called before the first frame update
    void Start()
    {
        ParentRS = transform.parent.GetComponent<RockSlideSkill>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if(target != null)
            {
                ParentRS.HitAndKo(target);
                if (!TargetList.Contains(target))
                {
                    TargetList.Add(target);
                    if(Random.Range(0.0f,1.0f) + ((float)ParentRS.player.LuckPoint/30) > 0.7f)
                    {
                        target.Fear(2.5f,1);
                    }
                }

            }
        }
    }

}
