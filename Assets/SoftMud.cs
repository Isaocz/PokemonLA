using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftMud : MonoBehaviour
{
    public bool isBeUsed;
    SpriteRenderer SoftMudSprite;

    private void Start()
    {
        SoftMudSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isBeUsed)
        {
            SoftMudSprite.color -= new Color(0, 0, 0, Time.deltaTime);
            if(SoftMudSprite.color.a <= 0.05) { Destroy(gameObject); }
        }
    }
}
