using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireBigRoar : MonoBehaviour
{
    /// <summary>
    /// 父电击魔兽
    /// </summary>
    public Electivire ParentElectivire;

    /// <summary>
    /// 特工威力
    /// </summary>
    public int SpDmage;

    /// <summary>
    /// 击退值
    /// </summary>
    public float KOPoint;


    public float DestroyTime;


    private void Start()
    {
        Timer.Start(this, DestroyTime, () => { Destroy(this.gameObject); });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null )
            {
                if (p.playerData.DefBounsJustOneRoom >= -8)
                {
                    p.playerData.DefBounsJustOneRoom--;
                    p.ReFreshAbllityPoint();
                }
                if (ParentElectivire != null) {
                    Pokemon.PokemonHpChange(ParentElectivire.gameObject, p.gameObject, 0, 60, 0, PokemonType.TypeEnum.Normal);
                    p.KnockOutPoint = 10.0f;
                    p.KnockOutDirection = (p.transform.position - ParentElectivire.transform.position).normalized;
                }
            }
        }
    }
}
