using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCastformManger : MonoBehaviour
{
    public Empty ParentEmpty;
    float SetActiveTimer;
    int ChildIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right).normalized;
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
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
            if (SetActiveTimer >= ((ParentEmpty.isSilence ? 0.4f : 0.2f)))
            {
                SetActiveTimer = 0;
                transform.GetChild(ChildIndex).gameObject.SetActive(true);
                if (ParentEmpty.isEmptyConfusionDone) {
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
        if (SetActiveTimer > 6)
        {
            Destroy(gameObject);
        }
    }

}
