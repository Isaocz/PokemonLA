using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpearEmpty : Projectile
{
    private Vector3 PlayerPosition;
    private float initialMoveSpeed = 0.5f;
    private float finalMoveSpeed = 3f;
    private bool isMoving = false;

    private void Start()
    {
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
                //�����׶����
                Vector2 direction = targetPosition - (Vector2)transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            yield return null;
        }
        // ��׶����Բ�ĺ�ִ�������߼�
        Destroy(gameObject);
        // �������ٱ�׶���߸ı��׶��״̬��
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 90, 0, Type.TypeEnum.Ice);
        }
    }
    public void sf(Vector3 playerPosition)
    {
        PlayerPosition = playerPosition;
    }
}
