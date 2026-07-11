using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeButton : EnviromentButton
{
    /// <summary>
    /// ÑÌÎíÔ¤ÖÆ¼þ
    /// </summary>
    public SmokeButtonSmoke SmokePrefabs;

    public override void SwitchOFFEvent()
    {
        base.SwitchOFFEvent();

    }

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        Debug.Log("SmokeButton");

        Instantiate(SmokePrefabs, transform.position, Quaternion.identity);
    }
}
