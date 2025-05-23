using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSlashMew : Projectile
{
    public float airSlashSpeed = 9f;
    public int numSplitAirSlashes = 6;
    public GameObject SplitAirSlashPrefab;
    float angle;

    private Transform target; //目标
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        target = GameObject.FindWithTag("Player").transform;
        //计算朝向
        Vector3 direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * airSlashSpeed * Time.deltaTime * Mathf.Exp(timer));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //如果碰到障碍物或玩家，会召唤碎片
        if (collision.CompareTag("Enviroment") || collision.CompareTag("Room") || collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Player"))
            {
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Flying);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 5f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            SplitAirSlashes(collision.transform.position);
            Destroy(gameObject);
        }
    }
    void SplitAirSlashes(Vector3 collisionPosition)
    {
        for (int i = 0; i < numSplitAirSlashes; i++)
        {
            //计算角度
            float splitangle =angle + i * (360f / numSplitAirSlashes);
            //召唤碎片
            GameObject splitAirSlash = ObjectPoolManager.SpawnObject(SplitAirSlashPrefab, transform.position, Quaternion.Euler(0f, 0f, splitangle));
            splitAirSlash.GetComponent<SplitAirSlash>().empty = empty;
        }
    }
}
