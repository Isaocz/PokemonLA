using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSeedShoot : Skill
{
    public float moveSpeed;
    public Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }
}
