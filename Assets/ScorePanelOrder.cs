using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 结算界面的顺序；
/// </summary>
public class ScorePanelOrder : MonoBehaviour
{


    private void Start()
    {
        if (SaveLoader.saveLoader != null)
        {
            SaveData save = SaveLoader.saveLoader.saveData;
            int TotalAP = save.APTotal + ScoreCounter.Instance.TotalAP();
            while (TotalAP >= SaveData.ExpRequired[save.GroupLevel + ScoreCounter.Instance.isGroupLevelUp])
            {
                ScoreCounter.Instance.isGroupLevelUp += 1;
                
                //TotalAP -= SaveData.ExpRequired[save.GroupLevel + ScoreCounter.Instance.isGroupLevelUp];
                Debug.Log(ScoreCounter.Instance.isGroupLevelUp + "+" + SaveData.ExpRequired[save.GroupLevel + ScoreCounter.Instance.isGroupLevelUp] + "+" + TotalAP);
            }
        }
        else
        {
            int TotalAP = 120 + ScoreCounter.Instance.TotalAP();
            if (TotalAP >= SaveData.ExpRequired[1])
            {
                ScoreCounter.Instance.isGroupLevelUp += 1;
            }
        }
    }

    public void GameOver()
    {
        if (SaveLoader.saveLoader != null)
        {

            SaveData save = SaveLoader.saveLoader.saveData;
            save.AP += ScoreCounter.Instance.TotalAP();
            save.APTotal += ScoreCounter.Instance.TotalAP();
            save.RoleList[StartPanelPlayerData.PlayerData.Player.PlayerIndex].Candy += ScoreCounter.Instance.CandyBouns;
            save.RoleList[StartPanelPlayerData.PlayerData.Player.PlayerIndex].PlayCount += 1;
            save.GameCount += 1;

            ScoreCounter.Instance.ResetCounter();
            StartPanelPlayerData.PlayerData.ResetPlayerData();
            SaveSystem.saveSystem.SaveGame(save);

        }
    }

}
