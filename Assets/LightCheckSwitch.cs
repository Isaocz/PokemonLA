using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheckSwitch : MonoBehaviour
{

    public static int MAX_Count = 5;

    /// <summary>
    /// 检查次数
    /// </summary>
    [Range(1, 5)]
    public int CheckCount;

    /// <summary>
    /// 检查成功的次数
    /// </summary>
    public int OKCount = 0;


    /// <summary>
    /// 所有子检查灯的列表
    /// </summary>
    public List<LightCheckSwitchChildLight> AllChildLightList = new List<LightCheckSwitchChildLight> { };

    /// <summary>
    /// 预备的子检查灯的列表
    /// </summary>
    public List<LightCheckSwitchChildLight> ChildLightList = new List<LightCheckSwitchChildLight> { };

    /// <summary>
    /// 子灯X相对位置
    /// </summary>
    static float ChildLightLocalPositionX = -0.083f;

    /// <summary>
    /// 子灯Y相对位置
    /// </summary>
    static public List<List<float>> ChildLightLocalPositionYList = new List<List<float>>
    {
        new List<float>{ -0.369f },
        new List<float>{ -0.203f, -0.523f },
        new List<float>{ -0.080f, -0.355f, -0.630f },
        new List<float>{ -0.025f, -0.243f, -0.461f, -0.679f },
        new List<float>{ -0.010f, -0.210f, -0.410f, -0.610f, -0.810f }
    };


    /// <summary>
    /// 成功音效
    /// </summary>
    public AudioSource SucessAudio;

    /// <summary>
    /// 检查是否成功
    /// </summary>
    public bool isCheckSucess = false;





    // Start is called before the first frame update
    void Start()
    {
        OKCount = 0;
        InitChildLight(CheckCount);
    }

    void InitChildLight(int count)
    {
        count = Mathf.Clamp(count, 1, MAX_Count);
        for (int i = 0; i < MAX_Count; i++)
        {
            if (i < count) {
                AllChildLightList[i].gameObject.SetActive(true);
                if (!ChildLightList.Contains(AllChildLightList[i])) { ChildLightList.Add(AllChildLightList[i]); }
                AllChildLightList[i].transform.localPosition = new Vector3(ChildLightLocalPositionX, ChildLightLocalPositionYList[count - 1][i], 0.0f);
            }
            else
            {
                AllChildLightList[i].gameObject.SetActive(false);
            }
        }
    }





    /// <summary>
    /// 增加检查数
    /// </summary>
    /// <param name="c"></param>
    public void CountPlus(int c)
    {
        if (!isCheckSucess) {
            OKCount += c;
            OKCount = Mathf.Clamp(OKCount, 0, CheckCount);
            LightCheck();
            SucessCheck();
        }
    }

    /// <summary>
    /// 减少检查数
    /// </summary>
    /// <param name="c"></param>
    public void CountMinus(int c)
    {
        if (!isCheckSucess) {
            OKCount -= c;
            OKCount = Mathf.Clamp(OKCount, 0, CheckCount);
            LightCheck();
            SucessCheck();
        }
    }

    /// <summary>
    /// 根据检查数点亮子灯
    /// </summary>
    void LightCheck()
    {
        OKCount = Mathf.Clamp(OKCount, 0, CheckCount);
        for (int i = 0; i < CheckCount; i++)
        {
            if ( i < OKCount)
            {
                ChildLightList[i].On();
            }
            else
            {
                ChildLightList[i].OFF();
            }

        }
    }


    /// <summary>
    /// 检查是否成功
    /// </summary>
    void SucessCheck()
    {
        if (OKCount >= CheckCount)
        {
            isCheckSucess = true;
            SucessEvent();
        }
    }


    /// <summary>
    /// 成功事件
    /// </summary>
    public virtual void SucessEvent()
    {
        SucessAudio.Play();
    }

}
