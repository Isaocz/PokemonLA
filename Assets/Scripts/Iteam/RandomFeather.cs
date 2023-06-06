using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFeather : MonoBehaviour
{
    public GameObject FeatherList;
    public GameObject OutPut;

    private void Start()
    {
        int RandomPoint = Random.Range(0,FeatherList.transform.childCount);
        OutPut = Instantiate(FeatherList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}
