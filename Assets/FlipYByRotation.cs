using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipYByRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(transform.rotation.eulerAngles.z >= 90 && transform.rotation.eulerAngles.z < 270)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
