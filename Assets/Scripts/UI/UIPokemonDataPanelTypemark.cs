using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonDataPanelTypemark : MonoBehaviour
{
    Image TypeColorBar;
    Image TypeMark;
    Image TeraMask;
    Text TypeName;



    public Sprite None;
    public Sprite Nurmal;
    public Sprite Fight;
    public Sprite Fly;
    public Sprite Poison;
    public Sprite Ground;
    public Sprite Rock;
    public Sprite Bug;
    public Sprite Ghost;
    public Sprite Steel;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Grass;
    public Sprite Electric;
    public Sprite Psychic;
    public Sprite Ice;
    public Sprite Dragon;
    public Sprite Dark;
    public Sprite Fairy;

    private void Start()
    {
        TypeColorBar = gameObject.GetComponent<Image>();
        TypeMark = transform.GetChild(0).GetComponent<Image>();
        TypeName = transform.GetChild(1).GetComponent<Text>();
        if(transform.childCount == 3)
        {
            TeraMask = transform.GetChild(2).GetComponent<Image>();
        }
        
    }
    // Start is called before the first frame update
    public void GetChildTypeMark(int type)
    {
        if (TypeColorBar != null)
        {
            switch (type)
            {
                case 0:

                    TypeColorBar.color = new Color(0, 0, 0, 0);
                    TypeMark.sprite = None;
                    TypeMark.color = new Color(0, 0, 0, 0);
                    TypeName.text = "无";
                    TypeName.color = new Color(0, 0, 0, 0);
                    if (TeraMask != null) { TeraMask.color = new Color(0, 0, 0, 0); }

                    break;
                case 1:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Nurmal;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "一般";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }

                    break;
                case 2:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Fight;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "格斗";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 3:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Fly;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "飞行";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 4:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Poison;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "毒";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 5:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Ground;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "地面";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 6:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Rock;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "岩石";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 7:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Bug;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "虫";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 8:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Ghost;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "幽灵";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 9:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Steel;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "钢";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 10:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Fire;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "火";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 11:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Water;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "水";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 12:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Grass;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "草";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 13:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Electric;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "电";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 14:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Psychic;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "超能";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 15:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Ice;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "冰";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 16:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Dragon;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "龙";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 17:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Dark;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "恶";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
                case 18:
                    TypeColorBar.color = Type.TypeColor[type];
                    TypeMark.sprite = Fairy;
                    TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    TypeName.text = "妖精";
                    TypeName.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                    if (TeraMask != null) { TeraMask.color = new Color(1, 1, 1, 1); }
                    break;
            }
        }
    }
    }
