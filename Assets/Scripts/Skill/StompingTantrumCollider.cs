using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingTantrumCollider : MonoBehaviour
{
    StompingTantrum ParentST;
    SubStompingTantrum ParentSubST;

    private void Start()
    {
        ParentST = transform.parent.GetComponent<StompingTantrum>();
        ParentSubST = transform.parent.GetComponent<SubStompingTantrum>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            if (ParentST != null)
            {
                ParentST.isSTHitDone = true;
                ParentST.HitAndKo(other.gameObject);
            }
            else if (ParentSubST != null)
            {
                ParentSubST.isSTHitDone = true;
                ParentSubST.HitAndKo(other.gameObject);
            }
        }
        if ((ParentST != null && ParentST.SkillFrom == 2) || (ParentSubST != null && ParentSubST.isPlusSkill)) {
            SoftMud softMud = other.GetComponent<SoftMud>();
            if (softMud != null)
            {
                if (ParentST != null)
                {
                    Instantiate(ParentST.STMud, other.transform.position, Quaternion.identity);
                }
                else if (ParentSubST != null)
                {
                    Instantiate(ParentSubST.STMud, other.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
