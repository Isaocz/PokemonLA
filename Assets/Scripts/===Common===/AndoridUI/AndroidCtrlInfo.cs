using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCtrlInfo : MonoBehaviour
{
    public static AndroidCtrlInfo info;

    private void Awake()
    {
        info = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("ShowAndroidCtrInfo" , 0) ;
        transform.gameObject.SetActive(PlayerPrefs.GetInt("ShowAndroidCtrInfo") == 0);
    }


    public void CloseInfo()
    {
        PlayerPrefs.SetInt("ShowAndroidCtrInfo", 1);
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
