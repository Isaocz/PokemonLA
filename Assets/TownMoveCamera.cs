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
    /// ����ͼ �����ı��ʱ������ ���� cinemachine�ı߽� ����λ
    /// �߽硣
    /// </summary>
    public void changeBound( TownMap.TownPlayerState state )
    {
        if (cinemachineConfiner != null
            && playerTransform != null
            && virtualCamera != null)
        {
            CinemachineConfiner confiner = new CinemachineConfiner();
            cinemachineConfiner.m_Damping = 0f;//���� ��ײ����ϵ����ʹ�� ��������� �߽磬���ᵯ��
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




    public CinemachineVirtualCamera virtualCamera; // ����Cinemachine Virtual Camera���
    public GameObject cameraBounds; // ����CameraBounds��Ϸ����
    private Transform playerTransform; // ��ҵ�Transform���




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
            // ��������ͷ����
            virtualCamera.Follow = player.transform;

            // ��������ͷ�߽�
            CameraPolygon cameraPolygon = cameraBounds.GetComponent<CameraPolygon>();
            if (cameraPolygon != null)
            {
                cameraPolygon.CameraPolygonPoints ( TownMap.townMap.CameraBoard() );
            }
            else
            {
                Debug.LogWarning("δ�������趨��Χ");
            }
        }
        else
        {
            Debug.LogWarning("δ���ҵ����");
        }
    }
}
