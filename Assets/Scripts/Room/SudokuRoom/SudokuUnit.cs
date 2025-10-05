using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuUnit : MonoBehaviour
{
    /// <summary>
    /// �õ�Ԫ���������й�
    /// </summary>
    public Vector3Int RowColBlock = new Vector3Int(-1, -1 , -1);

    /// <summary>
    /// �𰸱�־��Sprite����
    /// </summary>
    public Sprite[] MarkSpriteList;

    /// <summary>
    /// ��Ԫ���Sprite����
    /// </summary>
    public Sprite[] UnitSpriteList;


    /// <summary>
    /// ����������������
    /// </summary>
    Sudoku66Manager ParentManage;


    /// <summary>
    /// �õ�Ԫ�����ȷ��
    /// </summary>
    public int AnswerNum = -1;

    /// <summary>
    /// �õ�Ԫ�����ʵ������ ��-1����õ�Ԫ���ڶ�
    /// </summary>
    public int DisplayNum = -1;

    /// <summary>
    /// �õ�Ԫ������Ĵ�
    /// </summary>
    public List<SudokuPushBox> InputNumList = new List<SudokuPushBox> { };


    /// <summary>
    /// �õ�Ԫ���ǲ���Ŀ������
    /// </summary>
    public bool isTarget = false;
    public Sprite QuestSprite;

    int color;

    /// <summary>
    /// ���õ�Ԫ�������
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
    /// ���õ�Ԫ��
    /// </summary>
    /// <param name="RCB">���������й�</param>
    /// <param name="Spacing">��Ԫ����</param>
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
    /// ���ܺ���ʾ��
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
