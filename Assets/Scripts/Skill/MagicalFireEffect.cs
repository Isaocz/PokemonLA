using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFireEffect : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当敌人进入触发器时对敌人造成伤害并消失
        if(collision.tag == "Empty" || collision.tag == "Room" || collision.tag == "Enviroment")
        {
            Destroy(this.gameObject);
            if (collision.tag == "Empty") {
                Empty target = collision.GetComponent<Empty>();
                transform.parent.GetComponent<MagicalFire>().HitAndKo(target);
                target.SpAChange(-1, 0.0f);
            }
        }
    }

    private void OnDestroy()
    {
        var main = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.loop = false;
        transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        transform.DetachChildren();
    }
}
