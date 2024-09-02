using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlayerCollider : MonoBehaviour
{
    public BerryTree ParentBerryTree;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" &&( other.gameObject.layer == LayerMask.NameToLayer("PlayerFly") || other.gameObject.layer == LayerMask.NameToLayer("PlayerJump")))
        {
            ParentBerryTree.player = other.gameObject.GetComponent<PlayerControler>();
            if (ParentBerryTree.player != null)
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Drop");
            }
        }
    }
}
