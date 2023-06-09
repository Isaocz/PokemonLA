using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxTurnView : MonoBehaviour
{
    public int TurnViewIndex;
    Snorlax snorlax;
    void Start()
    {
        snorlax = this.transform.parent.GetComponent<Snorlax>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment"))
        {
            switch (TurnViewIndex)
            {
                case 1:
                    snorlax.isEmptyD = false;
                    break;
                case 2:
                    snorlax.isEmptyU = false;
                    break;
                case 3:
                    snorlax.isEmptyL = false;
                    break;
                case 4:
                    snorlax.isEmptyR = false;
                    break;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment"))
        {
            switch (TurnViewIndex)
            {
                case 1:
                    snorlax.isEmptyD = true;
                    break;
                case 2:
                    snorlax.isEmptyU = true;
                    break;
                case 3:
                    snorlax.isEmptyL = true;
                    break;
                case 4:
                    snorlax.isEmptyR = true;
                    break;
            }
        }
    }
}
