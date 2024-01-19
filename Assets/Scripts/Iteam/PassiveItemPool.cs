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

    public static ItemPool UltraBall
    = new ItemPool
    {
        WriteList = new List<int> { },
        BlackList = new List<int> { 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, }
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
        WriteList = new List<int> { 30,31,32,34,69,70,71,124,125,126,127,128,129,130,131,132, },
        BlackList = new List<int> { }
    };

}
