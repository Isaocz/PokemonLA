using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireLightningStrikeManger : MonoBehaviour
{
    /// <summary>
    /// 父电击魔兽
    /// </summary>
    public Electivire ParentElectivire;

    /// <summary>
    /// 生成雷电列表
    /// </summary>
    public Dictionary<float, List<Vector2>> ThunderList;

    /// <summary>
    /// 闪电
    /// </summary>
    public ElectivireLightning Lightning;

    /// <summary>
    /// 音效
    /// </summary>
    public GameObject SEObj;




    bool isStart = false;

    float thunderTimer = 0.0f;


    public void SetThunderList(Dictionary<float, List<Vector2>> oldD , Electivire e)
    {
        //深拷贝
        ThunderList = new Dictionary<float, List<Vector2>>();
        foreach (var kv in oldD)
        {
            // 深拷贝 List<Vector2>
            var newList = new List<Vector2>(kv.Value);

            // 加入新字典
            ThunderList[kv.Key] = newList;
        }
        //设置敌人
        ParentElectivire = e;
    }

    /// <summary>
    /// 随机在敌人周围生成闪电
    /// </summary>
    /// <param name="room"></param>
    /// <param name="count">闪电个数</param>
    /// <param name="e"></param>
    /// <param name="Dis">距离敌人距离</param>
    public void RandomSetThunder(int count , Electivire e , float Dis , float time)
    {
        //设置敌人
        ParentElectivire = e;

        ThunderList = new Dictionary<float, List<Vector2>>();
        transform.position = e.ParentPokemonRoom.transform.position;

        List<Vector2> RandomList = new List<Vector2> { };
        for (int i = 0; i < count; i++)
        {
            Vector2 v = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right * Random.Range(3.0f, Dis);
            v = new Vector2(
            Mathf.Clamp(v.x,                    //方向*速度
            e.ParentPokemonRoom.RoomSize[2]  , //最小值
            e.ParentPokemonRoom.RoomSize[3]  ),//最大值
            Mathf.Clamp(v.y,                     //方向*速度 
            e.ParentPokemonRoom.RoomSize[1]  ,  //最小值
            e.ParentPokemonRoom.RoomSize[0]  ));//最大值
            RandomList.Add(v);
        }
        ThunderList[time] = RandomList;
    }



    private void Update()
    {
        thunderTimer += Time.deltaTime;
        if (ThunderList.Keys.Count != 0)
        {
            var keys = new List<float>(ThunderList.Keys);
            for (int i = 0; i < ThunderList.Keys.Count; i++)
            {
                float key = keys[i];
                if (thunderTimer >= key)
                {
                    SetOneThunder(ThunderList[key]);
                    ThunderList.Remove(key);
                }

            }
        }
        else
        {
            Timer.Start(this , 3.0f , ()=> { Destroy(gameObject); });
        }


        //音频生成间隔
        if (SEIntervalTimer <= TIME_SE_INTERVAL)
        {
            SEIntervalTimer += Time.deltaTime;
        }

    }


    void SetOneThunder(List<Vector2> l)
    {
        InstantiateSE();
        for (int i = 0; i < l.Count; i++)
        {
            SetOneThunder(l[i]);
        }
    }

    void SetOneThunder(Vector2 p)
    {
        if (ParentElectivire != null)
        {
            ElectivireLightning l = Instantiate(Lightning, (Vector3)p + transform.position, Quaternion.identity, transform);
            l.ParentElectivire = ParentElectivire;
        }
    }


    //生成音效的间隔
    static float TIME_SE_INTERVAL = 0.15f;

    //生成音效的间隔
    float SEIntervalTimer = 100.0f;

    /// <summary>
    /// 生成音效
    /// </summary>
    void InstantiateSE()
    {
        if (SEIntervalTimer > TIME_SE_INTERVAL)
        {
            SEIntervalTimer = 0;
            Instantiate(SEObj, transform.position, Quaternion.identity, transform);
        }
    }


}
