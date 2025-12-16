using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlinkCircleAtkChecker : MonoBehaviour
{

    public Klink ParentKlink;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentKlink.IsCircleAtkCheckerEnable()) {
            if (
                (other.gameObject.tag == "Player" && !ParentKlink.isEmptyInfatuationDone) ||
                (other.gameObject.tag == "Empty" && ParentKlink.isEmptyInfatuationDone)
                )
            {
                ParentKlink.UseCircleAtkSingle = true;
            }
        }
    }

}
