using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStick : MonoBehaviour
{
    public static Joystick joystick;
    bool isLeftKeyDown;
    bool isRightKeyDown;
    bool isUpKeyDown;
    bool isDownKeyDown;

    private void Awake()
    {
        joystick = transform.GetComponent<Joystick>();


    }

    private void Start()
    {
        MoveStick.joystick.GetComponent<VariableJoystick>().SetMode((InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) ? JoystickType.Fixed : JoystickType.Floating);
        if (InitializePlayerSetting.GlobalPlayerSetting.isJoystickFixed) { MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(true); MoveStick.joystick.transform.GetChild(0).localPosition = new Vector3(1700, 356, 0); }
        else { MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(false); }
    }



}
