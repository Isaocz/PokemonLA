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

    public GameObject cameraMaskLeft; 
    public GameObject cameraMaskRight; 
    public GameObject cameraMaskUp; 
    public GameObject cameraMaskDown;
    public GameObject vcam;

    public bool isCameraFollow;

    //方便其他脚本获取相机
    public static CameraAdapt MainCamera;

    private void Awake()
    {
        MainCamera = this;
    }

    public void HideCameraMasks()
    {
        // 隐藏摄像头遮罩
        if (cameraMaskLeft != null)
            cameraMaskLeft.SetActive(false);
        if (cameraMaskRight != null)
            cameraMaskRight.SetActive(false);
        if (cameraMaskUp != null)
            cameraMaskUp.SetActive(false);
        if (cameraMaskDown != null)
            cameraMaskDown.SetActive(false);
    }
    public void ShowCameraMasks()
    {
        // 显示摄像头遮罩
        if (cameraMaskLeft != null)
            cameraMaskLeft.SetActive(true);
        if (cameraMaskRight != null)
            cameraMaskRight.SetActive(true);
        if (cameraMaskUp != null)
            cameraMaskUp.SetActive(true);
        if (cameraMaskDown != null)
            cameraMaskDown.SetActive(true);
    }

    public void ActivateVcam()
    {
        vcam.SetActive(true);
    }

    public void DeactivateVcam()
    {
        vcam.SetActive(false);
    }

    private void OnGUI()
    {
        if (lastwidth != Screen.width || lastheight != Screen.height)
        {
            lastwidth = Screen.width;
            lastheight = Screen.height;
            adapt();
            //print("Resolution :" + Screen.width + " X " + Screen.height);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //aimRatio = (float)ConstantRoom.ROOM_WIDTH / ConstantRoom.ROOM_HIGHT;
        aimRatio = ConstantRoom.ROOM_SHOW_RATIO;
        halfWidth = aimRatio * halfHeightPre;
        vcam.SetActive(isCameraFollow);
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
