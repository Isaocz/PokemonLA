using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmighoul : FollowBaby
{

    public GameObject RandomMoney;
    bool BornAMoney;
    PlayerControler player;

    // Start is called before the first frame update
    void Start()
    {
        FollowBabyStart();
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
        if(!BornAMoney && MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear == 0)
        {
            if (Random.Range(0.0f , 1.0f) + ((float)player.LuckPoint/30) > 0.3f) { Instantiate(RandomMoney, transform.position, Quaternion.identity); } 
            BornAMoney = true;
        }
        if (MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear != 0)
        {
            BornAMoney = false;
        }
    }
}
