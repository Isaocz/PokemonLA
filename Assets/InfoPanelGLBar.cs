using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelGLBar : MonoBehaviour
{
    // 冒险团等级
    // 萌芽级冒险团  0-5000        磐石级冒险团  5000-18000    坚冰级冒险团 18000-40000 
    // 热火级冒险团  40000-80000   不屈级冒险团  80000-180000   顶点级冒险团 180000-300000 
    // 传说级冒险团  300000-550000 大地之冒险团  550000-900000  天之冒险团  900000-1000000    



    public static int[] ExpRequired = new int[] { 0, 5000, 18000, 40000, 80000, 180000, 300000, 550000, 900000, 1000000 };

    public Sprite BlueBar;
    public Sprite GreenBar;

    //徽章
    public GameObject[] Badgelist;

    //生成徽章的父对象
    public GameObject BadgeParentTransform;

    //等级条的Sprite
    public Sprite[] LevelBar;

    //等级条的Image
    public Image LevelBarImage;

    //表示经验进度的静态
    public static InfoPanelGLBar Instance;
    //表示经验条，以及一个浮点型变量，表示经验条的初始长度
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



    //初始化血条
    private void Awake()
    {
        Instance = this;

        if (originalSize == 0)
        {
            originalSize = Mask.transform.parent.GetComponent<RectTransform>().rect.width;
        }
    }

    SaveData save;

    // Start is called before the first frame update
    void Start()
    {
        SetLevel();
    }


    public void SetLevel()
    {
        if (SaveLoader.saveLoader != null)
        {
            save = SaveLoader.saveLoader.saveData;
            SetLevelBar(save.GroupLevel);
            if (save != null)
            {
                per = Mathf.Clamp((float)(save.APTotal - ExpRequired[save.GroupLevel]) / (float)(ExpRequired[save.GroupLevel + 1] - ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
                timer = 1 - per;
                Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * per);
            }
        }
        else
        {
            SetLevelBar(0);
            Per = Mathf.Clamp((float)(120.0f - GroupLevelBar.ExpRequired[0]) / (float)(GroupLevelBar.ExpRequired[0 + 1] - GroupLevelBar.ExpRequired[0]), 0.0f, 1.0f);
            timer = 1 - per;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * per);
        }
    }


    //根据冒险团等级设置经验条
    public void SetLevelBar(int Level)
    {
        //移除曾经的徽章
        if (BadgeParentTransform.transform.childCount != 0)
        {
            foreach (Transform child in BadgeParentTransform.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Instantiate(Badgelist[Level], BadgeParentTransform.transform.position, Quaternion.identity, BadgeParentTransform.transform);
        LevelBarImage.sprite = LevelBar[Level / 3];
    }




    //每帧检测一次当前血量，随之改变血条颜色。每帧检测一次血条是否改变，如果改变缓慢改变。
    // Update is called once per frame
    void Update()
    {

        if (!isHpUp && !isHpDown)
        {


            
            if (Mask.rectTransform.rect.width / originalSize > per) { ChangeExpDown(); }
            if (Mask.rectTransform.rect.width / originalSize < per) { ChangeExpUp(); }
        }

        //当调用血量上升函数时血条缓慢增加到指定值，反之缓慢减少到指定值
        if (isHpUp)
        {
            timer -= Time.deltaTime;
            Debug.Log(transform.name);
            Debug.Log(per);
            Debug.Log(timer);
            Debug.Log(originalSize * (1.0f - timer));
            Debug.Log(Mask.rectTransform.rect.width / originalSize);
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeExpUp();
        }
        if (isHpDown)
        {
            timer += Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeExpDown();
        }

        //改变血条颜色
        if (timer <= 0)
        {
            if (Mask.sprite == BlueBar) { Mask.sprite = GreenBar; }
        }
        else
        { if (Mask.sprite == GreenBar) { Mask.sprite = BlueBar; } }
    }

    //两个函数分别为表示表示血条增加和血条减少的函数
    public void ChangeExpUp()
    {



        isHpUp = true;
        Debug.Log(timer + "+" + (1 - per));
        if (timer <= 1 - per)
        {
            isHpUp = false;
            timer = 1 - per;
        }
    }
    public void ChangeExpDown()
    {
        isHpDown = true;
        if (timer >= 1 - per)
        {
            isHpDown = false;
            timer = 1 - per;
        }
    }

}
