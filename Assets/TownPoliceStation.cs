using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPoliceStation : TownHouse
{
    /// <summary>
    /// �ذ�
    /// </summary>
    public SpriteRenderer Floor;
    public Sprite[] FloorList;
    /// <summary>
    /// ���ذ� 0:ԭľ�ذ�
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
    /// ��ǽ�� 0:ԭľǽ��
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchWall(int Index)
    {
        Wall.sprite = WallList[Index];
    }



    /// <summary>
    /// ��ߵĵ�̺
    /// </summary>
    public SpriteRenderer MatL;
    public Sprite[] MatLList;
    /// <summary>
    /// �ұߵĵ�̺
    /// </summary>
    public SpriteRenderer MatR;
    public Sprite[] MatRList;
    /// <summary>
    /// ¥�ݴ���̺
    /// </summary>
    public SpriteRenderer FloorMat;
    public Sprite[] FloorMatList;
    /// <summary>
    /// ����Ĵ��̺
    /// </summary>
    public SpriteRenderer BigMat;
    public Sprite[] BigMatList;
    /// <summary>
    /// ����̺ 0:ð���߷����ɫ
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
    /// �칫��
    /// </summary>
    public GameObject OfficeTable;
    /// <summary>
    /// �칫��
    /// </summary>
    public GameObject OfficeChair;
    /// <summary>
    /// ���������ļ���
    /// </summary>
    public GameObject Gitar;
    /// <summary>
    /// �����������·���
    /// </summary>
    public GameObject Coat;
    /// <summary>
    /// �������������
    /// </summary>
    public GameObject Toy;
    /// <summary>
    /// ���������ڲ��ڰ칫�� 0������������������ 1���������������ϵ����� 2����������
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
    /// ��¯
    /// </summary>
    public SpriteRenderer Fireplace01;
    public Sprite[] Fireplace01List;
    /// <summary>
    /// ��¯���ڲ�
    /// </summary>
    public SpriteRenderer FireplaceInside;
    public Sprite[] FireplaceInsideList;
    /// <summary>
    /// ��¯�ڲ��ķ��ⲿ��
    /// </summary>
    public SpriteRenderer FireplaceInsideLight;
    public Sprite[] FireplaceInsideLightList;
    /// <summary>
    /// ��¯�����ʯͷ�ذ�
    /// </summary>
    public SpriteRenderer FireplaceRockFloor;
    public Sprite[] FireplaceRockFloorList;
    /// <summary>
    /// ����¯ 0:ʯͷ��¯
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
    /// ����
    /// </summary>
    public SpriteRenderer DatePoster;
    public Sprite[] DatePosterList;
    /// <summary>
    /// ������ -1;������ 0:Ƥ����
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchDatePoster(int Index)
    {
        if (Index == -1) { DatePoster.gameObject.SetActive(false); }
        else             { DatePoster.gameObject.SetActive(true); DatePoster.sprite = DatePosterList[Index]; }
    }



    /// <summary>
    /// �ļ���
    /// </summary>
    public SpriteRenderer FileBox;
    public Sprite[] FileBoxList;
    /// <summary>
    /// ���ļ��� 0:��Ƥ
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchFileBoxSprite(int Index)
    {
        FileBox.sprite = FileBoxList[Index];
    }


    /// <summary>
    /// ɳ��
    /// </summary>
    public SpriteRenderer Sofa;
    /// <summary>
    /// ɳ������
    /// </summary>
    public SpriteRenderer Sofa02;
    public Sprite[] SofatList;
    public Sprite[] Sofa02tList;
    /// <summary>
    /// ��ɳ�� 0:Ƥ���� 1���ٱ��
    /// </summary>
    public void SwitchSofaSprite( int Index) { 
        Sofa.sprite = SofatList[Index];
        Sofa02.sprite = Sofa02tList[Index]; 
    }


    /// <summary>
    /// ·����ŷ�Ļ���
    /// </summary>
    public GameObject LucarioFlower;
    /// <summary>
    /// ��·����ŷ�Ļ���
    /// </summary>
    /// <param name="Index"></param>
    public void SwitchLucarioFlower(bool b)
    {
        LucarioFlower.SetActive(b);
    }






}
