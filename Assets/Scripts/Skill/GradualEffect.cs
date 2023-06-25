using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualEffect : MonoBehaviour
{
    SpriteRenderer sp;
    //��ʱ��
    public float timer;
    //����ʱ��
    public float time = 0.5f;
    //�ȴ�ʱ�䣨������
    public float waittime = 2f;
    //͸���ȣ���Χ0-1
    public float alpha = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //����
        if (timer <= 0.5f)
        {
            alpha = (time - (0.5f-timer)) / time;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        }
        //����
        if (timer > waittime)
        {
            alpha = (time - (timer - waittime)) / time;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        }

    }
}
