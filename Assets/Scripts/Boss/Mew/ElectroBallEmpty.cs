using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectroBallEmpty : Projectile
{
    Vector3 mewposition;
    float speed = 1f;
    float rotationspeed;

    // Start is called before the first frame update
    void OnEnable()
    {
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        // 绕 Mew 旋转
        transform.RotateAround(mewposition, Vector3.forward, rotationspeed * Time.deltaTime);

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
                int rate = (int)(empty.speed / playerControler.speed);
                switch (rate)
                {
                    case 1: SpDmage = 60; break;
                    case 2: SpDmage = 80; break;
                    case 3: SpDmage = 120; break;
                    case 4: SpDmage = 150; break;
                    default: SpDmage = 40; break;
                }
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                playerControler.ParalysisFloatPlus(0.35f);
            }
        }
    }
    public void Initialize(Vector3 mewPosition, float rotationSpeed)
    {
        mewposition = mewPosition;
        rotationspeed = rotationSpeed;
    }
}
