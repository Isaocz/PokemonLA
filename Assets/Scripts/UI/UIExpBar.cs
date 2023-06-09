using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    

    //����һ����̬��������ʾ�������ĳ���
    public static UIExpBar Instance { get; private set; }

    //����һ����������ʾ�������ֵľ��������Լ�����������󳤶�,
    public Image Mask;
    float originnoSize;

    //����һ����ʾ�ȼ����ı�����һ�����ζ���level��ʾ�ȼ�������
    public Text Leveltext;
    int level;

    //����һ����������per����ʾ�����������ձ仯���ӣ��ȱ仯�İٷֱ�
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;


    //����һ����������count����ʾ��������������ʱ������Ĵ���
    public int Icount
    {
        get { return count; }
        set { count = value; }
    }
    int count;


    //����һ�������ͱ���isExUp��ʾ�Ƿ����ӣ���Ϊ��ʱ�������������ӡ�
    //����һ�������ͱ���isExUpOf��ʾ�������Ƿ��������Ϊ��ʱִ������Ĵ��롣
    //����һ�������ͱ���isZero��ʾ�������Ƿ�����,���˵Ļ����㾭������
    //�Լ�һ����ʱ��Timer�������þ��������������ӡ�
    bool isExpUp = false;
    bool isExpUpOf = false;
    bool isZero = false;
    float timer = 0;

    PlayerControler player;




    //��ʼ��������
    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    //��ʼʱ���������ĳ��ȣ������þ���������
    void Start()
    {
        originnoSize = Mask.rectTransform.rect.width;
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        level = player.Level;
    }

    //ÿ֡���һ�ξ������Ƿ���Ҫ�������Լ������Ƿ����
    // Update is called once per frame
    void Update()
    {
        //����������������������ʼ�������ӣ�����ʱ����������ֵʱ����������
        if (isExpUp)
        {
            timer += Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originnoSize*timer);
            ExpUp();
            if (timer >= per)
            {
                isExpUp = false;
            }
        }
        //��������������ʼ�������ӣ�������������һ��ʱ�����icount��һ���������㾭������ʼ���¼�ʱ�����������������Ϻ�ʼ����������ָ��ֵ��
        if (isExpUpOf)
        {
            timer += Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originnoSize * timer);
            if (count == 0 && timer >= per)
            {
                isExpUpOf = false;
                UIExpBar.Instance.Icount = 0;
            }
            if (timer > 1)
            {
                count--; 
                isZero = true;
                ExpZero();
            }

        }
        

    }

    //����һ�����������ô˺���ʱ��ʾ���Ӿ��鵫�����
    public void ExpUp()
    {
        isExpUp = true;
    }

    //����һ�����������ô˺���ʱ��ʾ���Ӿ��������
    public void ExpUpOverflow()
    {
         isExpUpOf = true;
    }

    //����һ�����������ô˺���ʱ���������㣬ÿ������ʱ�ȼ����ӣ���ʹ�ȼ���UI�����ÿ������ʱ������������Ѫ����
    void ExpZero()
    {
        if (isZero)
        {
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            timer = 0;
            isZero = false;
            isExpUp = false;
            level++;
            Leveltext.text = string.Format("{00}",level);
            player.LevelForSkill++;
            player.LearnNewSkill();
        }
    }
}
