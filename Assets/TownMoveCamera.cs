using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TownMoveCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 引用Cinemachine Virtual Camera组件
    public GameObject cameraBounds; // 引用CameraBounds游戏对象
    private Transform playerTransform; // 玩家的Transform组件

    TownPlayer player;

    private void Start()
    {
        player = FindObjectOfType<TownPlayer>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        MewCameraFollow();
    }

    private void Update()
    {
        //MewCameraFollow();
    }

    public void MewCameraFollow()
    {
        //GameObject playergo = FindObjectOfType<PlayerControler>().gameObject;
        //SceneManager.MoveGameObjectToScene(playergo, SceneManager.GetActiveScene());
        if (player == null) {  player = FindObjectOfType<TownPlayer>(); }
        Debug.Log(player);
        if (player != null)
        {
            // 启用摄像头跟随
            virtualCamera.Follow = player.transform;

            // 设置摄像头边界
            CameraPolygon cameraPolygon = cameraBounds.GetComponent<CameraPolygon>();
            if (cameraPolygon != null)
            {
                Debug.Log(TownMap.townMap.CameraBoard()[0]);
                cameraPolygon.CameraPolygonPoints ( TownMap.townMap.CameraBoard() );
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
