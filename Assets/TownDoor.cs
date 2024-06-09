using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownDoor : MonoBehaviour
{

    public TownMap town;
    public TownMap.TownPlayerState ParentHouse;
    TownPlayer Player;
    Camera MainCamera;

    //当传送点是城市中的某点时
    public Vector3 TownPosition = new Vector3(-1, -1, -1);

    //传送后玩家的面对方向
    public Vector2 PlayerR;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            town.State = ParentHouse;

            /*
            CameraPolygon cameraPolygon = TownMoveCamera.CameraMover.cameraBounds.GetComponent<CameraPolygon>();
            if (cameraPolygon != null)
            {
                cameraPolygon.CameraPolygonPoints(new Vector2[] {
                    new Vector2(30000.0f, 30000.0f),
                    new Vector2(-30000.0f, 30000.0f),
                    new Vector2(-30000.0f, -30000.0f),
                    new Vector2(30000.0f, -30000.0f)
                }
                );
            }
            else
            {
                Debug.LogWarning("未能正常设定范围");
            }
            */

            if (FindObjectOfType<TownPlayer>() != null) { Player = FindObjectOfType<TownPlayer>();}
            //Player.isCameraStop = true;
            Player.isCanNotMove = true;
            Player.GetComponent<Animator>().SetFloat("Speed" , 0);
            TPMask.In.TPStart = true;
            TPMask.In.BlackTime = 2f;

            

            Invoke("Move" , 1.1f);
            Invoke("SetCameraBoard", 1.2f);
            Invoke("SetCameraStop", 2.7f);
        }
    }

    void Move()
    {
        TownMoveCamera.CameraMover.changeBound();
        if (FindObjectOfType<TownPlayer>() != null) { 
            Player = FindObjectOfType<TownPlayer>();
            Animator animator = Player.GetComponent<Animator>();
            Player.look = PlayerR;
            animator.SetFloat("LookX" , PlayerR.x);
            animator.SetFloat("LookY" , PlayerR.y);
            if (TownPosition != new Vector3(-1,-1,-1)) {
                Player.transform.position = TownPosition;
            }
            else
            {
                Player.transform.position = town.InstancePlayerPosition();
            }
        }
        if (FindObjectOfType<Camera>() != null) { MainCamera = FindObjectOfType<Camera>(); MainCamera.transform.position = town.InstanceCameraPosition(); }


    }

    void SetCameraBoard()
    {
        
        CameraPolygon cameraPolygon = TownMoveCamera.CameraMover.cameraBounds.GetComponent<CameraPolygon>();
        if (cameraPolygon != null)
        {
            cameraPolygon.CameraPolygonPoints(TownMap.townMap.CameraBoard());
        }
        else
        {
            Debug.LogWarning("未能正常设定范围");
        }
        
        //Player.isCameraStop = false;
    }

    void SetCameraStop()
    {
        Player.isCanNotMove = false;
    }
}
