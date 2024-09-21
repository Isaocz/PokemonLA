using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Text text = transform.GetChild(0).GetComponent<Text>();
        text.text = "ǰ��Ϊ����ѩɽ����ע�Ᵽů��(ǰ��������δ���ţ���л�����浱ǰ�汾)";
    }

    public void ClearGame()
    {


        MapCreater m = FindObjectOfType<MapCreater>();
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.MaxFloor == FloorNum.GlobalFloorNum.FloorNumber + 1 && m != null)
        {
            if (ScoreCounter.Instance != null)
            {
                ScoreCounter.Instance.FloorBounsAP += APBounsPoint.FloorBouns(FloorNum.GlobalFloorNum.FloorNumber);
                ScoreCounter.Instance.CandyBouns += APBounsPoint.FloorCandyBouns(FloorNum.GlobalFloorNum.FloorNumber);
                ScoreCounter.Instance.TimePunishAP += APBounsPoint.TimePunish(m.MapTime, FloorNum.GlobalFloorNum.FloorNumber);
                ScoreCounter.Instance.ClearGameBouns = true;
            }
        }
    }

}
