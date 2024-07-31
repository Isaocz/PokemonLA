using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOff : Skill
{
    public GameObject TackleBlast;
    bool isKnockOffHitDone;
    int PlayerHP;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        PlayerHP = player.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target.IsHaveDropItem)
            {
                target.EmptyDrop();
                Damage *= 1.5f;
            }
            Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
            HitAndKo(target);
            isKnockOffHitDone = true;
        }
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2 && isKnockOffHitDone && PlayerHP > player.Hp)
        {
            Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, (PlayerHP - player.Hp) / 2, Type.TypeEnum.No);
        }
    }
}
