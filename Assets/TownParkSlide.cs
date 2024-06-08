using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownParkSlide : MonoBehaviour
{
    public float Speed;

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position += Time.deltaTime * (new Vector3(2.0f, 1.0f , 0)) * Speed;
        }
    }
}
