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
        text.text = "前方为萧萧雪山，请注意保暖！(前方区域尚未开放，感谢您试玩当前版本)";
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
