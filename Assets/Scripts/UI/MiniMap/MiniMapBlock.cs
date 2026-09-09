using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapBlock : MonoBehaviour
{
    //父房间
    public Room ParentRoom;

    //坐标
    public Vector3Int MiniMapBlockIndex;

    //玩家
    public PlayerControler player;


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

    public Sprite BabyRoomMark;
    public Sprite SkillStoreMark;
    public Sprite OtherRoomMark;
    public Sprite BossRoomMark;
    public Sprite StartRoomMark;

    public Transform ItemMarkParent;


    /// <summary>
    /// tp选择按钮
    /// </summary>
    public Button TPSelectButton;

    //该小地图区块对应的房间是否被访问
    public bool isThisRoomBeVisit;

    //该小地图区块对应的房间是否被清理
    public bool isThisRoomClear;

    //该小地图区块对应的房间内是否有玩家
    public bool isPlayerInThisRoom;






    private void Start()
    {
        Block = transform.GetComponent<Image>();
        i1 = ItemMarkParent.GetChild(0).GetComponent<Image>();
        i2 = ItemMarkParent.GetChild(1).GetComponent<Image>();
        i3 = ItemMarkParent.GetChild(2).GetComponent<Image>();
        i4 = ItemMarkParent.GetChild(3).GetComponent<Image>();
        EnsurePlayerAndRoom();
    }


    private void FixedUpdate()
    {
        //确保父房间
        EnsurePlayerAndRoom();
        //无父房间时结束
        if (ParentRoom == null) { return; }
        //有父房间时确定传送按钮活性化
        isThisRoomBeVisit = ParentRoom.isVisit;
        isThisRoomClear = (ParentRoom.isClear <= 0);
        isPlayerInThisRoom = ParentRoom.isInThisRoom;

        if (isThisRoomBeVisit) {
            //传送选择按钮非活性化
            if (!TPSelectButton.gameObject.activeInHierarchy)
            {
                //房间已经被清空 且 玩家不在当前房间内 活性化
                if (isThisRoomClear && !isPlayerInThisRoom)
                {
                    TPSelectButton.gameObject.SetActive(true);
                }
            }
            //传送选择按钮活性化
            else
            {
                //房间已经被清空 或 玩家在当前房间 非活性化
                if (!isThisRoomClear || isPlayerInThisRoom)
                {
                    TPSelectButton.gameObject.SetActive(false);
                }
            }
        }
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


    /// <summary>
    /// 0宝宝房 1技能商店 2其他 3boss 4初始房
    /// </summary>
    /// <param name="i"></param>
    public void ChangeRoomMark( int i )
    {
        Block = transform.GetComponent<Image>();
        switch (i)
        {
            case 0:
                Block.sprite = BabyRoomMark;
                break;
            case 1:
                Block.sprite = SkillStoreMark;
                break;
            case 2:
                Block.sprite = OtherRoomMark;
                break;
            case 3:
                Block.sprite = BossRoomMark;
                break;
            case 4:
                Block.sprite = StartRoomMark;
                break;
        }
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


    /// <summary>
    /// 确保房间和玩家
    /// </summary>
    void EnsurePlayerAndRoom()
    {
        //获取关联父房间
        if (ParentRoom == null) {
            MapCreater map = MapCreater.StaticMap;
            ParentRoom = map.RRoom[MiniMapBlockIndex];
        }
        //获取玩家
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }


    /// <summary>
    /// 传送按钮事件
    /// </summary>
    public void TPButtonEvent()
    {
        //点击后取消选择状态
        EventSystem.current.SetSelectedGameObject(null);
        //确保父房间和玩家
        EnsurePlayerAndRoom();
        //玩家或房间为空时结束
        if (ParentRoom == null || player == null) { return; }

        //确认玩家是否处于战斗状态
        MapCreater map = MapCreater.StaticMap;
        if (map.RRoom[player.NowRoom].isClear > 0)
        {
            UIGetANewItem.UI.JustSaySth("正在对战呢！", "不可以分心传送！");
            return;
        }
        //正在传送时不可以传送
        if (player.isTP) {
            return; 
        }
        //传送
        player.TP(MiniMapBlockIndex);
    }


}
