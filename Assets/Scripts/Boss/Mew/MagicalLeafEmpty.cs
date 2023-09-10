using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MagicalLeafEmpty : Projectile
{
    public float speed = 4.5f; // 魔法叶子的速度
    public float lifetime = 6f; // 魔法叶子存在的时间

    private float timer; // 计时器

    private Transform target; // 跟随的目标

    public void SetTarget(GameObject Target)
    {
        target = Target.transform;
    }
    private void Start()
    {
        timer = lifetime; // 初始化计时器
    }

    private void Update()
    {
        if (target != null)
        {
            // 根据目标的位置朝向目标移动
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            // 更新朝向
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        timer -= Time.deltaTime; // 更新计时器

        if (timer <= 0f)
        {
            Destroy(gameObject); // 在计时结束后销毁魔法叶子对象
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Grass);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            Destroy(gameObject); // 在碰撞后销毁魔法叶子对象
        }
    }
}
