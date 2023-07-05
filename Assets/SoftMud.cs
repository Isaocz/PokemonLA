using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftMud : MonoBehaviour
{
    public bool isBeUsed;
    bool isBorn;
    SpriteRenderer SoftMudSprite;

    private void Start()
    {
        SoftMudSprite = GetComponent<SpriteRenderer>();
        SoftMudSprite.color = new Color(1, 1, 1, 0);
        transform.localScale = new Vector3(0, 0, 1);
    }

    private void Update()
    {
        if (!isBorn)
        {
            SoftMudSprite.color += new Color(0, 0, 0, Time.deltaTime*2.5f);
            transform.localScale += new Vector3(Time.deltaTime * 2.5f, Time.deltaTime * 2.5f, 0);
            if (SoftMudSprite.color.a >= 1) {
                transform.localScale = new Vector3(1, 1, 1);
                isBorn = true;
            }
        }

        if (isBeUsed)
        {
            SoftMudSprite.color -= new Color(0, 0, 0, Time.deltaTime);
            if(SoftMudSprite.color.a <= 0.05) { Destroy(gameObject); }
        }
    }
}
