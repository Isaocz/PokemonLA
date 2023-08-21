using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 引用Cinemachine Virtual Camera组件
    public GameObject cameraBounds; // 引用CameraBounds游戏对象
    private Transform playerTransform; // 玩家的Transform组件

    private void Start()
    {
        // 禁用摄像头跟随
        virtualCamera.Follow = null;

        // 获取玩家的Transform组件
        PlayerControler player = FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void MewCameraFollow()
    {
        if (playerTransform != null)
        {
            // 启用摄像头跟随
            virtualCamera.Follow = playerTransform;

            // 设置摄像头边界
            CameraPolygon cameraPolygon = cameraBounds.GetComponent<CameraPolygon>();
            if (cameraPolygon != null)
            {
                cameraPolygon.CameraPolygonPoints(new Vector2[]
                {
                    new Vector2(3045, 2436),
                    new Vector2(2985, 2436),
                    new Vector2(2985, 2388),
                    new Vector2(3045, 2388)
                });
            }
        }
    }

}
