using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZRP : MonoBehaviour
{

    public GameObject ZButtonObj;
    ParkBoardSet SetComponent;


    // Start is called before the first frame update
    void Start()
    {
        ZButtonObj = transform.GetChild(0).gameObject;
        SetComponent = transform.GetComponent<ParkBoardSet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SetComponent.enabled && SetComponent.isZEnable)
        {
            if (!ZButtonObj.activeInHierarchy) {
                ZButtonObj.SetActive(true);
                ZButtonObj.transform.rotation = Quaternion.identity;
                ZButtonObj.transform.position = transform.position + 1.8f * Vector3.up;
            }
        }
        else
        {
            ZButtonObj.SetActive(false);
        }

    }
}
