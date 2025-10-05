using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // ����Cinemachine Virtual Camera���
    public GameObject cameraBounds; // ����CameraBounds��Ϸ����
    private Transform playerTransform; // ��ҵ�Transform���

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
        //Debug.Log("��ȡ���棺" + virtualCamera.Follow);
    }

    public void MewCameraFollow()
    {
        //GameObject playergo = FindObjectOfType<PlayerControler>().gameObject;
        //SceneManager.MoveGameObjectToScene(playergo, SceneManager.GetActiveScene());
        PlayerControler player = FindObjectOfType<PlayerControler>();
        if (player != null)
        {
            // ��������ͷ����
            virtualCamera.Follow = player.transform;

            // ��������ͷ�߽�
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
                Debug.LogWarning("δ�������趨��Χ");
            }
        }
        else
        {
            Debug.LogWarning("δ���ҵ����");
        }
    }
}
