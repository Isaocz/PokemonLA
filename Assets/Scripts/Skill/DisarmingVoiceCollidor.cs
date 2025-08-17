using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmingVoiceCollidor : MonoBehaviour
{
    DisarmingVoice ParentDV;

    private void Awake()
    {
        ParentDV = gameObject.transform.parent.GetComponent<DisarmingVoice>();
    }

    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            ParentDV.HitAndKo(target);
            ParentDV.SkillFrom02();
        }
    }
}
