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






    //���Ѫ���ĳ�ʼ���ȣ�����󳤶�
    // Start is called before the first frame update
    void Start()
    {
        originalSize = Mask.rectTransform.rect.width;
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
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
}
