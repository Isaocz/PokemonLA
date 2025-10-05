using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionScrollBar : MonoBehaviour
{


    //����һ����������ʾ�������ֵľ��������Լ�����������󳤶�,
    public Image Mask;
    float originnoSize;


    //����һ����������per����ʾ�����������ձ仯���ӣ��ȱ仯�İٷֱ�
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;


    //float timer = 0;

    PlayerAchievement achievement;


    public void SetBar(PlayerAchievement a)
    {
        achievement = a;
        originnoSize = Mask.rectTransform.rect.width;
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)achievement.Progress / (float)achievement.Target * originnoSize);
    }

}
