using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float Damage;

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            if (!playerControler.playerData.IsPassiveGetList[13])
            {
                playerControler.ChangeHp(-Damage, 0, 19);

                playerControler.KnockOutPoint = 1f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
        else if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            target.EmptyHpChange(10, 0, 19);
            target.EmptyKnockOut(0);
        }
    }


}
