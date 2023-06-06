using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flail : Skill
{
    bool IsTriggerDone = false;
    public GameObject FlailWater;

    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Instantiate(FlailWater, player.transform.position + Vector3.up*0.5f, Quaternion.identity , player.transform);
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        if (!IsTriggerDone) { StartExistenceTimer(); }
    }

    void FlailDamage(float PlayerHPPer)
    {
        if      (PlayerHPPer >= 0.6875f) { Damage = 20; }
        else if (PlayerHPPer >= 0.3542f && PlayerHPPer < 0.6875f) { Damage = 40; }
        else if (PlayerHPPer >= 0.2083f && PlayerHPPer < 0.3542f) { Damage = 80; }
        else if (PlayerHPPer >= 0.1042f && PlayerHPPer < 0.2083f) { Damage = 100; }
        else if (PlayerHPPer >= 0.0417f && PlayerHPPer < 0.1042f) { Damage = 150; }
        else if (PlayerHPPer < 0.0417f) { Damage = 200; }



    }

    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            IsTriggerDone = true;
            Empty target = other.GetComponent<Empty>();
            FlailDamage(((float)(player.Hp)) / ((float)(player.maxHp)));
            Debug.Log(((float)(player.Hp)) / ((float)(player.maxHp)));
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            gameObject.transform.position = other.transform.position;
        }
    }
}
