using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyButterFlyEmptyFinder : MonoBehaviour
{

    FairyButterfly ParentBF;

    private void Start()
    {
        ParentBF = transform.parent.GetComponent<FairyButterfly>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" && !ParentBF.isAttack && !ParentBF.isCanNotAttack)
        {
            Debug.Log("xxxxx");
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentBF.Target = e;
                ParentBF.isAttack = true;
            }
        }
    }
}
