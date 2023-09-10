using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCastformCircleManger : MonoBehaviour
{
    public Empty ParentEmpty;
    float SetActiveTimer;
    int ChildIndex;
    int[] ChildNum = new int[] { 6, 8, 12, 16, 24 };

    // Start is called before the first frame update
    void Start()
    {
        transform.position += (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right).normalized;
        for (int i = 0; i < transform.childCount; i++)
        {
            int Rotation = 360 / ChildNum[i];
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).position = Quaternion.AngleAxis(j * Rotation , Vector3.forward) * Vector3.right * (1.3f + 1.3f*i);
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
        if (ChildIndex <= 4)
        {

            if (SetActiveTimer >= ((ParentEmpty.isSilence ? 0.55f : 0.4f)))
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
        if (SetActiveTimer > 6)
        {
            Destroy(gameObject);
        }
    }
}
