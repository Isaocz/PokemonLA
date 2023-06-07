using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientPower : Skill
{
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        //移动刚体
        Vector3 postion = transform.position;
        postion.x += direction.x * 4.5f * Time.deltaTime;
        postion.y += direction.y * 4.5f * Time.deltaTime;
        transform.position = postion;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            //10%几率触发所有效果加成（受幸运值影响）
            if (Random.Range(0, 9) + (float)player.LuckPoint >= 8)
            {
                player.playerData.AtkBounsJustOneRoom += 1;
                player.playerData.SpABounsJustOneRoom += 1;
                player.playerData.DefBounsJustOneRoom += 1;
                player.playerData.SpDBounsJustOneRoom += 1;
                player.playerData.SpeBounsJustOneRoom += 1;
            }
        }
        else if (other.tag == "Room" || other.tag == "Enviroment")
        {
            //触碰墙体后消失
            Destroy(gameObject);
        }
    }
}
