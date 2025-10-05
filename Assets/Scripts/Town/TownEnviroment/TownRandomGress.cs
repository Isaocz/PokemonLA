using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownRandomGress : MonoBehaviour
{

    public RoomGroundBlockFile FloorFile;


    // Start is called before the first frame update
    void Start()
    {
        SetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloor()
    {
        for (int i = 0; i < 90; i++)
        {
            for (int j = 0; j < 48; j++)
            {
                float x = Random.Range(0.0f, 1.0f);
                if (FloorFile.OutPutWeightIndex(x) != -1)
                {
                    Instantiate(FloorFile.transform.GetChild(FloorFile.OutPutWeightIndex(x)), transform.position + new Vector3(i - 44.5f, j - 4.5f, 0), Quaternion.identity, transform.GetChild(0));
                }
            }
        }
    }
}
