using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyLightMask : MonoBehaviour
{

    public SpriteRenderer ParentSprite;

    public SpriteRenderer SelfSprite;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SelfSprite.sprite != ParentSprite.sprite)
        {
            SelfSprite.sprite = ParentSprite.sprite;
        }
    }
}
