using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrickeyunrMoveShadow : MonoBehaviour
{

    public SpriteRenderer sprite01;
    public SpriteRenderer sprite02;


    // Update is called once per frame
    void Update()
    {

        sprite01.color -= new Color(0,0,0,Time.deltaTime*1.7f);
        sprite02.color -= new Color(0,0,0,Time.deltaTime* 1.7f);
        if (sprite01.color.a < 0.05f)
        {
            Destroy(gameObject);
        }

    }
}
