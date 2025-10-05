using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyHpBar : MonoBehaviour
{
    /// <summary>
    /// �������������ٶȼӳ�
    /// </summary>
    public static float SPEED_SHIELD_BAR_UP = 2.5f;

    //����һ��ͼƬ���󣬱�ʾѪ�����Լ�һ�������ͱ�������ʾѪ���ĳ�ʼ����
    //Ѫ��
    public Image HpMask;
    //����
    public Image ShieldMask;
    float originalSize;

    //����һ�������ͱ�������ʾѪ���仯�ı�����һ�������ͱ�������ʾ�Ƿ�����Ѫ����һ�������ͱ�������ʾ�Ƿ����Ѫ�����Լ�һ�������ͱ�ʾ����Ѫ���ı�ļ�ʱ��
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;

    float timer;
    bool isHpUp = false;
    bool isHpDown = false;

    //����һ�������ͱ�������ʾѪ���仯�ı�����һ�������ͱ�������ʾ�Ƿ�����Ѫ����һ�������ͱ�������ʾ�Ƿ����Ѫ�����Լ�һ�������ͱ�ʾ����Ѫ���ı�ļ�ʱ��
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



    //TODO���뽥�����
    Image[] hpImages;
    float fadetimer;
    float fadeduration;
    bool isFading;
    bool fadeReverse;

    /// <summary>
    /// ���ܱ�־
    /// </summary>
    public GameObject ShieldMark;


    //���Ѫ���ĳ�ʼ���ȣ�����󳤶�
    // Start is called before the first frame update
    void Awake()
    {
        originalSize = HpMask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        fadetimer = 0f;
    }


    /// <summary>
    /// ��ʼ��������
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



    //ÿ֡���һ�ε�ǰѪ������֮�ı�Ѫ����ɫ��ÿ֡���һ��Ѫ���Ƿ�ı䣬����ı仺���ı䡣
    // Update is called once per frame
    void Update()
    {

        //������Ѫ����������ʱѪ���������ӵ�ָ��ֵ����֮�������ٵ�ָ��ֵ
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

        //�ı�Ѫ����ɫ
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

        //TODO ���޸Ļ���
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

    //���������ֱ�Ϊ��ʾ��ʾѪ�����Ӻ�Ѫ�����ٵĺ���
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


    //���������ֱ�Ϊ��ʾ��ʾ�������Ӻ�Ѫ�����ٵĺ���
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
    /// ���ܱ�־���
    /// </summary>
    public void GetShieldMark()
    {
        if (ShieldMark != null)
        {
            ShieldMark.GetComponent<Animator>().SetBool("Have" , true);
        }
    }


    /// <summary>
    /// ���ܱ�־�ѿ�
    /// </summary>
    public void BreakShieldMark()
    {
        if (ShieldMark != null)
        {
            ShieldMark.GetComponent<Animator>().SetBool("Have", false);
        }
    }



    //TODO ���޸Ļ���
    /// <summary>
    /// Ѫ�����롢����
    /// </summary>
    /// <param name="FadeDuration">���뵭������ʱ��</param>
    /// <param name="Reverse">���򵭳���������</param>
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
