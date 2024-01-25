using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirliaConfusionManger : MonoBehaviour
{

    public Kirlia ParentKirlia;
    float RandomRotation;
    KirliaConfusion c1;
    KirliaConfusion c2;
    KirliaConfusion c3;

    // Start is called before the first frame update
    void Start()
    {
        RandomRotation = Random.Range(0,360);
        c1 = transform.GetChild(0).GetComponent<KirliaConfusion>();
        c1.R = 0 + RandomRotation; c1.StartPosition = c1.transform.position; c1.empty = ParentKirlia;
        c1.transform.rotation = Quaternion.AngleAxis(RandomRotation , Vector3.forward) * c1.transform.rotation;
        c2 = transform.GetChild(1).GetComponent<KirliaConfusion>();
        c2.R = 120 + RandomRotation; c2.StartPosition = c2.transform.position; c2.empty = ParentKirlia;
        c2.transform.rotation = Quaternion.AngleAxis(RandomRotation, Vector3.forward) * c2.transform.rotation;
        c3 = transform.GetChild(2).GetComponent<KirliaConfusion>();
        c3.R = 240 + RandomRotation; c3.StartPosition = c3.transform.position; c3.empty = ParentKirlia;
        c3.transform.rotation = Quaternion.AngleAxis(RandomRotation, Vector3.forward) * c3.transform.rotation;
        Invoke("DestroySelf", 10f);
    }


    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
