using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltEffect : MonoBehaviour
{
    [Tooltip("闪电最大距离"), SerializeField]
    private float MaxRange = 1;


    [Tooltip("闪电股数"), SerializeField]
    private float MaxCount = 1;


    [Tooltip("闪电最大宽度"), SerializeField]
    private float MaxWidth = 1;

    /// <summary>
    /// 单股闪电预制件
    /// </summary>
    public DynamicLightningBoltItem LightningItem;



    private void Start()
    {
        for (int i = 0; i < MaxCount;i++ )
        {
            DynamicLightningBoltItem l = Instantiate(LightningItem , transform.position , Quaternion.identity , transform);
            l.MaxRange = MaxRange;
            l.MaxWidth = MaxWidth;
        }
    }

}
