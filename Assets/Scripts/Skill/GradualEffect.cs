using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualEffect : MonoBehaviour
{
    SpriteRenderer sp;
    //计时器
    public float timer;
    //渐变时间
    public float time = 0.5f;
    //等待时间（淡出）
    public float waittime = 2f;
    //透明度，范围0-1
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
        //淡入
        if (timer <= 0.5f)
        {
            alpha = (time - (0.5f-timer)) / time;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        }
        //淡出
        if (timer > waittime)
        {
            alpha = (time - (timer - waittime)) / time;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        }

    }
}
