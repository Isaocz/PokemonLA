using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFireEmpty : Projectile
{
    Vector3 mewposition;
    CircleCollider2D collider2D;
    float speed= 7f;
    float rotationspeed;
    float timer;
    int stage;

    // Start is called before the first frame update
    void OnEnable()
    {
        timer = 0f;
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.radius = 0.25f;
        transform.GetChild(0).localScale = new Vector3(3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // 绕 Mew 旋转
        transform.RotateAround(mewposition, Vector3.forward, rotationspeed * Time.deltaTime);

        if(stage == 3)
        {
            speed = 9f * Mathf.Cos(timer / 7f * Mathf.PI);
        }
        // 向外移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(timer > 6.5f && timer < 7f)
        {
            float t = (timer - 6.5f) / 0.5f;
            transform.GetChild(0).localScale = Vector3.Lerp(new Vector3(3f, 3f), new Vector3(0, 0), t);
            collider2D.radius = Mathf.Lerp(0.25f, 0, t);
        }
        else if (timer > 7f)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Fire);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
    public void ps(Vector3 mewPosition, float rotationSpeed, int currentStage)
    {
        mewposition = mewPosition;
        rotationspeed = rotationSpeed;
        stage = currentStage;
    }
}
