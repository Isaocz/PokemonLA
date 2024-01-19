using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlimmetBabySpikeDrop : Substitute
{

    public GameObject ToxicSpike;
    GlimmetBaby ParentGlimmet;

    private void Start()
    {
        ParentGlimmet = transform.GetComponent<GlimmetBaby>();
    }

    public override void SubStituteChangeHp(float ChangePoint, float ChangePointSp, int SkillType)
    {
        if (!ParentGlimmet.isDropDone) {
            ParentGlimmet.isDropDone = true;
            Instantiate(ToxicSpike, transform.position, Quaternion.identity);
            if (Random.Range(0.0f , 1.0f) > 0.5f) { Instantiate(ToxicSpike, transform.position + Vector3.up, Quaternion.identity); }
            if (Random.Range(0.0f , 1.0f) > 0.5f) { Instantiate(ToxicSpike, transform.position + Vector3.down, Quaternion.identity); }
            if (Random.Range(0.0f , 1.0f) > 0.5f) { Instantiate(ToxicSpike, transform.position + Vector3.right, Quaternion.identity); }
            if (Random.Range(0.0f , 1.0f) > 0.5f) { Instantiate(ToxicSpike, transform.position + Vector3.left, Quaternion.identity); }
        }
    }
}
