using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFlingHealEffect : MonoBehaviour
{
    /// <summary>
    /// 已经被回复玩家列表
    /// </summary>
    List<PlayerControler> PlayerList = new List<PlayerControler> { };

    /// <summary>
    /// 已经被回复敌人列表
    /// </summary>
    List<Empty> EmptyList = new List<Empty> { };

    private void Awake()
    {
        Timer.Start(this, 5.0f, () => { Destroy(gameObject); });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        //回复玩家
        if (collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null && !PlayerList.Contains(p))
            {
                Pokemon.PokemonHpChange(null , p.gameObject , 0 , 0 , p.maxHp/20 , PokemonType.TypeEnum.No);
                PlayerList.Add(p);
                Debug.Log(p.name);
            }
        }
        //回复敌人
        if (collision.gameObject.tag == "Empty")
        {
            Empty e = collision.gameObject.GetComponent<Empty>();
            if (e != null && !EmptyList.Contains(e))
            {
                Pokemon.PokemonHpChange(null, e.gameObject, 0, 0, e.maxHP / 10, PokemonType.TypeEnum.No);
                EmptyList.Add(e);
                Debug.Log(e.name);
            }
        }
    }
}
