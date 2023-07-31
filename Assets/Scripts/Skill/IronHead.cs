using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronHead : Skill
{
    GameObject HitEffect;

    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HitEffect = transform.GetChild(0).gameObject;
        HitEffect.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    bool isBounsDone;
    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if(SkillFrom == 2 && target.EmptyHp <= 0 && !isBounsDone)
                {
                    isBounsDone = true;
                    if (Random.Range(0.0f, 1.0f) >= 0.5f && player.playerData.DefBounsJustOneRoom <= 8) {
                        player.playerData.DefBounsJustOneRoom++;
                    }
                    else {
                        if (player.playerData.SpDBounsJustOneRoom <= 8) { player.playerData.SpDBounsJustOneRoom++; }
                    }
                    player.ReFreshAbllityPoint();
                }
                HitEffect.transform.parent = target.transform;
                HitEffect.SetActive(true);
                if (animator != null) { animator.SetTrigger("Hit"); }
            }
        }
    }
}
