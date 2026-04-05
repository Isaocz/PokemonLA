using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCollidor : MonoBehaviour
{
    Bounce ParentBo;

    private void Start()
    {
        ParentBo = transform.parent.GetComponent<Bounce>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (ParentBo != null) { ParentBo.isBoHitDone = true; ParentBo.HitAndKo(other.gameObject); }
            if (e != null)
            {
                if (ParentBo != null)
                {
                    e.EmptyParalysisDone(1, 10, 0.3f + ((float)ParentBo.player.LuckPoint / 30));
                }
            }
        }
    }
}
