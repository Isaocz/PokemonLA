using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStone : MonoBehaviour
{

    public bool isInLake;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerControler>() != null)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerControler>() != null)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
