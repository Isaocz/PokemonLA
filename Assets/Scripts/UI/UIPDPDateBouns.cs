using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPDPDateBouns : MonoBehaviour
{
    public Image PlusBouns;
    public Image MinusBouns;
    public Image MegaPlusBouns;
    public Image MegaMinusBouns;

    public Transform BigStartGroup;
    public Transform SmallStartGroup;

    // Start is called before the first frame update
    void Start()
    {
        GetBounsMark(0);
    }

    public void GetBounsMark( int Bouns )
    {
        int AbilityBouns = Mathf.Clamp(Bouns , -30 , 30);
        bool isPlus = (AbilityBouns > 0);
        for (int i = 0; i < BigStartGroup.transform.childCount; i++)
        {
            GameObject child = BigStartGroup.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        for (int i = 0; i < SmallStartGroup.transform.childCount; i++)
        {
            GameObject child = SmallStartGroup.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        Image smallstart = isPlus ? PlusBouns : MinusBouns;
        Image bigstart = isPlus ? MegaPlusBouns : MegaMinusBouns;

        int BounsCount = Mathf.Abs(AbilityBouns);
        for (; BounsCount >= 10; BounsCount -= 10)
        {
            Instantiate(bigstart, Vector3.zero, Quaternion.identity, BigStartGroup.transform);
        }
        for (; BounsCount > 0; BounsCount --)
        {
            Instantiate(smallstart, Vector3.zero, Quaternion.identity, SmallStartGroup.transform);
        }
    }

}
