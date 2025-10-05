using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyHpBar : MonoBehaviour
{
    /// <summary>
    /// 护盾条上升的速度加成
    /// </summary>
    public static float SPEED_SHIELD_BAR_UP = 2.5f;

    //声明一个图片对象，表示血条，以及一个浮点型变量，表示血条的初始长度
    //血条
    public Image HpMask;
    //护盾
    public Image ShieldMask;
    float originalSize;

    //声明一个浮点型变量，表示血量变化的比例。一个布尔型变量，表示是否增加血量。一个布尔型变量，表示是否减少血量。以及一个浮点型表示缓慢血量改变的计时器
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;

    float timer;
    bool isHpUp = false;
    bool isHpDown = false;

    //声明一个浮点型变量，表示血量变化的比例。一个布尔型变量，表示是否增加血量。一个布尔型变量，表示是否减少血量。以及一个浮点型表示缓慢血量改变的计时器
    public float ShieldPer
    {
        get { return shieldPer; }
        set { shieldPer = value; }
    }
    float shieldPer;

    float shieldTimer = 1.0f;
    bool isShieldUp = false;
    bool isShieldDown = false;




    Empty ParentEmpty;



    //TODO渐入渐出相关
    Image[] hpImages;
    float fadetimer;
    float fadeduration;
    bool isFading;
    bool fadeReverse;

    /// <summary>
    /// 护盾标志
    /// </summary>
    public GameObject ShieldMark;


    //获得血条的初始长度，既最大长度
    // Start is called before the first frame update
    void Awake()
    {
        originalSize = HpMask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        fadetimer = 0f;
    }


    /// <summary>
    /// 初始化护盾条
    /// </summary>
    public void StartShieldBar(float ShieldPer)
    {
        if (ParentEmpty == null)
        {
            originalSize = HpMask.rectTransform.rect.width;
            ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        }
        ShieldMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * ShieldPer);
        shieldTimer = 1.0f - ShieldPer;
    }



    //每帧检测一次当前血量，随之改变血条颜色。每帧检测一次血条是否改变，如果改变缓慢改变。
    // Update is called once per frame
    void Update()
    {

        //当调用血量上升函数时血条缓慢增加到指定值，反之缓慢减少到指定值
        if (isHpUp)
        {
            timer -= ((ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.Boss || ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? 0.18f : 2.3f)*Time.deltaTime;
            timer = Mathf.Clamp(timer, 0.0f, 1.0f);
            HpMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpUp();
        }
        if (isHpDown)
        {
            timer += ((ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.Boss || ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? 0.18f : 2.3f) * Time.deltaTime;
            timer = Mathf.Clamp(timer, 0.0f, 1.0f);
            HpMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpDown();
        }


        if (isShieldUp)
        {
            Debug.Log("UP");
            shieldTimer -= ((ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.Boss || ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? 0.18f : 2.3f) * Time.deltaTime * SPEED_SHIELD_BAR_UP;
            shieldTimer = Mathf.Clamp(shieldTimer, 0.0f, 1.0f);
            ShieldMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - shieldTimer));
            ChangeShieldUp();
        }
        if (isShieldDown)
        {
            shieldTimer += ((ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.Boss || ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? 0.18f : 2.3f) * Time.deltaTime;
            shieldTimer = Mathf.Clamp(shieldTimer, 0.0f, 1.0f);
            ShieldMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - shieldTimer));
            ChangeShieldDown();
        }

        //改变血条颜色
        if (timer <= 0.5f)
        {
            HpMask.color = new Color((255 / 255f), (255 / 255f), (255 / 255f), (255 / 255f));
        }
        else if ((0.5f < timer) && (timer < 0.8f))
        {
            HpMask.color = new Color((255 / 255f), (255 / 255f), (0 / 255f), (255 / 255f));
        }
        else if (timer >= 0.8f)
        {
            HpMask.color = new Color((255 / 255f), (120 / 255f), (47 / 255f), (255 / 255f));
        }

        //TODO 需修改护盾
        if (isFading)
        {
            fadetimer += Time.deltaTime;
            float t = fadetimer / fadeduration;
            for (int i = 0; i < hpImages.Length; i++)
            {
                if (fadeReverse)
                {
                    hpImages[i].color = new Color(hpImages[i].color.r, hpImages[i].color.g, hpImages[i].color.b, Mathf.Lerp(0f, 1f, t));
                }
                else
                {
                    hpImages[i].color = new Color(hpImages[i].color.r, hpImages[i].color.g, hpImages[i].color.b, Mathf.Lerp(1f, 0f, t));
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
            timer = 1 - per;
            HpMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
        }
    }
    public void ChangeHpDown()
    {
        isHpDown = true;
        if (timer >= 1 - per)
        {
            isHpDown = false;
            timer = 1 - per;
            HpMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
        }
    }


    //两个函数分别为表示表示护盾增加和血条减少的函数
    public void ChangeShieldUp()
    {
        isShieldUp = true;
        Debug.Log(isShieldUp + "+" + shieldTimer + "+" + shieldPer);
        if (shieldTimer <= 1 - shieldPer)
        {
            isShieldUp = false;
            shieldTimer = 1 - shieldPer;
            ShieldMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - shieldTimer));
            if (shieldPer == 0)
            {
                BreakShieldMark();
            }
            else
            {
                GetShieldMark();
            }
        }
    }
    public void ChangeShieldDown()
    {
        isShieldDown = true;
        if (shieldTimer >= 1 - shieldPer)
        {
            isShieldDown = false;
            shieldTimer = 1 - shieldPer;
            ShieldMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - shieldTimer));
            if (shieldPer == 0)
            {
                BreakShieldMark();
            }
            else
            {
                GetShieldMark();
            }
        }
    }


    /// <summary>
    /// 护盾标志获得
    /// </summary>
    public void GetShieldMark()
    {
        if (ShieldMark != null)
        {
            ShieldMark.GetComponent<Animator>().SetBool("Have" , true);
        }
    }


    /// <summary>
    /// 护盾标志裂开
    /// </summary>
    public void BreakShieldMark()
    {
        if (ShieldMark != null)
        {
            ShieldMark.GetComponent<Animator>().SetBool("Have", false);
        }
    }



    //TODO 需修改护盾
    /// <summary>
    /// 血条渐入、渐出
    /// </summary>
    /// <param name="FadeDuration">淡入淡出持续时间</param>
    /// <param name="Reverse">是则淡出，否则淡入</param>
    public void Fade(float FadeDuration, bool Reverse)
    {
        hpImages = new Image[]
        {
            HpMask,
            HpMask.transform.parent.GetComponent<Image>(),
            HpMask.transform.parent.GetChild(0).GetComponent<Image>(),
            HpMask.transform.parent.GetChild(1).GetComponent<Image>(),
            HpMask.transform.parent.GetChild(2).GetComponent<Image>(),
            HpMask.transform.parent.GetChild(4).GetComponent<Image>(),
            HpMask.transform.GetChild(0).GetComponent<Image>()
        };
        isFading = true;
        fadeduration = FadeDuration;
        fadeReverse = Reverse;
    }
}
