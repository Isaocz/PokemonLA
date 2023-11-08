using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBallp2 : Projectile
{
    List<PlayerControler> playerlist = new List<PlayerControler>();
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler player = collision.GetComponent<PlayerControler>();
            if (player != null && !playerlist.Contains(player))
            {
                playerlist.Add(player);
                int rate = (int)(empty.speed / player.speed);
                switch (rate)
                {
                    case 1:SpDmage = 60;break;
                    case 2:SpDmage = 80;break;
                    case 3:SpDmage = 120;break;
                    case 4:SpDmage = 150;break;
                    default:SpDmage = 40;break;
                }
                
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric);
                player.KnockOutPoint = 1.5f;
                player.KnockOutDirection = (player.transform.position - transform.position).normalized;
            }
        }
    }
}
