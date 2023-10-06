using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    private float timer;
    [Tooltip("���ų���ʱ��")]
    public float scaleDuration = 0.5f;
    [Tooltip("��ʼ���Ŵ�С")]
    public Vector3 initialScale = new Vector3(1.3f, 1.3f, 0f);
    [Tooltip("Ŀ�����Ŵ�С")]
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
