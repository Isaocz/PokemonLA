using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    //地图的种类
    public enum MapType
    {
        Forest,
        Cave,
        Snow,
    }
    public MapType NowMapType;

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
    int PCCreatCount;

    public Room StoreRoomUp;
    public Room StoreRoomLeft;
    public Room StoreRoomRight;
    public Vector3Int StoreRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isStoreRoomSpawn;
    int StoreCreatCount;


    public List<Room> BossRoomList;
    Room BossRoom;
    public Vector3Int BossRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isBossRoomSpawn;
    int BossRoomCreatCount;

    public Room SkillShopRoom;
    public Vector3Int SkillShopRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isSkillShopRoomSpawn;
    int SkillShopRoomCreatCount;


    public Room MewsRoom;
    public Vector3Int MewRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isMewRoomSpawn;
    int MewRoomCreatCount;
    bool isBornMewRoom;


    public Room BabyCenterRoom;
    public Vector3Int BabyCenterRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isBabyCenterRoomSpawn;
    int BabyCenterRoomCreatCount;
    bool isBornBabyCenterRoom;

    public Room MintRoom;
    public Vector3Int MintRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isMintRoomSpawn;
    int MintRoomCreatCount;
    bool isBornMintRoom;

    public Room BerryTreeRoom;
    public Vector3Int BerryTreeRoomPoint = new Vector3Int(10000, 10000, 0);
    bool isBerryTreeRoomSpawn;
    int BerryTreeRoomCreatCount;
    bool isBornBerryTreeRoom;


    bool isReset;

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



    //========================一些控制根据道具特性等生成地图的变量==============================


    //道具099 皮皮鲸喷壶
    public bool IsWailmerPail { get { return isWailmerPail; } set { isWailmerPail = value; } }
    bool isWailmerPail;


    //======================================================



    private void Awake()
    {
        StaticMap = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        //========================一些控制根据道具特性等生成地图的变量==============================

        PlayerControler p = GameObject.FindObjectOfType<PlayerControler>();
        PlayerData pd = p.GetComponent<PlayerData>();
        //道具099 皮皮鲸喷壶
        IsWailmerPail = pd.IsPassiveGetList[99];


        //======================================================










        //根据当前层数设置地图
        if (FloorNum.GlobalFloorNum != null)
        {
            //如果本剧游戏中已生成过梦幻房间，那么每层生成的概率为12%，如果还未生成，生成概率为(0.12f * Mathf.Pow(1.5f, 当前层数 - 1)));

            float x = Random.Range(0.0f, 1.0f);
            isBornMewRoom = x <= (FloorNum.GlobalFloorNum.isMewRoomBeCreated ? 0.20f : (10 * Mathf.Pow(1.5f, FloorNum.GlobalFloorNum.FloorNumber - 1)));
            Debug.Log((FloorNum.GlobalFloorNum.isMewRoomBeCreated ? 0.20f : (0.25f * Mathf.Pow(1.5f, FloorNum.GlobalFloorNum.FloorNumber - 1))));
            Debug.Log(x);
            //如果本剧游戏中已生成过Baby房间，那么不能再产生Baby房间，如果还未生成，生成概率为(0.3f - 0.1f * (当前层数 - 1))
            isBornBabyCenterRoom = Random.Range(0.0f, 1.0f) <= (FloorNum.GlobalFloorNum.isBabyCenterBeCreated ? 0 : (0.3f - 0.1f * (FloorNum.GlobalFloorNum.FloorNumber - 1)));
            //如果本剧游戏中已生成过Mint房间，那么不能再产生Mint房间，如果还未生成，生成概率为(0.3f - 0.1f * (当前层数 - 1))
            isBornMintRoom = Random.Range(0.0f, 1.0f) <= (FloorNum.GlobalFloorNum.isMintRoomBeCreated ? 0 : (0.3f - 0.1f * (FloorNum.GlobalFloorNum.FloorNumber - 1)));
            //每层生成树果房的概率为8%
            isBornBerryTreeRoom = Random.Range(0.0f, 1.0f) <= 0.08f;
            //Debug.Log(FloorNum.GlobalFloorNum.FloorNumber.ToString() + "+" + FloorNum.GlobalFloorNum.MapSize[FloorNum.GlobalFloorNum.FloorNumber].ToString()+ "+" + FloorNum.GlobalFloorNum.MapSize[0].ToString() + "+" + FloorNum.GlobalFloorNum.MapSize[1].ToString() + "+" + FloorNum.GlobalFloorNum.MapSize[2].ToString() );
            StepMin = FloorNum.GlobalFloorNum.MapSize[FloorNum.GlobalFloorNum.FloorNumber];
            BossRoom = BossRoomList[Random.Range(0, BossRoomList.Count)];
            while (FloorNum.GlobalFloorNum.isBossRoomBeCreated[BossRoom.GetComponent<BossRoom>().BossIndex])
            {
                BossRoom = BossRoomList[Random.Range(0, BossRoomList.Count)];
            }
            FloorNum.GlobalFloorNum.isBossRoomBeCreated[BossRoom.GetComponent<BossRoom>().BossIndex] = true;
        }
        else
        {
            isBornMewRoom = Random.Range(0.0f, 1.0f) <= 0.25f;
            isBornBabyCenterRoom = Random.Range(0.0f, 1.0f) <= 0.3f;
            isBornMintRoom = Random.Range(0.0f, 1.0f) <= 0.3f;
            isBornBerryTreeRoom = Random.Range(0.0f, 1.0f) <= 0.08f;
            StepMin = 8;
            BossRoom = BossRoomList[Random.Range(0,BossRoomList.Count)];
        }
        Debug.Log(isBornMewRoom);
        Debug.Log(isBornBabyCenterRoom);
        Debug.Log(isBornMintRoom);


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
        if (!isReset) { BuildVRoom(); }
        if (!isReset) { BuiledStoreRoom(); }
        if (!isReset) { BuiledBossRoom(); }
        if (!isReset) { BuiledBossRoom(); }
        if (isReset && isBornMewRoom) { BuiledMewRoom(); }
        if (isReset && isBornBabyCenterRoom) { BuiledBabyCenterRoom(); }
        if (isReset && isBornMintRoom) { BuiledMintRoom(); }
        if (isReset && isBornBerryTreeRoom) { BuiledBerryTreeRoom(); }

        while ( !isPCRoomSpawn || !isStoreRoomSpawn || !isBossRoomSpawn  )
        {
            ResetMap();
        }

        //遍历所有虚拟房间，如果该坐标不是特殊房间，在该坐标生成真实房间
        foreach (Vector3Int item in VRoom.Keys)
        {
            string roomname = item.ToString();
            if(item == new Vector3Int(0, 0, 0))
            {
                Room room = Instantiate(StarRoom, new Vector3(item.x * 30, item.y * 24, 0), Quaternion.identity);
                room.CreatWall();
                room.transform.name = roomname;
                RRoom.Add(item, room);
            }
            else
            {

                if (item != PCRoomPoint && item != StoreRoomPoint && item != BossRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    
                    BaseRoom = SwithABaseRoom(item);
                    Room room = Instantiate(BaseRoom, new Vector3(item.x * 30, item.y * 24, 0), Quaternion.identity);
                    room.CreatWall();
                    room.transform.name = roomname;
                    if (!RRoom.ContainsKey(item)) { RRoom.Add(item, room); }
                    else { RRoom[item] = room; }
                }
            }
        }
        /*
        foreach (Vector3Int k in RRoom.Keys)
        {
            Debug.Log(k + "+" + RRoom[k]);
        }
        */
    }



    //=======================================如果生成的地图无法生成特殊房间时使用，重置所有地图============================================

    void ResetMap()
    {
        isReset = true;
        Debug.Log("MapReset" + RRoom.Count);
        foreach ( Vector3Int k in RRoom.Keys)
        {
            Debug.Log("MapReset");
            Debug.Log("Reset" + "+" + k + "+" + RRoom[k]);
            Destroy(RRoom[k].gameObject);
            Debug.Log(RRoom[k].gameObject);
        }

        //初始化
        {
            isPCRoomSpawn = false; isStoreRoomSpawn = false; isBossRoomSpawn = false;
            PCCreatCount = 0; StoreCreatCount = 0; BossRoomCreatCount = 0;
            PCRoomPoint = Vector3Int.zero; StoreRoomPoint = Vector3Int.zero; BossRoomPoint = Vector3Int.zero; NowPoint = Vector3Int.zero;
            SpawnR = 1.0f; SpawnChance = 1.0f;

            BabyCenterRoomPoint = new Vector3Int(10000, 10000, 0);
            isBabyCenterRoomSpawn = false;
            BabyCenterRoomCreatCount = 0;

            BerryTreeRoomPoint = new Vector3Int(10000, 10000, 0);
            isBerryTreeRoomSpawn = false;
            BerryTreeRoomCreatCount = 0;

            MewRoomPoint = new Vector3Int(10000, 10000, 0);
            isMewRoomSpawn = false;
            MewRoomCreatCount = 0;

            MintRoomPoint = new Vector3Int(10000, 10000, 0);
            isMintRoomSpawn = false;
            MintRoomCreatCount = 0;

            SkillShopRoomPoint = new Vector3Int(10000, 10000, 0);
            SkillShopRoomCreatCount = 0;
            isSkillShopRoomSpawn = false;
        }


        VRoom.Clear(); RRoom.Clear();
        RRoom = new Dictionary<Vector3Int, Room> { };
        BuildVRoom();
        BuiledPCroom();
        BuiledStoreRoom();
        BuiledBossRoom();
        BuiledSkillShopRoom();
        if (isBornMewRoom) { BuiledMewRoom(); }
        if (isBornBabyCenterRoom) { BuiledBabyCenterRoom(); }
        if (isBornMintRoom) { BuiledMintRoom(); }
        if (isBornBerryTreeRoom) { BuiledBerryTreeRoom(); }
    }

    //=======================================如果生成的地图无法生成特殊房间时使用，重置所有地图============================================













    //==============================================生成特殊房间的部分========================================================

    //生成一个计算虚拟普通房间的函数
    void BuildVRoom()
    {

        //如果当前虚拟房间点不是一个虚拟房间，使该房间变为虚拟房间
        if(!VRoom.ContainsKey(NowPoint)) { VRoom.Add(NowPoint, 0); }

        //当当前虚拟房间数大于需要生成的最小房间数时，房间生成概率降低10%
        if(VRoom.Count > StepMin) { SpawnChance -= 0.12f; }

        //进行一次随机如果小于生成概率就随机向某个方向生成一个虚拟房间
        if (Random.Range(0.0f,1.0f) <= SpawnChance)
        {
            NowPoint += RandomRoomDirection();
            BuildVRoom();
        }

    }


    void BuiledPCroom()
    {
        if (!isPCRoomSpawn) {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != StoreRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowPCRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isPCRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowPCRoomPoint = item + Vector3Int.up;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.right))
                        {
                            roomname = NowPCRoomPoint.ToString();
                            VRoom.Add(NowPCRoomPoint, 0);
                            PCRoomPoint = NowPCRoomPoint;
                            isPCRoomSpawn = true;
                            Room room = Instantiate(PCRoomUp, new Vector3(NowPCRoomPoint.x * 30, NowPCRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "PC" + roomname;
                            RRoom.Add(NowPCRoomPoint, room);
                            return;
                        }
                    }
                    if (!isPCRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {
                        NowPCRoomPoint = item + Vector3Int.left;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up))
                        {
                            roomname = NowPCRoomPoint.ToString();
                            VRoom.Add(NowPCRoomPoint, 0);
                            PCRoomPoint = NowPCRoomPoint;
                            isPCRoomSpawn = true;
                            Room room = Instantiate(PCRoomLeft, new Vector3(NowPCRoomPoint.x * 30, NowPCRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "PC" + roomname;
                            RRoom.Add(NowPCRoomPoint, room);
                            return;
                        }
                    }
                    if (!isPCRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {
                        NowPCRoomPoint = item + Vector3Int.right;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowPCRoomPoint + Vector3Int.right))
                        {
                            roomname = NowPCRoomPoint.ToString();
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
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isPCRoomSpawn)
        {
            SpawnR += 0.2f;
            if(SpawnR >= 8.0f) { SpawnR = 1.0f; }
            PCCreatCount++;
            if (PCCreatCount >= 20) { ResetMap(); }
            else { BuiledPCroom(); }

        }
        SpawnR = 1.0f;

    }

    void BuiledStoreRoom()
    {
        if (!isStoreRoomSpawn) {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (item != BossRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowStoreRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isStoreRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowStoreRoomPoint = item + Vector3Int.up;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.right))
                        {
                            roomname = NowStoreRoomPoint.ToString();
                            VRoom.Add(NowStoreRoomPoint, 0);
                            StoreRoomPoint = NowStoreRoomPoint;
                            isStoreRoomSpawn = true;
                            Room room = Instantiate(StoreRoomUp, new Vector3(NowStoreRoomPoint.x * 30, NowStoreRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Store" + roomname;
                            RRoom.Add(NowStoreRoomPoint, room);
                            return;
                        }
                    }
                    if (!isStoreRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {
                        NowStoreRoomPoint = item + Vector3Int.left;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up))
                        {
                            roomname = NowStoreRoomPoint.ToString();
                            VRoom.Add(NowStoreRoomPoint, 0);
                            StoreRoomPoint = NowStoreRoomPoint;
                            isStoreRoomSpawn = true;
                            Room room = Instantiate(StoreRoomLeft, new Vector3(NowStoreRoomPoint.x * 30, NowStoreRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Store" + roomname;
                            RRoom.Add(NowStoreRoomPoint, room);
                            return;
                        }
                    }
                    if (!isStoreRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {
                        NowStoreRoomPoint = item + Vector3Int.right;
                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowStoreRoomPoint + Vector3Int.right))
                        {
                            roomname = NowStoreRoomPoint.ToString();
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

        }

        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isStoreRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            StoreCreatCount++;
            if (StoreCreatCount >= 20) { ResetMap(); }
            else { BuiledStoreRoom(); }
        }
        SpawnR = 1.0f;

    }

    void BuiledBossRoom()
    {
        if (!isBossRoomSpawn) {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != StoreRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowBossRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isBossRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowBossRoomPoint = item + Vector3Int.up;
                        if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBossRoomPoint.ToString();
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
                    if (!isBossRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {
                        NowBossRoomPoint = item + Vector3Int.left;
                        if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up))
                        {
                            roomname = NowBossRoomPoint.ToString();
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
                    if (!isBossRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {
                        NowBossRoomPoint = item + Vector3Int.right;
                        if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBossRoomPoint.ToString();
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

                    if (!isBossRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {
                        NowBossRoomPoint = item + Vector3Int.down;
                        if ((Random.Range(0.0f, 1.0f) < 0.25f) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBossRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBossRoomPoint.ToString();
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
        }


        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isBossRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BossRoomCreatCount++;
            if (BossRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledBossRoom(); }
        }
        SpawnR = 1.0f;

    }

    void BuiledSkillShopRoom()
    {

        if (!isSkillShopRoomSpawn)
        {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (item != BossRoomPoint && item != StoreRoomPoint && item != PCRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowSkillShopRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isSkillShopRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowSkillShopRoomPoint = item + Vector3Int.up;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.right))
                        {
                            roomname = NowSkillShopRoomPoint.ToString();
                            VRoom.Add(NowSkillShopRoomPoint, 0);
                            SkillShopRoomPoint = NowSkillShopRoomPoint;
                            isSkillShopRoomSpawn = true;
                            Room room = Instantiate(SkillShopRoom, new Vector3(NowSkillShopRoomPoint.x * 30, NowSkillShopRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "SkillShop" + roomname;
                            RRoom.Add(NowSkillShopRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isSkillShopRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {
                        

                        NowSkillShopRoomPoint = item + Vector3Int.left;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.up))
                        {
                            roomname = NowSkillShopRoomPoint.ToString();
                            VRoom.Add(NowSkillShopRoomPoint, 0);
                            SkillShopRoomPoint = NowSkillShopRoomPoint;
                            isSkillShopRoomSpawn = true;
                            Room room = Instantiate(SkillShopRoom, new Vector3(NowSkillShopRoomPoint.x * 30, NowSkillShopRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "SkillShop" + roomname;
                            RRoom.Add(NowSkillShopRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isSkillShopRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {
                        
                        NowSkillShopRoomPoint = item + Vector3Int.right;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.right))
                        {
                            roomname = NowSkillShopRoomPoint.ToString();
                            VRoom.Add(NowSkillShopRoomPoint, 0);
                            SkillShopRoomPoint = NowSkillShopRoomPoint;
                            isSkillShopRoomSpawn = true;
                            Room room = Instantiate(SkillShopRoom, new Vector3(NowSkillShopRoomPoint.x * 30, NowSkillShopRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "SkillShop" + roomname;
                            RRoom.Add(NowSkillShopRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isSkillShopRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {

                        NowSkillShopRoomPoint = item + Vector3Int.down;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowSkillShopRoomPoint + Vector3Int.right))
                        {
                            roomname = NowSkillShopRoomPoint.ToString();
                            VRoom.Add(NowSkillShopRoomPoint, 0);
                            SkillShopRoomPoint = NowSkillShopRoomPoint;
                            isSkillShopRoomSpawn = true;
                            Room room = Instantiate(SkillShopRoom, new Vector3(NowSkillShopRoomPoint.x * 30, NowSkillShopRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "SkillShop" + roomname;
                            RRoom.Add(NowSkillShopRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }

                }

            }
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isSkillShopRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            SkillShopRoomCreatCount++;
            if (SkillShopRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledSkillShopRoom(); }

        }
        SpawnR = 1.0f;
    }

    void BuiledMewRoom()
    {

        if (!isMewRoomSpawn)
        {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != StoreRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowMewRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isMewRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowMewRoomPoint = item + Vector3Int.up;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMewRoomPoint.ToString();
                            VRoom.Add(NowMewRoomPoint, 0);
                            MewRoomPoint = NowMewRoomPoint;
                            isMewRoomSpawn = true;
                            Room room = Instantiate(MewsRoom, new Vector3(NowMewRoomPoint.x * 30, NowMewRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "SkillShop" + roomname;
                            RRoom.Add(NowMewRoomPoint, room);
                            if (FloorNum.GlobalFloorNum != null) { FloorNum.GlobalFloorNum.isMewRoomBeCreated = true; }
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isMewRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {


                        NowMewRoomPoint = item + Vector3Int.left;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.up))
                        {
                            roomname = NowMewRoomPoint.ToString();
                            VRoom.Add(NowMewRoomPoint, 0);
                            MewRoomPoint = NowMewRoomPoint;
                            isMewRoomSpawn = true;
                            Room room = Instantiate(MewsRoom, new Vector3(NowMewRoomPoint.x * 30, NowMewRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "MewRoom" + roomname;
                            RRoom.Add(NowMewRoomPoint, room);
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMewRoomBeCreated = true;
                            }
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isMewRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {

                        NowMewRoomPoint = item + Vector3Int.right;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMewRoomPoint.ToString();
                            VRoom.Add(NowMewRoomPoint, 0);
                            MewRoomPoint = NowMewRoomPoint;
                            isMewRoomSpawn = true;
                            Room room = Instantiate(MewsRoom, new Vector3(NowMewRoomPoint.x * 30, NowMewRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "MewRoom" + roomname;
                            RRoom.Add(NowMewRoomPoint, room);
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMewRoomBeCreated = true;
                            }
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isMewRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {

                        NowMewRoomPoint = item + Vector3Int.down;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMewRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMewRoomPoint.ToString();
                            VRoom.Add(NowMewRoomPoint, 0);
                            MewRoomPoint = NowMewRoomPoint;
                            isMewRoomSpawn = true;
                            Room room = Instantiate(MewsRoom, new Vector3(NowMewRoomPoint.x * 30, NowMewRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "MewRoom" + roomname;
                            RRoom.Add(NowMewRoomPoint, room);
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMewRoomBeCreated = true;
                            }
                            room.CreatWall();
                            return;
                        }
                    }

                }

            }
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isMewRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            MewRoomCreatCount++;
            if (MewRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledMewRoom(); }

        }
        SpawnR = 1.0f;
    }

    void BuiledBabyCenterRoom()
    {

        if (!isBabyCenterRoomSpawn)
        {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != StoreRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != MintRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowBabyCenterRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isBabyCenterRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowBabyCenterRoomPoint = item + Vector3Int.up;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBabyCenterRoomPoint.ToString();
                            VRoom.Add(NowBabyCenterRoomPoint, 0);
                            BabyCenterRoomPoint = NowBabyCenterRoomPoint;
                            isBabyCenterRoomSpawn = true;
                            Room room = Instantiate(BabyCenterRoom, new Vector3(NowBabyCenterRoomPoint.x * 30, NowBabyCenterRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BabyCenter" + roomname;
                            RRoom.Add(NowBabyCenterRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isBabyCenterBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isBabyCenterRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {


                        NowBabyCenterRoomPoint = item + Vector3Int.left;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.up))
                        {
                            roomname = NowBabyCenterRoomPoint.ToString();
                            VRoom.Add(NowBabyCenterRoomPoint, 0);
                            BabyCenterRoomPoint = NowBabyCenterRoomPoint;
                            isBabyCenterRoomSpawn = true;
                            Room room = Instantiate(BabyCenterRoom, new Vector3(NowBabyCenterRoomPoint.x * 30, NowBabyCenterRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BabyCenter" + roomname;
                            RRoom.Add(NowBabyCenterRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isBabyCenterBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isBabyCenterRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {

                        NowBabyCenterRoomPoint = item + Vector3Int.right;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBabyCenterRoomPoint.ToString();
                            VRoom.Add(NowBabyCenterRoomPoint, 0);
                            BabyCenterRoomPoint = NowBabyCenterRoomPoint;
                            isBabyCenterRoomSpawn = true;
                            Room room = Instantiate(BabyCenterRoom, new Vector3(NowBabyCenterRoomPoint.x * 30, NowBabyCenterRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BabyCenter" + roomname;
                            RRoom.Add(NowBabyCenterRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isBabyCenterBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isBabyCenterRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {

                        NowBabyCenterRoomPoint = item + Vector3Int.down;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBabyCenterRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBabyCenterRoomPoint.ToString();
                            VRoom.Add(NowBabyCenterRoomPoint, 0);
                            BabyCenterRoomPoint = NowBabyCenterRoomPoint;
                            isBabyCenterRoomSpawn = true;
                            Room room = Instantiate(BabyCenterRoom, new Vector3(NowBabyCenterRoomPoint.x * 30, NowBabyCenterRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BabyCenter" + roomname;
                            RRoom.Add(NowBabyCenterRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isBabyCenterBeCreated = true;
                            }
                            return;
                        }
                    }

                }

            }
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isBabyCenterRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BabyCenterRoomCreatCount++;
            if (BabyCenterRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledBabyCenterRoom(); }

        }
        SpawnR = 1.0f;
    }

    void BuiledMintRoom()
    {

        if (!isMintRoomSpawn)
        {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (item != BossRoomPoint && item != StoreRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != BerryTreeRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowMintRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isMintRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowMintRoomPoint = item + Vector3Int.up;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMintRoomPoint.ToString();
                            VRoom.Add(NowMintRoomPoint, 0);
                            MintRoomPoint = NowMintRoomPoint;
                            isMintRoomSpawn = true;
                            Room room = Instantiate(MintRoom, new Vector3(NowMintRoomPoint.x * 30, NowMintRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Mint" + roomname;
                            RRoom.Add(NowMintRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMintRoomBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isMintRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {


                        NowMintRoomPoint = item + Vector3Int.left;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.up))
                        {
                            roomname = NowMintRoomPoint.ToString();
                            VRoom.Add(NowMintRoomPoint, 0);
                            MintRoomPoint = NowMintRoomPoint;
                            isMintRoomSpawn = true;
                            Room room = Instantiate(MintRoom, new Vector3(NowMintRoomPoint.x * 30, NowMintRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Mint" + roomname;
                            RRoom.Add(NowMintRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMintRoomBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isMintRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {

                        NowMintRoomPoint = item + Vector3Int.right;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMintRoomPoint.ToString();
                            VRoom.Add(NowMintRoomPoint, 0);
                            MintRoomPoint = NowMintRoomPoint;
                            isMintRoomSpawn = true;
                            Room room = Instantiate(MintRoom, new Vector3(NowMintRoomPoint.x * 30, NowMintRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Mint" + roomname;
                            RRoom.Add(NowMintRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMintRoomBeCreated = true;
                            }
                            return;
                        }
                    }
                    if (!isMintRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {

                        NowMintRoomPoint = item + Vector3Int.down;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowMintRoomPoint + Vector3Int.right))
                        {
                            roomname = NowMintRoomPoint.ToString();
                            VRoom.Add(NowMintRoomPoint, 0);
                            MintRoomPoint = NowMintRoomPoint;
                            isMintRoomSpawn = true;
                            Room room = Instantiate(MintRoom, new Vector3(NowMintRoomPoint.x * 30, NowMintRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "Mint" + roomname;
                            RRoom.Add(NowMintRoomPoint, room);
                            room.CreatWall();
                            if (FloorNum.GlobalFloorNum != null)
                            {
                                FloorNum.GlobalFloorNum.isMintRoomBeCreated = true;
                            }
                            return;
                        }
                    }

                }

            }
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isMintRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            MintRoomCreatCount++;
            if (MintRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledMintRoom(); }

        }
        SpawnR = 1.0f;
    }

    void BuiledBerryTreeRoom()
    {

        if (!isBerryTreeRoomSpawn)
        {
            //遍历所有虚拟房间，如果该房间距离初始房间的距离大于房间最小数字的平方初一生成半径，且该房间不是boss房间或者商店房间，有概率在刚房间周围生成PC房间
            foreach (Vector3Int item in VRoom.Keys)
            {
                if (item != BossRoomPoint && item != StoreRoomPoint && item != PCRoomPoint && item != SkillShopRoomPoint && item != MewRoomPoint && item != BabyCenterRoomPoint && item != MintRoomPoint)
                {
                    string roomname = item.ToString();
                    //声明一个坐标变量，用来储存当前遍历房间的周围房间的坐标
                    Vector3Int NowBerryTreeRoomPoint;

                    //如果当前房间的上面无房间，则以该房间上面的房间作为临时坐标，
                    //检查临时坐标房间的上左右是否为空，如果为空有10%的概率添加该房间进入虚拟房间，并且PC房间坐标等于当前坐标，
                    //既之后以该虚拟坐标生成PC房间。PC房间是否生成的布尔型变量变为是，并且跳出该函数。
                    //之后也用相似方法检测该房间的左和右，但不检测下
                    if (!isBerryTreeRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.up))
                    {
                        NowBerryTreeRoomPoint = item + Vector3Int.up;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBerryTreeRoomPoint.ToString();
                            VRoom.Add(NowBerryTreeRoomPoint, 0);
                            BerryTreeRoomPoint = NowBerryTreeRoomPoint;
                            isBerryTreeRoomSpawn = true;
                            Room room = Instantiate(BerryTreeRoom, new Vector3(NowBerryTreeRoomPoint.x * 30, NowBerryTreeRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BerryTree" + roomname;
                            RRoom.Add(NowBerryTreeRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isBerryTreeRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.left))
                    {


                        NowBerryTreeRoomPoint = item + Vector3Int.left;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.up))
                        {
                            roomname = NowBerryTreeRoomPoint.ToString();
                            VRoom.Add(NowBerryTreeRoomPoint, 0);
                            BerryTreeRoomPoint = NowBerryTreeRoomPoint;
                            isBerryTreeRoomSpawn = true;
                            Room room = Instantiate(BerryTreeRoom, new Vector3(NowBerryTreeRoomPoint.x * 30, NowBerryTreeRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BerryTree" + roomname;
                            RRoom.Add(NowBerryTreeRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isBerryTreeRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.right))
                    {

                        NowBerryTreeRoomPoint = item + Vector3Int.right;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.up) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBerryTreeRoomPoint.ToString();
                            VRoom.Add(NowBerryTreeRoomPoint, 0);
                            BerryTreeRoomPoint = NowBerryTreeRoomPoint;
                            isBerryTreeRoomSpawn = true;
                            Room room = Instantiate(BerryTreeRoom, new Vector3(NowBerryTreeRoomPoint.x * 30, NowBerryTreeRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BerryTree" + roomname;
                            RRoom.Add(NowBerryTreeRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }
                    if (!isBerryTreeRoomSpawn && !VRoom.ContainsKey(item + Vector3Int.down))
                    {

                        NowBerryTreeRoomPoint = item + Vector3Int.down;

                        if ((Random.Range(0.0f, 1.0f) < 0.10f) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.left) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.down) && !VRoom.ContainsKey(NowBerryTreeRoomPoint + Vector3Int.right))
                        {
                            roomname = NowBerryTreeRoomPoint.ToString();
                            VRoom.Add(NowBerryTreeRoomPoint, 0);
                            BerryTreeRoomPoint = NowBerryTreeRoomPoint;
                            isBerryTreeRoomSpawn = true;
                            Room room = Instantiate(BerryTreeRoom, new Vector3(NowBerryTreeRoomPoint.x * 30, NowBerryTreeRoomPoint.y * 24, 0), Quaternion.identity);
                            room.transform.name = "BerryTree" + roomname;
                            RRoom.Add(NowBerryTreeRoomPoint, room);
                            room.CreatWall();
                            return;
                        }
                    }

                }

            }
        }
        //如果本轮没有生成商店，增加生成半径并递归，如果生成半径过大重置生成半径
        if (!isBerryTreeRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            BerryTreeRoomCreatCount++;
            if (BerryTreeRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledBerryTreeRoom(); }

        }
        SpawnR = 1.0f;
    }

    //==============================================生成特殊房间的部分========================================================



























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



    /// <summary>
    /// 根据房间形状，在当前房间白名单内选择一个房间生成
    /// </summary>
    /// <returns></returns>
    Room SwithABaseRoom( Vector3Int RoomVector )
    {

        //当前房间周围是否有房间的情况 ， 如某一个房间上下右有房间 ， 则该房间Blocked = true ，true ， false ，true
        //对于某一房间在所有方向，如果某方向有相邻的房间，则该方向被房间内环境物体堵住的房间不可被采用
        //既当某一方向没有相邻房间时 ， 该方向是否被环境物阻挡皆可 ， 当有相邻房间时，该房间的该方向必须不被阻挡
        //也就是 i=0,1,2,3时，满足(!Blcoked[i] || (Blocked && !Room.isBlockerIN[i])的房间才可被采用
        

        bool[] Bolcked = new bool[] { VRoom.ContainsKey(RoomVector + Vector3Int.up)  , VRoom.ContainsKey(RoomVector + Vector3Int.down) , VRoom.ContainsKey(RoomVector + Vector3Int.left) , VRoom.ContainsKey(RoomVector + Vector3Int.right) };

        int x = RoomWhiteList[Random.Range(0, RoomWhiteList.Count)];
        int count = 0;
        while (!JudgeRoomBlacked(Bolcked, BaseRoomList.transform.GetChild(x).GetComponent<Room>()))
        {
            x = RoomWhiteList[Random.Range(0, RoomWhiteList.Count)];
            count += 1;
            if (count == 20) {
                
                RoomBlackList = RoomWhiteList.Union(RoomBlackList).ToList();
                RoomWhiteList = RoomBlackList;
                RoomBlackList = new List<int> { };
                string DebugString = "";
                for (int j = 0; j < RoomWhiteList.Count; j++) { DebugString += RoomWhiteList[j].ToString() + ","; }
                Debug.Log(DebugString + "+" + RoomWhiteList.Count + "+" + RoomBlackList.Count + "+" + RoomVector);
            }
            if (count >= 100)
            {
                return StarRoom;
                break;
            }
        }

        RoomWhiteList.Remove(x);
        RoomBlackList.Add(x);
        if (RoomWhiteList.Count == 0)
        {
            RoomWhiteList = RoomBlackList;
            RoomBlackList = new List<int> { };
        }
        Room OutPut = BaseRoomList.transform.GetChild(x).GetComponent<Room>();
        return OutPut;
    }

    /// <summary>
    /// 判断某一个房间的阻挡情况是否可以被采用
    /// </summary>
    bool JudgeRoomBlacked( bool[] Blocked , Room JudgeRoom )
    {
        bool Output = true;
        for (int i = 0; i <= 3; i++)
        {
            if (Blocked[i] && JudgeRoom.isBlockerIn[i]) { Output = false; }
        }
        if (Output) { Debug.Log(JudgeRoom + "+" + Blocked[0]+"+"+ Blocked[1] + "+" + Blocked[2] + "+" + Blocked[3] + "+" + JudgeRoom.isBlockerIn[0] + "+" + JudgeRoom.isBlockerIn[1] + "+" + JudgeRoom.isBlockerIn[2] + "+" + JudgeRoom.isBlockerIn[3]); }
        return Output;
    }




}
