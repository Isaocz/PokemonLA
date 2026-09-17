using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    //声明一个静态函数，表示血量
    public static UIHealthBar Instance;

    //声明一个图片对象，表示血条，以及一个浮点型变量，表示血条的初始长度
    //声明两个文本对象，表示当前血量和最大血量
    public Image Mask;
    public Text NowHpText;
    public Text MaxHpText;
    // Compact HP: integer arithmetic deliberately truncates instead of rounding.
    private Vector3[] hpTextScales;
    // Previous alternate font: private Font compactFont;
    private Font[] normalFonts;
    private Color[] normalTextColors;
    private int[] normalFontSizes;
    public static string FormatHealth(long value)
    {
        if (value < 1000) return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        long unit = value >= 1000000000 ? 1000000000 : value >= 1000000 ? 1000000 : 1000;
        string suffix = unit == 1000000000 ? "B" : unit == 1000000 ? "M" : "K";
        long whole = value / unit;
        long tenth = (value % unit) / (unit / 10);
        return whole.ToString(System.Globalization.CultureInfo.InvariantCulture) +
            (tenth == 0 ? "" : "." + tenth) + suffix;
    }

    // Existing health writers can still assign raw numbers. Normalize after their updates,
    // before Canvas rendering, without rewriting gameplay HP or shrinking cumulatively.
    private void LateUpdate()
    {
        FitHealthText(NowHpText, 0);
        FitHealthText(MaxHpText, 1);
    }
    private void FitHealthText(Text label, int index)
    {
        if (label == null || hpTextScales == null) return;
        if (long.TryParse(label.text, out long value)) label.text = FormatHealth(value);
        // Original MyNum now includes . K M B. Keep the same glyph style, size and tint.
        // Previous compact-font implementation retained for reference:
        // // MyNum contains digits only. The compact zpix font includes decimal points and K/M/B.
        // bool compact = label.text.EndsWith("K") || label.text.EndsWith("M") || label.text.EndsWith("B");
        // // Previous: label.font = compact && compactFont != null ? compactFont : normalFonts[index];
        // // Old digit atlas is dark; the complete font has white glyphs and needs an explicit dark tint.
        // bool useCompact = compact && compactFont != null;
        // label.font = useCompact ? compactFont : normalFonts[index];
        // label.fontSize = useCompact ? 12 : normalFontSizes[index];
        // label.color = useCompact ? new Color(0.25f, 0.25f, 0.19f, normalTextColors[index].a) : normalTextColors[index];
        // float factor = Mathf.Clamp(label.rectTransform.rect.width / Mathf.Max(1f, label.preferredWidth), 0.5f, 1f);
        // if (useCompact) factor = Mathf.Min(factor, 0.75f);
        label.font = normalFonts[index];
        label.fontSize = normalFontSizes[index];
        label.color = normalTextColors[index];
        float factor = Mathf.Clamp(label.rectTransform.rect.width / Mathf.Max(1f, label.preferredWidth), 0.5f, 1f);
        label.transform.localScale = hpTextScales[index] * factor;
    }
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

    PlayerControler player;


    //初始化血条
    private void Awake()
    {
        Instance = this;
        // Previous alternate font load: compactFont = Resources.Load<Font>("MewVisuals/HealthCompact");
        normalTextColors = new[] { NowHpText.color, MaxHpText.color };
        normalFontSizes = new[] { NowHpText.fontSize, MaxHpText.fontSize };
        NowHpText.verticalOverflow = MaxHpText.verticalOverflow = VerticalWrapMode.Overflow;
        normalFonts = new[] { NowHpText.font, MaxHpText.font };
        hpTextScales = new[] { NowHpText.transform.localScale, MaxHpText.transform.localScale };
        NowHpText.horizontalOverflow = MaxHpText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }




    //获得血条的初始长度，既最大长度
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
              // Previous full-number display: MaxHpText.text = string.Format("{000}", player.maxHp);
              MaxHpText.text = FormatHealth(player.maxHp);
              // Previous full-number display: NowHpText.text = string.Format("{000}", player.Hp);
              NowHpText.text = FormatHealth(player.Hp);
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
    


    //每帧检测一次当前血量，随之改变血条颜色。每帧检测一次血条是否改变，如果改变缓慢改变。
    // Update is called once per frame
    void Update()
    {
        if (!isHpUp && !isHpDown )
        {
            if (Mask.rectTransform.rect.width/ originalSize > per ) { ChangeHpDown(); }
            if (Mask.rectTransform.rect.width/ originalSize < per) { ChangeHpUp(); }
        }

        //当调用血量上升函数时血条缓慢增加到指定值，反之缓慢减少到指定值
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

        //改变血条颜色
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

    //两个函数分别为表示表示血条增加和血条减少的函数
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
