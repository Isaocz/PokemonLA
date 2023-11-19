using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : Projectile
{
    private CircleCollider2D triggerCollider;
    public float colorChange = 2f;
    private bool isTriggerEnabled = false;

    void OnEnable()
    {
        isTriggerEnabled = false;
        triggerCollider = GetComponent<CircleCollider2D>();
        Invoke("ChangeColor", colorChange);
        triggerCollider.enabled = false; // ��ʼ״̬�½��ô�������ײ��
    }

    private void ChangeColor()
    {
        isTriggerEnabled = true; // ���ô�������ײ��
        triggerCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggerEnabled && collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Ghost);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
