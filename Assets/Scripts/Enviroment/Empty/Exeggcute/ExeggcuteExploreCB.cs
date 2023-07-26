using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExeggcuteExploreCB : MonoBehaviour
{
    public int SpDmage = 30;

    // 记录已碰撞的对象
    private Dictionary<GameObject, bool> collisionMap = new Dictionary<GameObject, bool>();

    private Exeggcute empty;
    private string aimTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == aimTag && !collisionMap.ContainsKey(collision.gameObject))
        {
            collisionMap.Add(collision.gameObject, true);
            PlayerControler playerControler = collision.gameObject.GetComponent<PlayerControler>();
            if (aimTag == "Player")
            {
                //PlayerControler playerControler = collision.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 30, 0, 0, Type.TypeEnum.Grass);
                if (playerControler != null) {
                    //playerControler.ChangeHp(0, -(30 * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), (int)Type.TypeEnum.Grass);
                    playerControler.KnockOutPoint = 10;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            else if(aimTag == "Empty")
            {
                Empty e = collision.GetComponent<Empty>();
                e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), (int)Type.TypeEnum.Grass);
            }

        }
    }

    public void SetEmptyInfo(Exeggcute e)
    {
        empty = e;
    }

    public void SetAimTag(string tag)
    {
        aimTag = tag;
    }
}
