using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpaceItem : MonoBehaviour
{
    public GameObject SpaceItemList;
    public GameObject OutPut;


    private void Start()
    {
        int RandomPoint = (int)(Random.Range(4.8f, 5.7f) * 10);//5.8f
        if (RandomPoint <= 50) { RandomPoint -= 48; }
        OutPut = Instantiate(SpaceItemList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}
