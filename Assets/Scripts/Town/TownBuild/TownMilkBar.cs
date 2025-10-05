using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMilkBar : TownHouse
{

    /// <summary>
    /// 地板
    /// </summary>
    public SpriteRenderer Floor;
    public Sprite[] FloorList;
    /// <summary>
    /// 换地板 0:哞哞牛奶地板
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
    /// 换墙壁 0:黄白墙
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchWall(int Index)
    {
        Wall.sprite = WallList[Index];
    }

    /// <summary>
    /// 门口的地毯
    /// </summary>
    public SpriteRenderer Mat;
    public Sprite[] MatList;
    /// <summary>
    /// 换地毯 0:红色哞哞地毯
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchMat(int Index)
    {
        Mat.sprite = MatList[Index];
    }



    /// <summary>
    /// 横着的沙发
    /// </summary>
    public SpriteRenderer SofaH;
    public Sprite[] SofaHList;
    /// <summary>
    /// 竖着的沙发
    /// </summary>
    public SpriteRenderer SofaV;
    public Sprite[] SofaVList;
    /// <summary>
    /// 换沙发 0:百变怪沙发
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofa(int Index)
    {
        SofaH.sprite = SofaHList[Index];
        SofaV.sprite = SofaVList[Index];
    }

    /// <summary>
    /// 沙发茶几
    /// </summary>
    public SpriteRenderer SofaTable;
    public Sprite[] SofaTableList;
    /// <summary>
    /// 换沙发茶几 0:玻璃茶几
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofaTable(int Index)
    {
        SofaTable.sprite = SofaTableList[Index];
    }

    /// <summary>
    /// 沙发地图
    /// </summary>
    public SpriteRenderer SofaMat;
    public Sprite[] SofaMatList;
    /// <summary>
    /// 换沙发地图 0:皮卡丘地毯
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofaMat(int Index)
    {
        SofaMat.sprite = SofaMatList[Index];
    }



    /// <summary>
    /// 壶壶饮料吧
    /// </summary>
    public GameObject DrinkBar;
    /// <summary>
    /// 设置壶壶饮料吧
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDrinkBar(bool b)
    {
        DrinkBar.SetActive(b);
    }



    /// <summary>
    /// 吧台冰箱
    /// </summary>
    public GameObject Refrigerator;
    /// <summary>
    /// 设置吧台冰箱
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchRefrigerator(bool b)
    {
        Refrigerator.SetActive(b);
    }


    /// <summary>
    /// 利欧路办公区
    /// </summary>
    public GameObject RioluOffice;
    /// <summary>
    /// 长吧台
    /// </summary>
    public GameObject Bar;
    /// <summary>
    /// 设置利欧路是否来到奶馆
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchRioluOffice(bool b)
    {
        RioluOffice.SetActive(b);
        Bar.GetComponent<Animator>().SetBool("IsRiolu" , b);
    }


    /// <summary>
    /// 吧台升级
    /// </summary>
    public GameObject BarPlus;
    /// <summary>
    /// 点心柜台
    /// </summary>
    public GameObject CakeBar;
    /// <summary>
    /// 设置吧台升级
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchBarPlus(bool b)
    {
        BarPlus.SetActive(b);
        CakeBar.SetActive(b);
    }




    /// <summary>
    /// 吧台椅
    /// </summary>
    public SpriteRenderer BarChairs00;
    public Sprite[] BarChairs0List ;
    public SpriteRenderer BarChairs01;
    public Sprite[] BarChairs1List;
    public SpriteRenderer BarChairs02;
    public Sprite[] BarChairs2List;
    public SpriteRenderer BarChairs03;
    public Sprite[] BarChairs3List;
    public SpriteRenderer BarChairs04;
    public Sprite[] BarChairs4List;
    public SpriteRenderer BarChairs05;
    public Sprite[] BarChairs5List;
    public SpriteRenderer BarChairs06;
    public Sprite[] BarChairs6List;
    public SpriteRenderer BarChairs07;
    public Sprite[] BarChairs7List;


    /// <summary>
    /// 换吧台椅 0:精灵球风格
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchBarChairs(int Index)
    {
        BarChairs00.sprite = BarChairs0List[Index];
        BarChairs01.sprite = BarChairs1List[Index];
        BarChairs02.sprite = BarChairs2List[Index];
        BarChairs03.sprite = BarChairs3List[Index];
        BarChairs04.sprite = BarChairs4List[Index];
        BarChairs05.sprite = BarChairs5List[Index];
        BarChairs06.sprite = BarChairs6List[Index];
        BarChairs07.sprite = BarChairs7List[Index];
    }


    /// <summary>
    /// 火炉
    /// </summary>
    public GameObject FirePlace;
    /// <summary>
    /// 火炉的外观
    /// </summary>
    public SpriteRenderer FirePlaceOutside;
    public Sprite[] FirePlaceSpriteList;
    /// <summary>
    /// 火炉的内部
    /// </summary>
    public SpriteRenderer FirePlaceInside;
    public Sprite[] FirePlaceInsideList;
    /// <summary>
    /// 火炉中的柴火
    /// </summary>
    public SpriteRenderer FirePlaceWood;
    public Sprite[] FirePlaceWoodList;
    /// <summary>
    /// 火炉的外观遮罩
    /// </summary>
    public SpriteRenderer FirePlaceOutsideMask;
    public Sprite[] FirePlaceOutsideMaskList;
    /// <summary>
    /// 火炉的内部遮罩
    /// </summary>
    public SpriteRenderer FirePlaceInsideMask;
    public Sprite[] FirePlaceInsideMaskList;
    /// <summary>
    /// 火炉的铁甲遮罩
    /// </summary>
    public SpriteRenderer FirePlaceInsideIronMask;
    public Sprite[] FirePlaceInsideIronMaskList;



    /// <summary>
    /// 换火炉 0:没有火炉 1：火雉鸡火炉
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFirePlace(int Index)
    {
        if (Index == 0)
        {
            FirePlace.SetActive(false);
        }
        else
        {
            FirePlace.SetActive(true);
            FirePlaceOutside.sprite = FirePlaceSpriteList[Index - 1];
            FirePlaceInside.sprite = FirePlaceInsideList[Index - 1];
            FirePlaceWood.sprite = FirePlaceWoodList[Index - 1];
            FirePlaceOutsideMask.sprite = FirePlaceOutsideMaskList[Index - 1];
            FirePlaceInsideMask.sprite = FirePlaceInsideMaskList[Index - 1];
            FirePlaceInsideIronMask.sprite = FirePlaceInsideIronMaskList[Index - 1];

        }
    }


    /// <summary>
    /// 餐厅椅子
    /// </summary>
    public GameObject Chairs;
    /// <summary>
    /// 设置餐厅椅子
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchChairs(bool b)
    {
        Chairs.SetActive(b);
    }


    /// <summary>
    /// 衣帽架
    /// </summary>
    public GameObject CoatHanger;
    /// <summary>
    /// 设置衣帽架
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchCoatHanger(bool b)
    {
        CoatHanger.SetActive(b);
    }


    /// <summary>
    /// 飞镖
    /// </summary>
    public GameObject Darts;
    /// <summary>
    /// 设置飞镖
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDarts(bool b)
    {
        Darts.SetActive(b);
    }



    /// <summary>
    /// 书架
    /// </summary>
    public SpriteRenderer Books;
    public Sprite[] BooksList;
    /// <summary>
    /// 换书架 0:普通书架
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchBooks(int Index)
    {
        Books.sprite = BooksList[Index];
    }

}
