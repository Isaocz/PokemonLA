using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAirSlash : Projectile
{
    public float SplitAirSlashSpeed = 10f;
    private float timer;
    private SpriteRenderer sr;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        
        sr = GetComponent<SpriteRenderer>();
        timer = 0f;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * SplitAirSlashSpeed * Time.deltaTime);
        if(timer > 3.5f && timer < 4f)
        {
            float t = (4f - timer) / 0.5f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, t);
        }
        else if (timer > 4f)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Flying);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
