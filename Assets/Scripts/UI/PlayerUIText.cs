using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIText : MonoBehaviour
{
    [Range(1f, 5f)]
    public float Duration = 2f;
    Text uitext;
    float timer;
    bool Timing;
    bool Shaking;
    // Start is called before the first frame update
    void Start()
    {
        uitext = GetComponent<Text>();
        uitext.color = new Color(uitext.color.r, uitext.color.g, uitext.color.b, 0f);
        Timing = false;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(uitext.text))
        {
            timer += Time.deltaTime;
            if (Shaking)
            {
                float angle = Mathf.Sin(Time.time * 10f) * 10f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
            if (!Timing)
            {
                Timing = true;
                Timer.Start(this, 2f, () =>
                {
                    uitext.text = string.Empty;
                });
            }

            if (timer <= 0.5f)
            {
                uitext.color = new Color(uitext.color.r, uitext.color.g, uitext.color.b, 2 * timer);
            }
            else if (timer > 0.5f && timer < Duration - 0.5f)
            {
                uitext.color = new Color(uitext.color.r, uitext.color.g, uitext.color.b, 1f);
            }
            else if (timer > Duration - 0.5f && timer <=Duration)
            {
                uitext.color = new Color(uitext.color.r, uitext.color.g, uitext.color.b, -2 * timer + 2 * Duration);
            }
            else
            {
                uitext.color = new Color(uitext.color.r, uitext.color.g, uitext.color.b, 0f);
                if (Shaking)
                {
                    Shaking = false;
                }
            }
        }
    }
    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="newText">文本</param>
    /// <param name="shake">是否摆动</param>
    public void SetText(string newText, bool shake = false)
    {
        // 设置文本
        uitext.text = newText;
        timer = 0f;
        Timing = false;
        if (shake)
        {
            Shaking = shake;
        }
    }
}
