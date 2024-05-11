using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFuryCutter : SubSkill
{

    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        if (MainSkill.SkillIndex == 457 || MainSkill.SkillIndex == 458)
        {
            MainSkill.Damage *= Count;
            MainSkill.GetComponent<FuryCutter>().Count = Count;
        }
        player.RemoveASubSkill(subskill);
        Destroy(gameObject);
    }
}
