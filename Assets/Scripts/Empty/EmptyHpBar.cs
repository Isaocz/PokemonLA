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
    Image[] images;
    float fadetimer;
    float fadeduration;
    bool isFading;
    bool fadeReverse;


    //获得血条的初始长度，既最大长度
    // Start is called before the first frame update
    void Start()
    {
        originalSize = Mask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        fadetimer = 0f;
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

        if (isFading)
        {
            fadetimer += Time.deltaTime;
            float t = fadetimer / fadeduration;
            for (int i = 0; i < images.Length; i++)
            {
                if (fadeReverse)
                {
                    images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.Lerp(0f, 1f, t));
                }
                else
                {
                    images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.Lerp(1f, 0f, t));
                }
            }
            if(fadetimer > fadeduration)
            {
                isFading = false;
                fadetimer = 0f;
            }
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
    /// <summary>
    /// 血条渐入、渐出
    /// </summary>
    /// <param name="FadeDuration">淡入淡出持续时间</param>
    /// <param name="Reverse">是则淡出，否则淡入</param>
    public void Fade(float FadeDuration, bool Reverse)
    {
        images = new Image[]
        {
            Mask,
            Mask.transform.parent.GetComponent<Image>(),
            Mask.transform.parent.GetChild(0).GetComponent<Image>(),
            Mask.transform.parent.GetChild(1).GetComponent<Image>(),
            Mask.transform.parent.GetChild(2).GetComponent<Image>(),
            Mask.transform.parent.GetChild(4).GetComponent<Image>(),
            Mask.transform.GetChild(0).GetComponent<Image>()
        };
        isFading = true;
        fadeduration = FadeDuration;
        fadeReverse = Reverse;
    }
}
