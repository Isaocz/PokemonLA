using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{

    public static MapCreater StaticMap; 
    //声明一个房间变量，表示基础的房间
    public Room StarRoom;
    Room BaseRoom;
    public GameObject BaseRoomList;
    //声明一个房间变量，表示宝可梦中心房间,一个坐标变量，用来存储PC房间的虚拟坐标，一个布尔型变量，表示是否生成过PC房间。
    //声明一个房间变量，表示商店房间,一个坐标变量，用来存储商店房间的虚拟坐标，一个布尔型变量，表示是否生成过商店房间。
    //声明一个房间变量，表示boss房间,一个坐标变量，用来存储boss房间的虚拟坐标，一个布尔型变量，表示是否生成过boss房间。
    public Room PCRoomUp;
    public Room PCRoomLeft;
    public Room PCRoomRight;
    public Vector3Int PCRoomPoint;
    bool isPCRoomSpawn;

    public Room StoreRoomUp;
    public Room StoreRoomLeft;
    public Room StoreRoomRight;
    public Vector3Int StoreRoomPoint;
    bool isStoreRoomSpawn;

    public Room BossRoom;
    public Vector3Int BossRoomPoint;
    bool isBossRoomSpawn;


    //声明一个整形变量，表示生成的最小房间。
    //一个浮点型变量，表示生成概率。
    //一个浮点型变量，表示特殊房间的生成半径。
    public int StepMin;
    float SpawnChance = 1.0f;
    float SpawnR = 1.0f;


    //声明一个整形数组，表示虚拟的房间布局
    //一个向量表示虚拟房间布局的目前房间点
    public Dictionary<Vector3Int, int> VRoom = new Dictionary<Vector3Int, int>();
    public Dictionary<Vector3Int, Room> RRoom = new Dictionary<Vector3Int, Room>();
    Vector3Int NowPoint = new Vector3Int(0,0,0);


    List<int> RoomBlackList = new List<int> { };
    List<int> RoomWhiteList = new List<int> { };


    private void Awake()
    {
        StaticMap = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BaseRoomList.transform.childCount; i++)
        {
            RoomWhiteList.Add(i);
        }
        //初始化虚拟地图
        VRoom.Clear();
        RRoom.Clear();
        //生成地图
        CreateRoom();
        UiMiniMap.Instance.CreatMiniMap();


    }



    //声明一个生成真实房间的函数
    void CreateRoom()
    {
        //生成虚拟房间和PC房间和商店
        BuildVRoom();
        BuiledPCroom();
        BuiledStoreRoom();
        BuiledBossRoom();

        //遍历所有虚拟房间，如果该坐标不是特殊房间，在该坐标生成真实房间
        foreach (Vector3Int item in VRoom.Keys)
        {
            string roomname = item.ToString();
            if (item != PCRoomPoint && item != StoreRoomPoint && item != BossRoomPoint)
            {
                if (item == new Vector3Int(0, 0, 0))
                {
                    Room room = Instantiate(StarRoom, new Vector3(item.x * 30, item.y * 24, 0), Quaternion.identity);
                    room.CreatWall();
                    room.transform.name = roomname;
                    RRoom.Add(item, room);
                }
                else
                {
                    BaseRoom = SwithABaseRoom();
                    Room room = Instantiate(BaseRoom, new Vector3(item.x * 30, item.y * 24, 0), Quaternion.identity);
                    room.CreatWall();
                    room.transform.name = roomname;
                    RRoom.Add(item, room);
                }

            }
        }
    }

    Room SwithABaseRoom()
    {
        int x = RoomWhiteList[Random.Range(0, RoomWhiteList.Count-1)];
        RoomWhiteList.Remove(x);
        RoomBlackList.Add(x);
        if(RoomWhiteList.Count == 0)
        {
            RoomWhiteList = RoomBlackList;
            RoomBlackList = new List<int> { };
        }
        Room OutPut = BaseRoomList.transform.GetChild(x).GetComponent<Room>();
        return OutPut;
    }

    //生成一个计算虚拟普通房间的函数
    void BuildVRoom()
    {

        //如果当前虚拟房间点不是一个虚拟房间，使该房间变为虚拟房间
        if(!VRoom.ContainsKey(NowPoint)) { VRoom.Add(NowPoint, 0); }

        //当当前虚拟房间数大于需要生成的最小房间数时，房间生成概率降低10%
        if(VRoom.Count > StepMin) { SpawnChance -= 0.1f; }

        //进行一次随机如果小于生成概率就随机向某个方向生成一个虚拟房间
        if (Random.Range(0.0f,1.0f) <= SpawnChance)
        {
            NowPoint += RandomRoomDirection();
            BuildVRoom();
        }

    }


    void BuiledPCroom()
    {

        //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != StoreRoomPoint)
            {
                string roomname = item.ToString();
                //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                Vector3Int NowPCRoomPoint;

                //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                //之后也用相似方法检测该房间的左和右，但不检测下
                if (!VRoom.ContainsKey(item + Vector3Int.up))
                {
                    NowPCRoomPoint = item + Vector3Int.up;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.right))
                    {
                        
                        VRoom.Add(NowPCRoomPoint, 0);
                        PCRoomPoint = NowPCRoomPoint;
                        isPCRoomSpawn = true;
                        Room room = Instantiate(PCRoomUp, new Vector3(NowPCRoomPoint.x * 30, NowPCRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "PC" + roomname;
                        RRoom.Add(NowPCRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.left))
                {
                    NowPCRoomPoint = item + Vector3Int.left;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up))
                    {
                        VRoom.Add(NowPCRoomPoint, 0);
                        PCRoomPoint = NowPCRoomPoint;
                        isPCRoomSpawn = true;
                        Room room = Instantiate(PCRoomLeft, new Vector3(NowPCRoomPoint.x * 30, NowPCRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "PC" + roomname;
                        RRoom.Add(NowPCRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.right))
                {
                    NowPCRoomPoint = item + Vector3Int.right;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowPCRoomPoint, 0);
                        PCRoomPoint = NowPCRoomPoint;
                        isPCRoomSpawn = true;
                        Room room = Instantiate(PCRoomRight, new Vector3(NowPCRoomPoint.x * 30, NowPCRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "PC" + roomname;
                        RRoom.Add(NowPCRoomPoint, room);
                        return;
                    }
                }

            }

        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isPCRoomSpawn)
        {
            SpawnR += 0.2f;
            if(SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BuiledPCroom();

        }
        SpawnR = 1.0f;

    }
    void BuiledStoreRoom()
    {

        //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != PCRoomPoint)
            {
                string roomname = item.ToString();
                //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                Vector3Int NowStoreRoomPoint;

                //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                //之后也用相似方法检测该房间的左和右，但不检测下
                if (!VRoom.ContainsKey(item + Vector3Int.up))
                {
                    NowStoreRoomPoint = item + Vector3Int.up;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowStoreRoomPoint, 0);
                        StoreRoomPoint = NowStoreRoomPoint;
                        isStoreRoomSpawn = true;
                        Room room = Instantiate(StoreRoomUp, new Vector3(NowStoreRoomPoint.x * 30, NowStoreRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Store" + roomname;
                        RRoom.Add(NowStoreRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.left))
                {
                    NowStoreRoomPoint = item + Vector3Int.left;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up))
                    {
                        VRoom.Add(NowStoreRoomPoint, 0);
                        StoreRoomPoint = NowStoreRoomPoint;
                        isStoreRoomSpawn = true;
                        Room room = Instantiate(StoreRoomLeft, new Vector3(NowStoreRoomPoint.x * 30, NowStoreRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Store" + roomname;
                        RRoom.Add(NowStoreRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.right))
                {
                    NowStoreRoomPoint = item + Vector3Int.right;
                    if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowStoreRoomPoint, 0);
                        StoreRoomPoint = NowStoreRoomPoint;
                        isStoreRoomSpawn = true;
                        Room room = Instantiate(StoreRoomRight, new Vector3(NowStoreRoomPoint.x * 30, NowStoreRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Store" + roomname;
                        RRoom.Add(NowStoreRoomPoint, room);
                        return;
                    }
                }

            }

        }

        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isStoreRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BuiledStoreRoom();
        }
        SpawnR = 1.0f;

    }

    void BuiledBossRoom()
    {

        //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != StoreRoomPoint && item != PCRoomPoint)
            {
                string roomname = item.ToString();
                //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                Vector3Int NowBossRoomPoint;

                //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                //之后也用相似方法检测该房间的左和右，但不检测下
                if (!VRoom.ContainsKey(item + Vector3Int.up))
                {
                    NowBossRoomPoint = item + Vector3Int.up;
                    if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowBossRoomPoint, 0);
                        BossRoomPoint = NowBossRoomPoint;
                        isBossRoomSpawn = true;
                        Room room = Instantiate(BossRoom, new Vector3(NowBossRoomPoint.x * 30, NowBossRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Boss" + roomname;
                        room.CreatNextFloorWall();
                        RRoom.Add(NowBossRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.left))
                {
                    NowBossRoomPoint = item + Vector3Int.left;
                    if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up))
                    {
                        VRoom.Add(NowBossRoomPoint, 0);
                        BossRoomPoint = NowBossRoomPoint;
                        isBossRoomSpawn = true;
                        Room room = Instantiate(BossRoom, new Vector3(NowBossRoomPoint.x * 30, NowBossRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Boos" + roomname;
                        room.CreatNextFloorWall();
                        RRoom.Add(NowBossRoomPoint, room);
                        return;
                    }
                }
                if (!VRoom.ContainsKey(item + Vector3Int.right))
                {
                    NowBossRoomPoint = item + Vector3Int.right;
                    if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowBossRoomPoint, 0);
                        BossRoomPoint = NowBossRoomPoint;
                        isBossRoomSpawn = true;
                        Room room = Instantiate(BossRoom, new Vector3(NowBossRoomPoint.x * 30, NowBossRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Boss" + roomname;
                        room.CreatNextFloorWall();
                        RRoom.Add(NowBossRoomPoint, room);
                        return;
                    }
                }

                if (!VRoom.ContainsKey(item + Vector3Int.down))
                {
                    NowBossRoomPoint = item + Vector3Int.down;
                    if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                    {
                        VRoom.Add(NowBossRoomPoint, 0);
                        BossRoomPoint = NowBossRoomPoint;
                        isBossRoomSpawn = true;
                        Room room = Instantiate(BossRoom, new Vector3(NowBossRoomPoint.x * 30, NowBossRoomPoint.y * 24, 0), Quaternion.identity);
                        room.transform.name = "Boss" + roomname;
                        room.CreatNextFloorWall();
                        RRoom.Add(NowBossRoomPoint, room);
                        return;
                    }
                }

            }

        }

        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isBossRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BuiledBossRoom();
        }
        SpawnR = 1.0f;

    }

    //一个随机方向的函数
    Vector3Int RandomRoomDirection()
    {
        switch (Random.Range(0,4))
        {
            case 0:
                return new Vector3Int(-1, 0, 0);
            case 1:
                return new Vector3Int(1, 0, 0);
            case 2:
                return new Vector3Int(0, -1, 0);
            default:
                return new Vector3Int(0, 1, 0);
        }
    }
}
