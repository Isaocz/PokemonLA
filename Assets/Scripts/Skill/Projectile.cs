using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Start is called before the first frame update
    public new Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Empty empty;
    public FollowBaby Baby;
    public int BabyLevel;
    protected Vector3 BornPosition;
    float Speed;
    protected Vector2 LunchDirection;
    public int Dmage;
    public int SpDmage;
    public PokemonType.TypeEnum ProType;
    
    // Start is called before the first frame update


    public void AwakeProjectile()
    {
        BornPosition = transform.position;
        if (GetComponent<SpriteRenderer>() != null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if (GetComponent<Rigidbody2D>() != null) { rigidbody2D = GetComponent<Rigidbody2D>(); }
    }

    // Update is called once per frame
    public void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            if (spriteRenderer.material.color.a >= 0)
            {
                spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            }
            if ((transform.position - BornPosition).magnitude >= ProjectileRange + 3)
            {
                Destroy(gameObject);
            }

        }
    }


    public void CollisionDestory()
    {
        spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
        if (spriteRenderer.material.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    //发射当前子弹
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }

    public void LaunchNotForce(Vector2 direction, float speed)
    {
        //Debug.Log(direction);
        LunchDirection = direction.normalized; Speed = speed;
    }
    protected void MoveNotForce()
    {
        //Debug.Log ((LunchDirection.x * Speed * Time.deltaTime).ToString() +  (LunchDirection.y * Speed * Time.deltaTime).ToString() + name );
        transform.position = transform.position + new Vector3(LunchDirection.x * Speed * Time.deltaTime , LunchDirection.y * Speed * Time.deltaTime, 0);
    }
}
