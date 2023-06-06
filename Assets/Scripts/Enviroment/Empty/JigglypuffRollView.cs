using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglypuffRollView : MonoBehaviour
{
    Jigglypuff jigglypuff;
    void Start()
    {
        jigglypuff = this.transform.parent.GetComponent<Jigglypuff>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water"))
        {
            switch (this.transform.name)
            {
                case "RollViewD":
                    jigglypuff.TurnToD = true;
                    break;
                case "RollViewU":
                    jigglypuff.TurnToU = true;
                    break;
                case "RollViewL":
                    jigglypuff.TurnToL = true;
                    break;
                case "RollViewR":
                    jigglypuff.TurnToR = true;
                    break;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water"))
        {
            switch (this.transform.name)
            {
                case "RollViewD":
                    jigglypuff.TurnToD = false;
                    break;
                case "RollViewU":
                    jigglypuff.TurnToU = false;
                    break;
                case "RollViewL":
                    jigglypuff.TurnToL = false;
                    break;
                case "RollViewR":
                    jigglypuff.TurnToR = false;
                    break;
            }
        }
    }
}
