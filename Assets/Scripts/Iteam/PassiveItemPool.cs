using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemPool : MonoBehaviour
{
    public struct ItemPool
    {
        public List<int> WriteList;
        public List<int> BlackList;

    }

    public enum PoolType
    {
        All,
        UltraBall,
        Mint,
        Baby,
        Store,
    }

    public static ItemPool RetunPool(PoolType type)
    {
        ItemPool OutPut = All;
        switch (type)
        {
            case PoolType.All:
                OutPut = All;
                break;
            case PoolType.UltraBall:
                OutPut = UltraBall;
                break;
            case PoolType.Mint:
                OutPut = Mint;
                break;
            case PoolType.Baby:
                OutPut = Baby;
                break;
        }
        return OutPut;
    }

    public static ItemPool All
    = new ItemPool
    {
        WriteList = new List<int> { },
        BlackList = new List<int> { }
    };

    public static ItemPool ExceptMewItem
    = new ItemPool
    {
        WriteList = new List<int> { },
        BlackList = new List<int> { 55 , 56 , 57 , 58 }
    };

    public static ItemPool UltraBall
    = new ItemPool
    {
        WriteList = new List<int> { },
        BlackList = new List<int> { 55, 56, 57, 58, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, }
    };

    public static ItemPool Mint
    = new ItemPool
    {
        WriteList = new List<int> { 34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54, },
        BlackList = new List<int> { }
    };

    public static ItemPool Baby
    = new ItemPool
    {
        WriteList = new List<int> { 30,31,32,33,69,70,71,124,125,126,127,128,129,130,131,132, },
        BlackList = new List<int> { }
    };

    public static ItemPool Store
    = new ItemPool
    {
        WriteList = new List<int> { 1  , 4  , 5  , 6  , 7  , 8  , 10 , 11 , 12 , 13 ,
                                    14 , 16 , 17 , 18 , 19 , 20 , 21 , 22 , 23 , 25 ,
                                    28 , 59 , 60 , 61 , 63 , 79 , 80 , 81 , 82 , 83 ,
                                    84 , 85 , 86 , 89 , 90 , 98 , 99 , 100, 101, 102,
                                    105, 112, 113, 114, 115, 117, 118, 119, 120, 121,
                                    122, 123, 135, 136, 137, 138,
        },
        BlackList = new List<int> { }
    };

}
