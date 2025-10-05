using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null)
        {
            playerControler.ChangeHp(-20, 0, 0);
            Destroy(gameObject);
        }
    }
}
