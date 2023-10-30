using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFireEffect : MonoBehaviour
{

    MagicalFire ParentMF;

    void Start()
    {
        ParentMF = transform.parent.GetComponent<MagicalFire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当敌人进入触发器时对敌人造成伤害并消失
        if(collision.tag == "Empty" || collision.tag == "Room" || collision.tag == "Enviroment")
        {
            Destroy(this.gameObject);
            if (collision.tag == "Empty") {
                Empty target = collision.GetComponent<Empty>();
                ParentMF.HitAndKo(target);
                if (!ParentMF.isSpDownDone) { target.SpAChange(-1, 0.0f); ParentMF.isSpDownDone = true; }
                if (ParentMF.SkillFrom == 2) {
                    target.EmptyBurnDone(0.4f, 10.0f, 0.1f + ((float)ParentMF.player.LuckPoint / 30.0f));
                }
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
