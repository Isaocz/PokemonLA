using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualWingbeatSon : MonoBehaviour
{
    public GameObject TackleBlast;
    DualWingbeat ParentDualWingbeat;


    private void Start()
    {
        ParentDualWingbeat = transform.parent.GetComponent<DualWingbeat>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
                ParentDualWingbeat.HitAndKo(target);
                if (ParentDualWingbeat.SkillFrom == 2) {
                    ParentDualWingbeat.AddSubDW();
                }
            }
        }
    }
}
