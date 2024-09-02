using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    public static MoveArrow arrow;
    bool isStart;
    public LongPressNutton UpArrow;
    public LongPressNutton DownArrow;
    public LongPressNutton LeftArrow;
    public LongPressNutton RightArrow;

    public bool isUpArrowPressDown;
    public bool isDownArrowPressDown;
    public bool isLeftArrowPressDown;
    public bool isRightArrowPressDown;

    private void Awake()
    {
        arrow = this;
    }

    private void Update()
    {
        if (UpArrow.isDown) { isUpArrowPressDown = true; }
        else { isUpArrowPressDown = false; }
        if (DownArrow.isDown) { isDownArrowPressDown = true; }
        else { isDownArrowPressDown = false; }
        if (LeftArrow.isDown) { isLeftArrowPressDown = true; }
        else { isLeftArrowPressDown = false; }
        if (RightArrow.isDown) { isRightArrowPressDown = true; }
        else { isRightArrowPressDown = false; }
    }

    private void LateUpdate()
    {
        if (!isStart) { SetArrow(); isStart = true; }
    }

    public void SetArrow()
    {

        if (InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 0 || InitializePlayerSetting.GlobalPlayerSetting.ControlTypr == 3)

        {
            arrow.gameObject.SetActive(true);
            SetArrowOffsetAndScale();
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }
    }

    public static void SetArrowOffsetAndScale()
    {
        float xoffset = InitializePlayerSetting.GlobalPlayerSetting.ArrowXOffset;
        float yoffset = InitializePlayerSetting.GlobalPlayerSetting.ArrowYOffset;
        float scale = InitializePlayerSetting.GlobalPlayerSetting.ArrowScale;
        float spacing = InitializePlayerSetting.GlobalPlayerSetting.ArrowSpacing;
        arrow.transform.localPosition = new Vector3(-900.0f + (700.0f * xoffset), -75 + (200.0f * yoffset), 0);
        arrow.transform.localScale = new Vector3(1 + scale, 1 + scale, 1 + scale);
        arrow.UpArrow.transform.localPosition = new Vector3(0, 60.0f + 40.0f * spacing, 0);
        arrow.DownArrow.transform.localPosition = new Vector3(0, -(60.0f + 40.0f * spacing), 0);
        arrow.LeftArrow.transform.localPosition = new Vector3(-(60.0f + 40.0f * spacing), 0, 0);
        arrow.RightArrow.transform.localPosition = new Vector3((60.0f + 40.0f * spacing), 0, 0);


    }
}
