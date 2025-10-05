using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoruaFakeTear : MonoBehaviour
{
    List<Empty> AlreadySPDDown = new List<Empty> { };
    Baby ParentBaby;

    private void Start()
    {
        ParentBaby = transform.parent.GetComponent<Baby>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.isTrigger)
        {
            if (other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if ((e != null) && !AlreadySPDDown.Contains(e))
                {
                    AlreadySPDDown.Add(e);
                    if (e.SpDUpLevel > -8) {
                        e.SpDChange(((ParentBaby.player.playerData.IsPassiveGetList[87]) ? -2 : -1), 10f);
                    }
                    if (e.DefUpLevel > -8) {
                        e.DefChange(((ParentBaby.player.playerData.IsPassiveGetList[87]) ? -2 : -1), 10f);
                    }
                    Timer.Start(this , 8.0f , ()=> { if (e != null && AlreadySPDDown.Contains(e)) { AlreadySPDDown.Remove(e); } });
                }
            }
        }
    }
}
