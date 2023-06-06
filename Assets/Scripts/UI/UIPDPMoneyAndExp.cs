using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPDPMoneyAndExp : MonoBehaviour
{
    Text MoneyMuch;
    Text NowExp;
    Text NextExp;
    Text Lv;
    Image ExpBar;
    float ExpBarOrangeSize;

    private void Start()
    {
        MoneyMuch = transform.GetChild(1).GetComponent<Text>();
        NowExp = transform.GetChild(4).GetComponent<Text>();
        NextExp = transform.GetChild(6).GetComponent<Text>();
        Lv = transform.GetChild(9).GetComponent<Text>();
        ExpBar = transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<Image>();
        ExpBarOrangeSize = transform.GetChild(7).GetChild(0).GetComponent<RectTransform>().rect.width;
    }

    // Start is called before the first frame update
    public void GetMoneyAndExp( PlayerControler player)
    {
        if (MoneyMuch != null) {
            MoneyMuch.text = player.Money.ToString();
            NowExp.text = player.Ex.ToString();
            NextExp.text = (player.maxEx - player.Ex).ToString();
            Lv.text = player.Level.ToString();
            ExpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ExpBarOrangeSize * ((float)player.Ex / (float)player.maxEx));
        }

    }
}
