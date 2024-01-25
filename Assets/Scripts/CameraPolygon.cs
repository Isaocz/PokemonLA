using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPolygon : MonoBehaviour
{
    public PolygonCollider2D polygonCollider; // 引用Polygon Collider 2D组件

    // 在需要的时候调用该方法来更新Polygon Collider 2D的点位
    public void CameraPolygonPoints(Vector2[] newPoints)
    {
        polygonCollider.points = newPoints;
    }

}
