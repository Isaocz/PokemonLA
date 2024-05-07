using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSkillBar : MonoBehaviour , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Transform originalParent;
    int DragSkillIndex;


    public bool[] isSkillList = new bool[] { false, false, false, false };
    public Vector3[] SkillBarPosition = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    public GameObject[] SkillBarList = new GameObject[] { null, null, null, null };
    public GameObject[] OriginaSkillBarList = new GameObject[] { null, null, null, null };
    
    
    /// <summary>
    /// 重排序技能
    /// </summary>
    void ResortSkillBar()
    {
        for (int i = 6; i < 10; i++)
        {
            for (int j = i; j < 10; j++)
            {
                if (transform.parent.GetChild(i).position.y < transform.parent.GetChild(j).position.y) { transform.parent.GetChild(j).SetSiblingIndex(i); }
            }
        }
    }

    /// <summary>
    /// 获取有多少个技能
    /// </summary>
    void InsSkill()
    {
        isSkillList = new bool[] { false, false, false, false };
        SkillBarPosition = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        SkillBarList = new GameObject[] { null, null, null, null };
        OriginaSkillBarList = new GameObject[] { null, null, null, null };

        int SkillNum = (transform.parent.GetChild(6).gameObject.activeSelf ? 1 : 0) + (transform.parent.GetChild(7).gameObject.activeSelf ? 1 : 0) + (transform.parent.GetChild(8).gameObject.activeSelf ? 1 : 0) + (transform.parent.GetChild(9).gameObject.activeSelf ? 1 : 0);
        for (int i = 1; i < 5; i++)
        {
            if (transform.parent.GetChild(i + 5).gameObject.activeSelf) {
                if (transform.parent.GetChild(i + 5).gameObject != gameObject) { isSkillList[i - 1] = true; }
                else { DragSkillIndex = i; }
                SkillBarPosition[i - 1] = transform.parent.GetChild(i + 5).position;
                SkillBarList[i - 1] = transform.parent.GetChild(i + 5).gameObject;
                OriginaSkillBarList[i - 1] = transform.parent.GetChild(i + 5).gameObject;
            }
        }
    }

    /// <summary>
    /// 清空缓存
    /// </summary>
    void ClearList()
    {
        for (int i = 0; i < isSkillList.Length; i++) { isSkillList[i] = false; SkillBarPosition[i] = Vector3.zero; SkillBarList[i] = null; OriginaSkillBarList[i] = null; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ResortSkillBar();
        InsSkill();
        // 记录开始拖拽时的原始位置和父对象
        originalPosition = SkillBarPosition[DragSkillIndex - 1];
        originalParent = transform.parent;
        originalScale = transform.localScale;
        transform.localScale = originalScale * 0.7f; 
        // 将物品的父对象设置为Canvas，以便它可以在所有UI元素之上移动
        transform.SetParent(transform.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 更新物品的位置，使其跟随鼠标移动
        Vector3 t = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.position = new Vector3(transform.position.x , t.y , 0);

        float a = (SkillBarPosition[0].y - SkillBarPosition[1].y) * 0.3f;
        float b = Mathf.Abs(SkillBarPosition[0].y - SkillBarPosition[3].y) * 1.5f;
        if (transform.position.x > SkillBarPosition[0].x - b && transform.position.x <= SkillBarPosition[0].x + b)
        {
            if (isSkillList[0] && transform.position.y > SkillBarPosition[0].y - a && transform.position.y < SkillBarPosition[0].y + a)
            {
                Debug.Log(originalPosition);
                Vector3 s = SkillBarList[0].transform.position;
                SkillBarList[0].transform.position = originalPosition;
                originalPosition = s;

                GameObject sg = SkillBarList[0];
                SkillBarList[0] = transform.gameObject;
                SkillBarList[DragSkillIndex - 1] = sg;


                isSkillList[0] = false; isSkillList[DragSkillIndex - 1] = true;
                DragSkillIndex = 1;


            }
            else if (isSkillList[1] && transform.position.y > SkillBarPosition[1].y - a && transform.position.y < SkillBarPosition[1].y + a)
            {
                Vector3 s = SkillBarList[1].transform.position;
                SkillBarList[1].transform.position = originalPosition;
                originalPosition = s;

                GameObject sg = SkillBarList[1];
                SkillBarList[1] = transform.gameObject;
                SkillBarList[DragSkillIndex - 1] = sg;


                isSkillList[1] = false; isSkillList[DragSkillIndex - 1] = true;
                DragSkillIndex = 2;
            }
            else if (isSkillList[2] && transform.position.y > SkillBarPosition[2].y - a && transform.position.y < SkillBarPosition[2].y + a)
            {
                Vector3 s = SkillBarList[2].transform.position;
                SkillBarList[2].transform.position = originalPosition;
                originalPosition = s;

                GameObject sg = SkillBarList[2];
                SkillBarList[2] = transform.gameObject;
                SkillBarList[DragSkillIndex - 1] = sg;


                isSkillList[2] = false; isSkillList[DragSkillIndex - 1] = true;
                DragSkillIndex = 3;
            }
            else if (isSkillList[3] && transform.position.y > SkillBarPosition[3].y - a && transform.position.y < SkillBarPosition[3].y + a)
            {
                Vector3 s = SkillBarList[3].transform.position;
                SkillBarList[3].transform.position = originalPosition;
                originalPosition = s;

                GameObject sg = SkillBarList[3];
                SkillBarList[3] = transform.gameObject;
                SkillBarList[DragSkillIndex - 1] = sg;


                isSkillList[3] = false; isSkillList[DragSkillIndex - 1] = true;
                DragSkillIndex = 4;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {




        float a = (SkillBarPosition[0].y - SkillBarPosition[1].y) * 0.3f;
        float b =  Mathf.Abs(SkillBarPosition[0].y - SkillBarPosition[3].y) * 1.5f;

        if (transform.position.x > SkillBarPosition[0].x - b && transform.position.x <= SkillBarPosition[0].x + b) { 
            if (isSkillList[0] && transform.position.y > SkillBarPosition[0].y - a && transform.position.y < SkillBarPosition[0].y + a)
            {
                transform.SetParent(originalParent);
                transform.position = SkillBarList[0].transform.position;
                SkillBarList[0].transform.position = originalPosition;
            }
            else if (isSkillList[1] && transform.position.y > SkillBarPosition[1].y - a && transform.position.y < SkillBarPosition[1].y + a)
            {
                transform.SetParent(originalParent);
                transform.position = SkillBarList[1].transform.position;
                SkillBarList[1].transform.position = originalPosition;
            }
            else if (isSkillList[2] && transform.position.y > SkillBarPosition[2].y - a && transform.position.y < SkillBarPosition[2].y + a)
            {
                transform.SetParent(originalParent);
                transform.position = SkillBarList[2].transform.position;
                SkillBarList[2].transform.position = originalPosition;
            }
            else if (isSkillList[3] && transform.position.y > SkillBarPosition[3].y - a && transform.position.y < SkillBarPosition[3].y + a)
            {
                transform.SetParent(originalParent);
                transform.position = SkillBarList[3].transform.position;
                SkillBarList[3].transform.position = originalPosition;
            }
            else
            {
                // 如果不是，那么将物品放回原来的位置和父对象
                transform.position = originalPosition;
                transform.SetParent(originalParent);
            }
        }
        else
        {
            // 如果不是，那么将物品放回原来的位置和父对象
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }


        transform.localScale = originalScale;

        ResortSkillBar();
        ChangeSkillOrder();
        ClearList();

    }


    void ChangeSkillOrder()
    {
        //Debug.Log(SkillBarList[0].name + SkillBarList[1].name + SkillBarList[2].name + SkillBarList[3].name );
        PlayerControler player = transform.parent.GetComponent<SkillPanel>().player;
        Skill[] OriginalSkillOrderList = new Skill[] { (player.Skill01 != null ? player.Skill01 : null), (player.Skill02 != null ? player.Skill02 : null), (player.Skill03 != null ? player.Skill03 : null), (player.Skill04 != null ? player.Skill04 : null) };
        bool[] OriginalSkillisCDList = new bool[] { (player.Skill01 != null ? player.isSkill01CD : false), (player.Skill02 != null ? player.isSkill02CD : false), (player.Skill03 != null ? player.isSkill03CD : false), (player.Skill04 != null ? player.isSkill04CD : false) };
        float[] OriginalSkillCDList = new float[] { (player.Skill01 != null ? player._Skill01Timer : 0), (player.Skill02 != null ? player._Skill02Timer : 0), (player.Skill03 != null ? player._Skill03Timer : 0), (player.Skill04 != null ? player._Skill04Timer : 0) };

        //Debug.Log(player.isSkill01CD.ToString() + player.isSkill02CD.ToString() + player.isSkill03CD.ToString() + player.isSkill04CD.ToString());
        //Debug.Log(player._Skill01Timer.ToString() + "+" + player._Skill02Timer.ToString() + "+" + player._Skill03Timer.ToString() + "+" + player._Skill04Timer.ToString());


        if (SkillBarList[0] != null) {
            switch (SkillBarList[0].name)
            {
                case "SkillPanelBar01":
                    //player.Skill01 = OriginalSkillOrderList[0];
                    break;
                case "SkillPanelBar02":
                    player.isSkill01CD = OriginalSkillisCDList[1];
                    player._Skill01Timer = OriginalSkillCDList[1];
                    player.Skill01 = OriginalSkillOrderList[1];
                    break;
                case "SkillPanelBar03":
                    player.isSkill01CD = OriginalSkillisCDList[2];
                    player._Skill01Timer = OriginalSkillCDList[2];
                    player.Skill01 = OriginalSkillOrderList[2];
                    break;
                case "SkillPanelBar04":
                    player.isSkill01CD = OriginalSkillisCDList[3];
                    player._Skill01Timer = OriginalSkillCDList[3];
                    player.Skill01 = OriginalSkillOrderList[3];
                    break;
            }
        }

        if (SkillBarList[1] != null) {
            switch (SkillBarList[1].name)
            {
                case "SkillPanelBar01":
                    player.isSkill02CD = OriginalSkillisCDList[0];
                    player._Skill02Timer = OriginalSkillCDList[0];
                    player.Skill02 = OriginalSkillOrderList[0];
                    break;
                case "SkillPanelBar02":
                    //player.Skill02 = OriginalSkillOrderList[1];
                    break;
                case "SkillPanelBar03":
                    player.isSkill02CD = OriginalSkillisCDList[2];
                    player._Skill02Timer = OriginalSkillCDList[2];
                    player.Skill02 = OriginalSkillOrderList[2];
                    break;
                case "SkillPanelBar04":
                    player.isSkill02CD = OriginalSkillisCDList[3];
                    player._Skill02Timer = OriginalSkillCDList[3];
                    player.Skill02 = OriginalSkillOrderList[3];
                    break;
            }
        }

        if (SkillBarList[2] != null) {
            switch (SkillBarList[2].name)
            {
                case "SkillPanelBar01":
                    player.isSkill03CD = OriginalSkillisCDList[0];
                    player._Skill03Timer = OriginalSkillCDList[0];
                    player.Skill03 = OriginalSkillOrderList[0];
                    break;
                case "SkillPanelBar02":
                    player.isSkill03CD = OriginalSkillisCDList[1];
                    player._Skill03Timer = OriginalSkillCDList[1];
                    player.Skill03 = OriginalSkillOrderList[1];
                    break;
                case "SkillPanelBar03":
                    //  player.Skill03 = OriginalSkillOrderList[2];
                    break;
                case "SkillPanelBar04":
                    player.isSkill03CD = OriginalSkillisCDList[3];
                    player._Skill03Timer = OriginalSkillCDList[3];
                    player.Skill03 = OriginalSkillOrderList[3];
                    break;
            }
        }

        if (SkillBarList[3] != null) {
            switch (SkillBarList[3].name)
            {
                case "SkillPanelBar01":
                    player.isSkill04CD = OriginalSkillisCDList[0];
                    player._Skill04Timer = OriginalSkillCDList[0];
                    player.Skill04 = OriginalSkillOrderList[0];
                    break;
                case "SkillPanelBar02":
                    player.isSkill04CD = OriginalSkillisCDList[1];
                    player._Skill04Timer = OriginalSkillCDList[1];
                    player.Skill04 = OriginalSkillOrderList[1];
                    break;
                case "SkillPanelBar03":
                    player.isSkill04CD = OriginalSkillisCDList[2];
                    player._Skill04Timer = OriginalSkillCDList[2];
                    player.Skill04 = OriginalSkillOrderList[2];
                    break;
                case "SkillPanelBar04":
                    //player.Skill04 = OriginalSkillOrderList[3];
                    break;
            }
        }

        //Debug.Log(player.isSkill01CD.ToString() + player.isSkill02CD.ToString() + player.isSkill03CD.ToString() + player.isSkill04CD.ToString());
        //Debug.Log(player._Skill01Timer.ToString() + "+" + player._Skill02Timer.ToString() + "+" + player._Skill03Timer.ToString() + "+" + player._Skill04Timer.ToString());

        transform.parent.GetChild(6).name = "SkillPanelBar01";
        transform.parent.GetChild(7).name = "SkillPanelBar02";
        transform.parent.GetChild(8).name = "SkillPanelBar03";
        transform.parent.GetChild(9).name = "SkillPanelBar04";
        if (player.Skill01 && !player.Is01imprison) { player.skillBar01.CDPlus(player.Skill01.ColdDown); }
        if (player.Skill02 && !player.Is02imprison) { player.skillBar02.CDPlus(player.Skill02.ColdDown); }      
        if (player.Skill03 && !player.Is03imprison) { player.skillBar03.CDPlus(player.Skill03.ColdDown); }
        if (player.Skill04 && !player.Is04imprison) { player.skillBar04.CDPlus(player.Skill04.ColdDown); }

        if (player.Skill01 != null) { 
            player.skillBar01.GetSkill(player.Skill01);
            if (player.isSkill01CD)
            {
                Debug.Log("1T");
                player.skillBar01.isCDStart = true;
                player.skillBar01.SetZero();
                player.skillBar01.CDPlus(player._Skill01Timer);
            }
            else
            {
                Debug.Log("1F");
                player.skillBar01.CDPlus(player.Skill01.ColdDown);
            }

        }
        if (player.Skill02 != null) {
            player.skillBar02.GetSkill(player.Skill02);
            if (player.isSkill02CD)
            {
                Debug.Log("2T");
                player.skillBar02.isCDStart = true;
                player.skillBar02.SetZero();
                player.skillBar02.CDPlus(player._Skill02Timer);
            }
            else
            {
                Debug.Log("2F");
                player.skillBar02.CDPlus(player.Skill02.ColdDown);
            }
        }
        if (player.Skill03 != null) { 
            player.skillBar03.GetSkill(player.Skill03);
            if (player.isSkill03CD)
            {
                Debug.Log("3T");
                player.skillBar03.isCDStart = true;
                player.skillBar03.SetZero();
                player.skillBar03.CDPlus(player._Skill03Timer);
            }
            else
            {
                Debug.Log("3F");
                player.skillBar03.CDPlus(player.Skill03.ColdDown);
            }
        }
        if (player.Skill04 != null) {
            player.skillBar04.GetSkill(player.Skill04);
            if (player.isSkill04CD)
            {
                Debug.Log("4T");
                player.skillBar04.isCDStart = true;
                player.skillBar04.SetZero();
                player.skillBar04.CDPlus(player._Skill04Timer);
            }
            else
            {
                Debug.Log("4F");
                player.skillBar04.CDPlus(player.Skill04.ColdDown);
            }
        }


        //Debug.Log(player.isSkill01CD.ToString() + player.isSkill02CD.ToString() + player.isSkill03CD.ToString() + player.isSkill04CD.ToString());
        //Debug.Log(player._Skill01Timer.ToString() + "+" + player._Skill02Timer.ToString() + "+" + player._Skill03Timer.ToString() + "+" + player._Skill04Timer.ToString());
    }


}
