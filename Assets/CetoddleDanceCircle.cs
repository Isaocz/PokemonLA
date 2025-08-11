using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetoddleDanceCircle : MonoBehaviour
{

    public Empty ParentEmpty;
    bool isDefDown;
    bool isSpDDown;

    List<Empty> EList = new List<Empty> { };

    private void Start()
    {
        Destroy(this.gameObject , 5.0f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject)
        {
            if (other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p)
                {
                    if (!isDefDown && p.playerData.DefBounsJustOneRoom > -4)
                    {
                        isDefDown = true;
                        p.playerData.DefBounsJustOneRoom--;
                        p.ReFreshAbllityPoint();
                    }
                    if (!isSpDDown && p.playerData.SpDBounsJustOneRoom > -4)
                    {
                        isSpDDown = true;
                        p.playerData.SpDBounsJustOneRoom--;
                        p.ReFreshAbllityPoint();
                    }
                }
            }
            else if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && !EList.Contains(e))
                {
                    EList.Add(e);
                    if (e.DefUpLevel > -4)
                    {
                        e.DefChange(-1, 15.0f);
                    }
                    if (e.SpDUpLevel > -4)
                    {
                        e.SpDChange(-1, 15.0f);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject)
        {
            if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && !EList.Contains(e))
                {
                    EList.Add(e);
                    if (e.DefUpLevel > -4)
                    {
                        e.DefChange(-1, 15.0f);
                    }
                    if (e.SpDUpLevel > -4)
                    {
                        e.SpDChange(-1, 15.0f);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject)
        {
            if (other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p)
                {
                    if (isDefDown)
                    {
                        isDefDown = false;
                        p.playerData.DefBounsJustOneRoom++;
                        p.ReFreshAbllityPoint();
                    }
                    if (isSpDDown)
                    {
                        isSpDDown = false;
                        p.playerData.SpDBounsJustOneRoom++;
                        p.ReFreshAbllityPoint();
                    }
                }
            }
            else if (other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && EList.Contains(e))
                {
                    EList.Remove(e);
                }
            }
        }
    }
}
