using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveList : MonoBehaviour
{

    public Sprite[] SpritesList;

    public static List<int> PassiveItemBlackList01 = new List<int>();


    public int GetARandomItemIndex(int CallCount)
    {
        int RandomItemIndex = Random.Range(0, SpritesList.Length);
        if (CallCount >= 30) { return RandomItemIndex; }
        if (PassiveItemBlackList01.Exists(t => t == RandomItemIndex))
        {
            return GetARandomItemIndex(CallCount+1);
        }
        else
        {
            PassiveItemBlackList01.Add(RandomItemIndex);
            return RandomItemIndex;
        }
    }
}
