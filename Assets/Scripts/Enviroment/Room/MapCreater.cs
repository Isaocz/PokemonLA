using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{

    public static MapCreater StaticMap; 
    //����һ�������������ʾ�����ķ���
    public Room StarRoom;
    Room BaseRoom;
    public GameObject BaseRoomList;
    //����һ�������������ʾ���������ķ���,һ����������������洢PC������������꣬һ�������ͱ�������ʾ�Ƿ����ɹ�PC���䡣
    //����һ�������������ʾ�̵귿��,һ����������������洢�̵귿����������꣬һ�������ͱ�������ʾ�Ƿ����ɹ��̵귿�䡣
    //����һ�������������ʾboss����,һ����������������洢boss������������꣬һ�������ͱ�������ʾ�Ƿ����ɹ�boss���䡣
    public Room PCRoomUp;
    public Room PCRoomLeft;
    public Room PCRoomRight;
    public Vector3Int PCRoomPoint;
    bool isPCRoomSpawn;
    int PCCreatCount;

    public Room StoreRoomUp;
    public Room StoreRoomLeft;
    public Room StoreRoomRight;
    public Vector3Int StoreRoomPoint;
    bool isStoreRoomSpawn;
    int StoreCreatCount;


    public Room BossRoom;
    public Vector3Int BossRoomPoint;
    bool isBossRoomSpawn;
    int BossRoomCreatCount;

    bool isReset;

    //����һ�����α�������ʾ���ɵ���С���䡣
    //һ�������ͱ�������ʾ���ɸ��ʡ�
    //һ�������ͱ�������ʾ���ⷿ������ɰ뾶��
    public int StepMin;
    float SpawnChance = 1.0f;
    float SpawnR = 1.0f;


    //����һ���������飬��ʾ����ķ��䲼��
    //һ��������ʾ���ⷿ�䲼�ֵ�Ŀǰ�����
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
        if (FloorNum.GlobalFloorNum != null) { StepMin = FloorNum.GlobalFloorNum.MapSize[FloorNum.GlobalFloorNum.FloorNumber]; }
        for (int i = 0; i < BaseRoomList.transform.childCount; i++)
        {
            RoomWhiteList.Add(i);
        }
        //��ʼ�������ͼ
        VRoom.Clear();
        RRoom.Clear();
        //���ɵ�ͼ
        CreateRoom();
        UiMiniMap.Instance.CreatMiniMap();


    }



    //����һ��������ʵ����ĺ���
    void CreateRoom()
    {
        if (!isReset)
        {
            //�������ⷿ���PC������̵�
            BuildVRoom();
            BuiledPCroom();
            BuiledStoreRoom();
            BuiledBossRoom();

        }
        foreach (Vector3 k in RRoom.Keys)
        {
            Debug.Log(k);
        }
        //�����������ⷿ�䣬��������겻�����ⷿ�䣬�ڸ�����������ʵ����
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

    //������ɵĵ�ͼ�޷��������ⷿ��ʱʹ�ã��������е�ͼ
    void ResetMap()
    {
        isReset = true;
        Debug.Log("MapReset");
        foreach( Room room in RRoom.Values)
        {
            Destroy(room.gameObject);
        }
        isPCRoomSpawn = false; isStoreRoomSpawn = false; isBossRoomSpawn = false;
        PCCreatCount = 0; StoreCreatCount = 0; BossRoomCreatCount = 0;
        PCRoomPoint = Vector3Int.zero; StoreRoomPoint = Vector3Int.zero; BossRoomPoint = Vector3Int.zero; NowPoint = Vector3Int.zero;
        SpawnR = 1.0f; SpawnChance = 1.0f;
        VRoom.Clear(); RRoom.Clear();
        RRoom = new Dictionary<Vector3Int, Room> { };
        BuildVRoom();
        BuiledPCroom();
        BuiledStoreRoom();
        BuiledBossRoom();

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

    //����һ������������ͨ����ĺ���
    void BuildVRoom()
    {

        //�����ǰ���ⷿ��㲻��һ�����ⷿ�䣬ʹ�÷����Ϊ���ⷿ��
        if(!VRoom.ContainsKey(NowPoint)) { VRoom.Add(NowPoint, 0); }

        //����ǰ���ⷿ����������Ҫ���ɵ���С������ʱ���������ɸ��ʽ���10%
        if(VRoom.Count > StepMin) { SpawnChance -= 0.1f; }

        //����һ��������С�����ɸ��ʾ������ĳ����������һ�����ⷿ��
        if (Random.Range(0.0f,1.0f) <= SpawnChance)
        {
            NowPoint += RandomRoomDirection();
            BuildVRoom();
        }

    }


    void BuiledPCroom()
    {

        //�����������ⷿ�䣬����÷�������ʼ����ľ�����ڷ�����С���ֵ�ƽ����һ���ɰ뾶���Ҹ÷��䲻��boss��������̵귿�䣬�и����ڸշ�����Χ����PC����
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != StoreRoomPoint)
            {
                string roomname = item.ToString();
                //����һ������������������浱ǰ�����������Χ���������
                Vector3Int NowPCRoomPoint;

                //�����ǰ����������޷��䣬���Ը÷�������ķ�����Ϊ��ʱ���꣬
                //�����ʱ���귿����������Ƿ�Ϊ�գ����Ϊ����10%�ĸ�����Ӹ÷���������ⷿ�䣬����PC����������ڵ�ǰ���꣬
                //��֮���Ը�������������PC���䡣PC�����Ƿ����ɵĲ����ͱ�����Ϊ�ǣ����������ú�����
                //֮��Ҳ�����Ʒ������÷��������ң����������
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
        //�������û�������̵꣬�������ɰ뾶���ݹ飬������ɰ뾶�����������ɰ뾶
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

        //�����������ⷿ�䣬����÷�������ʼ����ľ�����ڷ�����С���ֵ�ƽ����һ���ɰ뾶���Ҹ÷��䲻��boss��������̵귿�䣬�и����ڸշ�����Χ����PC����
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != BossRoomPoint && item != PCRoomPoint)
            {
                string roomname = item.ToString();
                //����һ������������������浱ǰ�����������Χ���������
                Vector3Int NowStoreRoomPoint;

                //�����ǰ����������޷��䣬���Ը÷�������ķ�����Ϊ��ʱ���꣬
                //�����ʱ���귿����������Ƿ�Ϊ�գ����Ϊ����10%�ĸ�����Ӹ÷���������ⷿ�䣬����PC����������ڵ�ǰ���꣬
                //��֮���Ը�������������PC���䡣PC�����Ƿ����ɵĲ����ͱ�����Ϊ�ǣ����������ú�����
                //֮��Ҳ�����Ʒ������÷��������ң����������
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

        //�������û�������̵꣬�������ɰ뾶���ݹ飬������ɰ뾶�����������ɰ뾶
        if (!isStoreRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            if (StoreCreatCount >= 20) { ResetMap(); }
            else { BuiledStoreRoom(); }
        }
        SpawnR = 1.0f;

    }

    void BuiledBossRoom()
    {

        //�����������ⷿ�䣬����÷�������ʼ����ľ�����ڷ�����С���ֵ�ƽ����һ���ɰ뾶���Ҹ÷��䲻��boss��������̵귿�䣬�и����ڸշ�����Χ����PC����
        foreach (Vector3Int item in VRoom.Keys)
        {
            if (Vector3Int.Distance(item, Vector3Int.zero) > (Mathf.Sqrt(StepMin)) / SpawnR && item != StoreRoomPoint && item != PCRoomPoint)
            {
                string roomname = item.ToString();
                //����һ������������������浱ǰ�����������Χ���������
                Vector3Int NowBossRoomPoint;

                //�����ǰ����������޷��䣬���Ը÷�������ķ�����Ϊ��ʱ���꣬
                //�����ʱ���귿����������Ƿ�Ϊ�գ����Ϊ����10%�ĸ�����Ӹ÷���������ⷿ�䣬����PC����������ڵ�ǰ���꣬
                //��֮���Ը�������������PC���䡣PC�����Ƿ����ɵĲ����ͱ�����Ϊ�ǣ����������ú�����
                //֮��Ҳ�����Ʒ������÷��������ң����������
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

        //�������û�������̵꣬�������ɰ뾶���ݹ飬������ɰ뾶�����������ɰ뾶
        if (!isBossRoomSpawn)
        {
            SpawnR += 0.2f;
            if (SpawnR >= 8.0f) { SpawnR = 1.0f; }
            if (BossRoomCreatCount >= 20) { ResetMap(); }
            else { BuiledBossRoom(); }
        }
        SpawnR = 1.0f;

    }

    //һ���������ĺ���
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
