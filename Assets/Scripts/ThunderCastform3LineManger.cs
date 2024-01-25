using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCastform3LineManger : MonoBehaviour
{
    public Empty ParentEmpty;
    float SetActiveTimer;
    int ChildIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).position = transform.position + new Vector3(-15.5f + j , ( (j < 10 || j > 19) ? (-8 + i):(8 - i)) ,0);
                transform.GetChild(i).GetChild(j).GetComponent<ThunderCastform>().empty = ParentEmpty;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis)
        {
            SetActiveTimer += Time.deltaTime;
        }
        if ((ParentEmpty.isSleepDone || ParentEmpty.isFearDone) && SetActiveTimer < 5.95f)
        {
            SetActiveTimer = 5.95f;
        }
        if (ChildIndex <= 16)
        {
            
            if (SetActiveTimer >= ((ParentEmpty.isSilence ? 0.45f : 0.3f)) )
            {
                SetActiveTimer = 0;
                transform.GetChild(ChildIndex).gameObject.SetActive(true);
                if (ParentEmpty.isEmptyConfusionDone)
                {
                    for (int i = 0; i < transform.GetChild(ChildIndex).childCount; i++)
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.7f)
                        {
                            transform.GetChild(ChildIndex).GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                ChildIndex++;
            }
        }
        if (SetActiveTimer > 10)
        {
            Destroy(gameObject);
        }
    }
}
