using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewStringShot : Projectile
{
    public GameObject WurmpleStringShot02;
    private float timer;
    private Vector3 startPos;
    private Vector3 endPos;

    public void SetTarget(Vector3 Pos)
    {
        endPos = Pos;
    }

    private void Awake()
    {
        AwakeProjectile();
    }

    private void OnEnable()
    {
        timer = 0f;
        startPos = transform.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, endPos, timer / 1.2f);
        if(timer > 1.2f)
        {
            Instantiate(WurmpleStringShot02, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") )
        {
            if (other.tag != ("Room"))
            {
                Instantiate(WurmpleStringShot02, (transform.position + other.transform.position) / 2, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            Destroy(gameObject);
        }
    }
}
