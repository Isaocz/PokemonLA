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
    /// ����ͼ �����ı��ʱ������ ���� cinemachine�ı߽� ����λ
    /// �߽硣
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
            cinemachineConfiner.m_Damping = 0f;//���� ��ײ����ϵ����ʹ�� ��������� �߽磬���ᵯ��
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
