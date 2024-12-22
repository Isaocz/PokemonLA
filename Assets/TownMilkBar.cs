using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMilkBar : TownHouse
{

    /// <summary>
    /// �ذ�
    /// </summary>
    public SpriteRenderer Floor;
    public Sprite[] FloorList;
    /// <summary>
    /// ���ذ� 0:����ţ�̵ذ�
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFloor(int Index)
    {
        Floor.sprite = FloorList[Index];
    }


    /// <summary>
    /// ǽ
    /// </summary>
    public SpriteRenderer Wall;
    public Sprite[] WallList;
    /// <summary>
    /// ��ǽ�� 0:�ư�ǽ
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchWall(int Index)
    {
        Wall.sprite = WallList[Index];
    }

    /// <summary>
    /// �ſڵĵ�̺
    /// </summary>
    public SpriteRenderer Mat;
    public Sprite[] MatList;
    /// <summary>
    /// ����̺ 0:��ɫ�����̺
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchMat(int Index)
    {
        Mat.sprite = MatList[Index];
    }



    /// <summary>
    /// ���ŵ�ɳ��
    /// </summary>
    public SpriteRenderer SofaH;
    public Sprite[] SofaHList;
    /// <summary>
    /// ���ŵ�ɳ��
    /// </summary>
    public SpriteRenderer SofaV;
    public Sprite[] SofaVList;
    /// <summary>
    /// ��ɳ�� 0:�ٱ��ɳ��
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofa(int Index)
    {
        SofaH.sprite = SofaHList[Index];
        SofaV.sprite = SofaVList[Index];
    }

    /// <summary>
    /// ɳ���輸
    /// </summary>
    public SpriteRenderer SofaTable;
    public Sprite[] SofaTableList;
    /// <summary>
    /// ��ɳ���輸 0:�����輸
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofaTable(int Index)
    {
        SofaTable.sprite = SofaTableList[Index];
    }

    /// <summary>
    /// ɳ����ͼ
    /// </summary>
    public SpriteRenderer SofaMat;
    public Sprite[] SofaMatList;
    /// <summary>
    /// ��ɳ����ͼ 0:Ƥ�����̺
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchSofaMat(int Index)
    {
        SofaMat.sprite = SofaMatList[Index];
    }



    /// <summary>
    /// �������ϰ�
    /// </summary>
    public GameObject DrinkBar;
    /// <summary>
    /// ���ú������ϰ�
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDrinkBar(bool b)
    {
        DrinkBar.SetActive(b);
    }



    /// <summary>
    /// ��̨����
    /// </summary>
    public GameObject Refrigerator;
    /// <summary>
    /// ���ð�̨����
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchRefrigerator(bool b)
    {
        Refrigerator.SetActive(b);
    }


    /// <summary>
    /// ��ŷ·�칫��
    /// </summary>
    public GameObject RioluOffice;
    /// <summary>
    /// ����̨
    /// </summary>
    public GameObject Bar;
    /// <summary>
    /// ������ŷ·�Ƿ������̹�
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchRioluOffice(bool b)
    {
        RioluOffice.SetActive(b);
        Bar.GetComponent<Animator>().SetBool("IsRiolu" , b);
    }


    /// <summary>
    /// ��̨����
    /// </summary>
    public GameObject BarPlus;
    /// <summary>
    /// ���Ĺ�̨
    /// </summary>
    public GameObject CakeBar;
    /// <summary>
    /// ���ð�̨����
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchBarPlus(bool b)
    {
        BarPlus.SetActive(b);
        CakeBar.SetActive(b);
    }




    /// <summary>
    /// ��̨��
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
    /// ����̨�� 0:��������
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
    /// ��¯
    /// </summary>
    public GameObject FirePlace;
    /// <summary>
    /// ��¯�����
    /// </summary>
    public SpriteRenderer FirePlaceOutside;
    public Sprite[] FirePlaceSpriteList;
    /// <summary>
    /// ��¯���ڲ�
    /// </summary>
    public SpriteRenderer FirePlaceInside;
    public Sprite[] FirePlaceInsideList;
    /// <summary>
    /// ��¯�еĲ��
    /// </summary>
    public SpriteRenderer FirePlaceWood;
    public Sprite[] FirePlaceWoodList;
    /// <summary>
    /// ��¯���������
    /// </summary>
    public SpriteRenderer FirePlaceOutsideMask;
    public Sprite[] FirePlaceOutsideMaskList;
    /// <summary>
    /// ��¯���ڲ�����
    /// </summary>
    public SpriteRenderer FirePlaceInsideMask;
    public Sprite[] FirePlaceInsideMaskList;
    /// <summary>
    /// ��¯����������
    /// </summary>
    public SpriteRenderer FirePlaceInsideIronMask;
    public Sprite[] FirePlaceInsideIronMaskList;



    /// <summary>
    /// ����¯ 0:û�л�¯ 1����������¯
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
    /// ��������
    /// </summary>
    public GameObject Chairs;
    /// <summary>
    /// ���ò�������
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchChairs(bool b)
    {
        Chairs.SetActive(b);
    }


    /// <summary>
    /// ��ñ��
    /// </summary>
    public GameObject CoatHanger;
    /// <summary>
    /// ������ñ��
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchCoatHanger(bool b)
    {
        CoatHanger.SetActive(b);
    }


    /// <summary>
    /// ����
    /// </summary>
    public GameObject Darts;
    /// <summary>
    /// ���÷���
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDarts(bool b)
    {
        Darts.SetActive(b);
    }



    /// <summary>
    /// ���
    /// </summary>
    public SpriteRenderer Books;
    public Sprite[] BooksList;
    /// <summary>
    /// ����� 0:��ͨ���
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchBooks(int Index)
    {
        Books.sprite = BooksList[Index];
    }

}
