using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    //����һ����̬��������ʾѪ��
    public static UIHealthBar Instance;

    //����һ��ͼƬ���󣬱�ʾѪ�����Լ�һ�������ͱ�������ʾѪ���ĳ�ʼ����
    //���������ı����󣬱�ʾ��ǰѪ�������Ѫ��
    public Image Mask;
    public Text NowHpText;
    public Text MaxHpText;
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

    PlayerControler player;


    //��ʼ��Ѫ��
    private void Awake()
    {
        Instance = this;
    }




    //���Ѫ���ĳ�ʼ���ȣ�����󳤶�
    // Start is called before the first frame update
    void Start()
    {
        if (originalSize == 0)
        {
            originalSize = Mask.rectTransform.rect.width;
        }
        player = FindObjectOfType<PlayerControler>();
        if(player != null)
        {
            Timer.Start(this, 0.1f, () =>
          {
              MaxHpText.text = string.Format("{000}", player.maxHp);
              NowHpText.text = string.Format("{000}", player.Hp);
              //Debug.Log("sasa");
              per = (float)player.Hp / (float)player.maxHp;
              timer = 1 - per;
              //SDebug.Log(player.Hp + "+" + player.maxHp + "+" + per + "+" + timer);
              Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)player.Hp / (float)player.maxHp * originalSize);
          }
            );
        }
    }


    public void InstanceHpBar()
    {
        if (originalSize == 0) { originalSize = Mask.rectTransform.rect.width; }
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize);
    }
    


    //ÿ֡���һ�ε�ǰѪ������֮�ı�Ѫ����ɫ��ÿ֡���һ��Ѫ���Ƿ�ı䣬����ı仺���ı䡣
    // Update is called once per frame
    void Update()
    {
        if (!isHpUp && !isHpDown )
        {
            if (Mask.rectTransform.rect.width/ originalSize > per ) { ChangeHpDown(); }
            if (Mask.rectTransform.rect.width/ originalSize < per) { ChangeHpUp(); }
        }

        //������Ѫ����������ʱѪ���������ӵ�ָ��ֵ����֮�������ٵ�ָ��ֵ
        if (isHpUp)
        {
            timer -= Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpUp();
        }
        if (isHpDown)
        {
            timer += Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeHpDown();
        }

        //�ı�Ѫ����ɫ
        if( timer <= 0.5f)
        {
            Mask.color = new Color((255/255f),(255/255f),(255/255f),(255/255f));
        }else if((0.5f < timer )&& (timer < 0.8f))
        {
            Mask.color = new Color((255 / 255f), (255 / 255f), (0 / 255f), (255 / 255f));
        }else if(timer >= 0.8f)
        {
            Mask.color = new Color((255 / 255f), (120 / 255f), (47 / 255f), (255 / 255f));
        }
    }

    //���������ֱ�Ϊ��ʾ��ʾѪ�����Ӻ�Ѫ�����ٵĺ���
    public void ChangeHpUp()
    {
        isHpUp = true;
        if(timer <= 1 - per)
        {
            isHpUp = false;
            timer = 1 - per;
        }
    }
    public void ChangeHpDown()
    {
        isHpDown = true;
        if(timer >= 1-per)
        {
            isHpDown = false;
            timer = 1 - per;
        }
    }






}
