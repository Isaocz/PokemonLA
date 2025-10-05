using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionScrollBar : MonoBehaviour
{


    //声明一个变量，表示用来遮罩的经验条，以及经验条的最大长度,
    public Image Mask;
    float originnoSize;


    //声明一个公开变量per，表示经验条的最终变化样子，既变化的百分比
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
