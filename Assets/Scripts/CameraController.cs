using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 引用Cinemachine Virtual Camera组件
    public GameObject cameraBounds; // 引用CameraBounds游戏对象
    private Transform playerTransform; // 玩家的Transform组件

    private void Start()
    {
        PlayerControler player = FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        //Debug.Log("获取跟随：" + virtualCamera.Follow);
    }

    public void MewCameraFollow()
    {
        //GameObject playergo = FindObjectOfType<PlayerControler>().gameObject;
        //SceneManager.MoveGameObjectToScene(playergo, SceneManager.GetActiveScene());
        PlayerControler player = FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            // 启用摄像头跟随
            virtualCamera.Follow = player.transform;

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
            else
            {
                Debug.LogWarning("未能正常设定范围");
            }
        }
        else
        {
            Debug.LogWarning("未能找到玩家");
        }
    }
}
