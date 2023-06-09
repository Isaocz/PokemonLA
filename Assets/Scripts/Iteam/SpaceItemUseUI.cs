using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceItemUseUI : MonoBehaviour
{
    // Start is called before the first frame update
    Sprite UIImageSprite;
    bool isUIAnimationStart;
    float UITimer;

    public void UIAnimationStart(Sprite s)
    {
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<Image>().rectTransform.localPosition = new Vector3(0, -0.8f, 0);
        UIImageSprite = s;
        isUIAnimationStart = true;
        gameObject.GetComponent<Image>().sprite = UIImageSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUIAnimationStart)
        {
            gameObject.GetComponent<Image>().color -= new Color(0, 0, 0, 0.005f);
            gameObject.GetComponent<Image>().rectTransform.localPosition += new Vector3(0,1.5f*Time.deltaTime,0);
            UITimer += 1.5f*Time.deltaTime;
            if(gameObject.GetComponent<Image>().color.a <= 0){ isUIAnimationStart = false;UITimer = 0; }
        }
    }
}
