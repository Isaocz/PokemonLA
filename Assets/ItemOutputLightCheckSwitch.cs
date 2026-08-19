using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutputLightCheckSwitch : LightCheckSwitch
{
    /// <summary>
    /// 输出道具
    /// </summary>
    public GameObject OutputItem;

    /// <summary>
    /// 输出位置
    /// </summary>
    public Vector3 OutputPosition;

    public override void SucessEvent()
    {
        base.SucessEvent();
        Instantiate(OutputItem, transform.position + OutputPosition, Quaternion.identity  );
    }
}
