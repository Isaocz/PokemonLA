using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyBar : MonoBehaviour
{
    //����һ����̬������ʾ��ɫ�Ľ�Ǯ��
    public static UIMoneyBar Instance;

    //����һ���ı������Ի�ȡ��Ǯ���ı�����������һ����Ǯ��
    public Text Moneytext;
    public int _Money
    {
        get { return money; }
        set { money = value; }
    }
    int money = 0;
    //����һ���ı������Ի�ȡʯͷ���ı�����������һ��ʯͷ��
    public Text Stonetext;
    public int _Stone
    {
        get { return stone; }
        set { stone = value; }
    }
    int stone = 0;

    //��ʼ����ɫ��Ǯ��
    private void Awake()
    {
        Instance = this;
    }


    //����һ������������Ǯ�仯ʱ�ı��ı���
    public void MoneyChange()
    {
        Moneytext.text = string.Format("{00}", money);
    }

    //����һ����������ʯͷ�仯ʱ�ı��ı���
    public void StoneChange()
    {
        Stonetext.text = string.Format("{00}", stone);
    }
}
