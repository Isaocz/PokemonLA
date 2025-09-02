using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpearEmpty : Projectile
{
    public GameObject IcicleSpearBreak;
    private Vector3 PlayerPosition;
    private SpriteRenderer sr;
    private float initialMoveSpeed = 1.0f;
    private float finalMoveSpeed = 4f;
    private bool isMoving = false;
    private bool isFin;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        isFin = false;
        isMoving = false;
        StartCoroutine(MoveToCenter());
    }

    private IEnumerator MoveToCenter()
    {
        Vector2 targetPosition = PlayerPosition;
        float currentMoveSpeed = initialMoveSpeed;

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerPosition, currentMoveSpeed * Time.deltaTime);

            if (!isMoving && currentMoveSpeed == initialMoveSpeed && Time.time >= 1f)
            {
                isMoving = true;
                currentMoveSpeed = finalMoveSpeed;
                //计算冰锥朝向
                Vector2 direction = targetPosition - (Vector2)transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            yield return null;
        }
        // 冰锥到达圆心后执行其他逻辑
        sr.enabled = false;
        isFin = true;

        if(IcicleSpearBreak!= null)
        {
            GameObject isb = Instantiate(IcicleSpearBreak, transform.position, Quaternion.identity);
            Destroy(isb, 0.5f);
        }
        
        Destroy(gameObject, 1f);
        // 例如销毁冰锥或者改变冰锥的状态等
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isFin == false)
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage,0 , 0, PokemonType.TypeEnum.Ice);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
    public void sf(Vector3 playerPosition)
    {
        PlayerPosition = playerPosition;
    }
}
