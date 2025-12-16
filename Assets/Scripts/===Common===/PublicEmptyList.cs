using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEmptyList : MonoBehaviour
{
    
    /// <summary>
    /// 所有敌人列表
    /// </summary>
    public static PublicEmptyList PrefabsEmptyList;

    public List<Empty> EmptyList;
    
    /// <summary>
    /// 寻找目标敌人
    /// </summary>
    /// <returns></returns>
    public Empty FoundEmpty(string emptyCD)
    {
        Empty output = null;
        for (int i = 0; i < EmptyList.Count; i++)
        {
            if (EmptyList[i].EmptyCD == emptyCD)
            {
                return EmptyList[i];
            }
        }
        return output;
    }

    private void Awake()
    {
        PrefabsEmptyList = this;
    }
}
