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
        //�����˽��봥����ʱ�Ե�������˺�����ʧ
        if(collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            transform.parent.GetComponent<MagicalFire>().HitAndKo(target);
            target.LevelChange(-1, "SpA");
            Destroy(this.gameObject);
        }
    }
}
