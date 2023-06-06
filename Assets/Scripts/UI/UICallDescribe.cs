using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICallDescribe : MonoBehaviour
{
    public string DescribeText;
    public string FirstText;
    public bool TwoMode;
    public UIDescribe DescribeUI;
    public void MouseEnter()
    {
        DescribeUI.MoveDescribe();
        DescribeUI.GetDescribeString(DescribeText,FirstText,TwoMode);
        DescribeUI.gameObject.SetActive(true);
    }
    public void MouseExit()
    {
        DescribeUI.MoveDescribe();
        DescribeUI.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DescribeUI.gameObject.SetActive(false);
    }


}
