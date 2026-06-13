using System.Collections.Generic;
using UnityEngine;

public class CornerSequenceDetector : MonoBehaviour
{
    public float maxInterval = 0.8f; // 每步之间最大允许间隔
    private float timer = 0f;

    private int openIndex = 0;
    private int closeIndex = 0;

    // 定义四个角落的区域（百分比）
    private Rect topLeft;
    private Rect topRight;
    private Rect bottomLeft;
    private Rect bottomRight;

    // 开启控制台的角落序列
    public List<string> openSequence = new List<string>()
    {
        "TL", "TR", "BL", "BR"
    };

    // 关闭控制台的角落序列
    public List<string> closeSequence = new List<string>()
    {
        "BR", "BL", "TR", "TL"
    };

    private void Start()
    {
        float w = Screen.width;
        float h = Screen.height;

        float size = 0.18f; // 每个角落区域占屏幕 18%

        topLeft = new Rect(0, h * (1 - size), w * size, h * size);
        topRight = new Rect(w * (1 - size), h * (1 - size), w * size, h * size);
        bottomLeft = new Rect(0, 0, w * size, h * size);
        bottomRight = new Rect(w * (1 - size), 0, w * size, h * size);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > maxInterval)
        {
            openIndex = 0;
            closeIndex = 0;
            timer = 0;
        }

        if (Input.touchCount == 0)
            return;

        Touch t = Input.GetTouch(0);
        if (t.phase != TouchPhase.Began)
            return;

        string corner = DetectCorner(t.position);
        if (corner == null)
            return;

        // --- 开启序列 ---
        if (corner == openSequence[openIndex])
        {
            openIndex++;
            timer = 0;

            if (openIndex >= openSequence.Count)
            {
                openIndex = 0;
                closeIndex = 0;
                OnOpenSequence();
            }
        }
        else openIndex = 0;

        // --- 关闭序列 ---
        if (corner == closeSequence[closeIndex])
        {
            closeIndex++;
            timer = 0;

            if (closeIndex >= closeSequence.Count)
            {
                openIndex = 0;
                closeIndex = 0;
                OnCloseSequence();
            }
        }
        else closeIndex = 0;
    }

    private string DetectCorner(Vector2 pos)
    {
        if (topLeft.Contains(pos)) return "TL";
        if (topRight.Contains(pos)) return "TR";
        if (bottomLeft.Contains(pos)) return "BL";
        if (bottomRight.Contains(pos)) return "BR";
        return null;
    }

    private void OnOpenSequence()
    {
        FindObjectOfType<DebugController>().ForceOpenConsole();
    }

    private void OnCloseSequence()
    {
        FindObjectOfType<DebugController>().ForceCloseConsole();
    }
}
