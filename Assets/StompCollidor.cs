using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompCollidor : MonoBehaviour
{
    Stomp ParentST;

    private void Start()
    {
        ParentST = transform.parent.GetComponent<Stomp>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                if (ParentST != null)
                {
                    ParentST.isSTHitDone = true;
                    ParentST.HitAndKo(e);
                    if ( Random.Range(0.0f , 1.0f) + ((float)ParentST.player.LuckPoint/30) > ((ParentST.SkillFrom == 2)?0.4f:0.7f ) ) { e.Fear(2.5f, 1); } 
                }
            }
        }
    }
}
