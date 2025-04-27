using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionBronzong : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryByRange(20);
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player") || other.tag == ("Room"))
        {
            isDestory = true;
            transform.GetComponent<EmptyTrace>().isCanNotMove = true;
            Destroy(rigidbody2D);
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Psychic);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 14);
                playerControler.KnockOutPoint = 5;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
