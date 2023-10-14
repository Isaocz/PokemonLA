using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubZenHeadbutt : SubSkill
{
    TraceEffect TE;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in MainSkill.transform)
        {
            if (t.GetComponent<TraceEffect>()) {
                TE = t.GetComponent<TraceEffect>();
                break; 
            }
        }
        if (TE == null) { TE = MainSkill.GetComponent<TraceEffect>(); }
        if (TE != null)
        {
            TE.distance += 1.3f;
            if (MainSkill.Damage != 0) { MainSkill.Damage += 20f; }
            if (MainSkill.SpDamage != 0) { MainSkill.SpDamage += 20f; }
            player.RemoveASubSkill(subskill);
        }
        Destroy(gameObject);

    }
}
