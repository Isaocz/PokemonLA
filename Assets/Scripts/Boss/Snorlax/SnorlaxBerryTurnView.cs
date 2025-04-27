using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxBerryTurnView : MonoBehaviour
{
    public int TurnViewIndex;
    SnorlaxBerry snorlaxBerry;
    void Start()
    {
        snorlaxBerry = this.transform.parent.GetComponent<SnorlaxBerry>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water") || collision.transform.tag == ("Player") || collision.transform.tag == ("Projectel") || (collision.transform.tag == ("Empty") && snorlaxBerry.isCanBeEat))
        {
            switch (TurnViewIndex)
            {
                case 1:
                    snorlaxBerry.TurnToD = true;
                    break;
                case 2:
                    snorlaxBerry.TurnToU = true;
                    break;
                case 3:
                    snorlaxBerry.TurnToL = true;
                    break;
                case 4:
                    snorlaxBerry.TurnToR = true;
                    break;
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water") ||  collision.transform.tag == ("Player") || collision.transform.tag == ("Projectel") || (collision.transform.tag == ("Empty") && snorlaxBerry.isCanBeEat))
        {
            switch (TurnViewIndex)
            {
                case 1:
                    snorlaxBerry.TurnToD = false;
                    break;
                case 2:
                    snorlaxBerry.TurnToU = false;
                    break;
                case 3:
                    snorlaxBerry.TurnToL = false;
                    break;
                case 4:
                    snorlaxBerry.TurnToR = false;
                    break;
            }
        }
    }
}
