using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlizzardEmpty : Projectile
{
    bool isInitialiazed;
    private float timer;
    public float delay = 0.5f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!isInitialiazed)
        {
            timer = 0;
            isInitialiazed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player" && timer > delay)
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Ice);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            if (Random.Range(0.0f,1.0f) >= 0.7f) { playerControler.PlayerFrozenFloatPlus(0.5f , 0.8f); }

        }
    }
}
