using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeAntidote : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null)
        {
            playerControler.ToxicFloatPlus(1);
            Destroy(gameObject);
        }
    }
}
