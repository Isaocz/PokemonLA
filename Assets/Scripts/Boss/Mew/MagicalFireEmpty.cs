using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFireEmpty : Projectile
{
    Vector3 mewposition;
    float speed= 7f;
    float rotationspeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �� Mew ��ת
        transform.RotateAround(mewposition, Vector3.forward, rotationspeed * Time.deltaTime);

        // �����ƶ�
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, SpDmage, 0, Type.TypeEnum.Fire);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
    public void ps(Vector3 mewPosition, float rotationSpeed)
    {
        mewposition = mewPosition;
        rotationspeed = rotationSpeed;
    }
}
