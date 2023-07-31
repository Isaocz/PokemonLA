using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEffect : MonoBehaviour
{
    /// <summary>
    /// ���ڴ��һЩ��Ҫʵ�����Ĺ�����Ч
    /// </summary>
    public static PublicEffect StaticPublicEffectList;

    private void Awake()
    {
        StaticPublicEffectList = this;
    }

    /// <summary>
    /// ��Ҫ��õ���Ч{0�����ĵ���Ч}
    /// </summary>
    /// <param name="EffectIndex"></param>
    /// <returns></returns>
    public GameObject ReturnAPublicEffect(int EffectIndex)
    {
        return transform.GetChild(EffectIndex).gameObject;
    }
}
