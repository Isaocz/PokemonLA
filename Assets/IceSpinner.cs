using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpinner : Skill
{
    bool IsFrozenDone = false;
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    bool MoveStop;
    float PlayerTurnTimer;
    Vector2 PlayerLookAt;
    Vector2 d;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = Vector3.zero;
        PlayerLookAt = player.look;
        d = PlayerLookAt;
        player.isRapidSpin = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        PlayerTurnTimer += Time.deltaTime;


        if (ExistenceTime <= 0.5f && MoveStop == false) { MoveStopF(); }
        if ((ExistenceTime <= 0.7f && ExistenceTime > 0.1f) && (player.animator.GetFloat("LookX") == PlayerLookAt.x) && (player.animator.GetFloat("LookY") == PlayerLookAt.y))
        {
            if (PlayerTurnTimer >= 0.03f)
            {

                PlayerTurnTimer = 0;
                d = TurnPlayer();
                Debug.Log(d);
            }
            player.animator.SetFloat("LookX", d.x);
            player.animator.SetFloat("LookY", d.y);
        }

        if (!MoveStop)
        {
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
            if (!IsFrozenDone)
            {
                target.Frozen(7.5f, 1, 0.2f + (float)player.LuckPoint / 30);
                IsFrozenDone = true;
            }
            HitAndKo(target);
        }
        if (other.GetComponent<GrassyTerrain>())
        {
            if (other.transform.childCount != 0) {
                for (int i = 0; i < other.transform.childCount; i++)
                {
                    Debug.Log(other.transform);
                    Debug.Log(other.transform.childCount);
                    Debug.Log(other.transform.GetChild(i));
                    if (other.transform.GetChild(i).GetComponent<SkillRangeCircle>()) {
                        other.transform.GetChild(i).GetComponent<SkillRangeCircle>().DestroyCircle();
                    }
                    if (other.transform.GetChild(i).GetComponent<ParticleSystem>()) {
                        var e = other.transform.GetChild(i).GetComponent<ParticleSystem>().emission;
                        e.enabled = false;
                    }
                }
                other.transform.DetachChildren();
            }
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        player.isRapidSpin = false;
    }

    public void MoveStopF()
    {
        MoveStop = true;
        ExistenceTime = 0.5f;
        transform.GetChild(4).parent = null;
        transform.GetChild(4).parent = null;
    }

    Vector2 TurnPlayer()
    {
        if (d.x == 0)
        {
            Debug.Log(1);
            return new Vector2(d.y, 0);
        }
        else if (d.y == 0)
        {
            Debug.Log(2);
            return new Vector2(0, -d.x);
        }
        else
        {
            Debug.Log(3);
            return PlayerLookAt;
        }
        //10 0-1 -10 01
    }
}
