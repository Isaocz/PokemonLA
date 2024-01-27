using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidSpinMoveStop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {       
        if ((other.tag == "Enviroment" || other.tag == "Room" || other.tag == "Water") && other.isTrigger == false)
        {
            transform.parent.GetComponent<RapidSpin>().MoveStopF();
        }
    }
}
