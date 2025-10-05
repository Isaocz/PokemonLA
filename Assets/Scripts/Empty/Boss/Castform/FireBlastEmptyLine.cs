using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastEmptyLine : MonoBehaviour
{

    public Empty ParentEmpty;
    public FireBlastEmptyFire ChildFire;
    float BornTimer;
    float TurnTimer;
    float BornTime = 0.1f;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis)
        {
            BornTimer += Time.deltaTime;
            TurnTimer += Time.deltaTime;
        }
        if ((ParentEmpty.isSleepDone || ParentEmpty.isFearDone || ParentEmpty.isDie) && TurnTimer < 12.95f)
        {
            TurnTimer = 12.95f;
        }

        if (TurnTimer <= 1.5f) {
            if (BornTimer > 0.1f)
            {
                BornTimer = 0;
                FireBlastEmptyFire f = Instantiate(ChildFire, transform.position, Quaternion.Euler(-90, 0, 0));
                f.LaunchNotForce(Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right, 4.5f);
                f.empty = ParentEmpty;
            }
        }
        else if (TurnTimer > 1.5f && TurnTimer <= 13f)
        {
            BornTime = Mathf.Clamp(BornTime + 0.075f * Time.deltaTime , 0 ,0.32f);
            if (BornTimer > (ParentEmpty.isEmptyConfusionDone ? 0.18f : BornTime))
            {
                BornTimer = 0;
                if (!ParentEmpty.isEmptyConfusionDone || (ParentEmpty.isEmptyConfusionDone && Random.Range(0.0f,1.0f)>0.5f)) {
                    FireBlastEmptyFire f = Instantiate(ChildFire, transform.position, Quaternion.Euler(-90, 0, 0));
                    f.LaunchNotForce(Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right, 4.5f);
                    f.empty = ParentEmpty;
                }
            }
        }
        else if (TurnTimer > 14f)
        {
            Destroy(gameObject);
        }
    }
}
