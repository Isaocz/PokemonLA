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
        float RandomPoint = (Random.Range(0.0f, 6.0f));
        if (RandomPoint < 6.0f && RandomPoint >= 5.0f)
        {
            OutPut = Instantiate(SkillItemList.transform.GetChild(2), transform.position, Quaternion.identity, transform).gameObject;
        }
        else if (RandomPoint <= 5.0f && RandomPoint >= 3.0f)
        {
            OutPut = Instantiate(SkillItemList.transform.GetChild(1), transform.position, Quaternion.identity, transform).gameObject;
        }
        else
        {
            OutPut = Instantiate(SkillItemList.transform.GetChild(0), transform.position, Quaternion.identity, transform).gameObject;
        }
        OutPut.transform.parent = transform.parent;
        if (isLunch) { OutPut.GetComponent<IteamPickUp>().isLunch = true; }
        Destroy(gameObject);
    }
}
