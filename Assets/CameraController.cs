using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // ����Cinemachine Virtual Camera���
    public GameObject cameraBounds; // ����CameraBounds��Ϸ����
    private Transform playerTransform; // ��ҵ�Transform���

    private void Start()
    {
        // ��������ͷ����
        virtualCamera.Follow = null;

        // ��ȡ��ҵ�Transform���
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
            // ��������ͷ����
            virtualCamera.Follow = playerTransform;

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
        }
    }

}
