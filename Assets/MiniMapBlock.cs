using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapBlock : MonoBehaviour
{

    public enum MiniMapBlockMarkType
    {
        Heal,
        Berry,
        Feather,
        ExpCandy,
        SkillItem,
        RebBall,
        BlackBall,
        SkillBall,
        Posion,
        SpaceItem,
        Money,
        Stone,
        XAb,
        Tera,
    };

    public Sprite HealSprite;
    public Sprite BerrySprite;
    public Sprite FeatherSprite;
    public Sprite ExpCandySprite;
    public Sprite SkillItemSprite;
    public Sprite RedBallSprite;
    public Sprite BlackBallSprite;
    public Sprite SkillBallSprite;
    public Sprite PosionSprite;
    public Sprite SpaceItemSprite;
    public Sprite MoneySprite;
    public Sprite StoneSprite;
    public Sprite XabSprite;
    public Sprite TeraSprite;

    Image i1;
    Image i2;
    Image i3;
    Image i4;

    Image Block;

    private void Start()
    {
        Block = transform.GetComponent<Image>();
        i1 = transform.GetChild(0).GetComponent<Image>();
        i2 = transform.GetChild(1).GetComponent<Image>();
        i3 = transform.GetChild(2).GetComponent<Image>();
        i4 = transform.GetChild(3).GetComponent<Image>();
    }

    public void ChangeImageMark(MiniMapBlockMarkType type)
    {
        if (i1.sprite == null)
        {
            i1.sprite = SwitchSprite(type);
            i1.gameObject.SetActive(true);
            return;
        }
        if (i2.sprite == null)
        {
            i2.sprite = SwitchSprite(type);
            i2.gameObject.SetActive(true);
            return;
        }
        if (i3.sprite == null)
        {
            i3.sprite = SwitchSprite(type);
            i3.gameObject.SetActive(true);
            return;
        }
        if (i4.sprite == null)
        {
            i4.sprite = SwitchSprite(type);
            i4.gameObject.SetActive(true);
            return;
        }
    }

    public void ClearAllItem()
    {
        i1.sprite = null;
        i1.gameObject.SetActive(false);
        i2.sprite = null;
        i2.gameObject.SetActive(false);
        i3.sprite = null;
        i3.gameObject.SetActive(false);
        i4.sprite = null;
        i4.gameObject.SetActive(false);
    }


    Sprite SwitchSprite(MiniMapBlockMarkType type)
    {
        Sprite OutPut = SpaceItemSprite;
        switch (type)
        {
            case MiniMapBlockMarkType.Heal:
                OutPut = HealSprite;
                break;
            case MiniMapBlockMarkType.Berry:
                OutPut = BerrySprite;
                break;
            case MiniMapBlockMarkType.Feather:
                OutPut = FeatherSprite;
                break;
            case MiniMapBlockMarkType.ExpCandy:
                OutPut = ExpCandySprite;
                break;
            case MiniMapBlockMarkType.SkillItem:
                OutPut = SkillItemSprite;
                break;
            case MiniMapBlockMarkType.RebBall:
                OutPut = RedBallSprite;
                break;
            case MiniMapBlockMarkType.BlackBall:
                OutPut = BlackBallSprite;
                break;
            case MiniMapBlockMarkType.SkillBall:
                OutPut = SkillBallSprite;
                break;
            case MiniMapBlockMarkType.Posion:
                OutPut = PosionSprite;
                break;
            case MiniMapBlockMarkType.SpaceItem:
                OutPut = SpaceItemSprite;
                break;
            case MiniMapBlockMarkType.Money:
                OutPut = MoneySprite;
                break;
            case MiniMapBlockMarkType.Stone:
                OutPut = StoneSprite;
                break;
            case MiniMapBlockMarkType.XAb:
                OutPut = XabSprite;
                break;
            case MiniMapBlockMarkType.Tera:
                OutPut = TeraSprite;
                break;
        }
        return OutPut;
    }




}
