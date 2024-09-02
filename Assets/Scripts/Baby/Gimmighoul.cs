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
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowBabyUpdate();
        if(!BornAMoney && MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear <= 0)
        {
            if (Random.Range(0.0f , 1.0f) + ((float)player.LuckPoint/30) > ((player.playerData.IsPassiveGetList[87]) ? 0.5f : 0.7f)   ) { Instantiate(RandomMoney, transform.position, Quaternion.identity); } 
            BornAMoney = true;
        }
        if (MapCreater.StaticMap.RRoom[TargetPlayer.NowRoom].isClear > 0)
        {
            BornAMoney = false;
        }
    }
    public override void FollowBabyShot(Vector2Int Dir) { }
}
