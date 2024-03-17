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



}
