using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider slider;
    public AudioSource musicSource;
    Text ValueText;
    public enum VoiceType
    {
        ±≥æ∞“Ù¿÷,
        ”Œœ∑“Ù–ß,
    }
    public VoiceType Voice;

    void Start()
    {
        ValueText = transform.GetChild(4).GetComponent<Text>();
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        switch (Voice)
        {
            case VoiceType.±≥æ∞“Ù¿÷:
                slider.value = PlayerPrefs.GetFloat("BGMVolume"); break;
            case VoiceType.”Œœ∑“Ù–ß:
                slider.value = PlayerPrefs.GetFloat("SEVolume"); break;
        }
        
    }



    private void OnSliderValueChanged(float value)
    {
        if (AudioM.GlobalAudioM != null) {
            switch (Voice)
            {
                case VoiceType.±≥æ∞“Ù¿÷:
                    AudioM.GlobalAudioM.SetBgmVolume(value);
                    PlayerPrefs.SetFloat("BGMVolume", value);
                    ValueText.text = Mathf.RoundToInt(value * 100).ToString();
                    break;
                case VoiceType.”Œœ∑“Ù–ß:
                    AudioM.GlobalAudioM.SetSEVolume(value);
                    PlayerPrefs.SetFloat("SEVolume", value);
                    ValueText.text = Mathf.RoundToInt(value * 100).ToString();
                    break;
            }
        }
    }

}
