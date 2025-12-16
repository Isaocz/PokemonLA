using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClefairyGravity : MonoBehaviour
{
    public Empty ParentEmpty;

    private void OnTriggerStay2D(Collider2D other)
    {
        if ( other.tag == ("Player"))
        {
            if (!ParentEmpty.isEmptyInfatuationDone) {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p != null && !p.isSpeedChange && !p.playerData.IsPassiveGetList[13])
                {
                    p.SpeedChange();
                }
            }
            else
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p != null && p.isSpeedChange)
                {
                    p.SpeedRemove01(0.2f);
                }
            }
        }
        if ( other.tag == ("Empty"))
        {
            if (ParentEmpty.isEmptyInfatuationDone) {
                Empty e = other.GetComponent<Empty>();
                if (e != null && !e.isSpeedChange)
                {
                    e.SpeedChange();
                }
            }
            else
            {
                Empty e = other.GetComponent<Empty>();
                if (e != null && e.isSpeedChange)
                {
                    e.SpeedRemove01(0.2f);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!ParentEmpty.isEmptyInfatuationDone && other.tag == ("Player"))
        {
            if (other.GetComponent<PlayerControler>() != null)
            {
                other.GetComponent<PlayerControler>().SpeedRemove01(0.2f);
            }
        }
        if (ParentEmpty.isEmptyInfatuationDone && other.tag == ("Empty"))
        {
            if (other.GetComponent<Empty>() != null)
            {
                other.GetComponent<Empty>().SpeedRemove01(0.2f);
            }
        }
    }
}
