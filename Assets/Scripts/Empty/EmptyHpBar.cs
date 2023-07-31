using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyHpBar : MonoBehaviour
{
    //声明一个图片对象，表示血条，以及一个浮点型变量，表示血条的初始长度

    public Image Mask;
    float originalSize;

    //声明一个浮点型变量，表示变化的比例。一个布尔型变量，表示是否增加血量。一个布尔型变量，表示是否减少血量。以及一个浮点型表示缓慢改变的计时器
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;
    float timer;
    bool isHpUp = false;
    bool isHpDown = false;
    Empty ParentEmpty;






    //获得血条的初始长度，既最大长度
    // Start is called before the first frame update
    void Start()
    {
        originalSize = Mask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
    }



    //每帧检测一次当前血量，随之改变血条颜色。每帧检测一次血条是否改变，如果改变缓慢改变。
    // Update is called once per frame
    void Update()
    {
        //当调用血量上升函数时血条缓慢增加到指定值，反之缓慢减少到指定值
        if (isHpUp)
        {
            timer -= (ParentEmpty.isBoos? 0.18f : 2.3f)*Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpUp();
        }
        if (isHpDown)
        {
            timer += (ParentEmpty.isBoos ? 0.18f : 2.3f) * Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpDown();
        }

        //改变血条颜色
        if (timer <= 0.5f)
        {
            Mask.color = new Color((255 / 255f), (255 / 255f), (255 / 255f), (255 / 255f));
        }
        else if ((0.5f < timer) && (timer < 0.8f))
        {
            Mask.color = new Color((255 / 255f), (255 / 255f), (0 / 255f), (255 / 255f));
        }
        else if (timer >= 0.8f)
        {
            Mask.color = new Color((255 / 255f), (120 / 255f), (47 / 255f), (255 / 255f));
        }
    }

    //两个函数分别为表示表示血条增加和血条减少的函数
    public void ChangeHpUp()
    {
        isHpUp = true;
        if (timer <= 1 - per)
        {
            isHpUp = false;
        }
    }
    public void ChangeHpDown()
    {
        isHpDown = true;
        if (timer >= 1 - per)
        {
            isHpDown = false;
        }
    }
}
