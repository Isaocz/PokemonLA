using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    /// <summary>
    /// ���ױ���������ȡ�м�ֵ
    /// </summary>
    /// <param name="a">��ʼ��</param>
    /// <param name="b">������</param>
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
    /// ���ױ��������߼���
    /// </summary>
    /// <param name="t">�ٷֱ�</param>
    /// <param name="a">��ʼ��</param>
    /// <param name="b">�м��</param>
    /// <param name="c">������</param>
    /// <returns></returns>
    public static Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
