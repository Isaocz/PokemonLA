using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamManger : MonoBehaviour
{

    public float Rotation;
    public Empty ParentEmpty;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).position = Quaternion.AngleAxis(Rotation + 0, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0,0,Rotation));
        transform.GetChild(1).position = Quaternion.AngleAxis(Rotation + 90, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation+90));
        transform.GetChild(2).position = Quaternion.AngleAxis(Rotation + 180, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(2).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation+180));
        transform.GetChild(3).position = Quaternion.AngleAxis(Rotation + 270, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(3).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation+270));

        transform.GetChild(4).position = Quaternion.AngleAxis(Rotation + 0, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(4).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation));
        transform.GetChild(0).GetComponent<SolarBeamCastform>().empty = ParentEmpty;
        transform.GetChild(5).position = Quaternion.AngleAxis(Rotation + 90, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(5).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation + 90));
        transform.GetChild(1).GetComponent<SolarBeamCastform>().empty = ParentEmpty;
        transform.GetChild(6).position = Quaternion.AngleAxis(Rotation + 180, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(6).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation + 180));
        transform.GetChild(2).GetComponent<SolarBeamCastform>().empty = ParentEmpty;
        transform.GetChild(7).position = Quaternion.AngleAxis(Rotation + 270, Vector3.forward) * Vector3.right * 1.2f;
        transform.GetChild(7).rotation = Quaternion.Euler(new Vector3(0, 0, Rotation + 270));
        transform.GetChild(3).GetComponent<SolarBeamCastform>().empty = ParentEmpty;

        Invoke("SetLeserActiveTrue" , 1f);
    }

    // Update is called once per frame

    void SetLeserActiveTrue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
    }
}
