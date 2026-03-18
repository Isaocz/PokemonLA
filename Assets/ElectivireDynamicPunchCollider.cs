using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireDynamicPunchCollider : MonoBehaviour
{
    public Electivire ParentElectivire
    {
        get { return parentElectivire; }
        set { parentElectivire = value; }
    }
    Electivire parentElectivire;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        parentElectivire.DynamicPunchHit(collision.gameObject);
    }
}
