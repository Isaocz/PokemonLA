using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmpleStringShot02 : MonoBehaviour
{

    float StringShotExitTimer = 0;
    SpriteRenderer Sp;
    private void Start()
    {
        Sp = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        StringShotExitTimer += Time.deltaTime;
        if (StringShotExitTimer > 2.5f) { Sp.material.color = Sp.material.color -  new Color(0,0,0,0.75f*Time.deltaTime); }
        if (StringShotExitTimer > 3.9f) { Destroy(gameObject); }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == ("Player")) { other.GetComponent<PlayerControler>().SpeedChange(); }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player")) {
            other.GetComponent<PlayerControler>().SpeedRemove01(2.5f); 
        }
    }
}
