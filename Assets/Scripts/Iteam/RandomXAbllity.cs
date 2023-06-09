using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomXAbllity : MonoBehaviour
{
    public GameObject SpaceItemList;
    public GameObject OutPut;

    private void Start()
    {
        int RandomPoint = (int)(Random.Range(2.1f, 2.7f) * 10);//5.
        OutPut = Instantiate(SpaceItemList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}
