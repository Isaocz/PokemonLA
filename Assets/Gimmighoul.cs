using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmighoul : FollowBaby
{

    public GameObject RandomMoney;
    bool BornAMoney;

    // Start is called before the first frame update
    void Start()
    {
        FollowBabyStart();
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
        if(!BornAMoney && MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear == 0)
        {
            Instantiate(RandomMoney, transform.position, Quaternion.identity);
            BornAMoney = true;
        }
        if (MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear != 0)
        {
            BornAMoney = false;
        }
    }
}
