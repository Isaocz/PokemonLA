using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class TownDoor : MonoBehaviour
{

    public TownMap town;
    public TownMap.TownPlayerState ParentHouse;
    TownPlayer Player;
    TownNPC Townnpc;
    Camera MainCamera;

    //当传送点是城市中的某点时
    public Vector3 TownPosition = new Vector3(-1, -1, -1);

    //传送后玩家的面对方向
    public Vector2 PlayerR;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("存在" + other);
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
            Invoke("SetCameraStop", 3.5f);
        }
        else if (other.tag == "NPC")
        {
            TownNPC tnpc = other.GetComponent<TownNPC>();
            
            if (tnpc != null)
            {
                Townnpc = tnpc;
                Animator animator = tnpc.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetFloat("Speed", 0);
                }
                Invoke("TeleportNPC", 1.1f);
            }
        }
    }

    void Move()
    {
        TownMoveCamera.CameraMover.changeBound(TownMap.townMap.State);
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

    void TeleportNPC()
    {
        Debug.Log("已经传送" + Townnpc);
        Townnpc.location = StateConvert(ParentHouse);
        Animator animator = Townnpc.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat("LookX", PlayerR.x);
            animator.SetFloat("LookY", PlayerR.y);
        }

        if (TownPosition != new Vector3(-1, -1, -1))
        {
            Townnpc.transform.position = TownPosition;
        }
        else
        {
            Townnpc.transform.position = town.InstancePlayerPosition();
        }

    }
    /// <summary>
    /// 将TownMap中的TownPlayerState的枚举转化成TownNPC类中的NPCLocation的枚举，其包含的名字必须相同，若不相同Parse方法会抛出错误
    /// </summary>
    /// <param name="tps">TownMap类中的TownPlayerState枚举</param>
    /// <returns>TownNPC中的NPCLocation的枚举，用以表示NPC位置</returns>
    TownNPC.NPCLocation StateConvert(TownMap.TownPlayerState tps)
    {
        return (TownNPC.NPCLocation)Enum.Parse(typeof(TownNPC.NPCLocation), tps.ToString());
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
