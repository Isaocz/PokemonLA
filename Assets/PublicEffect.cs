using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEffect : MonoBehaviour
{
    /// <summary>
    /// 用于存放一些需要实例化的公共特效
    /// </summary>
    public static PublicEffect StaticPublicEffectList;

    private void Awake()
    {
        StaticPublicEffectList = this;
    }

    /// <summary>
    /// 需要获得的特效{0：会心的特效}
    /// </summary>
    /// <param name="EffectIndex"></param>
    /// <returns></returns>
    public GameObject ReturnAPublicEffect(int EffectIndex)
    {
        return transform.GetChild(EffectIndex).gameObject;
    }
}
