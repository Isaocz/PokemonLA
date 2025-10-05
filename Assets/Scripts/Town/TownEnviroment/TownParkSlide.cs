using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownParkSlide : MonoBehaviour
{
    public float Speed;
    public Vector3 SlideDir; 

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position += Time.deltaTime * SlideDir * Speed;
        }
    }
}
