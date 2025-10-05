using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sudoku66Manager : MonoBehaviour
{
    /// <summary>
    /// �������� �� ��������Ϊ6
    /// </summary>
    static int RCB = 6;

    /// <summary>
    /// ��Ԫ���б�
    /// </summary>
    SudokuUnit[,] SudokuUnitsList = new SudokuUnit[RCB, RCB];


    /// <summary>
    /// ����ģ��ʱ��ĳһ��ĳһ��ĳһ���Ѿ��е����� ��Vector2Int.x = 0 ��Vector2Int.x = 1 ��Vector2Int.x = 2
    /// </summary>
    Dictionary<Vector2Int, List<int>> RCBCondition = new Dictionary<Vector2Int, List<int>>();


    /// <summary>
    /// �ڶ���������ʱ��ĳһ��ĳһ��ĳһ���Ѿ��е����� ��Vector2Int.x = 0 ��Vector2Int.x = 1 ��Vector2Int.x = 2
    /// </summary>
    Dictionary<Vector2Int, List<int>> RCBDig = new Dictionary<Vector2Int, List<int>>();

    /// <summary>
    /// �ɹ���Ч
    /// </summary>
    public AudioClip SucessSE;
    /// <summary>
    /// ʧ����Ч
    /// </summary>
    public AudioClip FaildSE;
    AudioSource SEAudio;


    public GameObject Success;


    int[,] DefaultBoard =
    {
        { 0,0,0,1,1,1 },
        { 0,0,0,1,1,1 },
        { 2,2,2,3,3,3 },
        { 2,2,2,3,3,3 },
        { 4,4,4,5,5,5 },
        { 4,4,4,5,5,5 }
    };

    /// <summary>
    /// ����ð�ť
    /// </summary>
    public SudokuButton CheckButton;

    /// <summary>
    /// ������Ϊ����ĵ�Ԫ��
    /// </summary>
    public SudokuUnit QuestUnit;
    /// <summary>
    /// ���ڶ����ǲ�������ĵ�Ԫ��
    /// </summary>
    public List<SudokuUnit> EmptyUnit = new List<SudokuUnit> { };

















    // Start is called before the first frame update
    void Start()
    {
        int checkCount = 0;
        do
        {
            //��ʼ������
            InitRowCol();
            //����
            int[,] Block = BlockBoard();
            //�������ж�
            checkCount++;
            if (checkCount > 4) { Block = DefaultBoard; }
            SetUnitObjBlock(Block);
            //��ʼ���ֵ�
            InitDictionary();
        }
        while (!FillAnswer());//�����
        //�����ڶ��ֵ�
        InitRCBDig();
        //���õ�Ԫ����ʾ����
        SetUnitDisplayeNum();
        //����
        Dig(12);
        //��������
        SetQuest();


        //���õ�Ԫ��
        List<int> BlockColor = Enumerable.Range(0, 6).OrderBy(_ => Random.value).ToList();
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    SudokuUnit u = GetUnitByVector(ij);
                    u.SetUnit(RCB, 1.2f , BlockColor);
                }
            }
        }



        //���ô𰸼�����
        CheckButton.parentManager = this;

        SEAudio = transform.GetComponent<AudioSource>();
    }

















    //========================���������Ƿ���ȷ=================================
    /// <summary>
    /// �������Ĵ��Ƿ���ȷ
    /// </summary>
    public void CheckInputAnwser()
    {
        
        //δ����
        if (QuestUnit.InputNumList.Count == 0)
        {
            //CheckButton.StartCoroutine()
        }
        //�������ȡ�������ֵ
        else
        {
            //��ʾ��
            StartCoroutine(DisPlayAnswer());
        }
    }


    /// <summary>
    /// ��ʾ��
    /// </summary>
    /// <returns></returns>
    IEnumerator DisPlayAnswer()
    {
        yield return new WaitForSeconds(0.5f);
        while (EmptyUnit.Count > 0)
        {
            int i = Random.Range(0, EmptyUnit.Count);
            EmptyUnit[i].DisplayAnswer(RCB);
            EmptyUnit.Remove(EmptyUnit[i]);
            Debug.Log("While" + EmptyUnit.Count);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);

        if (EmptyUnit.Count == 0)
        {
            PlayerControler player = GameObject.FindObjectOfType<PlayerControler>();
            string s = "";
            //����������ֵ���ڴ� �ɹ��⿪
            if (QuestUnit.InputNumList[QuestUnit.InputNumList.Count - 1].BoxIndex == QuestUnit.AnswerNum)
            {
                //Debug.Log("Scuess");
                SEAudio.clip = SucessSE;
                SEAudio.Play();
                switch (QuestUnit.AnswerNum)
                {
                    case 1: player.playerData.HPBounsAlways++; s = "����ֵ"; break;
                    case 2: player.playerData.AtkBounsAlways++; s = "����"; break;
                    case 3: player.playerData.SpABounsAlways++; s = "�ع�"; break;
                    case 4: player.playerData.DefBounsAlways++; s = "����"; break;
                    case 5: player.playerData.SpDBounsAlways++; s = "�ط�"; break;
                    case 6: player.playerData.SpeBounsAlways++; s = "�����ٶ�"; break;
                };
                UIGetANewItem.UI.JustSaySth("����ˣ�", s + "�������ӳ������ˣ�");
                Instantiate(Success , QuestUnit.InputNumList[QuestUnit.InputNumList.Count - 1].transform.position + 1.2f*Vector3.up , Quaternion.Euler(new Vector3(45,0,0)) );
                Instantiate(Success , player.transform.position + player.SkillOffsetforBodySize[0] * Vector3.up , Quaternion.Euler(new Vector3(45, 0, 0)));
            }
            //����������ֵ�����ڴ� �⿪ʧ��
            else
            {
                //Debug.Log("Faild");
                SEAudio.clip = FaildSE;
                SEAudio.Play();
                switch (QuestUnit.AnswerNum)
                {
                    case 1: player.playerData.HPBounsAlways--; s = "����ֵ"; break;
                    case 2: player.playerData.AtkBounsAlways--; s = "����"; break;
                    case 3: player.playerData.SpABounsAlways--; s = "�ع�"; break;
                    case 4: player.playerData.DefBounsAlways--; s = "����"; break;
                    case 5: player.playerData.SpDBounsAlways--; s = "�ط�"; break;
                    case 6: player.playerData.SpeBounsAlways--; s = "�����ٶ�"; break;
                };
                UIGetANewItem.UI.JustSaySth("����ˣ�",s+"�������ӳ��½��ˣ�");
            }
            StopCoroutine(DisPlayAnswer());
            player.ReFreshAbllityPoint();
        }
    }



    //========================���������Ƿ���ȷ=================================





















    //========================��������=================================

    /// <summary>
    /// ��������
    /// </summary>
    void SetQuest()
    {
        //���е�Ԫ���к�����������ֵ
        int MaxALLength = -1;
        //�����������ٵ�һ����Ԫ��ĺ򲹶���
        List<int> MaxALAlternateList = new List<int> { };
        //�����������ٵĵ�Ԫ��Ķ���
        List<Vector2Int> MaxALLengthVector2 = new List<Vector2Int> { };

        //�������л�û�����𰸵ĵ�Ԫ�� �ҵ���ѡ������һ����Ԫ��
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //�������л�û�����𰸵ĵ�Ԫ��
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.DisplayNum == -1)
                    {
                        EmptyUnit.Add(GetUnitByVector(ij));
                        //��ȡ������
                        List<int> AL = Dig_GetUnitAlternateNum(ij);
                        if (AL.Count > MaxALLength)
                        {
                            //������ٺ򲹵ĵ�Ԫ��û�к򲹣��򲹶���Ϊ0�� ���������Ϸ� ����2����ֵ���ж�⣩
                            MaxALLength = AL.Count;
                            MaxALLengthVector2.Clear();
                            MaxALAlternateList = AL;
                            MaxALLengthVector2.Add(ij);
                        }
                        else if (AL.Count == MaxALLength)
                        {
                            MaxALLengthVector2.Add(ij);
                        }
                    }
                }
            }
        }

        //Debug.Log(string.Join("," , MaxALLengthVector2));

        //�����ȡһ�������ٵĵ�Ԫ��ΪĿ������
        Vector2Int QuestVector = MaxALLengthVector2[Random.Range(0, MaxALLengthVector2.Count)];
        QuestUnit = GetUnitByVector(QuestVector);
        QuestUnit.isTarget = true;
        EmptyUnit.Remove(QuestUnit);
    }

    //========================��������=================================



























    //========================�ڶ�=================================


    /// <summary>
    /// �ڶ�
    /// </summary>
    /// <param name="Target">Ŀ��ʣ����</param>
    void Dig(int Target)
    {
        //0-36���������
        List<int> cells = Enumerable.Range(0, 36).OrderBy(_ => Random.value).ToList();
        int clues = 36;

        foreach (int idx in cells)
        {
            //���ʣ����С�ڵ���Ŀ��ʣ���� �ڶ���� �����ڶ�
            if (clues <= Target) break;

            //��������л�ȡλ��
            int r = idx / RCB, c = idx % RCB;
            Vector2Int rc = new Vector2Int(r, c);
            SudokuUnit unit = GetUnitByVector(rc);

            Dig_RemoveUnit(rc, unit.DisplayNum);

            // ����Ψһ�������
            if (CountSolutionsMain() != 1)
            {
                Dig_FillUnit(rc, unit.AnswerNum);
            }
            else
            {
                clues--;
            }
        }
    }


    int CountSolutionsMain()
    {
        return CountSolutions(0);
    }

    /// <summary>
    /// ������� �����ǰ�������ڶ�� ��˵���������Ϸ� ���̷���
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    int CountSolutions(int count)
    {
        //��ǰ�������ڶ�� �������Ϸ� ���̷���
        if (count >= 2) { return count; }


        //���е�Ԫ���к�����������ֵ
        int MinALLength = 7;
        //�����������ٵ�һ����Ԫ��ĺ򲹶���
        List<int> MinALAlternateList = new List<int> { };
        //�����������ٵ�һ����Ԫ��
        Vector2Int MinALLengthVector2 = new Vector2Int(-1, -1);

        //�������л�û�����𰸵ĵ�Ԫ�� �ҵ���ѡ�����ٵ�һ����Ԫ��
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //�������л�û�����𰸵ĵ�Ԫ��
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.DisplayNum == -1)
                    {
                        //��ȡ������
                        List<int> AL = Dig_GetUnitAlternateNum(ij);
                        if (AL.Count < MinALLength)
                        {
                            //������ٺ򲹵ĵ�Ԫ��û�к򲹣��򲹶���Ϊ0�� ���������Ϸ� ����2����ֵ���ж�⣩
                            if (AL.Count == 0) { return 2; }
                            MinALLength = AL.Count;
                            MinALLengthVector2 = ij;
                            MinALAlternateList = AL;
                        }
                    }
                }
            }
        }


        //������
        if (MinALLengthVector2 == new Vector2Int(-1, -1)) { count++; return count; }

        //�����������к�
        for (int i = 0; i < MinALAlternateList.Count; i++)
        {
            int num = MinALAlternateList[i];
            Dig_FillUnit(MinALLengthVector2, num);
            count = CountSolutions(count);
            Dig_RemoveUnit(MinALLengthVector2, num);
        }

        return count;
    }




    /// <summary>
    /// ��ʼ���ڶ��ֵ� ÿһ������ȫ������
    /// </summary>  
    void InitRCBDig()
    {
        RCBDig.Clear();
        foreach (var kvp in RCBCondition)
        {
            RCBDig[kvp.Key] = new List<int>(kvp.Value);
        }
        SetUnitDisplayeNum();
    }


    /// <summary>
    /// ����������ȡĳ����Ԫ����ܵĺ�ѡ��
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    List<int> Dig_GetUnitAlternateNum(Vector2Int vector2)
    {
        //�򲹶���
        List<int> AL = new List<int> { };
        if (CheckUnitIsExist(vector2))
        {
            //ȷ����Ԫ����� �� ��ȡ��Ԫ��
            SudokuUnit unit = GetUnitByVector(vector2);
            List<int> RowCL = RCBDig[new Vector2Int(0, unit.RowColBlock.x)];
            List<int> ColCL = RCBDig[new Vector2Int(1, unit.RowColBlock.y)];
            List<int> BlockCL = RCBDig[new Vector2Int(2, unit.RowColBlock.z)];
            List<int> UnionCL = (RowCL.Union(ColCL.Union(BlockCL))).ToList();
            List<int> ALL = new List<int> { 1, 2, 3, 4, 5, 6 };
            AL = ALL.Except(UnionCL).ToList();
        }
        return AL;
    }


    /// <summary>
    /// �������е�Ԫ����ʾ����Ϊ��
    /// </summary>
    void SetUnitDisplayeNum()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    SudokuUnit unit = GetUnitByVector(ij);
                    unit.DisplayNum = unit.AnswerNum;
                }
            }
        }
    }

    /// <summary>
    /// �ڶ�ʱ ��UnitV������n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void Dig_FillUnit(Vector2Int UnitV, int n)
    {
        //���������ٵĵ�Ԫ��
        if (CheckUnitIsExist(UnitV))
        {
            //��䵥Ԫ��
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.DisplayNum = n;
            //��ȫ����
            RCBDig[new Vector2Int(0, unit.RowColBlock.x)].Add(n);
            RCBDig[new Vector2Int(1, unit.RowColBlock.y)].Add(n);
            RCBDig[new Vector2Int(2, unit.RowColBlock.z)].Add(n);
        }
    }

    /// <summary>
    /// �ڶ�ʱ ��UnitV���Ƴ�n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void Dig_RemoveUnit(Vector2Int UnitV, int n)
    {
        //���������ٵĵ�Ԫ��
        if (CheckUnitIsExist(UnitV))
        {
            //��䵥Ԫ��
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.DisplayNum = -1;
            //��ȫ����
            RCBDig[new Vector2Int(0, unit.RowColBlock.x)].Remove(n);
            RCBDig[new Vector2Int(1, unit.RowColBlock.y)].Remove(n);
            RCBDig[new Vector2Int(2, unit.RowColBlock.z)].Remove(n);
        }
    }

    //========================�ڶ�=================================




















    //========================��ʼ�����������=================================

    /// <summary>
    /// ��������� 
    /// </summary>
    bool FillAnswer()
    {
        //���е�Ԫ���к�����������ֵ
        int MinALLength = 7;
        //�����������ٵ�һ����Ԫ��ĺ򲹶���
        List<int> MinALAlternateList = new List<int> { };
        //�����������ٵ�һ����Ԫ��
        Vector2Int MinALLengthVector2 = new Vector2Int(-1, -1);

        //�������л�û�����𰸵ĵ�Ԫ�� �ҵ���ѡ�����ٵ�һ����Ԫ��
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //�������л�û�����𰸵ĵ�Ԫ��
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.AnswerNum == -1)
                    {
                        //��ȡ������
                        List<int> AL = GetUnitAlternateNum(ij);
                        if (AL.Count < MinALLength)
                        {
                            //�����ѡ��Ϊ0 ˵����ǰ�𰸲��Ϸ� ����
                            if (AL.Count <= 0) { return false; }
                            MinALLength = AL.Count;
                            MinALLengthVector2 = ij;
                            MinALAlternateList = AL;
                            
                        }
                        
                    }
                }
            }
        }

        //ȫ�����Ѿ����� ����true
        if (MinALLengthVector2 == new Vector2Int(-1, -1)) { return true; }

        

        //ֱ���󲿶���Ϊ�����û��ȫ������ ˵���÷�֧����ȷ ����
        while (MinALAlternateList.Count != 0)
        {
            //Debug.Log(MinALLengthVector2 + "+" + string.Join(",", MinALAlternateList));
            //�����ȡһ���� ������Ӷ������Ƴ�
            int x = MinALAlternateList[Random.Range(0, MinALAlternateList.Count)];
            MinALAlternateList.Remove(x);
            //����ѡ���
            FillUnit(MinALLengthVector2 , x);
            //����������һ���� �����һ��������ȷ���� ˵����ǰ��Ҳ����ȷ���� ����
            if (FillAnswer()) {  return true; }
            //Debug.Log("=========Remove=========");
            //��һ������ȷ ˵���˺򲹲���ȷ Ĩ�������� ������һ����
            RemoveUnit(MinALLengthVector2, x);
        }
        return false;
    }

    /// <summary>
    /// ��UnitV������n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void FillUnit(Vector2Int UnitV , int n)
    {
        //���������ٵĵ�Ԫ��
        if (CheckUnitIsExist(UnitV))
        {
            //��䵥Ԫ��
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.AnswerNum = n;
            //��ȫ����
            RCBCondition[new Vector2Int(0, unit.RowColBlock.x)].Add(n);
            RCBCondition[new Vector2Int(1, unit.RowColBlock.y)].Add(n);
            RCBCondition[new Vector2Int(2, unit.RowColBlock.z)].Add(n);
        }
    }

    /// <summary>
    /// ��UnitV���Ƴ�n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void RemoveUnit(Vector2Int UnitV, int n)
    {
        //���������ٵĵ�Ԫ��
        if (CheckUnitIsExist(UnitV))
        {
            //��䵥Ԫ��
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.AnswerNum = -1;
            //��ȫ����
            RCBCondition[new Vector2Int(0, unit.RowColBlock.x)].Remove(n);
            RCBCondition[new Vector2Int(1, unit.RowColBlock.y)].Remove(n);
            RCBCondition[new Vector2Int(2, unit.RowColBlock.z)].Remove(n);
        }
    }


    /// <summary>
    /// ����������ȡĳ����Ԫ����ܵĺ�ѡ��
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    List<int> GetUnitAlternateNum(Vector2Int vector2)
    {
        //�򲹶���
        List<int> AL = new List<int> { };
        if (CheckUnitIsExist(vector2))
        {
            //ȷ����Ԫ����� �� ��ȡ��Ԫ��
            SudokuUnit unit = GetUnitByVector(vector2);
            List<int> RowCL = RCBCondition[new Vector2Int(0, unit.RowColBlock.x)];
            List<int> ColCL = RCBCondition[new Vector2Int(1, unit.RowColBlock.y)];
            List<int> BlockCL = RCBCondition[new Vector2Int(2, unit.RowColBlock.z)];
            List<int> UnionCL = (RowCL.Union(ColCL.Union(BlockCL))).ToList();
            List<int> ALL = new List<int> { 1, 2, 3, 4, 5, 6 };
            AL = ALL.Except(UnionCL).ToList();
        }
        return AL;
    }

    /// <summary>
    /// ��ʼ������ֵ�
    /// </summary>
    void InitDictionary()
    {
        RCBCondition.Clear();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                RCBCondition.Add(ij , new List<int> { });
            }
        }
    }

    //========================��ʼ�����������=================================


























    //========================��ʼ���������������=================================



    /// <summary>
    /// ���� 6��6 ��������Ĺ����ְ�
    /// </summary>
    int[,] BlockBoard()
    {
        // ����Ĺ���Ű�
        int[,] BlockGroup = new int[RCB, RCB];
        // ÿ�����ĵ�Ԫ���б�
        List<Vector2Int>[] UnitListByBlock = new List<Vector2Int>[RCB];
        for (int i = 0; i < RCB; i++)
            UnitListByBlock[i] = new List<Vector2Int>();

        // ���ԣ���ʼ�������֣�����������ֱ���ɹ�
        do
        {
            InitBlockBoard(BlockGroup, UnitListByBlock);
            StartSeed(BlockGroup, UnitListByBlock);
        }
        while (!BlockUnit(BlockGroup, UnitListByBlock, 0));

        return BlockGroup;
    }

    /// <summary>
    /// ��չ��岢����ÿ�������б�
    /// </summary>
    void InitBlockBoard(int[,] BlockGroup, List<Vector2Int>[] UnitListByBlock)
    {
        for (int i = 0; i < RCB; i++)
            for (int j = 0; j < RCB; j++)
                BlockGroup[i, j] = -1;

        foreach (var list in UnitListByBlock)
            list.Clear();
    }

    /// <summary>
    /// ���ѡȡÿ���������ӣ���һ�����ӣ�
    /// </summary>
    void StartSeed(int[,] BlockGroup, List<Vector2Int>[] UnitListByBlock)
    {
        // �� 0��35 ����
        var flat = Enumerable
            .Range(0, RCB * RCB)
            .OrderBy(_ => Random.value)
            .ToList();

        // ǰ 6 ����Ϊ 6 ����������
        for (int b = 0; b < RCB; b++)
        {
            int idx = flat[b];
            var seed = new Vector2Int(idx / RCB, idx % RCB);
            UnitListByBlock[b].Add(seed);
            BlockGroup[seed.x, seed.y] = b;
        }
    }

    /// <summary>
    /// ��������ÿ������step ��ʾ�ѷ���ķ����Ӹ���
    /// </summary>
    bool BlockUnit(int[,] BlockGroup, List<Vector2Int>[] UnitListByBlock, int step)
    {
        // ȫ�����ӣ�6*6����ȥ 6 �����Ӻ��ܹ�Ҫ���� 30 ��
        if (step >= RCB * (RCB - 1))
            return true;

        // ����Ҫ�����Ĺ�������0��5 ѭ��
        int BlockIndex = step % RCB;

        // ���������Ѿ��� 6 ��������һ��
        if (UnitListByBlock[BlockIndex].Count >= RCB)
            return BlockUnit(BlockGroup, UnitListByBlock, step + 1);

        // �ռ����п���չ���ڵ�
        List<Vector2Int> ALList = GetALList(BlockGroup, UnitListByBlock[BlockIndex]);
        if (ALList.Count == 0)
            return false; // ����ͬ������

        // ���ϴ�ƺ�ѡ�б�
        for (int i = ALList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = ALList[i]; ALList[i] = ALList[j]; ALList[j] = tmp;
        }

        // �����˳�������к�
        foreach (var v in ALList)
        {
            UnitListByBlock[BlockIndex].Add(v);
            BlockGroup[v.x, v.y] = BlockIndex;

            if (BlockUnit(BlockGroup, UnitListByBlock, step + 1))
                return true;

            // ����
            UnitListByBlock[BlockIndex].RemoveAt(UnitListByBlock[BlockIndex].Count - 1);
            BlockGroup[v.x, v.y] = -1;
        }

        return false;
    }

    /// <summary>
    /// ��ȡĳ���������п���չ�ڵ㣨�������ҡ���Χ����δ���䣩
    /// </summary>
    List<Vector2Int> GetALList(int[,] BlockGroup, List<Vector2Int> BlockList)
    {
        var Output = new List<Vector2Int>();
        foreach (var u in BlockList)
        {
            foreach (var dir in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                var v = u + dir;
                if (isVectorSpace(BlockGroup, v) && !BlockList.Contains(v) && !Output.Contains(v))
                    Output.Add(v);
            }
        }
        return Output;
    }

    /// <summary>
    /// ��� v �Ƿ��� 6��6 ��Χ����δ���������κι�
    /// </summary>
    bool isVectorSpace(int[,] BlockGroup, Vector2Int v)
    {
        return v.x >= 0 && v.x < RCB
            && v.y >= 0 && v.y < RCB
            && BlockGroup[v.x, v.y] == -1;
    }



    void SetUnitObjBlock(int[,] BlockMap)
    {
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    SudokuUnit u = GetUnitByVector(ij);
                    u.RowColBlock.z = BlockMap[i, j];
                }
            }
        }
    }

    //========================��ʼ���������������=================================
















    /**

    //========================��ʼ���ָ���������=================================


    /// <summary>
    /// �ָԪ��Ϊ������
    /// </summary>
    void SplitBlock()
    {
        //��ʼ��
        InitBlock();
        //�ָ�����
        for (int b = 0; b < RCB; b++)
        {
            //������
            Vector2Int StartUnit = GetStartUnit();
            SplitUnit(StartUnit, b, 0);
        }
        //����Ƿ���Ҫ������
        CheckSplit();

    }

    /// <summary>
    /// ����ĳ����ĳ�� �ɹ������򷵻سɹ� ʧ�ܷ���ʧ��
    /// </summary>
    /// <param name="UnitVector">ĳ��</param>
    /// <param name="Block">ĳ��</param>
    /// <param name="SplitCount">�������</param>
    bool SplitUnit(Vector2Int UnitVector, int Block, int SplitCount)
    {
        //��������Ѿ��ﵽ���� ��ɱ��η���
        if (SplitCount >= 6)
        {
            return true;
        }

        //���δ�빬
        if (CheckUnitisNotBlock(UnitVector))
        {
            SudokuUnit unit = GetUnitByVector(UnitVector);
            //�����
            unit.RowColBlock.z = Block;

            //�����Χ�ĸ���δ�������빬���޷����ɴ�СΪ6�Ĺ� ȡ���õ����
            Vector2Int U = UnitVector + Vector2Int.up;
            Vector2Int D = UnitVector + Vector2Int.down;
            Vector2Int L = UnitVector + Vector2Int.left;
            Vector2Int R = UnitVector + Vector2Int.right;


            if (
                (CheckUnitisNotBlock(U) && CheckUnitMaxZoneNum(U, new List<Vector2Int> { }).Count < RCB) ||
                (CheckUnitisNotBlock(D) && CheckUnitMaxZoneNum(D, new List<Vector2Int> { }).Count < RCB) ||
                (CheckUnitisNotBlock(L) && CheckUnitMaxZoneNum(L, new List<Vector2Int> { }).Count < RCB) ||
                (CheckUnitisNotBlock(R) && CheckUnitMaxZoneNum(R, new List<Vector2Int> { }).Count < RCB)
                )
            {
                //�����������
                List<Vector2Int> ExVectorList = new List<Vector2Int> { };

                //���һ��
                if (
                    (!(CheckUnitisNotBlock(U)) || CheckUnitMaxZoneNum(U, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(D)) || CheckUnitMaxZoneNum(D, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(R)) || CheckUnitMaxZoneNum(R, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(L)) || CheckUnitMaxZoneNum(L, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1)
                    )
                { }
                //��������׶�(��������ͬ ������ͬʣ�൥Ԫ��պõ��ڸù�ʣ�൥Ԫ��)
                else if ((CheckUnitisNotBlock(U)) && CheckUnitMaxZoneNum(U, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(U, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(D)) && CheckUnitMaxZoneNum(D, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(D, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(R)) && CheckUnitMaxZoneNum(R, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(R, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(L)) && CheckUnitMaxZoneNum(L, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(L, new List<Vector2Int> { }); }
                else
                {
                    unit.RowColBlock.z = -1;
                    return false;
                }

                //��������
                if (ExVectorList.Count > 0)
                {
                    ExVectorList.Add(UnitVector);
                    for (int i = 0; i < ExVectorList.Count; i++)
                    {
                        SplitCount++;
                        SudokuUnit u = GetUnitByVector(ExVectorList[i]);
                        u.RowColBlock.z = Block;
                        u.SetBlock(Block);
                    }
                    return true;
                }


            }
            //�ɹ�����
            SplitCount++;
            unit.SetBlock(Block);
            //��������Ѿ��ﵽ���� ��ɱ��η���
            if (SplitCount >= 6)
            {
                return true;
            }
            //�����Χ���е㶼���������δ�������η��� ���Ϊ�������
            if (!(SplitCount >= 6) && CheckUnitisNotBlock(UnitVector + Vector2Int.up) && CheckUnitisNotBlock(UnitVector + Vector2Int.down) && CheckUnitisNotBlock(UnitVector + Vector2Int.right) && CheckUnitisNotBlock(UnitVector + Vector2Int.left))
            {
                return false;
            }

            //������һ����
            //���һ��������Ϊ��һ�������
            Vector2Int NextDir = RandomDir();
            //������������ĵ�Ԫ���ѱ�������ڷ�Χ�ڻ��߷ָ�õ������������Ԫ�񲻿ɱ��ָ� ���÷���
            int count = 0;
            while (!CheckUnitisNotBlock(UnitVector + NextDir) || !SplitUnit(UnitVector + NextDir, Block, SplitCount))
            {
                NextDir = RandomDir();
                count++;
                //����bug��·����
                if (count >= 100) { Debug.Log("Bug Count Over"); break; }
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    Vector2Int RandomDir()
    {
        Vector2 v = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right;
        return new Vector2Int((int)_mTool.MainVector2(v).x, (int)_mTool.MainVector2(v).y);
    }

    /// <summary>
    /// ���ĳһ����Ԫ����Χ���������е�Ԫ�������Թ��ɶ�������
    /// </summary>
    /// <param name="CheckUnitVector">��ǰ���ĵ�Ԫ������</param>
    /// <param name="CheckedList">�Ѿ�������������б�</param>
    /// <param name="Count">������</param>
    /// <returns></returns>
    List<Vector2Int> CheckUnitMaxZoneNum(Vector2Int CheckUnitVector, List<Vector2Int> CheckedList)
    {
        //�������bug�������ݹ�
        if (CheckedList.Count >= 100) { return CheckedList; }

        //��������Ƿ���ڲ���δ���ָ��빬�Ҳ�δ�����
        if (CheckUnitisNotBlock(CheckUnitVector) && !CheckedList.Contains(CheckUnitVector))
        {
            SudokuUnit unit = GetUnitByVector(CheckUnitVector);
            CheckedList.Add(CheckUnitVector);
            CheckedList = CheckUnitMaxZoneNum(CheckUnitVector + Vector2Int.up, CheckedList);
            CheckedList = CheckUnitMaxZoneNum(CheckUnitVector + Vector2Int.down, CheckedList);
            CheckedList = CheckUnitMaxZoneNum(CheckUnitVector + Vector2Int.right, CheckedList);
            CheckedList = CheckUnitMaxZoneNum(CheckUnitVector + Vector2Int.left, CheckedList);
        }
        return CheckedList;
    }

    /// <summary>
    /// ��ȡ���ηָ�ι�����ʼ��
    /// ѡ��ǰ����û���������ĵ�Ԫ���У�����û���������ĵ�Ԫ�����ٵĵ�Ԫ�������
    /// </summary>
    /// <returns></returns>
    Vector2Int GetStartUnit()
    {
        Vector2Int Output = new Vector2Int(0, 0);
        int minnum = 4; ;
        //�������е�Ԫ��
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                //�����ǰ��鵥Ԫ��Ĺ�Ϊ-1��
                if (CheckUnitisNotBlock(ij))
                {
                    if (GetAroundNoBlockUnitNum(ij) < minnum)
                    {
                        minnum = GetAroundNoBlockUnitNum(ij);
                        Output = ij;
                    }
                }
            }
        }
        return Output;
    }


    /// <summary>
    /// ��ȡĳ����Ԫ��������δ����ĳ���������е�Ԫ�������
    /// </summary>
    /// <returns></returns>
    int GetAroundNoBlockUnitNum(Vector2Int CheckUnit)
    {
        int output = 0;
        Vector2Int U = CheckUnit + Vector2Int.up;
        Vector2Int D = CheckUnit + Vector2Int.down;
        Vector2Int L = CheckUnit + Vector2Int.left;
        Vector2Int R = CheckUnit + Vector2Int.right;
        if (CheckUnitisNotBlock(U)) { output += 1; }
        if (CheckUnitisNotBlock(D)) { output += 1; }
        if (CheckUnitisNotBlock(L)) { output += 1; }
        if (CheckUnitisNotBlock(R)) { output += 1; }
        return output;
    }


    /// <summary>
    /// ��ʼ�����е�Ԫ��ΪΪ�������빬
    /// </summary>
    void InitBlock()
    {
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    GetUnitByVector(ij).RowColBlock.z = -1;
                }
            }
        }
    }

    /// <summary>
    /// ���ָ��Ƿ���ڴ���
    /// </summary>
    void CheckSplit()
    {
        int[] checkeList = { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    SudokuUnit u = GetUnitByVector(ij);
                    if (u.RowColBlock.z >= 0 && u.RowColBlock.z <= 5) { checkeList[u.RowColBlock.z]++; }

                }
            }
        }
        Debug.Log(string.Join(",", checkeList));
        if (!(checkeList[0] == 6 && checkeList[1] == 6 && checkeList[2] == 6 && checkeList[3] == 6 && checkeList[4] == 6 && checkeList[5] == 6)) { SplitBlock(); }
    }

    //========================��ʼ���ָ���������=================================


    **/












    //========================��ͨ=================================
    /// <summary>
    /// ��ȡĳ����Ԫ�������
    /// </summary>
    /// <param name="ChildOBJ"></param>
    /// <returns></returns>
    private Vector2Int UnitChildOBJ2Vector(Transform ChildOBJ)
    {
        int index = _mTool.GetChildIndex(ChildOBJ);
        if (index == -1) { return new Vector2Int(-1, -1); }
        return new Vector2Int(index / RCB, index % RCB);
    }

    /// <summary>
    /// ��鵥Ԫ���Ƿ��ڷ�Χ���Ҳ�����ĳ����
    /// </summary>
    /// <returns></returns>
    bool CheckUnitisNotBlock(Vector2Int CheckUnitVector)
    {
        if (CheckUnitIsExist(CheckUnitVector))
        {
            SudokuUnit u = GetUnitByVector(CheckUnitVector);
            if (u.RowColBlock.z == -1) { return true; }
            else { return false; }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ���ĳ�������Ƿ��ڷ�Χ��
    /// </summary>
    /// <param name="CheckUnit"></param>
    /// <returns></returns>
    bool CheckUnitIsExist(Vector2Int CheckUnitVector)
    {
        if (CheckUnitVector.x < 0 || CheckUnitVector.x > 5 || CheckUnitVector.y < 0 || CheckUnitVector.y > 5) { return false; }
        else { return true; }
    }

    /// <summary>
    /// ���������ȡĳ����Ԫ��
    /// </summary>
    /// <returns></returns>
    SudokuUnit GetUnitByVector(Vector2Int v)
    {
        return SudokuUnitsList[v.x, v.y];
    }

    /// <summary>
    /// ��ʼ���������е�����
    /// </summary>
    void InitRowCol()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2Int v = UnitChildOBJ2Vector(transform.GetChild(i));
            SudokuUnitsList[v.x, v.y] = transform.GetChild(i).GetComponent<SudokuUnit>();
            SudokuUnitsList[v.x, v.y].SetRowCol(v);
        }
    }
    //========================��ͨ=================================

}
