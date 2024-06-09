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
    public PolygonCollider2D polygonCollider;
    [SerializeField]
    public PolygonCollider2D polygonCollider1;

    [SerializeField]
    private int num = 1;


    /// <summary>
    /// 当地图 发生改变得时候，重新 设置 cinemachine的边界 来定位
    /// 边界。
    /// </summary>
    public void changeBound()
    {
        if (cinemachineConfiner != null
            && polygonCollider1 != null
            && polygonCollider != null
            && playerTransform != null
            && virtualCamera != null)
        {
            CinemachineConfiner confiner = new CinemachineConfiner();
            //transform.
            Debug.Log("      changeBound()              cinemachineConfiner.m_BoundingShape2D = polygonCollider");
            cinemachineConfiner.m_Damping = 0f;//设置 碰撞阻尼系数，使其 摄像机到达 边界，不会弹开
            cinemachineConfiner.InvalidatePathCache();
            if ((num % 2) != 0)
            {
                cinemachineConfiner.m_BoundingShape2D = null;
                //Destroy(cinemachineConfiner);
                confiner.m_BoundingShape2D = polygonCollider1;
                cameraBounds = polygonCollider1.gameObject;
                //camera.AddExtension(confiner);
                //transform.gameObject.AddComponent<CinemachineConfiner>();
                //cinemachineConfiner = transform.GetComponent<CinemachineConfiner>();
                //cinemachineConfiner = confiner;
                //cinemachineConfiner.InvalidatePathCache();
                //cinemachineConfiner.m_BoundingShape2D = polygonCollider1;
            }//
            else if ((num % 2) == 0)
            {
                cinemachineConfiner.m_BoundingShape2D = null;
                //Destroy(cinemachineConfiner);
                confiner.m_BoundingShape2D = polygonCollider;
                cameraBounds = polygonCollider.gameObject;
                //camera.AddExtension(confiner);
                //transform.gameObject.AddComponent<CinemachineConfiner>();
                //cinemachineConfiner = transform.GetComponent<CinemachineConfiner>();
                //cinemachineConfiner = confiner;
                //cinemachineConfiner.InvalidatePathCache();
                //cinemachineConfiner.m_BoundingShape2D = polygonCollider;
            }//
            //confiner.m_BoundingShape2D = polygonCollider1;
            //camera.AddExtension(confiner);
            cinemachineConfiner.m_BoundingShape2D = confiner.m_BoundingShape2D;
            num++;
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
        changeBound();

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
