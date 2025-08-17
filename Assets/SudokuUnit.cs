using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuUnit : MonoBehaviour
{
    /// <summary>
    /// 该单元所属的行列宫
    /// </summary>
    public Vector3Int RowColBlock = new Vector3Int(-1, -1 , -1);

    /// <summary>
    /// 答案标志的Sprite队列
    /// </summary>
    public Sprite[] MarkSpriteList;

    /// <summary>
    /// 单元格的Sprite队列
    /// </summary>
    public Sprite[] UnitSpriteList;


    /// <summary>
    /// 父对象数独管理器
    /// </summary>
    Sudoku66Manager ParentManage;


    /// <summary>
    /// 该单元格的正确答案
    /// </summary>
    public int AnswerNum = -1;

    /// <summary>
    /// 该单元格的现实的数字 ，-1代表该单元格被挖洞
    /// </summary>
    public int DisplayNum = -1;

    /// <summary>
    /// 该单元格输入的答案
    /// </summary>
    public List<SudokuPushBox> InputNumList = new List<SudokuPushBox> { };


    /// <summary>
    /// 该单元格是不是目标谜题
    /// </summary>
    public bool isTarget = false;
    public Sprite QuestSprite;

    int color;

    /// <summary>
    /// 设置单元格的行列
    /// </summary>
    public void SetRowCol(Vector2Int v)
    {
        RowColBlock.x = v.x;
        RowColBlock.y = v.y;
        AnswerNum = -1;
        DisplayNum = -1;
        ParentManage = transform.parent.GetComponent<Sudoku66Manager>();
    }


    /// <summary>
    /// 设置单元格
    /// </summary>
    /// <param name="RCB">数独的行列宫</param>
    /// <param name="Spacing">单元格间距</param>
    public void SetUnit( int RCB , float Spacing , List<int> BlockColor)
    {
        color = BlockColor[RowColBlock.z];
        SpriteRenderer Mark = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer Block = transform.GetComponent<SpriteRenderer>();

        if (isTarget)
        {
            Mark.sprite = QuestSprite;
            AddCollider();
        }
        else
        {
            if (DisplayNum != -1)
            {
                Mark.sprite = MarkSpriteList[color * RCB + DisplayNum - 1];
            }
            else
            {
                Mark.sprite = null;
            }
        }

        Block.sprite = UnitSpriteList[color];

        float length = Spacing * (float)(RCB-1);

        transform.position = transform.parent.position + new Vector3(-length/2.0f + (float)RowColBlock.x* Spacing , length / 2.0f - (float)RowColBlock.y * Spacing, 0);
    }


    /// <summary>
    /// 解密后显示答案
    /// </summary>
    public void DisplayAnswer(int RCB)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = MarkSpriteList[color * RCB + AnswerNum - 1];
        transform.GetComponent<AudioSource>().Play();

    }


    void AddCollider()
    {
        BoxCollider2D BoxC = transform.gameObject.AddComponent<BoxCollider2D>();
        BoxC.isTrigger = true;
        BoxC.offset = new Vector2(0, 0);
        BoxC.size = new Vector2(1.2f , 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SudokuPushBox box = collision.gameObject.GetComponent<SudokuPushBox>();
        if (box != null && !InputNumList.Contains(box))
        {
            InputNumList.Add(box);
            if (InputNumList.Count >= 1 && !ParentManage.CheckButton.isON) { ParentManage.CheckButton.GetComponent<Collider2D>().isTrigger = true; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SudokuPushBox box = collision.gameObject.GetComponent<SudokuPushBox>();
        if (box != null && InputNumList.Contains(box))
        {
            InputNumList.Remove(box);
            if (InputNumList.Count <= 0 && !ParentManage.CheckButton.isON) { ParentManage.CheckButton.GetComponent<Collider2D>().isTrigger = false; }
        }
    }

}
