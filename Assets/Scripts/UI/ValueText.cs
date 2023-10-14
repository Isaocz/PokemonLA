using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueText : MonoBehaviour
{
    public Slider slider;
    public Text volumeText;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        volumeText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
