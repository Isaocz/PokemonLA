using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleIcon : MonoBehaviour
{

    public Sprite[] IconList;
    Image IconImage;
    Image IconImage01;
    Image IconImage02;

    private void Start()
    {
        IconImage = GetComponent<Image>();
        IconImage01 = transform.GetChild(0).GetComponent<Image>();
        IconImage02 = transform.GetChild(1).GetComponent<Image>();
        IconImage.sprite = IconList[Random.Range(0,239)];
        IconImage01.sprite = IconImage.sprite;
        IconImage02.sprite = IconImage.sprite;
    }

    public void ChangeSprite()
    {
        IconImage.sprite = IconList[Random.Range(0, 239)];
        IconImage01.sprite = IconImage.sprite;
        IconImage02.sprite = IconImage.sprite;
    }

}
