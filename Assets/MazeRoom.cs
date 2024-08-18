using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoom : MonoBehaviour
{
    //�Թ�ǽ��
    public GameObject Wall;

    //�Թ��еĵ���
    public GameObject DropItem;
    Vector2Int ItemPos = new Vector2Int(-1,-1);

    public GameObject ItemSpike;


    //�Թ��Ŀ�͸�
    public int MazeWeight;
    public int Mazehight;

    //ǰ��λΪ�����յ�����꣬���һλ�����ķ���˳��Ϊ�������ң�������Start=��0��3��2���������Ϊ0��3�����ǽ
    public Vector3Int StartPoint;
    public Vector3Int GoalPoint;

    //����ǽ��ƫ����
    public Vector3 WallOffset;

    //�Թ��и���Ԫ
    struct MazeBlockUnit
    {

        public MazeBlockUnit(bool v , bool u, bool d, bool l, bool r) : this()
        {
            isVisited = v;
            isEmptyUp = u;
            isEmptyDown = d;
            isEmptyLeft = l;
            isEmptyRight = r;
        }

        //�õ�Ԫ�Ƿ񱻷���
        public bool isVisited ;

        //�õ�Ԫ�ϡ��£����ҷ��Ƿ���ǽ
        public bool isEmptyUp;
        public bool isEmptyDown;
        public bool isEmptyLeft;
        public bool isEmptyRight;

    }
    MazeBlockUnit[,] VMaze = new MazeBlockUnit[0,0];


    int ErrorCounte = 0;
    bool Error;

    // Start is called before the first frame update
    void Start()
    {
        WallOffset += transform.parent.position;
        VMaze = new MazeBlockUnit[MazeWeight, Mazehight];
        BuildVMap(0, 0);
        //PrintMaze();
        if (!Error) { BuildRMaze(); }
    }

    //��ӡ�Թ���Ԫ���
    void PrintMaze()
    {
        for (int i = 0; i < MazeWeight; i++)
        {
            for (int j = 0; j < Mazehight; j++)
            {
                Debug.Log(new Vector2(i,j) + "+" + VMaze[i, j].isVisited + "+" + VMaze[i, j].isEmptyUp + "+" + VMaze[i, j].isEmptyDown + "+" + VMaze[i, j].isEmptyLeft + "+" + VMaze[i, j].isEmptyRight);
            }
        }
    }

    //̽�����Թ���ͼ
    void BuildVMap ( int i , int j )
    {
        //Debug.Log(new Vector2Int(i,j));
        VMaze[i, j].isVisited = true;
        Vector2Int dir = RandomDir(i,j);
        while (dir != Vector2Int.zero) 
        {
            
            if (dir == Vector2.up) { VMaze[i, j].isEmptyUp = true; VMaze[i+dir.x, j+dir.y].isEmptyDown = true; }
            else if (dir == Vector2.down) { VMaze[i, j].isEmptyDown = true; VMaze[i + dir.x, j + dir.y].isEmptyUp = true; }
            else if (dir == Vector2.left) { VMaze[i, j].isEmptyLeft = true; VMaze[i + dir.x, j + dir.y].isEmptyRight = true; }
            else if (dir == Vector2.right) { VMaze[i, j].isEmptyRight = true; VMaze[i + dir.x, j + dir.y].isEmptyLeft = true; }
            BuildVMap(i + dir.x, j + dir.y);
            dir = RandomDir(i, j);

            ErrorCounte++;
            if (ErrorCounte >= 10000) { Error = true; return; }
        }
        if (dir == Vector2Int.zero && ItemPos == new Vector2Int(-1, -1)) { ItemPos = new Vector2Int(i, j); Debug.Log(ItemPos); }
    }

    //������ʵ��ͼ
    void BuildRMaze()
    {
        for (int i = 0; i < MazeWeight; i++)
        {
            for (int j = 0; j < Mazehight; j++)
            {
                
                Instantiate(Wall, new Vector3(i * 2 + 1.0f, j * 2 + 1.0f, 0) + WallOffset, Quaternion.Euler(0,0,0), transform); 
                if (i == 0) { Instantiate(Wall, new Vector3(i * 2 - 1.0f, j * 2 + 1.0f, 0) + WallOffset, Quaternion.Euler(0, 0, 0), transform);  }
                if (j == 0) { Instantiate(Wall, new Vector3(i * 2 + 1.0f, j * 2 - 1.0f, 0) + WallOffset, Quaternion.Euler(0, 0, 0), transform);  }
                if (i == 0 && j == 0) { Instantiate(Wall, new Vector3(i * 2 - 1.0f, j * 2 - 1.0f, 0) + WallOffset, Quaternion.Euler(0, 0, 0), transform);  }

                if ((i == StartPoint.x && j == StartPoint.y))
                {
                    Debug.Log(StartPoint);
                    if ((!VMaze[i, j].isEmptyUp || j == Mazehight - 1) && StartPoint.z != 0) { Instantiate(Wall, new Vector3(i * 2, j * 2 + 1.0f, 0) + WallOffset, Quaternion.identity, transform); }
                    if ((!VMaze[i, j].isEmptyRight || i == MazeWeight - 1) && StartPoint.z != 3) { Instantiate(Wall, new Vector3(i * 2 + 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform);}
                    if ((j == 0) && StartPoint.z != 1) { Instantiate(Wall, new Vector3(i * 2, j * 2 - 1.0f, 0) + WallOffset, Quaternion.identity, transform);  }
                    if ((i == 0) && StartPoint.z != 2) { Instantiate(Wall, new Vector3(i * 2 - 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform);  }
                }
                else if ((i == GoalPoint.x && j == GoalPoint.y))
                {
                    Debug.Log(GoalPoint);
                    if ((!VMaze[i, j].isEmptyUp || j == Mazehight - 1) && GoalPoint.z != 0) { Instantiate(Wall, new Vector3(i * 2, j * 2 + 1.0f, 0) + WallOffset, Quaternion.identity, transform);  }
                    if ((!VMaze[i, j].isEmptyRight || i == MazeWeight - 1) && GoalPoint.z != 3) { Instantiate(Wall, new Vector3(i * 2 + 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform); }
                    if ((j == 0) && GoalPoint.z != 1) { Instantiate(Wall, new Vector3(i * 2, j * 2 - 1.0f, 0) + WallOffset, Quaternion.identity, transform);  }
                    if ((i == 0) && GoalPoint.z != 2) { Instantiate(Wall, new Vector3(i * 2 - 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform);  }
                }
                else
                {
                    if ((!VMaze[i, j].isEmptyUp || j == Mazehight - 1)) { Instantiate(Wall, new Vector3(i * 2, j * 2 + 1.0f, 0) + WallOffset, Quaternion.identity, transform);  }
                    if ((!VMaze[i, j].isEmptyRight || i == MazeWeight - 1)) { Instantiate(Wall, new Vector3(i * 2 + 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform); }
                    if ((j == 0)) { Instantiate(Wall, new Vector3(i * 2, j * 2 - 1.0f, 0) + WallOffset, Quaternion.identity, transform); }
                    if ((i == 0)) { Instantiate(Wall, new Vector3(i * 2 - 1.0f, j * 2, 0) + WallOffset, Quaternion.identity, transform);  }
                }
                //Debug.Log(s);

                if (ItemPos == new Vector2Int(i, j))
                {
                    Instantiate(DropItem, new Vector3(i * 2, j * 2 + 0.25f, 0) + WallOffset, Quaternion.identity, transform.parent.transform.GetChild(4));
                    if (VMaze[i, j].isEmptyUp) { Instantiate(ItemSpike, new Vector3(i * 2, j * 2 + 0.4f + 1.0f, 0) + WallOffset, Quaternion.identity, transform); }
                    else if (VMaze[i, j].isEmptyDown) { Instantiate(ItemSpike, new Vector3(i * 2, j * 2 + 0.4f - 1.0f, 0) + WallOffset, Quaternion.identity, transform); }
                    else if (VMaze[i, j].isEmptyLeft) { Instantiate(ItemSpike, new Vector3(i * 2 - 1.0f, j * 2 + 0.4f, 0) + WallOffset, Quaternion.identity, transform); }
                    else if (VMaze[i, j].isEmptyRight){Instantiate(ItemSpike, new Vector3(i * 2 + 1.0f, j * 2 + 0.4f, 0) + WallOffset, Quaternion.identity, transform);                }

                }

            }
        }

    }


    //Ϊ��Ԫ��i��j����̽��������һ������
    Vector2Int RandomDir( int i , int j ) 
    {
        Vector2Int Output = Vector2Int.zero;
        //�򲹷���
        List<Vector2Int> DirList = new List<Vector2Int> { };
        //��Ԫ����е�Ԫ��δ�����ʣ�����������
        if (i > 0 && !VMaze[i-1,j].isVisited) {
            DirList.Add(Vector2Int.left); 
            //Debug.Log(new Vector2Int(i, j)+"Left");
        }
        //��Ԫ�ұ��е�Ԫ��δ�����ʣ�����ұ�����
        if (i < MazeWeight - 1 && !VMaze[i + 1, j].isVisited) {
            DirList.Add(Vector2Int.right);
            //Debug.Log(new Vector2Int(i, j) + "Right");
        }
        //��Ԫxia���е�Ԫ��δ�����ʣ����xia������
        if (j > 0 && !VMaze[i, j-1].isVisited) {
            DirList.Add(Vector2Int.down); 
            //Debug.Log(new Vector2Int(i, j) + "Down");
        }
        //��Ԫshang���е�Ԫ��δ�����ʣ����shang������
        if (j < Mazehight - 1 && !VMaze[i, j+1].isVisited) {
            DirList.Add(Vector2Int.up);
            //Debug.Log(new Vector2Int(i, j) + "Up");
        }
        if (DirList.Count != 0) {
            Output = DirList[Random.Range(0, DirList.Count)]; }
        return Output;
    }
}
