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
    float OriginalSize;

    private void Start()
    {
        LoadGame();
        //OriginalSize = ProgressBarMask.rectTransform.rect.width;
        OriginalSize = 1000;
        //ProgressBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
    }

    public void LoadGame()
    {
        StartCoroutine(StartLoadGame(3));

    }

    public IEnumerator StartLoadGame(int Sence)
    {
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
