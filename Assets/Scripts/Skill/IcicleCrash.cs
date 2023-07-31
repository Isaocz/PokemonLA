using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleCrash : Skill
{

    public GameObject IcicleCrashOBJ;

    // Start is called before the first frame update
    void Start()
    {
        if(SkillFrom == 2)
        {
            Instantiate(IcicleCrashOBJ, transform.position, Quaternion.identity, MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(7).gameObject.transform).transform.GetChild(0).GetComponent<IcicleCrashOBJ>().isUnBreakable = true;
        }
        else
        {
            Instantiate(IcicleCrashOBJ, transform.position, Quaternion.identity, MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(7).gameObject.transform);
        }
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if(target != null)
            {
                HitAndKo(target);
                if(Random.Range(0.0f , 1.0f) + ((float)player.LuckPoint/30) >= 0.7f)
                {
                    target.Fear(2.5f, 1);
                }
            }
        }
    }
}
