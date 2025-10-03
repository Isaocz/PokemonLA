using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEdge : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    public GameObject TackleBlast;
    int DoneDmage;
    bool isHPPoinrPlus;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        player.isCanNotMove = true;
        player.isInvincibleAlways = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        Vector2 postion = PlayerRigibody.position;
        postion.x += Direction.x * 2.5f * player.speed * Time.deltaTime;
        postion.y += Direction.y * 2.5f * player.speed * Time.deltaTime;
        PlayerRigibody.position = postion;
        if ( ExistenceTime <= 0.15)
        {
            GetComponent<Animator>().SetTrigger("Over");
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
                int Dmage = target.EmptyHp + target.EmptyShield;
                HitAndKo(target);
                Dmage -= target.EmptyHp + target.EmptyShield;
                DoneDmage += Dmage;
                if(SkillFrom == 2 && target.EmptyHp <= 0 && !isHPPoinrPlus)
                {
                    player.ChangeHPW(new Vector2Int(1,2));
                    isHPPoinrPlus = true;
                }
            }
        }
    }


    private void OnDestroy()
    {
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
        if (DoneDmage > 0) {
            if(DoneDmage / 3 >= player.Hp)
            {
                Pokemon.PokemonHpChange(null, player.gameObject, (player.Hp-1), 0, 0, PokemonType.TypeEnum.IgnoreType);
            }
            else
            {
                Pokemon.PokemonHpChange(null, player.gameObject, DoneDmage / 3, 0, 0, PokemonType.TypeEnum.IgnoreType);
            }
            player.KnockOutPoint = 0;
        }
    }
}
