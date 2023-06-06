using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPotion : MonoBehaviour
{
    public GameObject CCG;
    public GameObject Potion;
    public GameObject SuperPotion;
    public GameObject HyperPotion;
    public GameObject MaxPotion;
    public GameObject OutPut;


    private void Start()
    {
        float RandomPoint = Random.Range(0.0f, 1.0f);
        if (RandomPoint < 0.25f)
        {
            OutPut = Instantiate(CCG, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.25f && RandomPoint < 0.83f)
        {
            OutPut = Instantiate(Potion, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.83f && RandomPoint < 0.92f)
        {
            OutPut = Instantiate(SuperPotion, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.92f && RandomPoint < 0.96f)
        {
            OutPut = Instantiate(HyperPotion, transform.position, Quaternion.identity, transform);
        }
        else
        {
            OutPut = Instantiate(MaxPotion, transform.position, Quaternion.identity, transform);
        }
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}
