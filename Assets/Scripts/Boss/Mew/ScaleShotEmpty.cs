using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleShotEmpty : Projectile
{
    private float initialMoveSpeed = 5f; // 初始移动速度
    private float slowMoveSpeed = 1f; // 变慢后的移动速度
    private float fastMoveSpeed = 7f; // 再次加速后的移动速度

    private float timer; // 计时器
    private bool isSpeedIncreased; // 是否已经加速过

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        isSpeedIncreased = false;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpeedIncreased)
        {
            // 先快
            transform.Translate(Vector3.up * initialMoveSpeed * Time.deltaTime);
        }
        else
        {
            // 慢
            transform.Translate(Vector3.up * slowMoveSpeed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (timer >= 1f && !isSpeedIncreased)
        {
            // 在2秒后变慢
            isSpeedIncreased = true;
            timer = 1f;
        }

        if (timer >= 2f && isSpeedIncreased)
        {
            // 再过2秒变快
            slowMoveSpeed = fastMoveSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 80, 0, Type.TypeEnum.Dragon);
        }
    }
}
