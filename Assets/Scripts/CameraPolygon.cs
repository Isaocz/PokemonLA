using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPolygon : MonoBehaviour
{
    public PolygonCollider2D polygonCollider; // ����Polygon Collider 2D���

    // ����Ҫ��ʱ����ø÷���������Polygon Collider 2D�ĵ�λ
    public void CameraPolygonPoints(Vector2[] newPoints)
    {
        polygonCollider.points = newPoints;
    }

}
