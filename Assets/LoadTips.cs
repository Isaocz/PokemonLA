using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTips : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Text>().text = _mTool.Tips[Random.Range(0, _mTool.Tips.Length)];
    }
}
