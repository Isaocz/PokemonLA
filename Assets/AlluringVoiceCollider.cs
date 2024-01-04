using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlluringVoiceCollider : MonoBehaviour
{
    AlluringVoice ParentDV;
    private void Awake()
    {
        ParentDV = gameObject.transform.parent.GetComponent<AlluringVoice>();
    }

    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            ParentDV.HitAndKo(target);
            if (target.SpAUpLevel > 0 || target.SpDUpLevel > 0 || target.AtkUpLevel > 0 || target.DefUpLevel > 0) 
            {
                target.ConfusionFloatPlus(1);
            }
            ParentDV.SkillFrom02();
        }
    }
}
