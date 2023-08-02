using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkillItem : MonoBehaviour
{
    public GameObject SkillItemList;
    public GameObject OutPut;
    public bool isLunch;

    private void Start()
    {
        int RandomPoint = (int)(Random.Range(0.0f, 4.0f));
        if (RandomPoint >= 3) { RandomPoint = 0; }
        OutPut = Instantiate(SkillItemList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        if (isLunch) { OutPut.GetComponent<IteamPickUp>().isLunch = true; }
        Destroy(gameObject);
    }
}
