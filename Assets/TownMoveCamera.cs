using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TownMoveCamera : MonoBehaviour
{






    [SerializeField]
    public CinemachineConfiner cinemachineConfiner;


    [SerializeField]
    public PolygonCollider2D[] PolygonColliderList;



    /// <summary>
    /// 当地图 发生改变得时候，重新 设置 cinemachine的边界 来定位
    /// 边界。
    /// </summary>
    public void changeBound( TownMap.TownPlayerState state )
    {
        if (cinemachineConfiner != null
            && playerTransform != null
            && virtualCamera != null)
        {
            CinemachineConfiner confiner = new CinemachineConfiner();
            cinemachineConfiner.m_Damping = 0f;//设置 碰撞阻尼系数，使其 摄像机到达 边界，不会弹开
            cinemachineConfiner.InvalidatePathCache();
            cinemachineConfiner.m_BoundingShape2D = null;
            switch (state)
            {
                case TownMap.TownPlayerState.inTown:
                    confiner.m_BoundingShape2D = PolygonColliderList[0];
                    cameraBounds = PolygonColliderList[0].gameObject;
                    break;
                case TownMap.TownPlayerState.inMilkBar:
                    confiner.m_BoundingShape2D = PolygonColliderList[1];
                    cameraBounds = PolygonColliderList[1].gameObject;
                    break;
                case TownMap.TownPlayerState.inWoodenHouse:
                    confiner.m_BoundingShape2D = PolygonColliderList[2];
                    cameraBounds = PolygonColliderList[2].gameObject;
                    break;
            }
            cinemachineConfiner.m_BoundingShape2D = confiner.m_BoundingShape2D;
        }
    }


    public static TownMoveCamera CameraMover;




    public CinemachineVirtualCamera virtualCamera; // 引用Cinemachine Virtual Camera组件
    public GameObject cameraBounds; // 引用CameraBounds游戏对象
    private Transform playerTransform; // 玩家的Transform组件




    TownPlayer player;

    private void Awake()
    {
        CameraMover = this;
    }

    private void Start()
    {
        player = FindObjectOfType<TownPlayer>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        changeBound(TownMap.townMap.StartState);

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
