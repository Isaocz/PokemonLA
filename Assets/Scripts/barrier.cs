using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrier : MonoBehaviour
{
    PlayerControler player;
    SpriteRenderer sr;
    float timer;//计时器
    bool inFade;//是否需要渐变
    bool io;//Fade in & out
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        timer = 0;
        inFade = false;
        io = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inFade)
        {
            if (io && timer <= 0.5f)
            {
                timer += Time.deltaTime;
            }
            else if (!io && timer >= 0f)
            {
                timer -= Time.deltaTime;
            }
            if(timer > 0.5f)
            {
                timer = 0.5f;
            }
            else if(timer < 0f)
            {
                timer = 0f;
            }
            float t = Mathf.Clamp01(timer / 0.5f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, t);//进行渐变
            if(t >= 1)
            {
                inFade = false;
            }
        }
        float distance = Vector3.Distance(player.transform.position, transform.position);//计算物体与玩家间距离
        if (distance < 2f)
        {
            Fadein();
        }
        else
        {
            Fadeout();
        }

        if (Mew.MewBossKilled)
        {
            Destroy(gameObject);
        }
    }

    void Fadein()
    {
        inFade = true;
        io = true;
    }

    void Fadeout()
    {
        inFade = true;
        io = false;
    }
}
