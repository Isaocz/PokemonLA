using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornBF : MonoBehaviour
{
    public GameObject BF;

    private void Start()
    {
        if (Random.Range(0.0f,1.0f) < 0.1f)
        {
            Instantiate(BF , transform.position + new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f), 0) , Quaternion.identity , transform);
            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                Instantiate(BF, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity, transform);
                if (Random.Range(0.0f, 1.0f) < 0.2f)
                {
                    Instantiate(BF, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity, transform);
                }
            }
            
        }
    }
}
