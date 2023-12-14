using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyHpBar : MonoBehaviour
{
    //����һ��ͼƬ���󣬱�ʾѪ�����Լ�һ�������ͱ�������ʾѪ���ĳ�ʼ����

    public Image Mask;
    float originalSize;

    //����һ�������ͱ�������ʾ�仯�ı�����һ�������ͱ�������ʾ�Ƿ�����Ѫ����һ�������ͱ�������ʾ�Ƿ����Ѫ�����Լ�һ�������ͱ�ʾ�����ı�ļ�ʱ��
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


    //���Ѫ���ĳ�ʼ���ȣ�����󳤶�
    // Start is called before the first frame update
    void Start()
    {
        originalSize = Mask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        fadetimer = 0f;
    }



    //ÿ֡���һ�ε�ǰѪ������֮�ı�Ѫ����ɫ��ÿ֡���һ��Ѫ���Ƿ�ı䣬����ı仺���ı䡣
    // Update is called once per frame
    void Update()
    {
        //������Ѫ����������ʱѪ���������ӵ�ָ��ֵ����֮�������ٵ�ָ��ֵ
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

        //�ı�Ѫ����ɫ
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

    //���������ֱ�Ϊ��ʾ��ʾѪ�����Ӻ�Ѫ�����ٵĺ���
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
    /// Ѫ�����롢����
    /// </summary>
    /// <param name="FadeDuration">���뵭������ʱ��</param>
    /// <param name="Reverse">���򵭳���������</param>
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
