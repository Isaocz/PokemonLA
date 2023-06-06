using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDescribe : MonoBehaviour
{
    public Text FirstText;
    public Text SecondText;
    float ImageWidth;
    // Start is called before the first frame update

    void Start()
    {
        ImageWidth = GetComponent<Image>().rectTransform.rect.width;
    }

    public void MoveDescribe()
    {
        gameObject.GetComponent<Image>().rectTransform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 3, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }

    public void GetDescribeString(string InputStr , string TwoModeString ,bool TwoMode)
    {
        if (!TwoMode)
        {
            FirstText.gameObject.SetActive(false);
        }
        else
        {
            FirstText.gameObject.SetActive(true);
            FirstText.text = TwoModeString;
        }
        int Xlength = Mathf.Clamp((int)(Mathf.Sqrt((float)InputStr.Length / 2) * 2), 10, 100);
        string OutputStr = "";
        for (int i = 0; i < InputStr.Length; i++)
        {
            if (i % Xlength == 0)
            {
                OutputStr += "\n";
            }
            OutputStr += InputStr[i];
        }
        SecondText.text = OutputStr;
        transform.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        transform.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().rectTransform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        GetComponent<VerticalLayoutGroup>().padding.left = (int)GetComponent<Image>().rectTransform.rect.width / 7;
        GetComponent<VerticalLayoutGroup>().padding.right = (int)GetComponent<Image>().rectTransform.rect.width / 14;
        GetComponent<VerticalLayoutGroup>().padding.top = (int)GetComponent<Image>().rectTransform.rect.height / 10;
        GetComponent<VerticalLayoutGroup>().padding.bottom = (int)GetComponent<Image>().rectTransform.rect.height / 10;

    }
}
