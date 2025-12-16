using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMaskCotrollor : MonoBehaviour
{
    public static NightMaskCotrollor GlobalNight;

    /// <summary>
    /// 昼夜切换的速度，需要NightChangeSpeed秒从白天切换为黑夜
    /// </summary>
    public static float NightChangeSpeed = 2.0f;

    /// <summary>
    /// 是否是夜晚
    /// </summary>
    public bool isNight;

    /// <summary>
    /// 夜晚遮罩变化
    /// </summary>
    bool isNightMaskChange = false;

    /// <summary>
    /// 夜晚计时器
    /// </summary>
    float NightTimer;

    /// <summary>
    /// 玩家
    /// </summary>
    PlayerControler player;

    /// <summary>
    /// 夜晚遮罩
    /// </summary>
    public SpriteRenderer NightMask;


    private void Awake()
    {
        GlobalNight = this;
        player = FindObjectOfType<PlayerControler>();
    }



    // Update is called once per frame
    void Update()
    {
        CheckPlayer();

        if (player != null)
        {
            //进入商店PC隐藏昼夜效果；
            if (player.NowRoom == MapCreater.StaticMap.PCRoomPoint || player.NowRoom == MapCreater.StaticMap.StoreRoomPoint)
            {
                if (transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    InPC();
                }
            }
            else
            {
                if (!transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    transform.GetChild(0).gameObject.SetActive(true);

                }
            }

            //=================================================改变昼夜时的滤镜颜色变化===============================================

            if (isNightMaskChange) {
                if (isNight)
                {
                    NightMask.color = NightMask.color + new Color(0, 0, 0, Time.deltaTime / NightChangeSpeed);
                    if (NightMask.color.a >= 1) { isNightMaskChange = false; }
                }
                else
                {
                    NightMask.color = NightMask.color - new Color(0, 0, 0, Time.deltaTime / NightChangeSpeed);
                    if (NightMask.color.a <= 0) { isNightMaskChange = false; }
                }
            }

            //=================================================改变昼夜时的滤镜颜色变化===============================================



        }
    }


    /// <summary>
    /// 检查玩家是否在PC或商店中
    /// </summary>
    void InPC()
    {
        if (player.NowRoom == MapCreater.StaticMap.PCRoomPoint || player.NowRoom == MapCreater.StaticMap.StoreRoomPoint)
        {
            NightMask.gameObject.SetActive(false);
        }
    }




    /// <summary>
    /// 切换为夜晚
    /// </summary>
    public void ChangeToNight()
    {
        if (!isNight)
        {
            isNight = true;
            isNightMaskChange = true;
        }
    }


    /// <summary>
    /// 切换为白天
    /// </summary>
    public void ChangeToDay()
    {
        if (isNight)
        {
            isNight = false;
            isNightMaskChange = true;
        }
    }


    /// <summary>
    /// 获取玩家
    /// </summary>
    void CheckPlayer()
    {
        if (player == null && FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber >= 0)
        {
            player = FindObjectOfType<PlayerControler>();
        }
    }

}
