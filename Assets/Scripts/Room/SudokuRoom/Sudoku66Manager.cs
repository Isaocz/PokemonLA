using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sudoku66Manager : MonoBehaviour
{
    /// <summary>
    /// 数独的行 列 宫个数都为6
    /// </summary>
    static int RCB = 6;

    /// <summary>
    /// 单元格列表
    /// </summary>
    SudokuUnit[,] SudokuUnitsList = new SudokuUnit[RCB, RCB];


    /// <summary>
    /// 填充答案模板时，某一行某一列某一宫已经有的数字 行Vector2Int.x = 0 列Vector2Int.x = 1 宫Vector2Int.x = 2
    /// </summary>
    Dictionary<Vector2Int, List<int>> RCBCondition = new Dictionary<Vector2Int, List<int>>();


    /// <summary>
    /// 挖洞构造数独时，某一行某一列某一宫已经有的数字 行Vector2Int.x = 0 列Vector2Int.x = 1 宫Vector2Int.x = 2
    /// </summary>
    Dictionary<Vector2Int, List<int>> RCBDig = new Dictionary<Vector2Int, List<int>>();

    /// <summary>
    /// 成功音效
    /// </summary>
    public AudioClip SucessSE;
    /// <summary>
    /// 失败音效
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
    /// 检查用按钮
    /// </summary>
    public SudokuButton CheckButton;

    /// <summary>
    /// 被设置为谜题的单元格
    /// </summary>
    public SudokuUnit QuestUnit;
    /// <summary>
    /// 被挖洞但是不是迷题的单元格
    /// </summary>
    public List<SudokuUnit> EmptyUnit = new List<SudokuUnit> { };

















    // Start is called before the first frame update
    void Start()
    {
        int checkCount = 0;
        do
        {
            //初始化行列
            InitRowCol();
            //生长
            int[,] Block = BlockBoard();
            //过生成判定
            checkCount++;
            if (checkCount > 4) { Block = DefaultBoard; }
            SetUnitObjBlock(Block);
            //初始化字典
            InitDictionary();
        }
        while (!FillAnswer());//填入答案
        //复制挖洞字典
        InitRCBDig();
        //设置单元格显示数字
        SetUnitDisplayeNum();
        //开挖
        Dig(12);
        //设置谜题
        SetQuest();


        //设置单元格
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



        //设置答案检查相关
        CheckButton.parentManager = this;

        SEAudio = transform.GetComponent<AudioSource>();
    }

















    //========================检查输入的是否正确=================================
    /// <summary>
    /// 检查输入的答案是否正确
    /// </summary>
    public void CheckInputAnwser()
    {
        
        //未输入
        if (QuestUnit.InputNumList.Count == 0)
        {
            //CheckButton.StartCoroutine()
        }
        //多个输入取最后输入值
        else
        {
            //显示答案
            StartCoroutine(DisPlayAnswer());
        }
    }


    /// <summary>
    /// 显示答案
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
            //如果最后输入值等于答案 成功解开
            if (QuestUnit.InputNumList[QuestUnit.InputNumList.Count - 1].BoxIndex == QuestUnit.AnswerNum)
            {
                //Debug.Log("Scuess");
                SEAudio.clip = SucessSE;
                SEAudio.Play();
                switch (QuestUnit.AnswerNum)
                {
                    case 1: player.playerData.HPBounsAlways++; s = "生命值"; break;
                    case 2: player.playerData.AtkBounsAlways++; s = "攻击"; break;
                    case 3: player.playerData.SpABounsAlways++; s = "特攻"; break;
                    case 4: player.playerData.DefBounsAlways++; s = "防御"; break;
                    case 5: player.playerData.SpDBounsAlways++; s = "特防"; break;
                    case 6: player.playerData.SpeBounsAlways++; s = "攻击速度"; break;
                };
                UIGetANewItem.UI.JustSaySth("答对了！", s + "的能力加成上升了！");
                Instantiate(Success , QuestUnit.InputNumList[QuestUnit.InputNumList.Count - 1].transform.position + 1.2f*Vector3.up , Quaternion.Euler(new Vector3(45,0,0)) );
                Instantiate(Success , player.transform.position + player.SkillOffsetforBodySize[0] * Vector3.up , Quaternion.Euler(new Vector3(45, 0, 0)));
            }
            //如果最后输入值不等于答案 解开失败
            else
            {
                //Debug.Log("Faild");
                SEAudio.clip = FaildSE;
                SEAudio.Play();
                switch (QuestUnit.AnswerNum)
                {
                    case 1: player.playerData.HPBounsAlways--; s = "生命值"; break;
                    case 2: player.playerData.AtkBounsAlways--; s = "攻击"; break;
                    case 3: player.playerData.SpABounsAlways--; s = "特攻"; break;
                    case 4: player.playerData.DefBounsAlways--; s = "防御"; break;
                    case 5: player.playerData.SpDBounsAlways--; s = "特防"; break;
                    case 6: player.playerData.SpeBounsAlways--; s = "攻击速度"; break;
                };
                UIGetANewItem.UI.JustSaySth("答错了！",s+"的能力加成下降了！");
            }
            StopCoroutine(DisPlayAnswer());
            player.ReFreshAbllityPoint();
        }
    }



    //========================检查输入的是否正确=================================





















    //========================设置谜题=================================

    /// <summary>
    /// 设置谜题
    /// </summary>
    void SetQuest()
    {
        //所有单元格中候补数量的最少值
        int MaxALLength = -1;
        //候补数量的最少的一个单元格的候补队列
        List<int> MaxALAlternateList = new List<int> { };
        //候补数量的最少的单元格的队列
        List<Vector2Int> MaxALLengthVector2 = new List<Vector2Int> { };

        //遍历所有还没有填充答案的单元格 找到候选数最多的一个单元格
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //遍历所有还没有填充答案的单元格
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.DisplayNum == -1)
                    {
                        EmptyUnit.Add(GetUnitByVector(ij));
                        //获取候补数字
                        List<int> AL = Dig_GetUnitAlternateNum(ij);
                        if (AL.Count > MaxALLength)
                        {
                            //如果最少候补的单元格没有候补（候补队列为0） 则数独不合法 返回2（等值于有多解）
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

        //随机获取一个候补最少的单元格为目标谜题
        Vector2Int QuestVector = MaxALLengthVector2[Random.Range(0, MaxALLengthVector2.Count)];
        QuestUnit = GetUnitByVector(QuestVector);
        QuestUnit.isTarget = true;
        EmptyUnit.Remove(QuestUnit);
    }

    //========================设置谜题=================================



























    //========================挖洞=================================


    /// <summary>
    /// 挖洞
    /// </summary>
    /// <param name="Target">目标剩余量</param>
    void Dig(int Target)
    {
        //0-36的随机队列
        List<int> cells = Enumerable.Range(0, 36).OrderBy(_ => Random.value).ToList();
        int clues = 36;

        foreach (int idx in cells)
        {
            //如果剩余量小于等于目标剩余量 挖洞完毕 结束挖洞
            if (clues <= Target) break;

            //根据随机列获取位置
            int r = idx / RCB, c = idx % RCB;
            Vector2Int rc = new Vector2Int(r, c);
            SudokuUnit unit = GetUnitByVector(rc);

            Dig_RemoveUnit(rc, unit.DisplayNum);

            // 若不唯一，则回退
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
    /// 解决数独 如果当前数独存在多解 则说明数独不合法 立刻返回
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    int CountSolutions(int count)
    {
        //当前数独存在多解 数独不合法 立刻返回
        if (count >= 2) { return count; }


        //所有单元格中候补数量的最少值
        int MinALLength = 7;
        //候补数量的最少的一个单元格的候补队列
        List<int> MinALAlternateList = new List<int> { };
        //候补数量的最少的一个单元格
        Vector2Int MinALLengthVector2 = new Vector2Int(-1, -1);

        //遍历所有还没有填充答案的单元格 找到候选数最少的一个单元格
        for (int i = 0; i < RCB; i++)
        {
            for (int j = 0; j < RCB; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //遍历所有还没有填充答案的单元格
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.DisplayNum == -1)
                    {
                        //获取候补数字
                        List<int> AL = Dig_GetUnitAlternateNum(ij);
                        if (AL.Count < MinALLength)
                        {
                            //如果最少候补的单元格没有候补（候补队列为0） 则数独不合法 返回2（等值于有多解）
                            if (AL.Count == 0) { return 2; }
                            MinALLength = AL.Count;
                            MinALLengthVector2 = ij;
                            MinALAlternateList = AL;
                        }
                    }
                }
            }
        }


        //解数独
        if (MinALLengthVector2 == new Vector2Int(-1, -1)) { count++; return count; }

        //尝试输入所有候补
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
    /// 初始化挖洞字典 每一个条件全部满足
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
    /// 根据条件获取某个单元格可能的候补选项
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    List<int> Dig_GetUnitAlternateNum(Vector2Int vector2)
    {
        //候补队列
        List<int> AL = new List<int> { };
        if (CheckUnitIsExist(vector2))
        {
            //确保单元格存在 ， 获取单元格
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
    /// 设置所有单元格显示数字为答案
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
    /// 挖洞时 在UnitV中填入n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void Dig_FillUnit(Vector2Int UnitV, int n)
    {
        //填充候补数最少的单元格
        if (CheckUnitIsExist(UnitV))
        {
            //填充单元格
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.DisplayNum = n;
            //补全条件
            RCBDig[new Vector2Int(0, unit.RowColBlock.x)].Add(n);
            RCBDig[new Vector2Int(1, unit.RowColBlock.y)].Add(n);
            RCBDig[new Vector2Int(2, unit.RowColBlock.z)].Add(n);
        }
    }

    /// <summary>
    /// 挖洞时 在UnitV中移除n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void Dig_RemoveUnit(Vector2Int UnitV, int n)
    {
        //填充候补数最少的单元格
        if (CheckUnitIsExist(UnitV))
        {
            //填充单元格
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.DisplayNum = -1;
            //补全条件
            RCBDig[new Vector2Int(0, unit.RowColBlock.x)].Remove(n);
            RCBDig[new Vector2Int(1, unit.RowColBlock.y)].Remove(n);
            RCBDig[new Vector2Int(2, unit.RowColBlock.z)].Remove(n);
        }
    }

    //========================挖洞=================================




















    //========================初始化答案数字填充=================================

    /// <summary>
    /// 答案数字填充 
    /// </summary>
    bool FillAnswer()
    {
        //所有单元格中候补数量的最少值
        int MinALLength = 7;
        //候补数量的最少的一个单元格的候补队列
        List<int> MinALAlternateList = new List<int> { };
        //候补数量的最少的一个单元格
        Vector2Int MinALLengthVector2 = new Vector2Int(-1, -1);

        //遍历所有还没有填充答案的单元格 找到候选数最少的一个单元格
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                if (CheckUnitIsExist(ij))
                {
                    //遍历所有还没有填充答案的单元格
                    SudokuUnit unit = GetUnitByVector(ij);
                    if (unit.AnswerNum == -1)
                    {
                        //获取候补数字
                        List<int> AL = GetUnitAlternateNum(ij);
                        if (AL.Count < MinALLength)
                        {
                            //如果候选数为0 说明当前答案不合法 回退
                            if (AL.Count <= 0) { return false; }
                            MinALLength = AL.Count;
                            MinALLengthVector2 = ij;
                            MinALAlternateList = AL;
                            
                        }
                        
                    }
                }
            }
        }

        //全部答案已经填入 返回true
        if (MinALLengthVector2 == new Vector2Int(-1, -1)) { return true; }

        

        //直到后部队列为空如果没有全部填入 说明该分支不正确 回溯
        while (MinALAlternateList.Count != 0)
        {
            //Debug.Log(MinALLengthVector2 + "+" + string.Join(",", MinALAlternateList));
            //随机获取一个候补 并把其从队列中移除
            int x = MinALAlternateList[Random.Range(0, MinALAlternateList.Count)];
            MinALAlternateList.Remove(x);
            //填入选择候补
            FillUnit(MinALLengthVector2 , x);
            //尝试填入下一个候补 如果下一个候补是正确填入 说明当前候补也是正确填入 返回
            if (FillAnswer()) {  return true; }
            //Debug.Log("=========Remove=========");
            //上一步不正确 说明此候补不正确 抹除该填入 尝试下一个候补
            RemoveUnit(MinALLengthVector2, x);
        }
        return false;
    }

    /// <summary>
    /// 在UnitV中填入n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void FillUnit(Vector2Int UnitV , int n)
    {
        //填充候补数最少的单元格
        if (CheckUnitIsExist(UnitV))
        {
            //填充单元格
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.AnswerNum = n;
            //补全条件
            RCBCondition[new Vector2Int(0, unit.RowColBlock.x)].Add(n);
            RCBCondition[new Vector2Int(1, unit.RowColBlock.y)].Add(n);
            RCBCondition[new Vector2Int(2, unit.RowColBlock.z)].Add(n);
        }
    }

    /// <summary>
    /// 在UnitV中移除n
    /// </summary>
    /// <param name="UnitV"></param>
    /// <param name="n"></param>
    void RemoveUnit(Vector2Int UnitV, int n)
    {
        //填充候补数最少的单元格
        if (CheckUnitIsExist(UnitV))
        {
            //填充单元格
            SudokuUnit unit = GetUnitByVector(UnitV);
            unit.AnswerNum = -1;
            //补全条件
            RCBCondition[new Vector2Int(0, unit.RowColBlock.x)].Remove(n);
            RCBCondition[new Vector2Int(1, unit.RowColBlock.y)].Remove(n);
            RCBCondition[new Vector2Int(2, unit.RowColBlock.z)].Remove(n);
        }
    }


    /// <summary>
    /// 根据条件获取某个单元格可能的候补选项
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    List<int> GetUnitAlternateNum(Vector2Int vector2)
    {
        //候补队列
        List<int> AL = new List<int> { };
        if (CheckUnitIsExist(vector2))
        {
            //确保单元格存在 ， 获取单元格
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
    /// 初始化填充字典
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

    //========================初始化答案数字填充=================================


























    //========================初始化生长锯齿数独宫=================================



    /// <summary>
    /// 生成 6×6 锯齿数独的宫划分板
    /// </summary>
    int[,] BlockBoard()
    {
        // 输出的宫编号板
        int[,] BlockGroup = new int[RCB, RCB];
        // 每个宫的单元格列表
        List<Vector2Int>[] UnitListByBlock = new List<Vector2Int>[RCB];
        for (int i = 0; i < RCB; i++)
            UnitListByBlock[i] = new List<Vector2Int>();

        // 重试：初始化＋播种＋轮流生长，直到成功
        do
        {
            InitBlockBoard(BlockGroup, UnitListByBlock);
            StartSeed(BlockGroup, UnitListByBlock);
        }
        while (!BlockUnit(BlockGroup, UnitListByBlock, 0));

        return BlockGroup;
    }

    /// <summary>
    /// 清空宫板并清理每个宫的列表
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
    /// 随机选取每个宫的种子（第一个格子）
    /// </summary>
    void StartSeed(int[,] BlockGroup, List<Vector2Int>[] UnitListByBlock)
    {
        // 把 0…35 打乱
        var flat = Enumerable
            .Range(0, RCB * RCB)
            .OrderBy(_ => Random.value)
            .ToList();

        // 前 6 个作为 6 个宫的种子
        for (int b = 0; b < RCB; b++)
        {
            int idx = flat[b];
            var seed = new Vector2Int(idx / RCB, idx % RCB);
            UnitListByBlock[b].Add(seed);
            BlockGroup[seed.x, seed.y] = b;
        }
    }

    /// <summary>
    /// 轮流生长每个宫：step 表示已分配的非种子格数
    /// </summary>
    bool BlockUnit(int[,] BlockGroup, List<Vector2Int>[] UnitListByBlock, int step)
    {
        // 全部格子（6*6）减去 6 个种子后，总共要分配 30 个
        if (step >= RCB * (RCB - 1))
            return true;

        // 本轮要生长的宫索引：0…5 循环
        int BlockIndex = step % RCB;

        // 如果这个宫已经满 6 格，跳到下一步
        if (UnitListByBlock[BlockIndex].Count >= RCB)
            return BlockUnit(BlockGroup, UnitListByBlock, step + 1);

        // 收集所有可扩展的邻点
        List<Vector2Int> ALList = GetALList(BlockGroup, UnitListByBlock[BlockIndex]);
        if (ALList.Count == 0)
            return false; // 死胡同，回溯

        // 随机洗牌候选列表
        for (int i = ALList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = ALList[i]; ALList[i] = ALList[j]; ALList[j] = tmp;
        }

        // 按随机顺序尝试所有候补
        foreach (var v in ALList)
        {
            UnitListByBlock[BlockIndex].Add(v);
            BlockGroup[v.x, v.y] = BlockIndex;

            if (BlockUnit(BlockGroup, UnitListByBlock, step + 1))
                return true;

            // 回溯
            UnitListByBlock[BlockIndex].RemoveAt(UnitListByBlock[BlockIndex].Count - 1);
            BlockGroup[v.x, v.y] = -1;
        }

        return false;
    }

    /// <summary>
    /// 获取某个宫的所有可扩展邻点（上下左右、范围内且未分配）
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
    /// 格点 v 是否在 6×6 范围内且未被分配入任何宫
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

    //========================初始化生长锯齿数独宫=================================
















    /**

    //========================初始化分割锯齿数独宫=================================


    /// <summary>
    /// 分割单元格为六个宫
    /// </summary>
    void SplitBlock()
    {
        //初始化
        InitBlock();
        //分割六次
        for (int b = 0; b < RCB; b++)
        {
            //获得起点
            Vector2Int StartUnit = GetStartUnit();
            SplitUnit(StartUnit, b, 0);
        }
        //检查是否需要再生成
        CheckSplit();

    }

    /// <summary>
    /// 分配某点入某宫 成功分配则返回成功 失败返回失败
    /// </summary>
    /// <param name="UnitVector">某点</param>
    /// <param name="Block">某宫</param>
    /// <param name="SplitCount">分配次数</param>
    bool SplitUnit(Vector2Int UnitVector, int Block, int SplitCount)
    {
        //如果分配已经达到六次 完成本次分配
        if (SplitCount >= 6)
        {
            return true;
        }

        //如果未入宫
        if (CheckUnitisNotBlock(UnitVector))
        {
            SudokuUnit unit = GetUnitByVector(UnitVector);
            //则分配
            unit.RowColBlock.z = Block;

            //如果周围四个点未被分配入宫且无法构成大小为6的宫 取消该点分配
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
                //特例处理队列
                List<Vector2Int> ExVectorList = new List<Vector2Int> { };

                //最后一宫
                if (
                    (!(CheckUnitisNotBlock(U)) || CheckUnitMaxZoneNum(U, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(D)) || CheckUnitMaxZoneNum(D, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(R)) || CheckUnitMaxZoneNum(R, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1) &&
                    (!(CheckUnitisNotBlock(L)) || CheckUnitMaxZoneNum(L, new List<Vector2Int> { }).Count <= RCB - SplitCount - 1)
                    )
                { }
                //特例处理阶段(走入死胡同 但死胡同剩余单元格刚好等于该宫剩余单元格)
                else if ((CheckUnitisNotBlock(U)) && CheckUnitMaxZoneNum(U, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(U, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(D)) && CheckUnitMaxZoneNum(D, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(D, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(R)) && CheckUnitMaxZoneNum(R, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(R, new List<Vector2Int> { }); }
                else if ((CheckUnitisNotBlock(L)) && CheckUnitMaxZoneNum(L, new List<Vector2Int> { }).Count == RCB - SplitCount - 1) { ExVectorList = CheckUnitMaxZoneNum(L, new List<Vector2Int> { }); }
                else
                {
                    unit.RowColBlock.z = -1;
                    return false;
                }

                //处理特例
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
            //成功分配
            SplitCount++;
            unit.SetBlock(Block);
            //如果分配已经达到六次 完成本次分配
            if (SplitCount >= 6)
            {
                return true;
            }
            //如果周围所有点都分配完毕且未到达六次分配 输出为错误分配
            if (!(SplitCount >= 6) && CheckUnitisNotBlock(UnitVector + Vector2Int.up) && CheckUnitisNotBlock(UnitVector + Vector2Int.down) && CheckUnitisNotBlock(UnitVector + Vector2Int.right) && CheckUnitisNotBlock(UnitVector + Vector2Int.left))
            {
                return false;
            }

            //分配下一个点
            //随机一个方向，作为下一个分配点
            Vector2Int NextDir = RandomDir();
            //如果改随机方向的单元格已被分配或不在范围内或者分割该点后会造成其他单元格不可被分割 重置方向
            int count = 0;
            while (!CheckUnitisNotBlock(UnitVector + NextDir) || !SplitUnit(UnitVector + NextDir, Block, SplitCount))
            {
                NextDir = RandomDir();
                count++;
                //出现bug无路可走
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
    /// 检查某一个单元格周围连续的所有单元格最多可以构成多大的区域
    /// </summary>
    /// <param name="CheckUnitVector">当前检查的单元格坐标</param>
    /// <param name="CheckedList">已经检查过的坐标的列表</param>
    /// <param name="Count">区域数</param>
    /// <returns></returns>
    List<Vector2Int> CheckUnitMaxZoneNum(Vector2Int CheckUnitVector, List<Vector2Int> CheckedList)
    {
        //如果发生bug，跳出递归
        if (CheckedList.Count >= 100) { return CheckedList; }

        //检查坐标是否存在并且未被分割入宫且并未被检查
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
    /// 获取本次分割本次宫的起始点
    /// 选择当前所有没有所属宫的单元格中，四周没有所属宫的单元格最少的单元格的坐标
    /// </summary>
    /// <returns></returns>
    Vector2Int GetStartUnit()
    {
        Vector2Int Output = new Vector2Int(0, 0);
        int minnum = 4; ;
        //遍历所有单元格
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Vector2Int ij = new Vector2Int(i, j);
                //如果当前检查单元格的宫为-1，
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
    /// 获取某个单元格四周尚未属于某个宫的所有单元格的数量
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
    /// 初始化所有单元格为为被分配入宫
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
    /// 检查分割是否存在错误
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

    //========================初始化分割锯齿数独宫=================================


    **/












    //========================共通=================================
    /// <summary>
    /// 获取某个单元格的坐标
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
    /// 检查单元格是否在范围内且不属于某个宫
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
    /// 检查某个坐标是否在范围内
    /// </summary>
    /// <param name="CheckUnit"></param>
    /// <returns></returns>
    bool CheckUnitIsExist(Vector2Int CheckUnitVector)
    {
        if (CheckUnitVector.x < 0 || CheckUnitVector.x > 5 || CheckUnitVector.y < 0 || CheckUnitVector.y > 5) { return false; }
        else { return true; }
    }

    /// <summary>
    /// 根据坐标获取某个单元格
    /// </summary>
    /// <returns></returns>
    SudokuUnit GetUnitByVector(Vector2Int v)
    {
        return SudokuUnitsList[v.x, v.y];
    }

    /// <summary>
    /// 初始化所有行列的坐标
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
    //========================共通=================================

}
