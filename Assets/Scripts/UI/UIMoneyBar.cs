using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyBar : MonoBehaviour
{
    //声明一个静态变量表示角色的金钱数
    public static UIMoneyBar Instance;

    //声明一个文本对象，以获取金钱的文本对象，再声明一个金钱数
    public Text Moneytext;
    public int _Money
    {
        get { return money; }
        set { money = value; }
    }
    int money = 0;
    //声明一个文本对象，以获取石头的文本对象，再声明一个石头数
    public Text Stonetext;
    public int _Stone
    {
        get { return stone; }
        set { stone = value; }
    }
    int stone = 0;

    //初始化角色金钱数
    private void Awake()
    {
        Instance = this;
    }


    //声明一个函数，当金钱变化时改变文本。
    public void MoneyChange()
    {
        Moneytext.text = string.Format("{00}", money);
    }

    //声明一个函数，当石头变化时改变文本。
    public void StoneChange()
    {
        Stonetext.text = string.Format("{00}", stone);
    }
}
