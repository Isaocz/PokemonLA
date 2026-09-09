using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemButton : EnviromentButton
{



    //随机概率 道具 敌人 TP
    static List<float> Percent = new List<float> { 0.4f, 0.4f, 0.25f };

    /// <summary>
    /// 随机掉落物
    /// </summary>
    public RandomDropItem dropItem;

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
        Debug.Log("ItemButton");
        ParentRoom = FindParentRoom();
        if (ParentRoom != null) {
            float total = 0.0f;
            for (int i = 0; i < Percent.Count; i++) { total += Percent[i]; }
            float r = Random.Range(0.0f, total);

            float cumulative = 0f;

            int index = -1;

            for (int i = 0; i < Percent.Count; i++)
            {
                cumulative += Percent[i];
                if (r <= cumulative)
                {
                    index = i;
                    break;
                }
            }



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

            // index 就是随机结果：0=道具, 1=敌人, 2=TP
            switch (index)
            {
                case 0:
                    Debug.Log("随机结果：道具");
                    Instantiate(dropItem , p , Quaternion.identity);
                    break;
                case 1:
                    Debug.Log("随机结果：敌人");
                    // 复制静态列表，避免修改原始数据
                    List<Empty> elist = new List<Empty>(PublicEmptyList.PrefabsEmptyList.EmptyList);

                    // 过滤掉所有 BossLevel != NormalEmpty 的对象
                    elist = elist.Where(e => e.EmptyBossLevel == Empty.emptyBossLevel.NormalEmpty).ToList();

                    // 如果过滤后没有元素，避免报错
                    if (elist.Count == 0)
                    {
                        Debug.LogWarning("没有符合 NormalEmpty 的敌人！");
                        return;
                    }

                    // 随机取一个
                    int r1 = Random.Range(0, elist.Count);
                    Empty randomEnemy = elist[r1];
                    Instantiate(randomEnemy, p, Quaternion.identity, ParentRoom.EmptyFile());
                    ParentRoom.isClear += 1;

                    Debug.Log("最终随机敌人：" + randomEnemy.name);
                    break;
                case 2:
                    Debug.Log("随机结果：TP");
                    if (player == null)
                    {
                        player = GameObject.FindObjectOfType<PlayerControler>();
                    }
                    if (player == null) { break; }
                    //Debug.Log(MapCreater.StaticMap.VRoom.Count);
                    Vector3Int v = MapCreater.StaticMap.VRoom.Keys.ElementAt(Random.Range(0, MapCreater.StaticMap.VRoom.Count));
                    player.TP(v);

                    break;
            }
        }
    }
}
