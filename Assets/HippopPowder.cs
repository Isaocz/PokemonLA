using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippopPowder : MonoBehaviour
{
    //0¶¾·Û 1Âé±Ô·Û 2Ë¯Ãß·Û
    public int PowderType;

    // Start is called before the first frame update

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
                        e.EmptyToxicDone(0.15f , 20 , 1);
                        break;
                    case 1:
                        e.EmptyParalysisDone(0.15f, 10f, 1);
                        break;
                    case 2:
                        e.EmptySleepDone(0.15f, 7.5f, 1);
                        break;
                }
            }
            
        }
    }
}
