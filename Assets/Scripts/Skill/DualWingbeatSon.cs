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
            ParentDualWingbeat.HitAndKo(other.gameObject);
            Instantiate(TackleBlast, other.transform.position, Quaternion.identity);
            if (ParentDualWingbeat.SkillFrom == 2)
            {
                ParentDualWingbeat.AddSubDW();
            }
        }
    }
}
