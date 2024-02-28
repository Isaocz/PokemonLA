using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpinnerMoveStop : MonoBehaviour
{

    private void Start()
    {
        BoxCollider2D b1 = transform.GetComponent<BoxCollider2D>();
        BoxCollider2D b2 = transform.parent.GetComponent<Skill>().player.GetComponent<BoxCollider2D>();
        b1.offset = b2.offset;
        b1.size = b2.size;
        b1.edgeRadius = b2.edgeRadius + 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Enviroment" || other.tag == "Room" || other.tag == "Water") && other.isTrigger == false)
        {
            transform.parent.GetComponent<IceSpinner>().MoveStopF();
        }
    }
}
