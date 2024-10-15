using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTextInSequence : MonoBehaviour
{
    /// <summary>
    /// ��һ��Ҫ���ŵ�text
    /// </summary>
    public GameObject GameObjectNextText;

    public float NextTime;

    public float Waittime;

    /// <summary>
    /// ��ʾ������text
    /// </summary>
    public Text ScoreText;

    int PlusScore = 0;


    public bool isIngore;


    /// <summary>
    /// Ŀ�����
    /// </summary>
    public int TargetScore = -1;
    public int DisplayScore = 0;

    //�����Ƿ����
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
        �޼���,
        ��������,
        ̽�����佱��,
        �򵽵��˽���,
        ���߽���,
        ���ܽ���,
        ʱ��ͷ�,
        ���˳ͷ�,
        ��ɱ���ð��,
        ԾԾ���Ե�ð�ռ�,
        �ϼ�,
        �ǹ�,
        ��ɫƿ��,
        ��ɫƿ��,
        ð���ž�����,
    }
    public ScoreType scoreType;

    private void Start()
    {
        if (StartPanelPlayerData.PlayerData != null && StartPanelPlayerData.PlayerData.Player != null && scoreType == ScoreType.�ǹ�)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = StartPanelPlayerData.PlayerData.Player.PlayerCandyHD;
        }
    }

    public bool isThisTextIngore()
    {
        bool Output = false;
        switch (scoreType)
        {
            case ScoreType.�޼���:
                Output = false;
                break;
            case ScoreType.��������:
                if (ScoreCounter.Instance.FloorBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.̽�����佱��:
                if (ScoreCounter.Instance.RoomBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.�򵽵��˽���:
                if (ScoreCounter.Instance.EmptyBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.���߽���:
                if (ScoreCounter.Instance.ItemBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.���ܽ���:
                if (ScoreCounter.Instance.SkillBounsAP == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.ʱ��ͷ�:
                if (ScoreCounter.Instance.TimePunishAPMax() == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.���˳ͷ�:
                if (ScoreCounter.Instance.DmagePunishAPMax() == 0) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.��ɱ���ð��:
                if (!ScoreCounter.Instance.ClearGameBouns) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.ԾԾ���Ե�ð�ռ�:
                if (!ScoreCounter.Instance.RoleBouns) { Output = true; }
                else { Output = false; }
                break;
            case ScoreType.�ϼ�:
                Output = false;
                break;
            case ScoreType.�ǹ�:
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
            case ScoreType.�޼���:
                TargetScore = -1;
                isCounterOver = true;
                DisplayOver();
                return;
            case ScoreType.��������:
                TargetScore = ScoreCounter.Instance.FloorBounsAP;
                break;
            case ScoreType.̽�����佱��:
                TargetScore = ScoreCounter.Instance.RoomBounsAP;
                break;
            case ScoreType.�򵽵��˽���:
                TargetScore = ScoreCounter.Instance.EmptyBounsAP;
                break;
            case ScoreType.���߽���:
                TargetScore = ScoreCounter.Instance.ItemBounsAP;
                break;
            case ScoreType.���ܽ���:
                TargetScore = ScoreCounter.Instance.SkillBounsAP;
                break;
            case ScoreType.ʱ��ͷ�:
                TargetScore = ScoreCounter.Instance.TimePunishAPMax();
                break;
            case ScoreType.���˳ͷ�:
                TargetScore = ScoreCounter.Instance.DmagePunishAPMax();
                break;
            case ScoreType.��ɱ���ð��:
                
                break;
            case ScoreType.ԾԾ���Ե�ð�ռ�:
                
                break;
            case ScoreType.�ϼ�:
                TargetScore = ScoreCounter.Instance.TotalAP();
                break;
            case ScoreType.�ǹ�:
                TargetScore = ScoreCounter.Instance.CandyBouns;
                break;
        }

        if ((scoreType == ScoreType.��ɱ���ð�� || scoreType == ScoreType.ԾԾ���Ե�ð�ռ� )) {
            isCounterOver = true;
            ScoreText.GetComponent<Animator>().SetTrigger("Shine");
            DisplayOver();
        }
        else if (scoreType == ScoreType.ð���ž�����)
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
