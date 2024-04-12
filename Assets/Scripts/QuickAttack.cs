using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuickAttack : Skill
{
    public float dashAmount;
    public float dashSpeed;
    public GameObject PlayerShadow;
    Sprite playerTexture;
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    float timer = 0f;
    List<Empty> enemylist = new List<Empty>();

    private bool isDashing = false;
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ��̷���
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = player.look;

        //��ȡ���ͼƬ
        playerTexture = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;

        Dash();
    }

    void Dash()
    {
        Vector3 dashPosition = new Vector3( Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).x, player.NowRoom.x*30 - 12.7f, player.NowRoom.x * 30 + 12.7f) , Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).y, player.NowRoom.y * 24 - 9.0f, player.NowRoom.y * 24 + 9.0f), 0);
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
    void Update()
    {
        StartExistenceTimer();
        if (isDashing)
        {
            //���������Ӱ
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0.02f;
                GameObject playershadow = Instantiate(PlayerShadow, player.transform.position, quaternion.identity);
                playershadow.GetComponent<SpriteRenderer>().sprite = playerTexture;
                Destroy(playershadow, 0.3f);
            }

            player.isInvincible = true;

            //��·���ϵĵ�������˺�
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Empty"))
                {
                    Empty enemy = collider.GetComponent<Empty>();
                    if(enemy != null && !enemylist.Contains(enemy))
                    {
                        enemylist.Add(enemy);
                        HitAndKo(enemy);
                    }
                }
            }
        }
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
