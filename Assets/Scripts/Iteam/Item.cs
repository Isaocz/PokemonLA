using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemTagEunm
    {
        ������ = 1,
        ������ = 2,
    }
    public Sprite StoreImage;
    public int Price;
    public string ItemDescribe;
    public string ItemName;
    public int ItemTag;
    public Item.ItemTagEunm[] ItemTypeTag;
    //Tag1: ����
}
