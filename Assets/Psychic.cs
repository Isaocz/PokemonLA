using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psychic : Skill
{
    public float moveSpeed;
    private bool ishit;
    // Start is called before the first frame update
    void Start()
    {
        ishit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ishit)
        {
            StartExistenceTimer();
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty") && !ishit)
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy != null)
            {
                HitAndKo(enemy);
                ishit = true;

                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                Destroy(gameObject, 1f);

                if (Random.Range(0f, 1f) + player.LuckPoint / 30f > 0.8)
                    enemy.SpDChange(-1, 4f);
            }
        }
        if((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && !ishit)
        {
            ishit = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}
