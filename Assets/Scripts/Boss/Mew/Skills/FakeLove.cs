using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLove : Projectile
{
    public float CardinalSpeed;
    private float timer;
    private Vector3 moveDirection;
    public GameObject mew;
    public int SpAPower;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        float currentSpeed;
        currentSpeed = Mathf.Pow(CardinalSpeed, timer);
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
        // 计算旋转角度
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        // 应用旋转角度
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(mew, collision.gameObject, 0, SpAPower, 0, PokemonType.TypeEnum.Fairy);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                playerControler.ConfusionFloatPlus(0.4f);
            }
            Destroy(gameObject);
        }
    }
    public void Initialize(float moveSpeed, Vector3 direction)
    {
        this.CardinalSpeed = moveSpeed;
        this.moveDirection = direction;
    }
}
