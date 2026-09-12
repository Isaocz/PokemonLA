using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class IteamPickUp : Item
{
    
    //声明一个布尔型变量，表示物品是否被触发
    public bool isLunch = false;
    protected bool CanBePickUp = false;
    protected Vector3 StartPosition;
    protected Vector3 Direction;
    protected float LunchSpeed = 0.08f;



    //声明一个变量,代表物品移动的目标
    public GameObject targer;


    public bool BanLunchUp;
    public bool BanLunchDown;
    public bool BanLunchRight;
    public bool BanLunchLeft;
    int BanCount;

    public MiniMapBlock.MiniMapBlockMarkType MiniMapBlockMark;



    // Start is called before the first frame update
    void Start()
    {
        //获取玩家
        targer = GameObject.FindObjectOfType<PlayerControler>().gameObject;
        StartPosition = gameObject.transform.position;
        int r = Random.Range(0,360);
        while ((BanLunchUp && r > 45 && r <= 135) || (BanLunchLeft && r > 135 && r <= 225) || (BanLunchDown && r > 225 && r <= 315) || (BanLunchRight &&( r > 315 || r <= 45)))
        {
            r = Random.Range(0, 360);
            BanCount++;
            if (BanCount >= 20)
            {
                break;
            }
        }
        Direction = ( Quaternion.AngleAxis(r,Vector3.forward) * Vector3.right ).normalized;
    }

    void Update()
    {
        if (targer == null) {
            if (GameObject.FindObjectOfType<PlayerControler>() == null)
            {
                Destroy(gameObject);
            }
            else
            {
                targer = GameObject.FindObjectOfType<PlayerControler>().gameObject;
            }
        }
    }

    public void LunchItem()
    {
        //Debug.Log((transform.position.x));
        //Debug.Log((transform.position.x + 15.0f));
        //Debug.Log(((transform.position.x + 15.0f) / 30));
        //Debug.Log((((transform.position.x + 15.0f) / 30) * 30.0f));
        //gameObject.transform.position = new Vector3(  Mathf.Clamp(gameObject.transform.position.x + LunchSpeed * Direction.x , (((StartPosition.x+15.0f) / (int)30 )*30.0f) - 12.0f, (((StartPosition.x + 15.0f) / (int)30)*30.0f) + 12.0f), Mathf.Clamp(gameObject.transform.position.y + LunchSpeed * Direction.y, (((StartPosition.y + 12.0f) / (int)24)*24.0f) - 8.5f, (((StartPosition.y + 12.0f) / (int)24)*24.0f) + 8.5f) ,0);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + LunchSpeed * Direction.x, gameObject.transform.position.y + LunchSpeed * Direction.y, 0);
        LunchSpeed -= 0.0012f;
        if(LunchSpeed <= 0.002f)
        {
            CanBePickUp = true;
            LunchSpeed = 0.08f;
        }
    } 

    protected void DoNotLunch()
    {
        LunchSpeed -= 0.0012f;
        if (LunchSpeed <= 0.002f)
        {
            CanBePickUp = true;
            LunchSpeed = 0.08f;
        }
    }

    /// <summary>
    /// 道具被拾取事件
    /// </summary>
    protected virtual void PickUpEvent()
    {
        ItemPickUpSE();
    }

    /// <summary>
    /// 可拾取道具的音效
    /// </summary>
    protected void ItemPickUpSE()
    {
        //判空
        if (AudioManager.Instance == null) { return; }
        
        //薄荷球会预打开
        {
            MintBall mb = transform.GetComponent<MintBall>();
        }
        AudioManager.CommonBasicSFXList c = AudioManager.CommonBasicSFXList.GetNormalItem;
        //树果道具音效 树果类可拾取道具
        if (ItemTypeTag.Contains(ItemTagEunm.树果类))
        {
            c = AudioManager.CommonBasicSFXList.GetBerryItem;
        }
        //宝宝道具音效 宝宝类可拾取道具（暂时不存在）
        else if (ItemTypeTag.Contains(ItemTagEunm.宝宝类))
        {
            c = AudioManager.CommonBasicSFXList.GetBabyItem;
        }
        //大道具音效/精灵球打开音效 技能类道具或一次性道具或精灵球
        else
        {
            HeartScale hs = transform.GetComponent<HeartScale>();
            PPUp pp = transform.GetComponent<PPUp>(); ;
            SeedofMastery sm = transform.GetComponent<SeedofMastery>(); ;
            SpaceItem si = transform.GetComponent<SpaceItem>();
            PokemonBall pb = transform.GetComponent<PokemonBall>();
            SkillBall sb = transform.GetComponent<SkillBall>();
            if (new Component[] { hs, pp, sm, si }.Any(x => x != null))
            {
                c = AudioManager.CommonBasicSFXList.GetSpeaceItem;
            }
            else if (new Component[] { pb, sb }.Any(x => x != null))
            {
                c = AudioManager.CommonBasicSFXList.NULL;
            }
        }
        if (c == AudioManager.CommonBasicSFXList.NULL) { return; }

        if (AudioManager.Instance != null){ AudioManager.Instance.CommonBasicSFXPlayer.Play( c , transform.position); }
    }

    /// <summary>
    /// 可拾取道具的音效
    /// </summary>
    protected void ItemDropSE()
    {
        //判空
        if (AudioManager.Instance == null) { return; }
        AudioManager.CommonBasicSFXList c = AudioManager.CommonBasicSFXList.ItemDrop;
        AudioManager.Instance.CommonBasicSFXPlayer.Play(c, transform.position); 
    }

    /// <summary>
    /// 精灵球开启的音效
    /// </summary>
    protected void BallOpenSE()
    {
        //判空
        if (AudioManager.Instance == null) { return; }
        AudioManager.CommonBasicSFXList c = AudioManager.CommonBasicSFXList.BallOpen;
        AudioManager.Instance.CommonBasicSFXPlayer.Play(c, transform.position);
    }
}