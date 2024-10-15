using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPoliceStation : TownHouse
{
    /// <summary>
    /// 地板
    /// </summary>
    public SpriteRenderer Floor;
    public Sprite[] FloorList;
    /// <summary>
    /// 换地板 0:原木地板
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFloor(int Index)
    {
        Floor.sprite = FloorList[Index];
    }
    /// <summary>
    /// 墙
    /// </summary>
    public SpriteRenderer Wall;
    public Sprite[] WallList;
    /// <summary>
    /// 换墙壁 0:原木墙壁
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchWall(int Index)
    {
        Wall.sprite = WallList[Index];
    }



    /// <summary>
    /// 左边的地毯
    /// </summary>
    public SpriteRenderer MatL;
    public Sprite[] MatLList;
    /// <summary>
    /// 右边的地毯
    /// </summary>
    public SpriteRenderer MatR;
    public Sprite[] MatRList;
    /// <summary>
    /// 楼梯处地毯
    /// </summary>
    public SpriteRenderer FloorMat;
    public Sprite[] FloorMatList;
    /// <summary>
    /// 中央的大地毯
    /// </summary>
    public SpriteRenderer BigMat;
    public Sprite[] BigMatList;
    /// <summary>
    /// 换地毯 0:冒险者风格棕色
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchMat(int Index)
    {
        MatL.sprite = MatLList[Index];
        MatR.sprite = MatRList[Index];
        FloorMat.sprite = FloorMatList[Index];
        BigMat.sprite = BigMatList[Index];
    }



    /// <summary>
    /// 办公桌
    /// </summary>
    public GameObject OfficeTable;
    /// <summary>
    /// 办公椅
    /// </summary>
    public GameObject OfficeChair;
    /// <summary>
    /// 捷拉奥拉的吉他
    /// </summary>
    public GameObject Gitar;
    /// <summary>
    /// 捷拉奥拉的衣服架
    /// </summary>
    public GameObject Coat;
    /// <summary>
    /// 捷拉奥拉的玩具
    /// </summary>
    public GameObject Toy;
    /// <summary>
    /// 捷拉奥拉在不在办公室 0捷拉奥拉还不在镇上 1捷拉奥拉来到镇上但不在 2捷拉奥拉在
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchZeraoraOffice(int Index)
    {
        switch (Index)
        {
            case 0:
                OfficeTable.GetComponent<Animator>().SetBool("Zeraora", false);
                OfficeChair.SetActive(true);
                Gitar.SetActive(false);
                Coat.SetActive(false);
                Toy.SetActive(false);
                break;
            case 1:
                OfficeTable.GetComponent<Animator>().SetBool("Zeraora", false);
                OfficeChair.SetActive(true);
                Gitar.SetActive(true);
                Coat.SetActive(true);
                Toy.SetActive(false);
                break;
            case 2:
                OfficeTable.GetComponent<Animator>().SetBool("Zeraora" , true);
                OfficeChair.SetActive(false);
                Gitar.SetActive(true);
                Coat.SetActive(true);
                Toy.SetActive(true);
                break;
        }
    }



    /// <summary>
    /// 壁炉
    /// </summary>
    public SpriteRenderer Fireplace01;
    public Sprite[] Fireplace01List;
    /// <summary>
    /// 壁炉的内部
    /// </summary>
    public SpriteRenderer FireplaceInside;
    public Sprite[] FireplaceInsideList;
    /// <summary>
    /// 壁炉内部的发光部分
    /// </summary>
    public SpriteRenderer FireplaceInsideLight;
    public Sprite[] FireplaceInsideLightList;
    /// <summary>
    /// 壁炉下面的石头地板
    /// </summary>
    public SpriteRenderer FireplaceRockFloor;
    public Sprite[] FireplaceRockFloorList;
    /// <summary>
    /// 换火炉 0:石头火炉
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFireplace(int Index)
    {
        Fireplace01.sprite = Fireplace01List[Index];
        FireplaceInside.sprite = FireplaceInsideList[Index];
        FireplaceInsideLight.sprite = FireplaceInsideLightList[Index];
        FireplaceRockFloor.sprite = FireplaceRockFloorList[Index];
    }


    /// <summary>
    /// 日历
    /// </summary>
    public SpriteRenderer DatePoster;
    public Sprite[] DatePosterList;
    /// <summary>
    /// 换日历 -1;无日历 0:皮卡丘
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDatePoster(int Index)
    {
        if (Index == -1) { DatePoster.gameObject.SetActive(false); }
        else             { DatePoster.gameObject.SetActive(true); DatePoster.sprite = DatePosterList[Index]; }
    }



    /// <summary>
    /// 文件柜
    /// </summary>
    public SpriteRenderer FileBox;
    public Sprite[] FileBoxList;
    /// <summary>
    /// 换文件柜 0:铁皮
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFileBoxSprite(int Index)
    {
        FileBox.sprite = FileBoxList[Index];
    }


    /// <summary>
    /// 沙发
    /// </summary>
    public SpriteRenderer Sofa;
    /// <summary>
    /// 沙发扶手
    /// </summary>
    public SpriteRenderer Sofa02;
    public Sprite[] SofatList;
    public Sprite[] Sofa02tList;
    /// <summary>
    /// 换沙发 0:皮卡丘 1：百变怪
    /// </summary>
    public void SwitchSofaSprite( int Index) { 
        Sofa.sprite = SofatList[Index];
        Sofa02.sprite = Sofa02tList[Index]; 
    }


    /// <summary>
    /// 路卡利欧的花盆
    /// </summary>
    public GameObject LucarioFlower;
    /// <summary>
    /// 换路卡利欧的花盆
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchLucarioFlower(bool b)
    {
        LucarioFlower.SetActive(b);
    }






}
