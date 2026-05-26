using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeavileRush: MonoBehaviour
{
    /// <summary>
    /// 是否是分身释放的假特效
    /// </summary>
    public bool isFake = false;


    /// <summary>
    /// 父玛狃拉
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// 冲刺方向
    /// </summary>
    Vector2 Dir_Rush;

    /// <summary>
    /// 冲刺开始
    /// </summary>
    public virtual void RushStart(Vector2 d)
    {
        Dir_Rush = d;
    }


    /// <summary>
    /// 冲刺结束
    /// </summary>
    public virtual void RushOver()
    {
        Destroy(gameObject, 2.0f);
    }


}