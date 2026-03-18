using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireLightning : MonoBehaviour
{

    /// <summary>
    /// ¸¸µç»÷Ä§ÊÞ
    /// </summary>
    public Electivire ParentElectivire;

    /// <summary>
    /// ÉËº¦
    /// </summary>
    public int SpDmage;

    /// <summary>
    /// »÷ÍËÖµ
    /// </summary>
    public float KOPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null && ParentElectivire != null)
            {
                Pokemon.PokemonHpChange(ParentElectivire.gameObject, p.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Electric);
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                p.ParalysisFloatPlus(0.07f);
            }
        }
    }



}
