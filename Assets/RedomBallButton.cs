using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedomBallButton : EnviromentButton
{

    static float PERCENT_Ball = 0.5f;

    /// <summary>
    /// 掉落精灵球
    /// </summary>
    public PokemonBall Ball;

    /// <summary>
    /// 掉落雷电球
    /// </summary>
    public Voltorb voltorb;

    /// <summary>
    /// 所在房间
    /// </summary>
    //public Room ParentRoom;

    PlayerControler player;



    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("RemoveSpikeButton");
        ParentRoom = FindParentRoom();
        if (ParentRoom != null)
        {


            //随机位置
            Vector2 p = (Vector2)transform.position + (Vector2)(Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector2.up * 2.0f);
            p = ParentRoom.EnsurePointReachesRoom(p);
            int c = 0;
            while (!_mTool.isThisPointEmpty(p))
            {
                p = (Vector2)transform.position + (Vector2)(Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector2.up * 2.0f);
                p = ParentRoom.EnsurePointReachesRoom(p);
                c++;
                if (c >= 10) { p = (Vector2)transform.position; break; }
            }


            float r = Random.Range(0.0f, 1.0f);
            Debug.Log(r);
            if (r < PERCENT_Ball)
            {
                Debug.Log("随机结果：球");
                Instantiate(Ball, p, Quaternion.identity);
            }
            else
            {
                Debug.Log("随机结果：雷电球");
                Instantiate(voltorb, p, Quaternion.identity, ParentRoom.EmptyFile());
                ParentRoom.isClear += 1;

                //Debug.Log("最终随机敌人：" + randomEnemy.name);
            }

        }
    }

}
