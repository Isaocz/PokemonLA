using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudSlupEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    MudSlup ParentMudSlup;
    SubMudSlupPlus ParentSubMudSlup;

    private void Start()
    {
        if(gameObject.transform.parent.GetComponent<MudSlup>() != null)
        {
            ParentMudSlup = gameObject.transform.parent.GetComponent<MudSlup>();
        }
        if (gameObject.transform.parent.GetComponent<SubMudSlupPlus>() != null)
        {
            ParentSubMudSlup = gameObject.transform.parent.GetComponent<SubMudSlupPlus>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            
            target = other.GetComponent<Empty>();
            if (ParentMudSlup != null) { ParentMudSlup.HitAndKo(target); }
            if (ParentSubMudSlup != null) { ParentSubMudSlup.HitAndKo(target); }
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.Blind(3, 1);
            }
        }
    }
}
