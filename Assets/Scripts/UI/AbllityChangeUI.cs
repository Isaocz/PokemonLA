using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbllityChangeUI : MonoBehaviour
{
    public Image AbllityChangeImage;
    public Image AbllityChangeLevelImage;
    public int AbllityLevel;
    public Sprite Up01;
    public Sprite Up02;
    public Sprite Up03;
    public Sprite Up04;
    public Sprite Up05;
    public Sprite Up06;
    public Sprite Up07;
    public Sprite Up08;
    public Sprite Down01;
    public Sprite Down02;
    public Sprite Down03;
    public Sprite Down04;
    public Sprite Down05;
    public Sprite Down06;
    public Sprite Down07;
    public Sprite Down08;


    public void ChangeAblityLevel()
    {
        AbllityChangeImage = transform.GetComponent<Image>();
        AbllityChangeLevelImage = transform.GetChild(0).GetComponent<Image>();
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
    }
}
