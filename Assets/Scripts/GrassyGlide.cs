using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyGlide : GrassSkill
{

    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject TackleBlast;
    float MoveSpeed;

    List<GameObject> GrassList = new List<GameObject>();

    bool MoveStop;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        if (player.isInGrassyTerrain || (SkillFrom == 2 && player.InGressCount.Count != 0)) {
            player.isInvincibleAlways = true;
        }
        MoveSpeed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SkillFrom == 2)
        {
            for (int i = 0; i < player.InGressCount.Count; i++ )
            {
                if (!GrassList.Contains(player.InGressCount[i]))
                {
                    GrassList.Add(player.InGressCount[i]);
                    Damage += 3;
                    MoveSpeed += 0.25f;
                }
            }
        }

        StartExistenceTimer();
        if (ExistenceTime > 0.76f)
        {
            if (!MoveStop)
            {
                Vector2 postion = PlayerRigibody.position;
                postion.x += Direction.x * MoveSpeed * player.speed * Time.deltaTime;
                postion.y += Direction.y * MoveSpeed * player.speed * Time.deltaTime;
                PlayerRigibody.position = postion;
            }
        }
        else
        {
            MoveStop = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger) {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
                HitAndKo(target);
            }
            if (other.tag == "Enviroment" || other.tag == "Room")
            {
                MoveStop = true;
                ExistenceTime = 0.77f;
            }
        }
    }

    private void OnDestroy()
    {
        player.isInvincibleAlways = false;
    }
}
