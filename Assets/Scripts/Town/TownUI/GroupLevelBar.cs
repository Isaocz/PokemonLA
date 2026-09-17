using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupLevelBar : MonoBehaviour
{

    // 冒险团等级
    // 萌芽级冒险团  0-5000        磐石级冒险团  5000-18000    坚冰级冒险团 18000-40000 
    // 热火级冒险团  40000-80000   不屈级冒险团  80000-180000   顶点级冒险团 180000-300000 
    // 传说级冒险团  300000-550000 大地之冒险团  550000-900000  天之冒险团  900000-1000000    



    public static int[] ExpRequired = new int[] { 0 , 5000, 18000, 40000, 80000, 180000, 300000, 550000, 900000, 1000000 };

    public Sprite BlueBar;
    public Sprite GreenBar;

    public int GroupLevelUpCount;

    public bool isGroupLevelUp;

    //徽章
    public GameObject[] Badgelist;

    //生成徽章的父对象
    public GameObject BadgeParentTransform;

    //等级条的Sprite
    public Sprite[] LevelBar;

    //等级条的Image
    public Image LevelBarImage;

    //表示经验进度的静态
    public static GroupLevelBar Instance;
    //表示经验条，以及一个浮点型变量，表示经验条的初始长度
    public Image Mask;
    public float originalSize;

    //声明一个浮点型变量，表示变化的比例。一个布尔型变量，表示是否增加血量。一个布尔型变量，表示是否减少血量。以及一个浮点型表示缓慢改变的计时器
    public float Per
    {
        get { return per; }
        set { per = value; }
    }
    public float per;
    public float timer;
    public bool isHpUp = false;
    public bool isHpDown = false;


    public DisplayTextInSequence DisComp;


    /// <summary>
    /// 音效
    /// </summary>
    public AudioSource SEPlayer;


    /// <summary>
    /// 是否初始化结束 为经验条改变添加一个微小延迟（0.1s）
    /// </summary>
    bool isInitializationComplete = false;


    //初始化血条
    private void Awake()
    {
        Instance = this;

        if (originalSize == 0)
        {
            originalSize = Mask.rectTransform.rect.width;
        }
    }

    SaveData save;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start0" + "+" + per);
        SetLevel();
        GroupLevelUpCount = ScoreCounter.Instance.IsGroupLevelUp;
        if (GroupLevelUpCount > 0) { isGroupLevelUp = true; }

        //微小延迟，成真后才开始经验条变化
        Timer.Start(this, 0.4f, () => { isInitializationComplete = true; });
    }


    public void SetLevel()
    {
        Debug.Log("Start1" + "+" + per);
        if (SaveLoader.saveLoader != null)
        {
            save = SaveLoader.saveLoader.saveData;
            SetLevelBar(save.GroupLevel);
            Debug.Log("Start2" + "+" + per);
            if (save != null)
            {
                per = Mathf.Clamp((float)(save.APTotal - ExpRequired[save.GroupLevel]) / (float)(ExpRequired[save.GroupLevel + 1] - ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
                timer = 1 - per;
                Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * per);
                Debug.Log("Start3"+"+"+per);
                Debug.Log(save.APTotal + "+"+ ExpRequired[save.GroupLevel] + "+" + ExpRequired[save.GroupLevel + 1] + "+" + ExpRequired[save.GroupLevel] );
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
        Debug.Log("Reset");
        //移除曾经的徽章
        if (BadgeParentTransform.transform.childCount != 0) {
            foreach (Transform child in BadgeParentTransform.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Instantiate(Badgelist[Level], BadgeParentTransform.transform.position, Quaternion.identity,BadgeParentTransform.transform);
        LevelBarImage.sprite = LevelBar[Level / 3];
    }




    //每帧检测一次当前血量，随之改变血条颜色。每帧检测一次血条是否改变，如果改变缓慢改变。
    // Update is called once per frame
    void Update()
    {
        if (isInitializationComplete)
        {
            //不处于持续上升态
            if (!isHpUp && !isHpDown)
            {
                //经验条和百分比不匹配
                if (Mask.rectTransform.rect.width / originalSize > per) { ChangeExpDown(); }
                if (Mask.rectTransform.rect.width / originalSize < per) { ChangeExpUp(); }
                //百分比满了且仍需要升级
                if (per >= 1.0f && isGroupLevelUp) { ChangeExpUp(); }
            }

            //当调用血量上升函数时血条缓慢增加到指定值，反之缓慢减少到指定值
            if (isHpUp)
            {
                timer -= Time.deltaTime;
                Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
                ChangeExpUp();
            }
            else if (isHpDown)
            {
                timer += Time.deltaTime;
                Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
                ChangeExpDown();
            }

            //经验条上升音效
            if (SEPlayer != null)
            {
                if (isHpUp || isHpDown)
                {
                    if (!SEPlayer.isPlaying)
                    {
                        SEPlayer.Play();
                    }
                    SEPlayer.pitch = Mathf.Lerp(0.8f, 2.2f, (1.0f - timer) / 1.0f);
                    //Debug.Log((1.0f - timer) / 1.0f);
                }
                else
                {
                    if (SEPlayer.isPlaying)
                    {
                        SEPlayer.Stop();
                    }
                }
            }


            //改变血条颜色
            if (timer <= 0)
            {
                if (Mask.sprite == BlueBar) { Mask.sprite = GreenBar; }
            }
            else
            { if (Mask.sprite == GreenBar) { Mask.sprite = BlueBar; } }
        }
    }

    //两个函数分别为表示表示血条增加和血条减少的函数
    public void ChangeExpUp()
    {
        //Debug.Log(4);
        isHpUp = true;
        if (timer <= 1 - per)
        {
            //Debug.Log(3);
            isHpUp = false;
            timer = 1 - per;
            if ( per >= 1.0f)
            {
                //Debug.Log(2);
                if (GroupLevelUpCount > 0) {
                    //Debug.Log(1);
                    isGroupLevelUp = false;
                    GetComponent<Animator>().SetTrigger("Shine");
                    for (int i = 0; i < BadgeParentTransform.transform.childCount; i++)
                    {
                        //Debug.Log(BadgeParentTransform.transform.GetChild(0).gameObject.name);
                        BadgeParentTransform.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shine");
                    }
                }
            }
            else
            {
                if (DisComp != null)
                {
                    DisComp.DisplayOver();
                    DisComp = null;
                }
            }

        }
    }
    public void ChangeExpDown()
    {
        isHpDown = true;
        if (timer >= 1 - per)
        {
            isHpDown = false;
            timer = 1 - per;
            if ( per >= 1.0f)
            {
                if (GroupLevelUpCount > 0 && isGroupLevelUp) {
                    isGroupLevelUp = false;
                    GetComponent<Animator>().SetTrigger("Shine");
                    for (int i = 0; i < BadgeParentTransform.transform.childCount; i++)
                    {
                        BadgeParentTransform.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shine");
                    }
                }
            }
            else
            {

                if (DisComp != null)
                {
                    DisComp.DisplayOver();
                    DisComp = null;
                }
            }

        }
    }


    public void LevelUp()
    {
        int l = 0;
        if (save != null) { l = save.GroupLevel + 1; }
        else { l = 2; }
        Instantiate(Badgelist[l], BadgeParentTransform.transform.position, Quaternion.identity, BadgeParentTransform.transform).transform.SetAsFirstSibling() ;
        LevelBarImage.sprite = LevelBar[(l) / 3];
        //升级音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonBasicSFXList.GetSpeaceItem, Vector2.zero, true);
        }
    }

    public void LevelUpOver()
    {
        Debug.Log("Over");
        if (SaveLoader.saveLoader != null)
        {
            GroupLevelUpCount -= 1;
            if (GroupLevelUpCount > 0) { isGroupLevelUp = false; }
            save.GroupLevel += 1;
            save = SaveLoader.saveLoader.saveData;
            if (save != null)
            {
                per = Mathf.Clamp((float)(save.APTotal + ScoreCounter.Instance.TotalAP() - ExpRequired[save.GroupLevel]) / (float)(ExpRequired[save.GroupLevel + 1] - ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
                Debug.Log (per);
                Debug.Log (save.APTotal + ScoreCounter.Instance.TotalAP());
                Debug.Log (ExpRequired[save.GroupLevel]);
                Debug.Log (ExpRequired[save.GroupLevel + 1]);
                Debug.Log (ExpRequired[save.GroupLevel]);
                Debug.Log ((float)(save.APTotal + ScoreCounter.Instance.TotalAP() - ExpRequired[save.GroupLevel]));
                Debug.Log ((float)(ExpRequired[save.GroupLevel + 1] - ExpRequired[save.GroupLevel]));
                timer = 1.0f;
                Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            }
        }
        else
        {
            Per = Mathf.Clamp((float)(8120.0f - GroupLevelBar.ExpRequired[1]) / (float)(GroupLevelBar.ExpRequired[1 + 1] - GroupLevelBar.ExpRequired[1]), 0.0f, 1.0f);
            timer = 1.0f;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        }
    }


}
