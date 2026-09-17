using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTypeSwitch : UISwitch
{
    public SetSettingPanel ParentPanel;

    // Start is called before the first frame update
    public GameObject JSXOffset;
    public GameObject JSYOffset;
    public GameObject JSScale;
    public GameObject ArrowXOffset;
    public GameObject ArrowYOffset;
    public GameObject ArrowScale;
    public GameObject ArrowSpacing;


    private void Start()
    {

        SetSwitch(PlayerPrefs.GetInt("ControlTypr"));

    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("ControlTypr", Index);
        InitializePlayerSetting.GlobalPlayerSetting.ControlTypr = PlayerPrefs.GetInt("ControlTypr");
        SwitchConyrolType();
        //MoveStick.joystick.GetComponent<VariableJoystick>().SetMode((InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) ? JoystickType.Fixed : JoystickType.Floating);
        //if (InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) { MoveStick.joystick.transform.GetChild(0).localPosition = new Vector3(1500, 256, 0); }
        //else { MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(false); }
    }

    void SwitchConyrolType()
    {
        JSXOffset.SetActive(false);
        JSYOffset.SetActive(false);
        JSScale.SetActive(false);
        ArrowXOffset.SetActive(false);
        ArrowYOffset.SetActive(false);
        ArrowScale.SetActive(false);
        ArrowSpacing.SetActive(false);
        MoveArrow.arrow.SetArrow();
        MoveStick.joystick.transform.GetComponent<MoveStick>().SetJoyStick();
        switch (InitializePlayerSetting.GlobalPlayerSetting.ControlTypr)
        {
            case 0:    //十字键
                ArrowXOffset.SetActive(true);
                ArrowYOffset.SetActive(true);
                ArrowScale.SetActive(true);
                ArrowSpacing.SetActive(true);
                break;
            case 1:    //自由遥感
                JSScale.SetActive(true);
                break;
            case 2:    //固定摇杆
                JSXOffset.SetActive(true);
                JSYOffset.SetActive(true);
                JSScale.SetActive(true);
                break;
            case 3:    //结合模式
                ArrowXOffset.SetActive(true);
                ArrowYOffset.SetActive(true);
                ArrowScale.SetActive(true);
                ArrowSpacing.SetActive(true);
                JSXOffset.SetActive(true);
                JSYOffset.SetActive(true);
                JSScale.SetActive(true);
                break;
        }
        ParentPanel.SetPanel();
    }

}
