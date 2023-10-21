using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAttack : Skill
{
    public float dashAmount;
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    List<Empty> enemylist = new List<Empty>();
    // Start is called before the first frame update
    void Start()
    {
        //获取冲刺方向
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = player.look;

        Dash();
    }

    void Dash()
    {
        Vector3 dashPosition;
        //使用射线判断瞬移路径上是否有障碍物，有则被阻碍
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
        RaycastHit2D raycastHitEmpty = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));
        RaycastHit2D raycastHitEmptyUp = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));
        RaycastHit2D raycastHitEmptyDown = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, Direction, dashAmount, LayerMask.GetMask("Empty", "EmptyFly"));
        if (raycastHit2D.collider)
        {
            //射线与物体碰撞的点即为瞬移的最终位置
            dashPosition = raycastHit2D.point;
        }
        else
        {
            dashPosition = player.transform.position + (Vector3)Direction * dashAmount;
        }
        PlayerRigibody.MovePosition(dashPosition);
        //检测路径上是否有敌人
        if (raycastHitEmpty || raycastHitEmptyUp || raycastHitEmptyDown)
        {
            if (raycastHitEmpty.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (!enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
            if (raycastHitEmptyUp.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (!enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
            if (raycastHitEmptyDown.collider.gameObject.CompareTag("Empty"))
            {
                Empty enemy = raycastHitEmpty.collider.GetComponent<Empty>();
                if (!enemylist.Contains(enemy))
                    enemylist.Add(enemy);
            }
        }
        if (enemylist.Count != 0)
        {
            for(int i = 0; i < enemylist.Count; i++)
            {
                HitAndKo(enemylist[i]);
            }
        }
    }
    void Update()
    {
        StartExistenceTimer();
    }
}
