using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assurance : Skill
{
    public float dashAmount;
    public float dashSpeed;
    private float hp;
    private bool isDashing = false;
    private bool hpdown = false;
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    void Start()
    {
        if(SkillFrom != 2)
        {
            player.isCanNotMove = true;
        }

        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        hp = player.Hp;
        Timer.Start(this, 1f, () =>
        {
            Direction = player.look;
            Dash();
        });
    }

    void Update()
    {
        StartExistenceTimer();
        if(player.Hp < hp && hpdown == false)
        {
            hpdown = true;
            Damage *= 2;
            player.isCanNotMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
            }
        }
    }

    void Dash()
    {
        Vector3 dashPosition = new Vector3(Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).x, player.NowRoom.x * 30 - 12.7f, player.NowRoom.x * 30 + 12.7f), Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).y, player.NowRoom.y * 24 - 9.0f, player.NowRoom.y * 24 + 9.0f), 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(dashPosition, 1f);
        foreach (Collider2D collider in colliders)
        {//�����Ǽ��ˮ�棬�����㲻��ˮ�桢ǽ���ϰ�������λ��ǰ���Ƿ���ǽ���������ǣ�����ǽ��ˮ���������������λ�ƹ�ˮ�浫��λ�Ʋ���ǽ��ͬʱ���ᴫ�ͽ�ˮ��
            if (collider.CompareTag("Room") || collider.CompareTag("Enviroment") || collider.CompareTag("Water"))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment", "Water"));
                if (raycastHit2D.collider)
                {
                    dashPosition = raycastHit2D.point;
                }
                break;
            }
            else
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
                if (raycastHit2D.collider)
                {
                    dashPosition = raycastHit2D.point;
                }
            }
        }
        StartCoroutine(DashMovement(dashPosition));
    }

    IEnumerator DashMovement(Vector3 targetPosition)
    {
        isDashing = true;
        float startTime = Time.time;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float journeyLength = distance / dashSpeed;

        while (Time.time < startTime + journeyLength)
        {
            float distanceCovered = (Time.time - startTime) * dashSpeed;
            float fractionOfJourney = distanceCovered / distance;
            PlayerRigibody.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);
            yield return null;
        }

        // ���������ƶ�
        isDashing = false;
        player.isInvincible = false;
    }

}
