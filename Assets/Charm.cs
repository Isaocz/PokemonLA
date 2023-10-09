using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : Skill
{
    List<Empty> AtkDownTargetList = new List<Empty> { };

    private void Start()
    {
        if (SkillFrom == 2) {
            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型);
            if (Random.Range(0.0f , 1.0f) + (float)player.LuckPoint/30 > 0.8f ) 
            {
                player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅粉色普通型);
            }
        }
    }

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
