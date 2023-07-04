using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExeggcuteExploreCB : MonoBehaviour
{
    // 记录已碰撞的对象
    private Dictionary<GameObject, bool> collisionMap = new Dictionary<GameObject, bool>();

    private Exeggcute empty;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !collisionMap.ContainsKey(collision.gameObject))
        {
            collisionMap.Add(collision.gameObject, true);
            PlayerControler playerControler = collision.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 30, 0, 0, Type.TypeEnum.Grass);
            if (playerControler != null) {
                //playerControler.ChangeHp(0, -(30 * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), (int)Type.TypeEnum.Grass);
                playerControler.KnockOutPoint = 10;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }

    public void SetEmptyInfo(Exeggcute e)
    {
        empty = e;
    }
}
