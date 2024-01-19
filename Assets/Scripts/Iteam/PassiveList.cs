using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveList : MonoBehaviour
{

    public Sprite[] SpritesList;

    public static List<int> PassiveItemBlackList01 = new List<int>();


    public int GetARandomItemIndex(int CallCount , PassiveItemPool.ItemPool Pool)
    {
        int RandomItemIndex = Random.Range(0, SpritesList.Length);
        if (!(Pool.BlackList.Count == 0 && Pool.WriteList.Count == 0))
        {
            if (Pool.WriteList.Count != 0) { RandomItemIndex = Pool.WriteList[Random.Range(0, Pool.WriteList.Count)]; }
            if (Pool.BlackList.Count != 0) 
            {
                int c = 0;
                while (Pool.BlackList.Contains(RandomItemIndex))
                {
                    RandomItemIndex = Random.Range(0, SpritesList.Length);
                    c++;
                    if (c >= 100) { break; }
                }
            }
        }

        if (CallCount >= 30) { return RandomItemIndex; }
        if (PassiveItemBlackList01.Exists(t => t == RandomItemIndex))
        {
            return GetARandomItemIndex(CallCount+1 , Pool);
        }
        else
        {
            PassiveItemBlackList01.Add(RandomItemIndex);
            return RandomItemIndex;
        }
    }
}
