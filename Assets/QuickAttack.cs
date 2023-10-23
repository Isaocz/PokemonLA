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
        //获取冲刺方向
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = player.look;

        //获取玩家图片
        playerTexture = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;

        Dash();
    }

    void Dash()
    {
        Vector3 dashPosition;
        //使用射线判断瞬移路径上是否有障碍物，有则被阻碍
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment", "Water"));
        if (raycastHit2D.collider)
        {
            //射线与物体碰撞的点即为瞬移的最终位置
            dashPosition = raycastHit2D.point;
        }
        else
        {
            dashPosition = player.transform.position + (Vector3)Direction * dashAmount;
        }

        StartCoroutine(DashMovement(dashPosition));
        RaycastHit2D raycastHitEmpty = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));
        RaycastHit2D raycastHitEmptyUp = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));
        RaycastHit2D raycastHitEmptyDown = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));

        //检测路径上是否有敌人
        if (raycastHitEmpty || raycastHitEmptyUp || raycastHitEmptyDown)
        {
            if (raycastHitEmpty.collider != null || raycastHitEmpty.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (enemy != null && !enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
            if (raycastHitEmptyUp.collider!= null || raycastHitEmptyUp.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (enemy != null && !enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
            if (raycastHitEmptyDown.collider != null || raycastHitEmptyDown.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (enemy != null && !enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
        }
        if (enemylist.Count != 0)
        {
            for (int i = 0; i < enemylist.Count; i++)
            {
                HitAndKo(enemylist[i]);
            }
        }
    

}
    void Update()
    {
        StartExistenceTimer();
        if (isDashing)
        {
            //设置玩家阴影
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0.02f;
                GameObject playershadow = Instantiate(PlayerShadow, player.transform.position, quaternion.identity);
                playershadow.GetComponent<SpriteRenderer>().sprite = playerTexture;
                Destroy(playershadow, 0.3f);
            }

            player.isInvincible = true;
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

        // 结束高速移动
        isDashing = false;
        player.isInvincible = false;
    }
}
