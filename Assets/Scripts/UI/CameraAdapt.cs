using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdapt : MonoBehaviour
{

    public float halfHeightPre = 10.7415f;
    float aimRatio;
    float halfWidth;

    private float lastwidth = 0f;
    private float lastheight = 0f;

    private void OnGUI()
    {
        if (lastwidth != Screen.width || lastheight != Screen.height)
        {
            lastwidth = Screen.width;
            lastheight = Screen.height;
            adapt();
            //print("Resolution :" + Screen.width + " X" + Screen.height);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aimRatio = (float)ConstantRoom.ROOM_WIDTH / ConstantRoom.ROOM_HIGHT;
        halfWidth = aimRatio * halfHeightPre;
    }

    void adapt()
    {
        float winRatio = (float)Screen.width / Screen.height;
        if (winRatio >= aimRatio)
        {
            GetComponent<Camera>().orthographicSize = halfHeightPre;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = halfWidth / winRatio;
        }
    }
}
