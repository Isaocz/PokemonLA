using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireSuperChargePunchCollider : ElectivireDynamicPunchCollider
{
    // Start is called before the first frame update
    void Start()
    {
        ParentElectivire.LunchThuder(3, transform.position);
    }
}
