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

    bool isStart;

    private void Awake()
    {
        joystick = transform.GetComponent<Joystick>();


    }

    private void Start()
    {

    }

    private void LateUpdate()
    {
        if (!isStart) { SetJoyStick(); isStart = true; }
    }

    public void SetJoyStick()
    {
        if (InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 0)
        {
            joystick.gameObject.SetActive(false);
        }
        else
        {
            joystick.gameObject.SetActive(true);

            if (InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 1)
            {
                MoveStick.joystick.GetComponent<VariableJoystick>().SetMode(JoystickType.Floating);
                MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(false);
                SetJoyStickOffsetAndScale();
            }
            else if(InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 2 || InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 3)
            {
                MoveStick.joystick.GetComponent<VariableJoystick>().SetMode(JoystickType.Fixed);
                MoveStick.joystick.transform.GetChild(0).gameObject.SetActive(true);
                SetJoyStickOffsetAndScale();
            }

        }
    }

    public static void SetJoyStickOffsetAndScale()
    {
        float xoffset = InitializePlayerSetting.GlobalPlayerSetting.JoystickXOffset;
        float yoffset = InitializePlayerSetting.GlobalPlayerSetting.JoystickYOffset;
        float scale = InitializePlayerSetting.GlobalPlayerSetting.JoystickScale;
        MoveStick.joystick.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 256 * (1 + scale));
        MoveStick.joystick.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 256 * (1 + scale));
        MoveStick.joystick.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 128 * (1 + scale));
        MoveStick.joystick.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 128 * (1 + scale));
        if (InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 2 || InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 3)
        {
            MoveStick.joystick.transform.GetChild(0).localPosition = new Vector3(800.0f + (1000.0f * xoffset), 256.0f + (200.0f * yoffset), 0);
        }

    }




}
