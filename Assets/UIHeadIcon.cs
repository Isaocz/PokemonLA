using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeadIcon : MonoBehaviour
{

    public static UIHeadIcon StaticHeadIcon;
    Image Icon;

    private void Awake()
    {
        StaticHeadIcon = this;
        Icon = GetComponent<Image>();
    }

    public void ChangeHeadIcon(Sprite HeadIcon )
    {
        Icon.sprite = HeadIcon;
    }

}
