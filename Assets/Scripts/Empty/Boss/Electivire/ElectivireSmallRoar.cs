using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireSmallRoar : MonoBehaviour
{

    /// <summary>
    /// ¸¸µç»÷Ä§ÊÞ
    /// </summary>
    public Electivire ParentElectivire;


    public float DestroyTime;


    private void Start()
    {
        Timer.Start( this , DestroyTime, ()=> { Destroy(this.gameObject); });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null && ParentElectivire != null)
            {
                ParentElectivire.MarkPlayerBeSmallRoar(p);
            }
        }
    }

}
