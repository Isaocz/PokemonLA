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

    /// <summary>
    /// 下一个播放的延迟时间
    /// </summary>
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

    //计数是否结束
    bool isCounterOver;


    public ScrollRect ParentScrollRect;


    /// <summary>
    /// 循环音效
    /// </summary>
    //LoopingSEAudioPlayer LoopSEPlayer;
    /// <summary>
    /// 得分音效片段
    /// </summary>
    //public AudioClip ScoreClip;


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

    /// <summary>
    /// 计数类型
    /// </summary>
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


    /// <summary>
    /// 计数完毕音效
    /// </summary>
    public enum ScoreOverSEType
    {
        NormalSE,
        BigSE,
        Line,
        ScorePunish,
    }
    public ScoreOverSEType scoreOverSEType;



    private void Start()
    {
        if (StartPanelPlayerData.PlayerData != null && StartPanelPlayerData.PlayerData.Player != null && scoreType == ScoreType.糖果)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = StartPanelPlayerData.PlayerData.Player.PlayerCandyHD;
        }

        //分割线音效
        if (scoreType == ScoreType.无计数 && scoreOverSEType == ScoreOverSEType.Line)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.Line, Vector2.zero, true);
            }
        }
        //LoopSEPlayer = transform.GetComponent<LoopingSEAudioPlayer>();
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
            ScoreCountOverSE();
            DisplayOver();
        }
        else if (scoreType == ScoreType.冒险团经验条)
        {
            GroupLevelBar g = GetComponent<GroupLevelBar>();
            if (g != null)
            {
                //有存档
                if (SaveLoader.saveLoader != null)
                {
                    //Debug.Log("scorecountBefore" + "+" + g.per + "+" + g.Per);
                    SaveData save = SaveLoader.saveLoader.saveData;
                    g.SetLevelBar(save.GroupLevel);
                    g.Per += Mathf.Clamp((float)((float)ScoreCounter.Instance.TotalAP()) / (float)(GroupLevelBar.ExpRequired[save.GroupLevel + 1] - GroupLevelBar.ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
                    //Debug.Log("float" + "+" + Mathf.Clamp((float)((float)ScoreCounter.Instance.TotalAP() - GroupLevelBar.ExpRequired[save.GroupLevel]) / (float)(GroupLevelBar.ExpRequired[save.GroupLevel + 1] - GroupLevelBar.ExpRequired[save.GroupLevel]), 0.0f, 1.0f));
                    //Debug.Log(ScoreCounter.Instance.TotalAP() + "+" + GroupLevelBar.ExpRequired[save.GroupLevel] + "+" + GroupLevelBar.ExpRequired[save.GroupLevel + 1] + "+" + GroupLevelBar.ExpRequired[save.GroupLevel]);
                    //Debug.Log("scorecount" + "+" + g.per + "+" + g.Per);
                }
                // 无存档时 示例用
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
                //计分循环音效
                //if (LoopSEPlayer != null)
                //{
                //    LoopSEPlayer.PlayLoop(ScoreClip);
                //}
            }
            else if (TargetScore == 0 && !isCounterOver) 
            {
                DisplayScore = 0;
                ScoreText.text = DisplayScore.ToString();
                isCounterOver = true;
                ScoreText.GetComponent<Animator>().SetTrigger("Shine");
                ScoreCountOverSE();
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
                ScoreCountOverSE();
                DisplayOver();
            }
            else
            {
                //循环得分音效
                if (scoreType == ScoreType.糖果)
                {
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.ScoreLoop, Vector2.zero, true, Mathf.Lerp(0.2f, 0.6f, (float)DisplayScore / (float)TargetScore));
                }
                else
                {
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.ScoreLoop, Vector2.zero, true, 0.55f);
                }
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



    /// <summary>
    /// 计数完后音效
    /// </summary>
    public void ScoreCountOverSE()
    {
        if (AudioManager.Instance != null) {
            switch (scoreOverSEType)
            {
                case ScoreOverSEType.NormalSE:
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.GetScore, Vector2.zero, true);
                    break;
                case ScoreOverSEType.BigSE:
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.GetBigScore, Vector2.zero, true);
                    break;
                case ScoreOverSEType.ScorePunish:
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.ScorePunish, Vector2.zero, true);
                    break;
            } 
        }
    }
}
