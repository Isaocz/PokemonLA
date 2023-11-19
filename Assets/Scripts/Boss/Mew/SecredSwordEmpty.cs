using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecredSwordEmpty : Projectile
{
    private float angle;
    private float radius;
    private Vector3 position;
    private float timer;
    private float randomangle;
    private GameObject target;
    private Vector3 target2;
    public void Initialize(float Angle, float Radius, GameObject Target, float randomAngle)
    {
        angle = Angle;
        radius = Radius;
        target = Target;
        randomangle = randomAngle;
    }
    void Start()
    {
        position = target.transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius;
        Destroy(gameObject, 3f);
        Invoke("RecordPosition", 1.7f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 1.7f)
        {
            float t = (1 - Mathf.Exp(-2 * timer)) / (1 + Mathf.Exp(-2 * timer));//用双曲正切函数来表示0-1转换的过程
            float currentAngle = Mathf.Lerp(angle, angle + randomangle, t);

            position = target.transform.position + Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right * radius;
            transform.position = position;
            transform.right = Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right * -1;
        }
        else
        {
            float moveSpeed = 16f; // 移动速度
            transform.position = Vector3.MoveTowards(transform.position, target2, moveSpeed * Time.deltaTime);
        }
    }
    void RecordPosition()
    {
        target2 = target.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Fighting);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }

        }
    }
}
