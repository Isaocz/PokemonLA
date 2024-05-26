using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownBuildNowBoard : TownBillboard
{
    // Start is called before the first frame update
    void Start()
    {
        BillboardStart();
        animator.SetInteger("State" , Random.Range(0,4) );
    }

    private void Update()
    {
        BillboardUpdate();
    }

}
