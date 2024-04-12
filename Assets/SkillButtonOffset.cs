using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonOffset : MonoBehaviour
{
    private Slider slider;
    public AudioSource musicSource;

    public enum OffsetType
    {
        ����,
        ����,
        ����,
    }
    public OffsetType Offset;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        switch (Offset)
        {
            case OffsetType.����:
                slider.value = PlayerPrefs.GetFloat("SkillButtonXOffset");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonXOffset = slider.value;
                break;
            case OffsetType.����:
                slider.value = PlayerPrefs.GetFloat("SkillButtonYOffset");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonYOffset = slider.value;
                break;
            case OffsetType.����:
                slider.value = PlayerPrefs.GetFloat("SkillButtonScale");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonScale = slider.value;
                break;
        }

    }



    private void OnSliderValueChanged(float value)
    {
        if (AudioM.GlobalAudioM != null)
        {
            switch (Offset)
            {
                case OffsetType.����:
                    PlayerPrefs.SetFloat("SkillButtonXOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonXOffset = slider.value;
                    AndroidSkillButton.androidSkillButton.SetXOffset(value);
                    break;
                case OffsetType.����:
                    PlayerPrefs.SetFloat("SkillButtonYOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonYOffset = slider.value;
                    AndroidSkillButton.androidSkillButton.SetYOffset(value);
                    break;
                case OffsetType.����:
                    PlayerPrefs.SetFloat("SkillButtonScale", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonScale = slider.value;
                    AndroidSkillButton.androidSkillButton.SetScale(value);
                    break;
            }
        }
    }
}
