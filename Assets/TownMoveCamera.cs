using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TownMoveCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // ����Cinemachine Virtual Camera���
    public GameObject cameraBounds; // ����CameraBounds��Ϸ����
    private Transform playerTransform; // ��ҵ�Transform���

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
            // ��������ͷ����
            virtualCamera.Follow = player.transform;

            // ��������ͷ�߽�
            CameraPolygon cameraPolygon = cameraBounds.GetComponent<CameraPolygon>();
            if (cameraPolygon != null)
            {
                Debug.Log(TownMap.townMap.CameraBoard()[0]);
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
