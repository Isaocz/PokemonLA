using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleIcon : MonoBehaviour
{

    public Sprite[] IconList;
    Image IconImage;

    private void Start()
    {
        IconImage = GetComponent<Image>();
        IconImage.sprite = IconList[Random.Range(0,239)];
    }

}
