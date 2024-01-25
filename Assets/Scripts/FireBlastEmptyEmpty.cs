using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastEmptyEmpty : MonoBehaviour
{
    public Empty ParentEmpty;

    FireBlastEmptyLine FB1;
    FireBlastEmptyLine FB2;
    FireBlastEmptyLine FB3;
    FireBlastEmptyLine FB4;
    FireBlastEmptyLine FB5;

    float RotationSpeed;
    float NowRotation;
    float RotationPlusTimer;

    // Start is called before the first frame update
    void Start()
    {
        FB1 = transform.GetChild(0).GetComponent<FireBlastEmptyLine>();
        FB2 = transform.GetChild(1).GetComponent<FireBlastEmptyLine>();
        FB3 = transform.GetChild(2).GetComponent<FireBlastEmptyLine>();
        FB4 = transform.GetChild(3).GetComponent<FireBlastEmptyLine>();
        FB5 = transform.GetChild(4).GetComponent<FireBlastEmptyLine>();

        FB1.ParentEmpty = ParentEmpty;
        FB2.ParentEmpty = ParentEmpty;
        FB3.ParentEmpty = ParentEmpty;
        FB4.ParentEmpty = ParentEmpty;
        FB5.ParentEmpty = ParentEmpty;
        RotationSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis)
        {
            RotationPlusTimer += Time.deltaTime;
        }

        
        if (RotationPlusTimer > 1.5f)
        {
            RotationSpeed = Mathf.Clamp(RotationSpeed + 15*Time.deltaTime, 0 ,60) ;
            if (ParentEmpty.isSilence) { RotationSpeed = 0; }
        }
        NowRotation += Time.deltaTime * RotationSpeed * (ParentEmpty.isEmptyConfusionDone ? 0.5f : 2f);
        if (NowRotation > 360) { NowRotation -= 360; }
        if (FB1 == null)
        {
            Destroy(gameObject);
        }
        else { 
            FB1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 18 + NowRotation));
            FB2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + NowRotation));
            FB3.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 162 + NowRotation));
            FB4.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 234 + NowRotation));
            FB5.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 306 + NowRotation));

            FB1.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation + 18, Vector3.forward) * Vector3.right * 1.2f);
            FB2.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation + 90, Vector3.forward) * Vector3.right * 1.2f);
            FB3.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation + 162, Vector3.forward) * Vector3.right * 1.2f);
            FB4.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation + 234, Vector3.forward) * Vector3.right * 1.2f);
            FB5.transform.position = (transform.position + Quaternion.AngleAxis(NowRotation + 306, Vector3.forward) * Vector3.right * 1.2f);
        }
    }
}
