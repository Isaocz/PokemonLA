using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    

    //声明一个静态变量，表示经验条的长度
    public static UIExpBar Instance { get; private set; }

    //声明一个变量，表示用来遮罩的经验条，以及经验条的最大长度,
    public Image Mask;
    float originnoSize;

    //声明一个表示等级的文本对象，一个整形对象level表示等级的数据
    public Text Leveltext;
    int level;

    //声明一个公开变量per，表示经验条的最终变化样子，既变化的百分比
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    float per;


    //声明一个公开变量count，表示经验条超出上限时会充满的次数
    public int Icount
    {
        get { return count; }
        set { count = value; }
    }
    int count;


    //声明一个布尔型变量isExUp表示是否增加，当为真时经验条缓慢增加。
    //声明一个布尔型变量isExUpOf表示经验条是否溢出，当为真时执行溢出的代码。
    //声明一个布尔型变量isZero表示经验条是否满了,满了的话清零经验条。
    //以及一个计时器Timer，可以让经验条缓慢的增加。
    bool isExpUp = false;
    bool isExpUpOf = false;
    bool isZero = false;
    float timer = 0;

    PlayerControler player;




    //初始化经验条
    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    //开始时捕获经验条的长度，并且让经验条归零
    void Start()
    {
        originnoSize = Mask.rectTransform.rect.width;
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        level = player.Level;
    }

    //每帧检测一次经验条是否需要增长，以及增长是否溢出
    // Update is called once per frame
    void Update()
    {
        //如果增长但增长不溢出，开始缓慢增加，当计时器到达增长值时结束增长。
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
        //如果增长溢出，开始缓慢增加，当经验条满了一次时溢出数icount减一，并且清零经验条开始重新计时，当溢出次数计算完毕后开始缓慢增长到指定值。
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

    //声明一个函数，调用此函数时表示增加经验但不溢出
    public void ExpUp()
    {
        isExpUp = true;
    }

    //声明一个函数，调用此函数时表示增加经验且溢出
    public void ExpUpOverflow()
    {
         isExpUpOf = true;
    }

    //声明一个函数，调用此函数时经验条清零，每次清零时等级增加，并使等级数UI输出。每次升级时输出升级后最大血量的
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
