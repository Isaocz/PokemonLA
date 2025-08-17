using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGetANewItem : MonoBehaviour
{
    public static UIGetANewItem UI;
    public Image UIImage;
    float Timer;
    bool isActive;
    // Start is called before the first frame update
    private void Awake()
    {
        UI = this;
    }

    public void GetANewItem(int ItemTag , string ItemName)
    {
        UIImage.gameObject.SetActive(true);
        UIImage.color = new Color (1, 1, 1, 1);
        isActive = true;
        Timer = 0;
        UIImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.196f, 0.196f, 0.196f, 1);
        UIImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.196f, 0.196f, 0.196f, 1);
        string ItemClass = "道具";
        switch (ItemTag)
        {
            case 0:
                ItemClass = "消耗品:";
                break;
            case 1:
                ItemClass = "一次性道具:";
                break;
            case 2:
                ItemClass = "被动道具:";
                break;
            case 3:
                ItemClass = "技能学习机:";
                break;
            case 4:
                ItemClass = "冒险伙伴:";
                break;
        }
        UIImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "获得了" + ItemClass; 
        UIImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "「" + ItemName　+ "」";
        LayoutRebuilder.ForceRebuildLayoutImmediate(UIImage.rectTransform);
    }

    public void JustSaySth(string FirstText, string SecondText)
    {
        UIImage.gameObject.SetActive(true);
        UIImage.color = new Color(1, 1, 1, 1);
        isActive = true;
        Timer = 0;
        UIImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.196f, 0.196f, 0.196f, 1);
        UIImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.196f, 0.196f, 0.196f, 1);
        UIImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = FirstText;
        UIImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SecondText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(UIImage.rectTransform);
    }



    private void Update()
    {
        if (isActive) {
            Timer += Time.deltaTime;
            if (Timer >= 0.9f)
            {
                UIImage.color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
                UIImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
                UIImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
                if (UIImage.color.a <= 0)
                {
                    Timer = 0;
                    UIImage.gameObject.SetActive(false);
                    isActive = false;
                }
            }
        }
    }
}
