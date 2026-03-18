using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBeKickDown : MonoBehaviour
{
    public bool isBeKickDown
    {
        get { return IsBeKickDown; }
        set { IsBeKickDown = value; }
    }
    bool IsBeKickDown;


    public virtual void BeKickDown()
    {
    }

}
