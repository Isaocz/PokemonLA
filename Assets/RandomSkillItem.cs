using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkillItem : MonoBehaviour
{
    public GameObject SkillItemList;
    public GameObject OutPut;


    private void Start()
    {
        int RandomPoint = (int)(Random.Range(0.0f, 3.0f));
        OutPut = Instantiate(SkillItemList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}
