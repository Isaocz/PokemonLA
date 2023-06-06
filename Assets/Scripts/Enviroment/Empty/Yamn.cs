using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yamn : Projectile
{
    bool isDestory;
    // Start is called before the first frame update
    private void Awake()
    {
        AwakeProjectile();
    }

    private void Start()
    {
        Invoke("DestoryYamn", 13);
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = new Vector3(Mathf.Clamp(transform.lossyScale.x + Time.deltaTime, 0, 1), Mathf.Clamp(transform.lossyScale.y + 1.2f*Time.deltaTime, 0, 1.4f) , 1);
        DestoryProjectile(10);
        if (isDestory)
        {
            spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, Time.deltaTime);
            if (spriteRenderer.material.color.a <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == ("Player"))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            playerControler.SleepFloatPlus(0.4f);
        }
        if (other.tag == ("Room") || other.tag == ("Enviroment"))
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    void DestoryYamn()
    {
        isDestory = true;
    }
}
