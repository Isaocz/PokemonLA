using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScence : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Progress;
    public Image ProgressBarMask;
    public RectTransform progressTrans;
    float OriginalSize = 0;

    private void Start()
    {
        OriginalSize = progressTrans.rect.width;
        LoadGame();
        //OriginalSize = ProgressBarMask.rectTransform.rect.width;
        //ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
    }

    public void LoadGame()
    {
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber == 0 && StartPanelPlayerData.PlayerData != null && !StartPanelPlayerData.PlayerData.isSeedGame ) { InitializePlayerSetting.GlobalPlayerSetting.ResetSeed(); }
        StartCoroutine(StartLoadGame(FloorNum.GlobalFloorNum.FloorNumber + 5));
    }

    public IEnumerator StartLoadGame(int Sence)
    {
        Random.InitState(InitializePlayerSetting.GlobalPlayerSetting.RoundSeed);
        int DisplayProgress = 0;
        int ToProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(Sence);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {          
            ToProgress = (int)(op.progress * 100);       
            while (DisplayProgress < ToProgress) {     
                DisplayProgress++;
                Progress.text = DisplayProgress.ToString() + "%";
                ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp((float)((OriginalSize* (float)DisplayProgress)/100) , 0 , OriginalSize));
                yield return new WaitForEndOfFrame(); 
            }    
            Progress.text = DisplayProgress.ToString() + "%";
            ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp((float)((OriginalSize * (float)DisplayProgress) / 100), 0, OriginalSize));
            yield return new WaitForEndOfFrame();
        }
        ToProgress = 100;
        while (DisplayProgress < ToProgress)
        {
            DisplayProgress++;
            Progress.text = DisplayProgress.ToString() + "%";
            ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp((float)((OriginalSize * (float)DisplayProgress) / 100), 0, OriginalSize));
            yield return new WaitForEndOfFrame();
        }
        Progress.text = DisplayProgress.ToString() + "%";
        ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp((float)((OriginalSize * (float)DisplayProgress) / 100), 0, OriginalSize));
        op.allowSceneActivation = true;


    }
}
