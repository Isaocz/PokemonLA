using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSettingPanel : MonoBehaviour
{

    public float Spacing;

    public GameObject Button;
    public GameObject SeedWords;
    public GameObject VoiceManger;
    public GameObject OtherGameSetting;
    public GameObject AndroidSetting;
    public GameObject KeyboardChanger;

    bool isIns;
    bool isLateIns = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other)
        {
            KeyboardChanger.gameObject.SetActive(true);
            AndroidSetting.gameObject.SetActive(false);
            //transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2553.556f);
        }
        else
        {
            AndroidSetting.gameObject.SetActive(true);
            KeyboardChanger.gameObject.SetActive(false);
            //transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1510.359f);

        }
    }

    public void SetPanel() { isLateIns = false; }

    private void LateUpdate()
    {
        if (!isIns) {
            SetSpacing();
            isIns = true;
        }
        if (!isLateIns)
        {
            isLateIns = true; isIns = false;
        }
    }

    void SetSpacing()
    {


        List<Transform> ChildList = new List<Transform>() { };
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                ChildList.Add(transform.GetChild(i));
            }
        }

        for (int i = 1; i < ChildList.Count; i++)
        {
            RectTransform r = ChildList[i].GetComponent<RectTransform>();
            Debug.Log(_mTool.GetLastGrandChild(ChildList[i - 1]).name);
            r.position = new Vector2(r.position.x, _mTool.GetLastGrandChild(ChildList[i - 1]).GetComponent<RectTransform>().position.y - Spacing);
        }


        //float t = GetHigh(ChildList);
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetHigh(ChildList)+ 120.0f);
    }


    float GetHigh(List<Transform> ChildList)
    {
        float Output = 0;
        Transform FirstChild = ChildList[0];
        float HighMax = FirstChild.GetComponent<RectTransform>().anchoredPosition.y;
        while (FirstChild.childCount != 0)
        {
            FirstChild = FirstChild.GetChild(0);
            HighMax += FirstChild.GetComponent<RectTransform>().anchoredPosition.y;
        }

        Transform LastChild = ChildList[ChildList.Count - 1];
        float HighMin = LastChild.GetComponent<RectTransform>().anchoredPosition.y;
        while (LastChild.childCount != 0)
        {
            Debug.Log(LastChild.name + "+" + LastChild.GetComponent<RectTransform>().position.y);
            LastChild = _mTool.GetLastChild(LastChild) ;
            HighMin += LastChild.GetComponent<RectTransform>().anchoredPosition.y;
        }

        Output = Mathf.Abs(HighMax - HighMin);
        Debug.Log(HighMax + "+" + HighMin + "+" + Output);
        return Output;
    }

    public void LookInfo()
    {
        AndroidCtrlInfo.info.Open();
    }

}
