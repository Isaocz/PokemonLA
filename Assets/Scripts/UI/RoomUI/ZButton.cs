using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZButton : MonoBehaviour
{

    public static ZButton Z;
    public bool IsZButtonDown { get { return isZButtonDown; } set { isZButtonDown = value; } }
    bool isZButtonDown;

    private void Awake()
    {
        Z = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }


    private void LateUpdate()
    {
        isZButtonDown = false;
        if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Interact"))) { Debug.Log("zzz"); isZButtonDown = true; }
    }
}
