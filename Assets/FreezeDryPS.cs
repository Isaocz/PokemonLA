using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDryPS : MonoBehaviour
{
    FreezeDry ParentFreezeDry;
    public GameObject Snow;

    // Start is called before the first frame update
    void Start()
    {
        ParentFreezeDry = transform.parent.GetComponent<FreezeDry>();
    }


    List<Empty> FrozeList = new List<Empty> { };

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if(target != null)
            {
                if (target.EmptyType01 == Type.TypeEnum.Water || target.EmptyType01 == Type.TypeEnum.Water)
                {
                    ParentFreezeDry.SpDamage *= 4;
                }

                if (!FrozeList.Contains(target))
                {
                    
                    if (ParentFreezeDry.SkillFrom == 2) { target.Cold(15); }
                    target.Frozen(7.5f, 1, 0.1f + ((float)ParentFreezeDry.player.LuckPoint / 30));

                }
                FrozeList.Add(target);

                ParentFreezeDry.HitAndKo(target);
            }
        }
    }
}
