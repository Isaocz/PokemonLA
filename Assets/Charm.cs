using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : Skill
{
    List<Empty> AtkDownTargetList = new List<Empty> { };

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime < 2.4f) { transform.GetComponent<Collider2D>().enabled = false; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" && other.GetComponent<Empty>() != null)
        {
            Empty target = other.GetComponent<Empty>();
            if (!AtkDownTargetList.Contains(target))
            {
                Debug.Log(target.AtkAbilityPoint + "+" + target);
                AtkDownTargetList.Add(target);
                target.AtkDown(0);
                target.AtkAbilityPoint *= 0.6f;
                if (SkillFrom == 2) { target.EmptyInfatuation(15,1); }
            }    
        }
    }

    private void OnDestroy()
    {
        for (int i = 0 ; i < AtkDownTargetList.Count; i++)
        {
            AtkDownTargetList[i].AtkDownRemove();
            AtkDownTargetList[i].AtkAbilityPoint /= 0.6f;
            Debug.Log(AtkDownTargetList[i].AtkAbilityPoint + "+" + AtkDownTargetList[i]);
        }
    }
}
