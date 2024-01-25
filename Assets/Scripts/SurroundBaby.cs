using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundBaby : Baby
{
    public float RSpeed;
    public float Radius;
    float RTimer;

    public void SurroundBabyStart()
    {
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }


    public void SurroundBabyUpdate()
    {
        RTimer += Time.deltaTime * RSpeed;
        if (RTimer >= 360) { RTimer = 0; }
        Vector3 RCenter = player.transform.position + player.SkillOffsetforBodySize[0] * Vector3.up;
        float RidusPlus = Mathf.Max(player.SkillOffsetforBodySize[1] , player.SkillOffsetforBodySize[2]);

        transform.position = new Vector3(RCenter.x + Mathf.Cos(RTimer) * (Radius + RidusPlus), RCenter.y + Mathf.Sin(RTimer) * (Radius + RidusPlus), 0);

    }
}
