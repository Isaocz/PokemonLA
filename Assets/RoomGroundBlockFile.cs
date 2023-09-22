using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGroundBlockFile : MonoBehaviour
{
    // Start is called before the first frame update
    public float[] BlockWeight = new float[] { };
    
    public int OutPutWeightIndex(float RandomFloat)
    {
        int OutPut = -1;
        float counter = 0;
        for (int i = 0; i < BlockWeight.Length; i++)
        {
            counter += BlockWeight[i];
            if (RandomFloat <= counter)
            {
                OutPut = i;
                break;
            }
        }
        return OutPut;
    }
}
