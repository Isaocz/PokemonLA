using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExpCandy : MonoBehaviour
{
    public GameObject Lcandy;
    public GameObject Mcandy;
    public GameObject Scandy;
    public GameObject OutPut;


    private void Start()
    {
        float RandomPoint = Random.Range(0.0f,1.0f);
        if(RandomPoint < 0.82f)
        {
            OutPut = Instantiate(Scandy , transform.position , Quaternion.identity , transform);
        }
        else if(RandomPoint >= 0.85f && RandomPoint < 0.96f)
        {
            OutPut = Instantiate(Mcandy, transform.position, Quaternion.identity, transform);
        }
        else
        {
            OutPut = Instantiate(Lcandy, transform.position, Quaternion.identity, transform);
        }
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }

}
