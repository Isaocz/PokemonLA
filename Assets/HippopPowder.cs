using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippopPowder : MonoBehaviour
{
    //0¶¾·Û 1Âé±Ô·Û 2Ë¯Ãß·Û
    public int PowderType;

    // Start is called before the first frame update

    Baby ParentBaby;

    private void Start()
    {
        ParentBaby = transform.parent.parent.GetComponent<Baby>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                switch (PowderType)
                {
                    case 0:
                        e.EmptyToxicDone(  ((ParentBaby.player.playerData.IsPassiveGetList[87]) ? 0.3f : 0.15f)  , 20 , 1);
                        break;
                    case 1:
                        e.EmptyParalysisDone(((ParentBaby.player.playerData.IsPassiveGetList[87]) ? 0.3f : 0.15f), 10f, 1);
                        break;
                    case 2:
                        e.EmptySleepDone(((ParentBaby.player.playerData.IsPassiveGetList[87]) ? 0.3f : 0.15f), 7.5f, 1);
                        break;
                }
            }
            
        }
    }
}
