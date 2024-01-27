using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRough : Skill
{
    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
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
            HitAndKo(target);
            if (SkillFrom == 2)
            {
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ٹ�����); }
                else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ӹ�����); }
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
                {
                    if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ٹ�����); }
                    else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.��ɫ���ӹ�����); }
                }
            }
            if (animator != null) { animator.SetTrigger("Hit"); }
            if (transform.childCount != 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = player.PlayerLocalScal;
                transform.DetachChildren();
            }
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.9f)
            {
                target.AtkChange(-1, 0.0f);
            }
        }
    }
}
