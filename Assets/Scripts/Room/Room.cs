using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    












    //获取墙和门的预制件
    public int RoomNum;
    public Wall WallU;
    public Wall WallD;
    public Wall WallL;
    public Wall WallR;
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
    public Color GateWayColor;

    public GameObject LockedGateWayUp;
    public GameObject LockedGateWayDown;
    public GameObject LockedGateWayLeft;
    public GameObject LockedGateWayRight;

    public RoomGroundBlockFile FloorFile;

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

    GridGraph RoomGraph;
    float GraphUpdateTimer;

    public float RoomWeight = 1.0f;


    /// <summary>
    /// 该房间某个方向是否被该房间内的环境物体遮挡，依次为上下左右，不被阻挡时为false，被阻挡时为true
    /// 如某个房间右侧门出有湖，使该房间无法再右侧开门，则该房间的isBlockerIn == {false false false true}
    /// </summary>
    public bool[] isBlockerIn = new bool[] { false, false, false, false };


    /// <summary>
    /// 房间的尺寸，玩家仅允许在这个区域内被击退，使用位移技能，敌人的移动也被限制在这个范围，依次为上下左右
    /// </summary>
    public float[] RoomSize = new float[] { 7.3f, -7.3f,-12.0f,  12.0f };








    /// <summary>
    /// 房间的四壁是否是墙 顺序依次为  0：U   1：D   2：R   3：L
    /// </summary>
    public List<bool> isWallAround
    {
        get { return iswallAround; }
        set { iswallAround = value; }
    }
    List<bool> iswallAround = new List<bool> { false, false, false, false };


    private void Start()
    {




        if (RoomTag == 0 || RoomTag == 3) {
            Empty = gameObject.transform.GetChild(3).gameObject;
        }

        ;
        Player = GameObject.FindObjectOfType<PlayerControler>().gameObject;
        playerControler = Player.GetComponent<PlayerControler>();
        if(RoomTag == 0 || RoomTag == -1 || RoomTag == 3)
        {
            SetFloor();
        }
        RoomGraph = _PathFinder.StaticPathFinder.CreatNewGrid(new Vector3Int((int)(transform.position.x), (int)(transform.position.y), 0));
        StartCoroutine(DeleteObjectsCoroutine());
    }


    private void Update()
    {
        if(playerControler == null)
        {
            Player = GameObject.FindObjectOfType<PlayerControler>().gameObject;
            playerControler = Player.GetComponent<PlayerControler>();
        }
        if (new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f), 0) == playerControler.NowRoom)
        {
            isVisit = true;
            isInThisRoom = true;
            if(RoomTag == 0 || RoomTag == 3)
            {
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(true);
            }
            GraphUpdateTimer += Time.deltaTime;
            if (GraphUpdateTimer >= 1)
            {
                GraphUpdateTimer = 0;
                _PathFinder.StaticPathFinder.UpdateGraph(RoomGraph);
            }

        }
        else
        {
            isInThisRoom = false;
        }
        if (isVisit && isClear > 0 && new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f), 0) != playerControler.NowRoom)
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
            Transform room = UiMiniMap.Instance.transform.Find(MiniMapRoomName);
            if (room)
            {
                Image RoomBlock = room.GetComponent<Image>();
                RoomBlock.color = new Vector4(255, 255, 255, 255);
                if (UiMiniMap.Instance.VisitedRoomList.Exists(t => t == RoomBlock)) { }
                else { UiMiniMap.Instance.VisitedRoomList.Add(RoomBlock); }
            }

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

        if ( isVisit && (RoomTag == 0 || RoomTag == 3) && !isItemDrop && isClear <= 0 && RandomDropItem != null)
        {
            isItemDrop = true;
            Debug.Log(1);
            Instantiate( RandomDropItem , transform.position+DropItemPosion , Quaternion.identity , transform );
            if (playerControler.playerData.IsPassiveGetList[134] && transform.GetComponent<BossRoom>() == null) {
                if (Random.Range(0.0f, 1.0f) + ((float)playerControler.LuckPoint / 30) >= 0.65f) { Instantiate(RandomDropItem, transform.position + DropItemPosion + Vector3.up * 0.2f, Quaternion.identity, transform); }
            }
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
                if (FloorFile.OutPutWeightIndex(x) != -1)
                {
                    Instantiate(FloorFile.transform.GetChild(FloorFile.OutPutWeightIndex(x)), transform.position + new Vector3(i - 14.5f, j - 11.5f, 0), Quaternion.identity, transform.GetChild(0));
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
            GameObject Wallup = Instantiate(WallU.gameObject, new Vector3(transform.position.x + WallU.Offset.x, transform.position.y + WallU.Offset.y, 0), Quaternion.identity,transform.GetChild(1));  isWallAround[0] = true;      
        }
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.up)       
        {        
            GameObject gatewayup = Instantiate(PCGateWayUp, new Vector3(transform.position.x, transform.position.y + 6.7f, 0), Quaternion.identity,gameObject.transform); isWallAround[0] = false;
        }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.up)
        {
            GameObject gatewayup = Instantiate(StoreGateWayUp, new Vector3(transform.position.x, transform.position.y + 6.7f, 0), Quaternion.identity, gameObject.transform); isWallAround[0] = false;
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.up)
        {
            GameObject gatewayup = Instantiate(BossGateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity , gameObject.transform); isWallAround[0] = false;
        }
        else if (NowRoomPoint + Vector3Int.up != Vector3Int.zero && ((mapCreater.SkillShopRoomPoint == NowRoomPoint + Vector3Int.up)
            || (mapCreater.MewRoomPoint == NowRoomPoint + Vector3Int.up)
            || (mapCreater.BabyCenterRoomPoint == NowRoomPoint + Vector3Int.up)
            || (mapCreater.MintRoomPoint == NowRoomPoint + Vector3Int.up)
            || (mapCreater.BerryTreeRoomPoint == NowRoomPoint + Vector3Int.up) ))
        {
            GameObject gatewayup = Instantiate(LockedGateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isWallAround[0] = false;
            gatewayup.GetComponent<LockedDoorSetText>().SetLockedDoorText(mapCreater, NowRoomPoint + Vector3Int.up);
        }
        else
        {            
            GameObject gatewayup = Instantiate(GateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isWallAround[0] = false;
        }

        



            
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.down))       
        {        
            GameObject Walldown = Instantiate(WallD.gameObject, new Vector3(transform.position.x + WallD.Offset.x, transform.position.y + WallD.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); isWallAround[1] = true;
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.down)
        {
            Instantiate(BossGateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity , transform); isWallAround[1] = false;
        }
        else if (NowRoomPoint + Vector3Int.down != Vector3Int.zero && ((mapCreater.SkillShopRoomPoint == NowRoomPoint + Vector3Int.down)
    || (mapCreater.MewRoomPoint == NowRoomPoint + Vector3Int.down)
    || (mapCreater.BabyCenterRoomPoint == NowRoomPoint + Vector3Int.down)
    || (mapCreater.MintRoomPoint == NowRoomPoint + Vector3Int.down)
    || (mapCreater.BerryTreeRoomPoint == NowRoomPoint + Vector3Int.down)))
        {
            GameObject gatewayup = Instantiate(LockedGateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, transform);
            gatewayup.GetComponent<LockedDoorSetText>().SetLockedDoorText(mapCreater, NowRoomPoint + Vector3Int.down); isWallAround[1] = false;
        }
        else         
        {       
            GameObject gatewaydown = Instantiate(GateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform); isWallAround[1] = false;
        }






            
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.left))        
        {       
            GameObject Wallleft = Instantiate(WallL.gameObject, new Vector3(transform.position.x + WallL.Offset.x, transform.position.y + WallL.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); isWallAround[3] = true;
        }        
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.left)          
        {    
            GameObject gatewayleft = Instantiate(PCGateWayLeft, new Vector3(transform.position.x - 13.8f, transform.position.y -2.84f, 0), Quaternion.identity, gameObject.transform); isWallAround[3] = false;
        }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.left)
        {
            GameObject gatewayleft = Instantiate(StoreGateWayLeft, new Vector3(transform.position.x - 14.1f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform); isWallAround[3] = false;
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.left)
        {
            GameObject gatewayleft = Instantiate(BossGateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90),transform); isWallAround[3] = false;
        }
        else if (NowRoomPoint + Vector3Int.left != Vector3Int.zero && ((mapCreater.SkillShopRoomPoint == NowRoomPoint + Vector3Int.left)
    || (mapCreater.MewRoomPoint == NowRoomPoint + Vector3Int.left)
    || (mapCreater.BabyCenterRoomPoint == NowRoomPoint + Vector3Int.left)
    || (mapCreater.MintRoomPoint == NowRoomPoint + Vector3Int.left)
    || (mapCreater.BerryTreeRoomPoint == NowRoomPoint + Vector3Int.left)))
        {
            GameObject gatewayup = Instantiate(LockedGateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.identity, gameObject.transform); isWallAround[3] = false;
            gatewayup.GetComponent<LockedDoorSetText>().SetLockedDoorText(mapCreater, NowRoomPoint + Vector3Int.left);
        }
        else
            {
                GameObject gatewayleft = Instantiate(GateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isWallAround[3] = false;
        }






           
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.right))          
        {         
            GameObject Wallright = Instantiate(WallR.gameObject, new Vector3(transform.position.x + WallR.Offset.x, transform.position.y + WallR.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); isWallAround[2] = true;
        }               
        else if (mapCreater.PCRoomPoint == NowRoomPoint + Vector3Int.right)
            {
                GameObject gatewayright = Instantiate(PCGateWayRight, new Vector3(transform.position.x+13.8f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform); isWallAround[2] = false;
        }
        else if (mapCreater.StoreRoomPoint == NowRoomPoint + Vector3Int.right)
        {
            GameObject gatewayright = Instantiate(StoreGateWayRight, new Vector3(transform.position.x + 14.1f, transform.position.y - 2.84f, 0), Quaternion.identity, gameObject.transform); isWallAround[2] = false;
        }
        else if (mapCreater.BossRoomPoint == NowRoomPoint + Vector3Int.right)
        {
            Instantiate(BossGateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), transform); isWallAround[2] = false;
        }
        else if (NowRoomPoint + Vector3Int.right != Vector3Int.zero && ((mapCreater.SkillShopRoomPoint == NowRoomPoint + Vector3Int.right)
    || (mapCreater.MewRoomPoint == NowRoomPoint + Vector3Int.right)
    || (mapCreater.BabyCenterRoomPoint == NowRoomPoint + Vector3Int.right)
    || (mapCreater.MintRoomPoint == NowRoomPoint + Vector3Int.right)
    || (mapCreater.BerryTreeRoomPoint == NowRoomPoint + Vector3Int.right)))
        {
            GameObject gatewayup = Instantiate(LockedGateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.identity, gameObject.transform); isWallAround[2] = false;
            gatewayup.GetComponent<LockedDoorSetText>().SetLockedDoorText(mapCreater, NowRoomPoint + Vector3Int.right);
        }
        else
            {
                GameObject gatewayright = Instantiate(GateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isWallAround[2] = false;
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




        GameObject Wallup = null;
        GameObject Walldown = null;
        GameObject Wallleft = null;
        GameObject Wallright = null;
        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.up))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayup = Instantiate(bossRoom.NextFloorDoorUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; isWallAround[0] = false; }
            else { Wallup = Instantiate(WallU.gameObject, new Vector3(transform.position.x + WallU.Offset.x, transform.position.y + WallU.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; isWallAround[0] = true; }
        }
        else
        {
            GameObject gatewayup = Instantiate(GateWayUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isWallAround[0] = false;
        }


        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.down))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewaydown = Instantiate(bossRoom.NextFloorDoorDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; isWallAround[1] = false; }
            else { Walldown = Instantiate(WallD.gameObject, new Vector3(transform.position.x + WallD.Offset.x, transform.position.y + WallD.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; isWallAround[1] = true; }
        }
        else
        {
            GameObject gatewaydown = Instantiate(GateWayDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform); isWallAround[1] = false;
        }



        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.left))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayleft = Instantiate(bossRoom.NextFloorDoorLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; isWallAround[3] = false; }
            else { Wallleft = Instantiate(WallL.gameObject, new Vector3(transform.position.x + WallL.Offset.x, transform.position.y + WallL.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; isWallAround[3] = true; }
        }
        else
        {
            GameObject gatewayleft = Instantiate(GateWayLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isWallAround[3] = false;
        }


        if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.right))
        {
            if (!isNextFloorDoorExit && Random.Range(0.0f, 1.0f) > NextFloorPer) { GameObject gatewayright = Instantiate(bossRoom.NextFloorDoorRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; isWallAround[2] = false; }
            else { Wallright = Instantiate(WallR.gameObject, new Vector3(transform.position.x + WallR.Offset.x, transform.position.y + WallR.Offset.y, 0), Quaternion.identity, transform.GetChild(1)); NextFloorPer -= 0.25f; isWallAround[2] = true; }
        }
        else
        {
            GameObject gatewayright = Instantiate(GateWayRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isWallAround[2] = false;
        }


        if (!isNextFloorDoorExit)
        {
            if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.up))
            {
                if (!isNextFloorDoorExit) { GameObject gatewayup = Instantiate(bossRoom.NextFloorDoorUp, new Vector3(transform.position.x, transform.position.y + 9.7f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; isWallAround[0] = false; if (Wallup!=null) { Destroy(Wallup.gameObject); } }
            }


            if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.down))
            {
                if (!isNextFloorDoorExit) { GameObject gatewaydown = Instantiate(bossRoom.NextFloorDoorDown, new Vector3(transform.position.x, transform.position.y - 8.35f, 0), Quaternion.identity, gameObject.transform); isNextFloorDoorExit = true; isWallAround[1] = false; if (Walldown != null) { Destroy(Walldown.gameObject); } }
            }


            if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.left))
            {
                if (!isNextFloorDoorExit ) { GameObject gatewayleft = Instantiate(bossRoom.NextFloorDoorLeft, new Vector3(transform.position.x - 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; isWallAround[3] = false; if (Wallleft != null) { Destroy(Wallleft.gameObject); } }
            }

            if (!mapCreater.VRoom.ContainsKey(NowRoomPoint + Vector3Int.right))
            {
                if (!isNextFloorDoorExit) { GameObject gatewayright = Instantiate(bossRoom.NextFloorDoorRight, new Vector3(transform.position.x + 13f, transform.position.y - 0.7135f, 0), Quaternion.Euler(0, 0, 90), gameObject.transform); isNextFloorDoorExit = true; isWallAround[2] = false; if (Wallright != null) { Destroy(Wallright.gameObject); } }
            }
        }


    }










    private IEnumerator DeleteObjectsCoroutine()
    {
        for(;;) 
        {
            if (playerControler!= null && playerControler.playerData.IsPassiveGetList[57])
            {
                DeleteGrass(transform);
            }
            yield return new WaitForSeconds(4f);
            Debug.Log("clear the grass complete");
        }
    }

    private void DeleteGrass(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // 如果子对象有NormalGrass脚本，销毁该对象
            if (child.TryGetComponent<NormalGress>(out var normalGrass)|| child.TryGetComponent<GressPlayerINOUT>(out var normalgrass))
            {
                Destroy(child.gameObject);
            }
            // 否则，递归调用DeleteObjects方法继续查找子对象
            else
            {
                DeleteGrass(child);
            }
        }
    }

















    //========================================获得房屋内所有道具===========================================

    public List<IteamPickUp> ItemList = new List<IteamPickUp> { };

    public void GetAllItem()
    {
        ItemList.Clear();
        if (RoomTag != 1 && RoomTag != 2) {
            if (transform.GetChild(4).gameObject.activeInHierarchy && transform.GetChild(4).childCount > 0)
            {
                for (int i = 0; i < transform.GetChild(4).childCount; i++)
                {
                    IteamPickUp item = transform.GetChild(4).GetChild(i).GetComponent<IteamPickUp>();
                    if (item != null)
                    {
                        if (item.GetComponent<PokemonBall>() != null && item.GetComponent<PokemonBall>().isEmpty) { Destroy(item.gameObject); }
                        else if (item.GetComponent<SkillBall>() != null && item.GetComponent<SkillBall>().isEmpty) { Destroy(item.gameObject); }
                        else { ItemList.Add(item); }
                    }
                }
            }
            if (transform.GetChild(5).gameObject.activeInHierarchy && transform.GetChild(5).childCount > 0)
            {
                for (int i = 0; i < transform.GetChild(5).childCount; i++)
                {
                    IteamPickUp item = transform.GetChild(5).GetChild(i).GetComponent<IteamPickUp>();
                    if (item != null)
                    {
                        if (item.GetComponent<PokemonBall>() != null && item.GetComponent<PokemonBall>().isEmpty) { Destroy(item.gameObject); }
                        else if (item.GetComponent<SkillBall>() != null && item.GetComponent<SkillBall>().isEmpty) { Destroy(item.gameObject); }
                        else { ItemList.Add(item); }
                    }
                }
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            IteamPickUp item = transform.GetChild(i).GetComponent<IteamPickUp>();
            if (item != null)
            {
                if (item.GetComponent<PokemonBall>() != null && item.GetComponent<PokemonBall>().isEmpty) { Destroy(item.gameObject); }
                else if (item.GetComponent<SkillBall>() != null && item.GetComponent<SkillBall>().isEmpty) { Destroy(item.gameObject); }
                else { ItemList.Add(item); }
                
            }
        }
        UiMiniMap.Instance.MiniMapItemMark(new Vector3Int((int)(transform.position.x / 30.0f), (int)(transform.position.y / 24.0f), 0) , ItemList );

    }

    //========================================获得房屋内所有道具===========================================


}
