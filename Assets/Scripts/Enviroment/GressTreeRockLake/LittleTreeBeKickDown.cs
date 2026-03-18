using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTreeBeKickDown : ObjectBeKickDown
{
    public GameObject PS1;
    public GameObject PS2;



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControler Player = other.gameObject.GetComponent<PlayerControler>();
            if (Player != null && Player.PlayerBodySize == 2)
            {
                BeKickDown();
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override void BeKickDown()
    {
        isBeKickDown = true;
        GetComponent<Animator>().SetTrigger("KickDown");
        Instantiate(PS1, transform.position, Quaternion.identity);
        Instantiate(PS2, transform.position, Quaternion.identity);
    }

}
