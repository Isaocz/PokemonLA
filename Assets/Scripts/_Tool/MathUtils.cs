using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    /// <summary>
    /// 二阶贝塞尔曲线取中间值
    /// </summary>
    /// <param name="a">开始点</param>
    /// <param name="b">结束点</param>
    /// <returns></returns>
    public static Vector2 BezierGetMiddle(Vector2 a, Vector2 b)
    {
        Vector2 m = Vector2.Lerp(a, b, 0.1f);
        Vector2 normal = Vector2.Perpendicular(a - b).normalized;
        float rd = Random.Range(-2f, 2f);
        float curveRatio = 0.3f;
        return m + (a - b).magnitude * curveRatio * rd * normal;
    }

    /// <summary>
    /// 二阶贝塞尔曲线计算
    /// </summary>
    /// <param name="t">百分比</param>
    /// <param name="a">开始点</param>
    /// <param name="b">中间点</param>
    /// <param name="c">结束点</param>
    /// <returns></returns>
    public static Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
