using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleCrashOBJ : MonoBehaviour
{
    public bool isBreak;
    public int ColliderCount;
    public bool isUnBreakable;

    private void Start()
    {
        if (!isUnBreakable) {
            Invoke("IceBreak", 20);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isBreak && other.gameObject.tag != "Enviroment" && other.gameObject.tag != "Room" && other.gameObject.tag != "Item" && other.gameObject.tag != "Spike")
        {
            ColliderCount++;
            if (ColliderCount >= Random.Range(1, 6))
            {
                IceBreak();
            }
            
        }
    }



    public void IceBreak()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            transform.parent.GetComponent<Animator>().SetTrigger("Break");

            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;
            
            var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            Emission2.enabled = false;
            Main2.loop = false;

            transform.DetachChildren();
        }
    }



    public void DestorySelf()
    {
        Destroy(gameObject);
    }

}
