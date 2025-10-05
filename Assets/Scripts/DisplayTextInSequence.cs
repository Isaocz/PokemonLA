using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTextInSequence : MonoBehaviour
{
    /// <summary>
    /// 下一个要播放的text
    /// </summary>
    public GameObject GameObjectNextText;

    public float NextTime;

    public float Waittime;

    /// <summary>
    /// 显示分数的text
    /// </summary>
    public Text ScoreText;

    int PlusScore = 0;


    public bool isIngore;


    /// <summary>
    /// 目标分数
    /// </summary>
    public int TargetScore = -1;
    public int DisplayScore = 0;

    //技术是否结束
    bool isCounterOver;


    public ScrollRect ParentScrollRect;



    public void DisplayNextText()
    {


        if (GameObjectNextText != null) {
            while (GameObjectNextText.GetComponent<DisplayTextInSequence>() != null && GameObjectNextText.GetComponent<DisplayTextInSequence>().isThisTextIngore())
            {
                GameObjectNextText = GameObjectNextText.GetComponent<DisplayTextInSequence>().GameObjectNextText;
            }
            GameObjectNextText.gameObject.SetActive(true); 
        }
        if (ParentScrollRect != null) {
            Canvas.ForceUpdateCanvases();
            ParentScrollRect.verticalNormalizedPosition = 0f;
        }
    }


    public enum ScoreType
    {
        无计数,
        层数奖励,
        探索房间奖励,
        打到敌人奖励,
        道具奖励,
        技能奖励,
        时间惩罚,
        受伤惩罚,
        完成本次冒险,
        跃跃欲试的冒险家,
        合计,
        糖果,
        银色瓶盖,
        金色瓶盖,
        冒险团经验条,
    }
    public ScoreType scoreType;

    private void Start()
    {
        if (StartPanelPlayerData.PlayerData != null && StartPanelPlayerData.PlayerData.Player != null && scoreType == ScoreType.糖果)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = StartPanelPlayerData.PlayerData.Player.PlayerCandyHD;
        }
    }

    public bool isThisTextIngore()
    {
        bool Output = false;
        switch (scoreType)
        {
            case ScoreType.无计数:
                Output = false;
                break;
            case ScoreType.层数奖励:
                if (ScoreCounter.Instance.FloorBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.探索房间奖励:
                if (ScoreCounter.Instance.RoomBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.打到敌人奖励:
                if (ScoreCounter.Instance.EmptyBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.道具奖励:
                if (ScoreCounter.Instance.ItemBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.技能奖励:
                if (ScoreCounter.Instance.SkillBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.时间惩罚:
                if (ScoreCounter.Instance.TimePunishAPMax() == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.受伤惩罚:
                if (ScoreCounter.Instance.DmagePunishAPMax() == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.完成本次冒险:
                if (!ScoreCounter.Instance.ClearGameBouns) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.跃跃欲试的冒险家:
                if (!ScoreCounter.Instance.RoleBouns) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.合计:
                Output = false;
                break;
            case ScoreType.糖果:
                Output = false;
                break;
        }
        return Output;
    }

    public void ScoreCount()
    {
        TargetScore = 0;
        switch (scoreType)
        {
            case ScoreType.无计数:
                TargetScore = -1;
                isCounterOver = true;
                DisplayOver();
                return;
            case ScoreType.层数奖励:
                TargetScore = ScoreCounter.Instance.FloorBounsAP;
                break;
            case ScoreType.探索房间奖励:
                TargetScore = ScoreCounter.Instance.RoomBounsAP;
                break;
            case ScoreType.打到敌人奖励:
                TargetScore = ScoreCounter.Instance.EmptyBounsAP;
                break;
            case ScoreType.道具奖励:
                TargetScore = ScoreCounter.Instance.ItemBounsAP;
                break;
            case ScoreType.技能奖励:
                TargetScore = ScoreCounter.Instance.SkillBounsAP;
                break;
            case ScoreType.时间惩罚:
                TargetScore = ScoreCounter.Instance.TimePunishAPMax();
                break;
            case ScoreType.受伤惩罚:
                TargetScore = ScoreCounter.Instance.DmagePunishAPMax();
                break;
            case ScoreType.完成本次冒险:
                
                break;
            case ScoreType.跃跃欲试的冒险家:
                
                break;
            case ScoreType.合计:
                TargetScore = ScoreCounter.Instance.TotalAP();
                break;
            case ScoreType.糖果:
                TargetScore = ScoreCounter.Instance.CandyBouns;
                break;
        }

        if ((scoreType == ScoreType.完成本次冒险 || scoreType == ScoreType.跃跃欲试的冒险家 )) {
            isCounterOver = true;
            ScoreText.GetComponent<Animator>().SetTrigger("Shine");
            DisplayOver();
        }
        else if (scoreType == ScoreType.冒险团经验条)
        {
            GroupLevelBar g = GetComponent<GroupLevelBar>();
            if (g != null)
            {
                if (SaveLoader.saveLoader != null)
                {
                    SaveData save = SaveLoader.saveLoader.saveData;
                    g.SetLevelBar(save.GroupLevel);
                    g.Per += Mathf.Clamp((float)((float)ScoreCounter.Instance.TotalAP() - GroupLevelBar.ExpRequired[save.GroupLevel]) / (float)(GroupLevelBar.ExpRequired[save.GroupLevel + 1] - GroupLevelBar.ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
                }
                else
                {
                    g.Per = Mathf.Clamp(g.Per + ((float)(ScoreCounter.Instance.TotalAP() / (float)(GroupLevelBar.ExpRequired[0 + 1] - GroupLevelBar.ExpRequired[0]))), 0.0f, 1.0f);
                }
                g.DisComp = this;
            }
        }
        else
        {
            if (TargetScore > 0) {
                PlusScore = Mathf.Clamp(TargetScore / 10, 1, TargetScore);
                StartCoroutine(Score(Waittime));
            }
            else if (TargetScore == 0 && !isCounterOver) 
            {
                DisplayScore = 0;
                ScoreText.text = DisplayScore.ToString();
                isCounterOver = true;
                ScoreText.GetComponent<Animator>().SetTrigger("Shine");
                DisplayOver();
            }

        }
    }



    IEnumerator Score(float Waittime)
    {
        while ((TargetScore >= 0 && DisplayScore <= TargetScore))
        {

            yield return new WaitForSeconds(Waittime);
            ScoreText.text = DisplayScore.ToString();

            if (PlusScore > 1)
            {
                while (DisplayScore + PlusScore >= TargetScore)
                {
                    PlusScore = PlusScore / 10;
                    if (PlusScore <= 1)
                    {
                        PlusScore = 1;
                        break;
                    }
                }
            }
            DisplayScore += PlusScore;
            if (TargetScore >= 0 && DisplayScore >= TargetScore && !isCounterOver)
            {
                isCounterOver = true;
                ScoreText.GetComponent<Animator>().SetTrigger("Shine");
                DisplayOver();
            }
        }
    }

    /*
    private void FixedUpdate()
    {
        if (ScoreText != null)
        {
            if (TargetScore > 0 && DisplayScore <= TargetScore)
            {
                ScoreText.text = DisplayScore.ToString();

                if (PlusScore > 1) {
                    while (DisplayScore + PlusScore >= TargetScore)
                    {
                        PlusScore = PlusScore / 10;
                        if (PlusScore <= 1) {
                            PlusScore = 1;
                            break;
                        }
                    }
                }

                Debug.Log(PlusScore);
                DisplayScore += PlusScore;
            }
            if (TargetScore > 0 &&  DisplayScore >= TargetScore && !isCounterOver)
            {
                isCounterOver = true;
                DisplayOver();
            }
        }
    }

    */

    public void DisplayOver()
    {
        //Invoke("DisplayNextText" , NextTime);

        Timer.Start(this , NextTime , ()=> { DisplayNextText(); });
        //DisplayNextText();
    }

}
