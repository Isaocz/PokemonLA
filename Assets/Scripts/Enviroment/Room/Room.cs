using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    //获取墙和门的预制件
    public int RoomNum;
    public GameObject Wallv;
    public GameObject Wallv2;
    public GameObject Wallh;
    public GameObject GateWayUp;
    public GameObject GateWayDown;
    public GameObject GateWayLeft;
    public GameObject GateWayRight;
    public GameObject PCGateWayUp;
    public GameObject PCGateWayLeft;
    public GameObject PCGateWayRight;
    public GameObject StoreGateWayUp;
    public GameObject StoreGateWayLeft;
    public GameObject StoreGateWayRight;
    public GameObject BossGateWayUp;
    public GameObject BossGateWayDown;
    public GameObject BossGateWayLeft;
    public GameObject BossGateWayRight;

    public GameObject FloorFile;

    public bool isVisit = false;
    public bool isInThisRoom = false;
    protected bool isMapCreated = false;
    public int isClear;
    protected GameObject Empty;

    public int RoomTag; 
    protected GameObject Player;
    protected PlayerControler playerControler;

    public GameObject RandomDropItem;
    public Vector3 DropItemPosion;
    protected bool isItemDrop;





    private void Start()
    {
        if (RoomTag == 0 || RoomTag == 3) {
            Empty = gameObject.transform.GetChild(3).gameObject;
        }
        
        Player = GameObject.FindWithTag("Player");
        playerControler = Player.GetComponent<PlayerControler>();
        if(RoomTag == 0 || RoomTag == -1 || RoomTag == 3)
        {
            SetFloor();
        }
        _PathFinder.StaticPathFinder.CreatNewGrid(new Vector3Int((int)(transform.position.x), (int)(transform.position.y), 0));
    }


    private void Update()
    {
        if (new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f), 0) == playerControler.NowRoom)
        {
            isVisit = true;
            isInThisRoom = true;
            if(RoomTag == 0 || RoomTag == 3)
            {
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(true);
            }
        }
        else
        {
            isInThisRoom = false;
        }
        if (isVisit && isClear != 0 && new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f), 0) != playerControler.NowRoom)
        {
            isVisit = false;
            if (RoomTag == 0 || RoomTag == 3)
            {
                transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        if (isVisit && !isMapCreated)
        {
           
            string MiniMapRoomName = new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y  / 24.0f), 0).ToString();
            Image RoomBlock = UiMiniMap.Instance.transform.Find(MiniMapRoomName).GetComponent<Image>();
            RoomBlock.color = new Vector4(255,255,255,255);
            if(UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomBlock)){     }
            else { UiMiniMap.Instance.VisitedRoomList.Add(RoomBlock); }

            string MiniMapRoomNameUp = new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f + 1), 0).ToString();
            if(UiMiniMap.Instance.transform.Find(MiniMapRoomNameUp) != null) 
            {
                Color x = UiMiniMap.Instance.transform.Find(MiniMapRoomNameUp).GetComponent<Image>().color;
                if (x.a == 0) {
                    x = new Vector4(0.7f, 0.7f, 0.7f, 1);
                    Image RoomUPBlock = UiMiniMap.Instance.transform.Find(MiniMapRoomNameUp).GetComponent<Image>();
                    RoomUPBlock.color = x;
                    if (UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomUPBlock)) { }
                    else { UiMiniMap.Instance.VisitedRoomList.Add(RoomUPBlock); }
                }
            }
            string MiniMapRoomNameDown = new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f -1), 0).ToString();
            if (UiMiniMap.Instance.transform.Find(MiniMapRoomNameDown) != null)
            {
                Color x = UiMiniMap.Instance.transform.Find(MiniMapRoomNameDown).GetComponent<Image>().color;
                if (x.a == 0)
                {
                    x = new Vector4(0.7f, 0.7f, 0.7f, 1);
                    Image RoomDownBlock = UiMiniMap.Instance.transform.Find(MiniMapRoomNameDown).GetComponent<Image>();
                    RoomDownBlock.color = x;
                    if (UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomDownBlock)) { }
                    else { UiMiniMap.Instance.VisitedRoomList.Add(RoomDownBlock); }
                }
            }
               
            string MiniMapRoomNameLeft = new Vector3Int((int)(transform.position.x / 30.0f + 1), (int)(transform.position.y / 24.0f), 0).ToString();
            if (UiMiniMap.Instance.transform.Find(MiniMapRoomNameLeft) != null) 
            {
                Color x = UiMiniMap.Instance.transform.Find(MiniMapRoomNameLeft).GetComponent<Image>().color;
                if (x.a == 0)
                {
                    x = new Vector4(0.7f, 0.7f, 0.7f, 1);
                    Image RoomLeftBlock = UiMiniMap.Instance.transform.Find(MiniMapRoomNameLeft).GetComponent<Image>();
                    RoomLeftBlock.color = x;
                    if (UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomLeftBlock)) { }
                    else { UiMiniMap.Instance.VisitedRoomList.Add(RoomLeftBlock); }
                }
            }
            string MiniMapRoomNameRight = new Vector3Int((int)(transform.position.x / 30.0f - 1), (int)(transform.position.y/ 24.0f), 0).ToString();
            if (UiMiniMap.Instance.transform.Find(MiniMapRoomNameRight) != null)
            {
                Color x = UiMiniMap.Instance.transform.Find(MiniMapRoomNameRight).GetComponent<Image>().color;
                if (x.a == 0)
                {
                    x = new Vector4(0.7f, 0.7f, 0.7f, 1);
                    Image RoomRightBlock = UiMiniMap.Instance.transform.Find(MiniMapRoomNameRight).GetComponent<Image>();
                    RoomRightBlock.color = x;
                    if (UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomRightBlock)) { }
                    else { UiMiniMap.Instance.VisitedRoomList.Add(RoomRightBlock); }
                }
            }

                isMapCreated = true;
        }

        if ( isVisit && (RoomTag == 0 || RoomTag == 3) && !isItemDrop && isClear == 0 && RandomDropItem != null)
        {
            isItemDrop = true;
            Debug.Log(1);
            Instantiate( RandomDropItem , transform.position+DropItemPosion , Quaternion.identity , transform );
            if (playerControler.ClearThisRoomEvent != null)
            {
                playerControler.ClearThisRoomEvent(playerControler);
            }
            if (RoomTag == 3) { BackGroundMusic.StaticBGM.ChangeBGMToBossWin(); }
        }
    }


    public void SetFloor()
    {
        for (int i = 0;i<30;i++)
        {
            for(int j = 0; j<24; j++)
            {
                float x = Random.Range(0.0f, 1.0f);
                if (x>=0 && x<0.2f)
                {
                    Instantiate(FloorFile.transform.GetChild(0),transform.position + new Vector3(i - 14.5f, j - 11.5f, 0), Quaternion.identity, transform.GetChild(0));
                }
                else if (x >= 0.2f && x < 0.25f)
                {
                    Instantiate(FloorFile.transform.GetChild(1), transform.position + new Vector3(i - 14.5f, j - 11.5f, 0), Quaternion.identity, transform.GetChild(0));
                }
                else if (x >= 0.3f && x < 0.35f)
                {
                    Instantiate(FloorFile.transform.GetChild(2), transform.position + new Vector3(i - 14.5f, j - 11.5f, 0), Quaternion.identity, transform.GetChild(0));
                }
            }
        }
    }


    // 声明一个生成墙的函数
    public void CreatWall()
    {
        MapCreater mapCreater = MapCreater.FindObjectOfType<MapCreater>();

        //获取当前房间的虚拟坐标
        Vector3Int NowRoomPoint = new Vector3Int ((int)(transform.position.x / 30), (int)(transform.position.y / 24), 0);


           
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.up)) 
        {     
            GameObject Wallup = Instantiate(Wallv, new Vector3(transform.position.x, transform.position.y + 8.25f, 0), Quaternion.identity,transform.GetChild(1));        
        }
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.up)       
        {        
            GameObject gatewayup = Instantiate(PCGateWayUp, new Vector3(transform.position.x, transform.position.y + 6.7f, 0), Quaternion.identity,gameObject.transform);                 
        }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.up)
        {
            GameObject gatewayup = Instantiate(StoreGateWayUp, new Vector3(transform.position.x, transform.position.y + 6.7f, 0), Quaternion.identity, gameObject.transform);
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.up)
        {
            GameObject gatewayup = Instantiate(BossGateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity , gameObject.transform);
        }
        else
        {            
            GameObject gatewayup = Instantiate(GateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform);              
        }





            
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.down))       
        {        
            GameObject Walldown = Instantiate(Wallv2, new Vector3(transform.position.x, transform.position.y - 9.75f, 0), Quaternion.identity, transform.GetChild(1));      
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.down)
        {
            Instantiate(BossGateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity , transform);
        }
        else         
        {       
            GameObject gatewaydown = Instantiate(GateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform);       
        }






            
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.left))        
        {       
            GameObject Wallleft = Instantiate(Wallh, new Vector3(transform.position.x - 13.75f, transform.position.y-1.5f, 0), Quaternion.identity, transform.GetChild(1));    
        }        
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.left)          
        {    
            GameObject gatewayleft = Instantiate(PCGateWayLeft, new Vector3(transform.position.x - 13.8f, transform.position.y -2.84f, 0), Quaternion.identity, gameObject.transform);   
        }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.left)
        {
            GameObject gatewayleft = Instantiate(StoreGateWayLeft, new Vector3(transform.position.x - 14.1f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform);
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.left)
        {
            GameObject gatewayleft = Instantiate(BossGateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90),transform);
        }
        else
            {
                GameObject gatewayleft = Instantiate(GateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform);
            }






           
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.right))          
        {         
            GameObject Wallright = Instantiate(Wallh, new Vector3(transform.position.x + 13.75f, transform.position.y-1.5f, 0), Quaternion.identity, transform.GetChild(1));           
        }               
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.right)
            {
                GameObject gatewayright = Instantiate(PCGateWayRight, new Vector3(transform.position.x+13.8f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform);
            }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.right)
        {
            GameObject gatewayright = Instantiate(StoreGateWayRight, new Vector3(transform.position.x + 14.1f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform);
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.right)
        {
            Instantiate(BossGateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), transform);
        }
        else
            {
                GameObject gatewayright = Instantiate(GateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform);
            }

        }




    public void CreatNextFloorWall()
    {
        MapCreater mapCreater = MapCreater.FindObjectOfType<MapCreater>();

        //获取当前房间的虚拟坐标
        Vector3Int NowRoomPoint = new Vector3Int((int)(transform.position.x / 30), (int)(transform.position.y / 24), 0);
        float NextFloorPer = 0.75f;
        BossRoom bossRoom = GetComponent<BossRoom>();
        bool isNextFloorDoorExit = false;


        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.up))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayup = Instantiate(bossRoom.NextFloorDoorUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; }
            else { GameObject Wallup = Instantiate(Wallv, new Vector3(transform.position.x, transform.position.y + 8.25f, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; }
        }
        else
        {
            GameObject gatewayup = Instantiate(GateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform);
        }






        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.down))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewaydown = Instantiate(bossRoom.NextFloorDoorDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; }
            else { GameObject Walldown = Instantiate(Wallv2, new Vector3(transform.position.x, transform.position.y - 9.75f, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; }
        }
        else
        {
            GameObject gatewaydown = Instantiate(GateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform);
        }







        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.left))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayleft = Instantiate(bossRoom.NextFloorDoorLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; }
            else { GameObject Wallleft = Instantiate(Wallh, new Vector3(transform.position.x - 13.75f, transform.position.y - 1.5f, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; }
        }
        else
        {
            GameObject gatewayleft = Instantiate(GateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform);
        }







        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.right))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayright = Instantiate(bossRoom.NextFloorDoorRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; }
            else { GameObject Wallright = Instantiate(Wallh, new Vector3(transform.position.x + 13.75f, transform.position.y - 1.5f, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; }
        }
        else
        {
            GameObject gatewayright = Instantiate(GateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform);
        }

    }
}
