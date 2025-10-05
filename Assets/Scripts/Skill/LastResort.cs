using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastResort : Skill
{


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null ) {
                HitAndKo(target);
                if (animator != null) { animator.SetTrigger("Hit"); }
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).position = target.transform.position;
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                transform.GetChild(0).parent = null;
            }
        }

    }
}
