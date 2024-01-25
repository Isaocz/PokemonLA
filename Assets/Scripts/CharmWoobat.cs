using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmWoobat : MonoBehaviour
{
    public Empty ParentEmpty;
    bool isAtkDown;
    bool isSpADown;

    List<Empty> EList = new List<Empty> { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject) {
            if (other.tag == "Player") {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p) {
                    if (!isAtkDown && p.playerData.AtkBounsJustOneRoom > -3)
                    {
                        isAtkDown = true;
                        p.playerData.AtkBounsJustOneRoom--;
                        p.ReFreshAbllityPoint();
                    }
                    if (!isSpADown && p.playerData.SpABounsJustOneRoom > -3)
                    {
                        isSpADown = true;
                        p.playerData.SpABounsJustOneRoom--;
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
                    if (e.AtkUpLevel > -3) {
                        e.AtkChange(-1, 15.0f);
                    }
                    if (e.SpAUpLevel > -3) {
                        e.SpAChange(-1, 15.0f);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject) {
            if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && !EList.Contains(e))
                {
                    EList.Add(e);
                    if (e.AtkUpLevel > -3)
                    {
                        e.AtkChange(-1, 15.0f);
                    }
                    if (e.SpAUpLevel > -3)
                    {
                        e.SpAChange(-1, 15.0f);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject) {
            if (other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p)
                {
                    if (isAtkDown)
                    {
                        isAtkDown = false;
                        p.playerData.AtkBounsJustOneRoom++;
                        p.ReFreshAbllityPoint();
                    }
                    if (isSpADown)
                    {
                        isSpADown = false;
                        p.playerData.SpABounsJustOneRoom++;
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
