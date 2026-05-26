using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileHealEffectRingParent : MonoBehaviour
{
    static float Interval = 0.75f;

    public List<GameObject> ChildRingList = new List<GameObject> { };


    public bool isOver = true;


    private void Start()
    {
        StartCoroutine(LaunchRing());
    }

    IEnumerator LaunchRing()
    {
        while (isOver)
        {
            for (int i = 0; i < ChildRingList.Count; i++)
            {
                if (!ChildRingList[i].activeInHierarchy) {
                    ChildRingList[i].SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(Interval);
        }
    }
}

