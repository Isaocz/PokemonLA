using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeButtonSmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Empty")
        {
            Empty e = collision.gameObject.GetComponent<Empty>();
            if (e != null && !e.isSilence)
            {
                e.Blind(0.8f, 1.0f);
            }
        }
    }
}
