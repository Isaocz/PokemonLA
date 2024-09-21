using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoleToggle : MonoBehaviour
{
    public Image image;
    public bool isOn;
    public Sprite SpriteNormal;
    public Sprite SpriteHL;

    private void Start()
    {
        isOn = false; image.sprite = SpriteNormal;
    }



    public void ChangeSprite()
    {
        if (isOn) { isOn = false; image.sprite = SpriteNormal; }
        else { isOn = true; image.sprite = SpriteHL; }
    }

    public void _OnMouseEnter()
    {
        image.sprite = SpriteHL;
    }

    public void _OnMouseExit()
    {
        if (!isOn) { image.sprite = SpriteNormal; }
    }

    public void _OnScroll()
    {
        Debug.Log(1);
    }
    
}
