using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJumpKickCollidor : MonoBehaviour
{
    HighJumpKick ParentHJK;

    private void Start()
    {
        ParentHJK = transform.parent.GetComponent<HighJumpKick>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            if (ParentHJK != null)
            {
                ParentHJK.isHJKHitDone = true;
                ParentHJK.HitAndKo(other.gameObject);
            }

        }
    }
}
