using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFireEmpty : Projectile
{
    Vector3 mewposition;
    float speed= 7f;
    float rotationspeed;
    float timer;
    int stage;

    // Start is called before the first frame update
    void OnEnable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        // 绕 Mew 旋转
        transform.RotateAround(mewposition, Vector3.forward, rotationspeed * Time.deltaTime);

        if(stage == 3)
        {
            timer += Time.deltaTime;
            speed = 9f * Mathf.Cos(timer / 7f * Mathf.PI);
        }
        // 向外移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);
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
