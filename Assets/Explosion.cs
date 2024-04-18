using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Skill
{

    public bool isBoom;
    CircleCollider2D CircleCollider;

    // Start is called before the first frame update
    void Start()
    {
        CircleCollider = GetComponent<CircleCollider2D>();
        CircleCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 2.1f && transform.parent != null)
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            GameObject p = null;
            if (player != null) { p = player.gameObject; }
            if (other.tag == "Empty")
            {
                HitAndKo(other.GetComponent<Empty>());
            }
            else if (other.tag == "Player")
            {
                Pokemon.PokemonHpChange(p, other.gameObject, Damage, 0, 0, Type.TypeEnum.Normal);
                other.GetComponent<PlayerControler>().KnockOutPoint = KOPoint/2.0f;
                other.GetComponent<PlayerControler>().KnockOutDirection = (Quaternion.AngleAxis(Random.Range(0,360) , Vector3.forward) * Vector2.right).normalized;

            }
        }
    }
}
