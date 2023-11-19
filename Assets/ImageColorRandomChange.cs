using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorRandomChange : MonoBehaviour
{


    Image image;
    float RAlpha;
    float GAlpha;
    float BAlpha;

    Color color01;
    Color color02;

    float ColorChangeTimer;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        color01 = new Color(Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), 1);
        color02 = new Color(Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), 1);
        DecideAlpha();
        image.color = color01;
    }

    private void Update()
    {
        ColorChangeTimer += 120 * Time.deltaTime;
        image.color = image.color = new Color(
            Mathf.Clamp(image.color.r + Time.deltaTime * RAlpha * 0.25f * (( Mathf.Sin(ColorChangeTimer) + 1) / 2), Mathf.Min(color01.r , color02.r) , Mathf.Max(color01.r, color02.r)), 
            Mathf.Clamp(image.color.g + Time.deltaTime * GAlpha * 0.25f * ((Mathf.Sin(ColorChangeTimer) + 1) / 2), Mathf.Min(color01.g, color02.g), Mathf.Max(color01.g, color02.g)),
            Mathf.Clamp(image.color.b + Time.deltaTime * BAlpha * 0.25f * ((Mathf.Sin(ColorChangeTimer) + 1) / 2), Mathf.Min(color01.b, color02.b), Mathf.Max(color01.b, color02.b) ) , 1);

        if (image.color == color02)
        {
            color01 = color02;
            color02 = new Color(Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), 1);
            DecideAlpha();
        }

    }

    void DecideAlpha()
    {
        RAlpha = color02.r - color01.r;
        GAlpha = color02.g - color01.g;
        BAlpha = color02.b - color01.b;
    }

}
