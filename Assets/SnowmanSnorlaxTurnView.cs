using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanSnorlaxTurnView : MonoBehaviour
{
    public int TurnViewIndex;
    SnowmanSnorlax snowmanSnorlax;
    void Start()
    {
        snowmanSnorlax = this.transform.parent.GetComponent<SnowmanSnorlax>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment"))
        {
            switch (TurnViewIndex)
            {
                case 1:
                    snowmanSnorlax.isEmptyD = false;
                    break;
                case 2:
                    snowmanSnorlax.isEmptyU = false;
                    break;
                case 3:
                    snowmanSnorlax.isEmptyL = false;
                    break;
                case 4:
                    snowmanSnorlax.isEmptyR = false;
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
                    snowmanSnorlax.isEmptyD = true;
                    break;
                case 2:
                    snowmanSnorlax.isEmptyU = true;
                    break;
                case 3:
                    snowmanSnorlax.isEmptyL = true;
                    break;
                case 4:
                    snowmanSnorlax.isEmptyR = true;
                    break;
            }
        }
    }
}
