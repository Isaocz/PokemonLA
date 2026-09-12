using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮点击音效
/// </summary>
public class ButtonClickSE : MonoBehaviour
{
    /// <summary>
    /// 点击音效种类
    /// </summary>
    public enum BUTTONCLICKSETYPE
    {
        Normal,  //一般点击音效
        None,    //无点击音效
    }

    /// <summary>
    /// 该按钮音效种类 默认为一般
    /// </summary>
    public BUTTONCLICKSETYPE ButtonClickSEType = BUTTONCLICKSETYPE.Normal;

    
    public void CallClickSE()
    {
        if (AudioManager.Instance != null && Camera.main != null)
        {
            switch (ButtonClickSEType)
            {
                //一般点击音效
                case BUTTONCLICKSETYPE.Normal:
                    AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.ButtonClick , Vector3.zero , true);
                    break;
                //无音效
                case BUTTONCLICKSETYPE.None:
                    break;
            }
        }
    }
}
