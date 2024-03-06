using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowDamage : MonoBehaviour
{
    private Toggle toggle;
    public int IsShowDamage = 1;
    public static UIShowDamage instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(isOn => IsShowDamage = isOn ? 1 : 0);
        PlayerPrefs.SetInt("ShowDamage", IsShowDamage);
    }
}
