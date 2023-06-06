using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Start is called before the first frame update
    public new Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Empty empty;
    protected Vector3 BornPosition;
    // Start is called before the first frame update


    public void AwakeProjectile()
    {
        BornPosition = transform.position;
        if (GetComponent<SpriteRenderer>() != null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if (GetComponent<Rigidbody2D>() != null) { rigidbody2D = GetComponent<Rigidbody2D>(); }
    }

    // Update is called once per frame
    public void DestoryProjectile(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            spriteRenderer.material.color = spriteRenderer.material.color - new Color (0,0,0,3f * Time.deltaTime);
            if((transform.position - BornPosition).magnitude >= ProjectileRange + 3)
            {
                Destroy(gameObject);
            }

        }

    }

    //发射当前子弹
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }
}
