using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroPumpEmptyManger : MonoBehaviour
{
    public Empty ParentEmpty;

    EmptyHydroPump HP1;
    EmptyHydroPump HP2;
    EmptyHydroPump HP3;
    EmptyHydroPump HP4;

    public float RotationSpeed;
    float NowRotation;
    float Timer;

    float ConfusionTimer;
    float ConfusionSpeed;

    // Start is called before the first frame update
    void Start()
    {
        HP1 = transform.GetChild(0).GetComponent<EmptyHydroPump>();
        HP2 = transform.GetChild(1).GetComponent<EmptyHydroPump>();
        HP3 = transform.GetChild(2).GetComponent<EmptyHydroPump>();
        HP4 = transform.GetChild(3).GetComponent<EmptyHydroPump>();
        HP1.ParentEmpty = ParentEmpty;
        HP2.ParentEmpty = ParentEmpty;
        HP3.ParentEmpty = ParentEmpty;
        HP4.ParentEmpty = ParentEmpty;
    }

    // Update is called once per frame
    void Update()
    {
        if (ParentEmpty.isEmptyConfusionDone)
        {
            ConfusionTimer += Time.deltaTime;
            if (ConfusionTimer >= 1.0f) {
                ConfusionTimer = 0;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { ConfusionSpeed = Random.Range(0.2f, 1.0f); }
                else { ConfusionSpeed = Random.Range(1.0f, 2.5f); }
            }
        }
        else
        {
            ConfusionSpeed = 1;
        }

        Timer += Time.deltaTime;
        if (Timer > 1) {
            if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis)
            {
                NowRotation += Time.deltaTime * (ParentEmpty.isSilence ? 0 : RotationSpeed) * ConfusionSpeed;
            }
            if (NowRotation > 360) { NowRotation -= 360; }
            if (HP1 != null) {
                HP1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, NowRotation));
                HP2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + NowRotation));
                HP3.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 + NowRotation));
                HP4.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270 + NowRotation));
                HP1.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation, Vector3.forward) * Vector3.right * 1.5f);
                HP2.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation, Vector3.forward) * Vector3.up * 1.5f);
                HP3.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation, Vector3.forward) * Vector3.left * 1.5f);
                HP4.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation, Vector3.forward) * Vector3.down * 1.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
