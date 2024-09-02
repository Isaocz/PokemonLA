using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBaby : Baby
{

    public void SwordBabyStart()
    {
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }


    public void SwordBabyUpdate()
    {

        Vector3 TargetPosition = player.transform.position + player.SkillOffsetforBodySize[0] * Vector3.up + (Vector3)player.look* (Mathf.Max(player.SkillOffsetforBodySize[1], player.SkillOffsetforBodySize[2]) + 0.4f);
        float TargetRotation = _mTool.Angle_360(player.look , Vector3.up);



        if (Mathf.Approximately(transform.position.x, TargetPosition.x) && Mathf.Approximately(transform.position.y, TargetPosition.y) )
        {

        }
        else
        {
            transform.position += new Vector3((TargetPosition - transform.position).normalized.x * Time.deltaTime * 7.0f, (TargetPosition - transform.position).normalized.y * Time.deltaTime * 7.0f, 0);
            if ((transform.position - TargetPosition).magnitude <= 0.05)
            {
                transform.position = TargetPosition;
            }
        }

        //transform.rotation = Quaternion.Euler(0, 0, TargetRotation);
        if (transform.rotation.eulerAngles.z != TargetRotation)
        {
            float x = (transform.rotation.eulerAngles.z - TargetRotation);
            if (x > 180) { x -= 360; }
            if (x < -180) { x += 360; }
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 10 * ((x > 0)? -1 : 1 )   );
            if (Mathf.Abs(transform.rotation.eulerAngles.z - TargetRotation) < 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, TargetRotation);
            }
        }

    }
}

