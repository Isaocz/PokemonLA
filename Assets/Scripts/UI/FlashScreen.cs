using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    public static FlashScreen instance;
    public Image img;
    public float flashTime;
    public Color flashColor;

    private Color defaultColor = Color.red;
    private float timer = 0f;
    void Awake()
    {
        instance = this;
        defaultColor = flashColor;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float t = Mathf.Clamp01(defaultColor.a * (timer / flashTime));
            img.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, t);
        }
        else defaultColor = flashColor;
    }

    public void flashScreen()
    {
        timer = flashTime;
    }

    public void flashScreen(float time, Color color)
    {
        timer = time;
        defaultColor = color;
    }
}
