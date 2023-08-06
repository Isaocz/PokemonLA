using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _mTool : MonoBehaviour
{
    public static float Angle_360(Vector3 from_, Vector3 to_)
    {
        if (from_.x <= 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }

    public static float Angle_360Y(Vector3 from_, Vector3 to_)
    {
        if (from_.y >= 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }
}
