using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbllityChangeUI : MonoBehaviour
{
    public Image AbllityChangeImage;
    public Image AbllityChangeLevelImage;
    public int AbllityLevel;



    public Sprite[] UpChangeLevelMark;
    public Sprite[] DownChangeLevelMark;


    public void ChangeAblityLevel()
    {
        AbllityLevel = Mathf.Clamp(AbllityLevel, -30, 30);

        AbllityChangeImage = transform.GetComponent<Image>();
        AbllityChangeLevelImage = transform.GetChild(0).GetComponent<Image>();
        if (AbllityLevel > 0) 
        {
            AbllityChangeLevelImage.sprite = UpChangeLevelMark[Mathf.Abs(AbllityLevel) - 1];
        }
        else if(AbllityLevel < 0)
        {
            AbllityChangeLevelImage.sprite = DownChangeLevelMark[Mathf.Abs(AbllityLevel) - 1];
        }
        else
        {
            AbllityChangeLevelImage.sprite = null;
        }

        /*
        switch (AbllityLevel)
        {
            case -1:
                AbllityChangeLevelImage.sprite = Down01;
                break;
            case -2:
                AbllityChangeLevelImage.sprite = Down02;
                break;
            case -3:
                AbllityChangeLevelImage.sprite = Down03;
                break;
            case -4:
                AbllityChangeLevelImage.sprite = Down04;
                break;
            case -5:
                AbllityChangeLevelImage.sprite = Down05;
                break;
            case -6:
                AbllityChangeLevelImage.sprite = Down06;
                break;
            case -7:
                AbllityChangeLevelImage.sprite = Down07;
                break;
            case -8:
                AbllityChangeLevelImage.sprite = Down08;
                break;
            case 1:
                AbllityChangeLevelImage.sprite = Up01;
                break;
            case 2:
                AbllityChangeLevelImage.sprite = Up02;
                break;
            case 3:
                AbllityChangeLevelImage.sprite = Up03;
                break;
            case 4:
                AbllityChangeLevelImage.sprite = Up04;
                break;
            case 5:
                AbllityChangeLevelImage.sprite = Up05;
                break;
            case 6:
                AbllityChangeLevelImage.sprite = Up06;
                break;
            case 7:
                AbllityChangeLevelImage.sprite = Up07;
                break;
            case 8:
                AbllityChangeLevelImage.sprite = Up08;
                break;
        }
    
         */
    
    }
}
