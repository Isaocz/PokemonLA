using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoochyenaTurnViewD : MonoBehaviour
{

    poochyena Poochyena;
    void Start()
    {
        Poochyena = this.transform.parent.GetComponent<poochyena>();
    }


private void OnTriggerStay2D(Collider2D collision)
{
    if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Empty"))
    {
        switch (this.transform.name)
        {
            case "TurnViewD":
                Poochyena.isEmptyD = false;
                break;
            case "TurnViewU":
                Poochyena.isEmptyU = false;
                break;
            case "TurnViewL":
                Poochyena.isEmptyL = false;
                break;
            case "TurnViewR":
                Poochyena.isEmptyR = false;
                break;
        }
    }

}
private void OnTriggerExit2D(Collider2D collision)
{
if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Empty"))
{
    switch (this.transform.name)
    {
        case "TurnViewD":
            Poochyena.isEmptyD = true;
            break;
        case "TurnViewU":
            Poochyena.isEmptyU = true;
            break;
        case "TurnViewL":
            Poochyena.isEmptyL = true;
            break;
        case "TurnViewR":
            Poochyena.isEmptyR = true;
            break;
    }
}
}
}
