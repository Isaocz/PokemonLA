using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    private float timer;
    [Tooltip("缩放持续时间")]
    public float scaleDuration = 0.5f;
    [Tooltip("初始缩放大小")]
    public Vector3 initialScale = new Vector3(1.3f, 1.3f, 0f);
    [Tooltip("目标缩放大小")]
    public Vector3 targetScale = new Vector3(1.25f, 1.25f, 0f);

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= scaleDuration)
        {
            transform.localScale = targetScale;
            enabled = false;
        }
        else
        {
            float t = timer / scaleDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        }
    }
}
