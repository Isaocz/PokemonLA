using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPosSwitch : UISwitch
{
    // Start is called before the first frame update

    private void Start()
    {
        SetSwitch(PlayerPrefs.GetInt("JoystickFixed"));

    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        PlayerPrefs.SetInt("JoystickFixed", Index);
        InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed = PlayerPrefs.GetInt("JoystickFixed") == 1;
        MoveStick.joystick.GetComponent<VariableJoystick>().SetMode((InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) ? JoystickType.Fixed : JoystickType.Floating);
        if (InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) { MoveStick.joystick.transform.GetChild(0).localPosition = new Vector3(1500,256,0); }
        else { MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(false); }
    }
}
