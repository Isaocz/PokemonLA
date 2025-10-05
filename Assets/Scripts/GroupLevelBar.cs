using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupLevelBar : MonoBehaviour
{

    // ð���ŵȼ�
    // ��ѿ��ð����  0-5000        ��ʯ��ð����  5000-18000    �����ð���� 18000-40000 
    // �Ȼ�ð����  40000-80000   ������ð����  80000-180000   ���㼶ð���� 180000-300000 
    // ��˵��ð����  300000-550000 ���֮ð����  550000-900000  ��֮ð����  900000-1000000    



    public static int[] ExpRequired = new int[] { 0 , 5000, 18000, 40000, 80000, 180000, 300000, 550000, 900000, 1000000 };

    public Sprite BlueBar;
    public Sprite GreenBar;

    public int GroupLevelUpCount;

    public bool isGroupLevelUp;

    //����
    public GameObject[] Badgelist;

    //���ɻ��µĸ�����
    public GameObject BadgeParentTransform;

    //�ȼ�����Sprite
    public Sprite[] LevelBar;

    //�ȼ�����Image
    public Image LevelBarImage;

    //��ʾ������ȵľ�̬
    public static GroupLevelBar Instance;
    //��ʾ���������Լ�һ�������ͱ�������ʾ�������ĳ�ʼ����
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


    public DisplayTextInSequence DisComp;


    //��ʼ��Ѫ��
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

        SetLevel();
        GroupLevelUpCount = ScoreCounter.Instance.IsGroupLevelUp;
        if (GroupLevelUpCount > 0 ) { isGroupLevelUp = true; }
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


    //����ð���ŵȼ����þ�����
    public void SetLevelBar(int Level)
    {
        //�Ƴ������Ļ���
        if (BadgeParentTransform.transform.childCount != 0) {
            foreach (Transform child in BadgeParentTransform.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Instantiate(Badgelist[Level], BadgeParentTransform.transform.position, Quaternion.identity,BadgeParentTransform.transform);
        LevelBarImage.sprite = LevelBar[Level / 3];
    }




    //ÿ֡���һ�ε�ǰѪ������֮�ı�Ѫ����ɫ��ÿ֡���һ��Ѫ���Ƿ�ı䣬����ı仺���ı䡣
    // Update is called once per frame
    void Update()
    {
        
        if (!isHpUp && !isHpDown)
        {
            if (Mask.rectTransform.rect.width / originalSize > per) { ChangeExpDown(); }
            if (Mask.rectTransform.rect.width / originalSize < per) { ChangeExpUp(); }
        }

        //������Ѫ����������ʱѪ���������ӵ�ָ��ֵ����֮�������ٵ�ָ��ֵ
        if (isHpUp)
        {
            timer -= Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeExpUp();
        }
        if (isHpDown)
        {
            timer += Time.deltaTime;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * (1.0f - timer));
            ChangeExpDown();
        }

        //�ı�Ѫ����ɫ
        if (timer <= 0)
        {
            if (Mask.sprite == BlueBar) { Mask.sprite = GreenBar; }
        }
        else
        { if (Mask.sprite == GreenBar) { Mask.sprite = BlueBar; } }
    }

    //���������ֱ�Ϊ��ʾ��ʾѪ�����Ӻ�Ѫ�����ٵĺ���
    public void ChangeExpUp()
    {
        isHpUp = true;
        if (timer <= 1 - per)
        {
            isHpUp = false;
            timer = 1 - per;
            if ( per >= 1.0f)
            {
                if (GroupLevelUpCount > 0) {
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
        Instantiate(Badgelist[save.GroupLevel + 1], BadgeParentTransform.transform.position, Quaternion.identity, BadgeParentTransform.transform).transform.SetAsFirstSibling() ;
        LevelBarImage.sprite = LevelBar[(save.GroupLevel+1) / 3];
    }

    public void LevelUpOver()
    {

        if (SaveLoader.saveLoader != null)
        {
            GroupLevelUpCount -= 1;
            if (GroupLevelUpCount > 0) { isGroupLevelUp = false; }
            save.GroupLevel += 1;
            save = SaveLoader.saveLoader.saveData;
            if (save != null)
            {
                per = Mathf.Clamp((float)(ScoreCounter.Instance.TotalAP() - ExpRequired[save.GroupLevel]) / (float)(ExpRequired[save.GroupLevel + 1] - ExpRequired[save.GroupLevel]), 0.0f, 1.0f);
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
