using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayDay : Skill
{
    public float moveSpeed;
    public GameObject DropMoney;
    private bool hit;
    private bool isdrop;
    private SpriteRenderer sr;
    private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        transform.GetChild(1).gameObject.SetActive(false);
        hit = false;
        isdrop = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy)
            {
                HitAndKo(enemy);
                hit = true;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                if (enemy.EmptyHp <= 0 && !isdrop)
                {
                    isdrop = true;
                    int Drops = Count2_5();
                    for(int i = 0; i < Drops; i++)
                    {
                        Instantiate(DropMoney, enemy.transform.position +  new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.identity);
                    }
                }
            }
        }
        if(collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            hit = true;
            isdrop = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
