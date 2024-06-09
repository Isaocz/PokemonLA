using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera; // ����Cinemachine Virtual Camera���
    TownPlayer player;
    private Transform playerTransform; // ��ҵ�Transform���

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<TownPlayer>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isCameraStop)
        {
            virtualCamera.enabled = false;
        }
        else
        {
            virtualCamera.enabled = true;
        }
    }
}
