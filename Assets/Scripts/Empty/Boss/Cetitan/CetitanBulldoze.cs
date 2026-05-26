using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanBulldoze : MonoBehaviour
{
    /// <summary>
    /// ¸¸ºÆ´ó¾¨
    /// </summary>
    public Cetitan ParentCetitan;

    /// <summary>
    /// ÍþÁ¦
    /// </summary>
    public int Dmage;

    /// <summary>
    /// »÷ÍËÖµ
    /// </summary>
    public float KOPoint;

    /// <summary>
    /// Åö×²Ïä
    /// </summary>
    CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = transform.GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    private void Update()
    {
        //Ïú»Ù±ùÖù
        if (ParentCetitan != null && circleCollider != null)
        {
            ParentCetitan.Break_IcleCrash_Circle(((Vector2)transform.position + circleCollider.offset) , circleCollider.radius);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ParentCetitan != null && collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentCetitan.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ground);
            if (p != null)
            {
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
            }
        }

    }

}
