using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLove : Projectile
{
    public float moveSpeed;
    private Vector3 moveDirection;
    public GameObject mew;
    public int SpAPower;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
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
            Pokemon.PokemonHpChange(mew, playerControler.gameObject, 0, SpAPower, 0, Type.TypeEnum.Fairy);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            Destroy(gameObject);
        }
    }
    public void Initialize(float moveSpeed, Vector3 direction)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = direction;
    }
}
