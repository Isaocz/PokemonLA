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
        ������������,
        ������������,
        ������������,
        ҡ�˺���,
        ҡ������,
        ҡ������,
        ʮ����������,
        ʮ����������,
        ʮ����������,
        ʮ���������,
    }
    public OffsetType Offset;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        switch (Offset)
        {
            case OffsetType.������������:
                slider.value = PlayerPrefs.GetFloat("SkillButtonXOffset");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonXOffset = slider.value;
                break;
            case OffsetType.������������:
                slider.value = PlayerPrefs.GetFloat("SkillButtonYOffset");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonYOffset = slider.value;
                break;
            case OffsetType.������������:
                slider.value = PlayerPrefs.GetFloat("SkillButtonScale");
                InitializePlayerSetting.GlobalPlayerSetting.SkillButtonScale = slider.value;
                break;


            case OffsetType.ҡ�˺���:
                slider.value = PlayerPrefs.GetFloat("JoystickXOffset");
                InitializePlayerSetting.GlobalPlayerSetting.JoystickXOffset = slider.value;
                break;
            case OffsetType.ҡ������:
                slider.value = PlayerPrefs.GetFloat("JoystickYOffset");
                InitializePlayerSetting.GlobalPlayerSetting.JoystickYOffset = slider.value;
                break;
            case OffsetType.ҡ������:
                slider.value = PlayerPrefs.GetFloat("JoystickScale");
                InitializePlayerSetting.GlobalPlayerSetting.JoystickScale = slider.value;
                break;
            case OffsetType.ʮ����������:
                slider.value = PlayerPrefs.GetFloat("ArrowXOffset");
                InitializePlayerSetting.GlobalPlayerSetting.ArrowXOffset = slider.value;
                break;
            case OffsetType.ʮ����������:
                slider.value = PlayerPrefs.GetFloat("ArrowYOffset");
                InitializePlayerSetting.GlobalPlayerSetting.ArrowYOffset = slider.value;
                break;
            case OffsetType.ʮ����������:
                slider.value = PlayerPrefs.GetFloat("ArrowScale");
                InitializePlayerSetting.GlobalPlayerSetting.ArrowScale = slider.value;
                break;
            case OffsetType.ʮ���������:
                slider.value = PlayerPrefs.GetFloat("ArrowSpacing");
                InitializePlayerSetting.GlobalPlayerSetting.ArrowSpacing = slider.value;
                break;
        }

    }



    private void OnSliderValueChanged(float value)
    {
        if (AudioM.GlobalAudioM != null)
        {
            switch (Offset)
            {
                case OffsetType.������������:
                    PlayerPrefs.SetFloat("SkillButtonXOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonXOffset = slider.value;
                    AndroidSkillButton.androidSkillButton.SetXOffset(value);
                    break;
                case OffsetType.������������:
                    PlayerPrefs.SetFloat("SkillButtonYOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonYOffset = slider.value;
                    AndroidSkillButton.androidSkillButton.SetYOffset(value);
                    break;
                case OffsetType.������������:
                    PlayerPrefs.SetFloat("SkillButtonScale", value);
                    InitializePlayerSetting.GlobalPlayerSetting.SkillButtonScale = slider.value;
                    AndroidSkillButton.androidSkillButton.SetScale(value);
                    break;


                case OffsetType.ҡ�˺���:
                    PlayerPrefs.SetFloat("JoystickXOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.JoystickXOffset = slider.value;
                    MoveStick.SetJoyStickOffsetAndScale();
                    break;
                case OffsetType.ҡ������:
                    PlayerPrefs.SetFloat("JoystickYOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.JoystickYOffset = slider.value;
                    MoveStick.SetJoyStickOffsetAndScale();
                    break;
                case OffsetType.ҡ������:
                    PlayerPrefs.SetFloat("JoystickScale", value);
                    InitializePlayerSetting.GlobalPlayerSetting.JoystickScale = slider.value;
                    MoveStick.SetJoyStickOffsetAndScale();
                    break;
                case OffsetType.ʮ����������:
                    PlayerPrefs.SetFloat("ArrowXOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.ArrowXOffset = slider.value;
                    MoveArrow.SetArrowOffsetAndScale();
                    break;
                case OffsetType.ʮ����������:
                    PlayerPrefs.SetFloat("ArrowYOffset", value);
                    InitializePlayerSetting.GlobalPlayerSetting.ArrowYOffset = slider.value;
                    MoveArrow.SetArrowOffsetAndScale();
                    break;
                case OffsetType.ʮ����������:
                    PlayerPrefs.SetFloat("ArrowScale", value);
                    InitializePlayerSetting.GlobalPlayerSetting.ArrowScale = slider.value;
                    MoveArrow.SetArrowOffsetAndScale();
                    break;
                case OffsetType.ʮ���������:
                    PlayerPrefs.SetFloat("ArrowSpacing", value);
                    InitializePlayerSetting.GlobalPlayerSetting.ArrowSpacing = slider.value;
                    MoveArrow.SetArrowOffsetAndScale();
                    break;
            }
        }
    }
}
