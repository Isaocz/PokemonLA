using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBoolAndroid : MonoBehaviour
{

    public bool b;
    // Start is called before the first frame update

    private void Awake()
    {
        if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other)
        {
            gameObject.SetActive(b);
        }
        else
        {
            gameObject.SetActive(!b);

        }
    }

}
