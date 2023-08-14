using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : Projectile
{
    private CircleCollider2D triggerCollider;
    public float colorChange = 2f;
    private bool isTriggerEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        Invoke("ChangeColor", colorChange);
        triggerCollider.enabled = false; // ³õÊ¼×´Ì¬ÏÂ½ûÓÃ´¥·¢Æ÷Åö×²Æ÷
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ChangeColor()
    {
        isTriggerEnabled = true; // ÆôÓÃ´¥·¢Æ÷Åö×²Æ÷
        triggerCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggerEnabled && collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, SpDmage, 0, Type.TypeEnum.Ghost);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
