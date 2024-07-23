using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDown : Skill
{

    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject TackleBlast;

    bool MoveStop;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        player.playerData.DefBounsAlways += 2;
        player.playerData.SpDBounsAlways += 2;
        if (SkillFrom == 2) { player.isInvincibleAlways = true; }
        player.ReFreshAbllityPoint();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (!MoveStop) {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * 2.5f * player.speed * Time.deltaTime;
            postion.y += Direction.y * 2.5f * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            Instantiate(TackleBlast, target.transform.position, Quaternion.identity );
            HitAndKo(target);
        }
        if (other.tag == "Enviroment" || other.tag == "Room" || other.gameObject.tag == "Water")
        {
            MoveStop = true;
            ExistenceTime = 0.01f;
        }
    }

    private void OnDestroy()
    {
        player.playerData.DefBounsAlways -= 2;
        player.playerData.SpDBounsAlways -= 2;
        player.ReFreshAbllityPoint();
        if (SkillFrom == 2) { player.isInvincibleAlways = false; }

    }
}
