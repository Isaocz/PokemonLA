using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPoisonEmpty : Projectile
{
    public float LaunchSpeed = 8f;
    public float BackSpeed = 4f;
    private int phase;
    Vector3 direction;
    Vector3 Backdirection;
    private Rigidbody2D rb;

    private bool canHurt;
    private float timer;

    /// <summary>
    /// 初始化十字毒刃
    /// </summary>
    /// <param name="Direction">方向</param>
    /// <param name="Phase">阶段，可去除</param>
    public void Initialize(Vector3 Direction, int Phase)
    {
        direction = Direction;
        phase = Phase;
    }
    public void Initialize(Vector3 Direction)
    {
        direction = Direction;
        phase = 1;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 13f);
        canHurt = false;
        timer = 0f;
        if (phase == 3)
        {
            float floatingX = Random.Range(-0.5f, 0.5f);
            float floatingY = Random.Range(-0.5f, 0.5f);
            Backdirection = ((-direction) + new Vector3(floatingX, floatingY, 0)).normalized;
            LaunchSpeed = LaunchSpeed * 1.2f;
            BackSpeed = BackSpeed * 1.2f;
        }
        else
        {
            Backdirection = (-direction);
        }

    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer < ((phase == 3) ? (3f / 1.2f) : 3f))  
        {
            rb.velocity = direction * LaunchSpeed;
            //发射时不能伤害玩家
            canHurt = false;
        }
        else
        {
            rb.velocity = Backdirection * BackSpeed;
            canHurt = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canHurt == true && collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Poison);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            playerControler.ToxicFloatPlus(0.3f);
        }
    }
}
