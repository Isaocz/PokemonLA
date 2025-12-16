using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroseedRollView : MonoBehaviour
{
    //ÖÖ×ÓÌúÇòÄ¸Ìå
    Ferroseed ferroseed;

    void Start()
    {
        ferroseed = this.transform.parent.GetComponent<Ferroseed>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ferroseed.GetComponent<Collider2D>());
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water") || collision.transform.tag == ("Empty"))
        {
            switch (this.transform.name)
            {
                case "RollViewD":
                    ferroseed.TurnToD = true;
                    break;
                case "RollViewU":
                    ferroseed.TurnToU = true;
                    break;
                case "RollViewL":
                    ferroseed.TurnToL = true;
                    break;
                case "RollViewR":
                    ferroseed.TurnToR = true;
                    break;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == ("Room") || collision.transform.tag == ("Enviroment") || collision.transform.tag == ("Water") || collision.transform.tag == ("Empty"))
        {
            switch (this.transform.name)
            {
                case "RollViewD":
                    ferroseed.TurnToD = false;
                    break;
                case "RollViewU":
                    ferroseed.TurnToU = false;
                    break;
                case "RollViewL":
                    ferroseed.TurnToL = false;
                    break;
                case "RollViewR":
                    ferroseed.TurnToR = false;
                    break;
            }
        }
    }
}
