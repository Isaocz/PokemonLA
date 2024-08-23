using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongPressNutton : MonoBehaviour
{

    public Sprite DownSprite;
    public Sprite UpSprite;
    public bool isDown;
    Button button;
    Image ButtonImage;


    private void Start()
    {
        button = GetComponent<Button>();
        ButtonImage = GetComponent<Image>();
        ButtonImage.sprite = UpSprite;
        isDown = false;

    }

    public virtual void ButtonDown()
    {
        Debug.Log(ButtonImage);
        Debug.Log(DownSprite);
        if (!isDown) { isDown = true; ButtonImage.sprite = DownSprite; }
        Debug.Log("Down");
    }

    public virtual void ButtonUp()
    {
        if (isDown) { isDown = false; ButtonImage.sprite = UpSprite; }
        Debug.Log("Up");
    }

}
