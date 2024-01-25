using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayDay : Skill
{
    public float moveSpeed;
    public RandomStarMoney DropMoney;
    private bool hit;
    private bool isdrop;
    private SpriteRenderer sr;
    private TrailRenderer tr;
    ParticleSystem PS;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        transform.GetChild(1).gameObject.SetActive(false);
        hit = false;
        isdrop = false;
        PS = transform.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            StartExistenceTimer();
            if(ExistenceTime < 0.5f)
            {
                float t = 1 - ExistenceTime / 0.5f;
                sr.color = Color.Lerp(new Color(sr.color.r, sr.color.g, sr.color.b, 1f), new Color(sr.color.r, sr.color.g, sr.color.b, 0f), t);
                tr.endColor = Color.Lerp(new Color(sr.color.r, sr.color.g, sr.color.b, 1f), new Color(sr.color.r, sr.color.g, sr.color.b, 0f), t);
                tr.startColor = Color.Lerp(new Color(sr.color.r, sr.color.g, sr.color.b, 1f), new Color(sr.color.r, sr.color.g, sr.color.b, 0f), t);
            }
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject, 1.05f);
        }
    }
    //0.65 0.25 0.1   2: 0.4425 3: 0.325 4: 0.195  5 :0.05  6:0.01
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy)
            {
                HitAndKo(enemy);
                var main = PS.main;
                main.loop = false;
                hit = true;
                sr.color = new Color(0,0,0,0);
                transform.GetChild(1).gameObject.SetActive(true);
                if (enemy.EmptyHp <= 0 && !isdrop)
                {
                    isdrop = true;
                    int Drops = Count1_3() + ((SkillFrom == 2)? Count1_3() : 0 );
                    for(int i = 0; i < Drops; i++)
                    {
                        Instantiate(DropMoney, enemy.transform.position +  new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.identity).isLunch = true;
                    }
                }
            }
        }
        if(collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            hit = true;
            isdrop = true;
            sr.color = new Color(0, 0, 0,0);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
