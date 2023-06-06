using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUIZHIBIANYUAN : Graphic
{

    private void OnGUI()
    {
        // 实时检测更新绘制 OnPopulateMesh 中 transform.child 位置
        SetAllDirty();
    }


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (transform.childCount <= 2)
        {
            return;
        }

        Color32 color32 = color;


        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                vh.AddVert(child.localPosition, color32, new Vector2(0f, 0f));
            }
            else
            {
                return;
            }
        }


        for (int i = 0; i < transform.childCount - 2; i++)
        {
            // 几何图形中的三角形
            vh.AddTriangle(i + 1, i + 2, 0);

        }

    }
}
