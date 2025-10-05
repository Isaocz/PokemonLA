using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsMew : Projectile
{
    private float moveSpeed;
    private float timer;
    private float colortimer;
    private Vector3 center;
    private Color spriteColor;
    private int typeindex;
    private PokemonType.TypeEnum type;

    private Color[] colors = new Color[]
    {
        new Color(0.7176471f, 0.6901961f, 0.6666667f, 0.5882353f), // Normal
        new Color(0.4433962f, 0.2580442f, 0.08993414f, 0.7803922f), // Fighting
        new Color(0.24644f, 0.4380967f, 0.735849f, 0.5882353f), // Flying
        new Color(0.5660378f, 0.08810966f, 0.4045756f, 0.5882353f), // Poison
        new Color(0.764151f, 0.6169877f, 0.2703364f, 0.7058824f), // Ground
        new Color(0.7075472f, 0.5511783f, 0.2636614f, 0.7372549f), // Rock
        new Color(0.7019608f, 0.7372549f, 0.2705882f, 0.5882353f), // Bug
        new Color(0.2357066f, 0.1169455f, 0.3396226f, 0.627451f), // Ghost
        new Color(0.5f, 0.5f, 0.5f, 0.5882353f), // Steel
        new Color(1, 0.2308041f, 0.08962262f, 0.6862745f), // Fire
        new Color(0.2705882f, 0.6196079f, 0.9803922f, 0.5882353f), // Water
        new Color(0.4823529f, 0.7137255f, 0.1960784f, 0.5882353f), // Grass
        new Color(0.4823529f, 0.7137255f, 0.1960784f, 0.5882353f), // Electric
        new Color(0.9921569f, 0.9372549f, 0.1529412f, 0.5882353f), // Psychic
        new Color(0.8301887f, 0.1683873f, 0.4182985f, 0.5882353f), // Ice
        new Color(0.509804f, 0.7294118f, 1, 0.5882353f), // Dragon
        new Color(0, 0.04001015f, 0.5137255f, 0.682353f), // Dark
        new Color(0.4056604f, 0.3041446f, 0.2851104f, 0.8313726f), // Fairy
    };

    void Start()
    {
        Vector3 direction = (center - transform.position).normalized;
        transform.right = direction;
        Destroy(gameObject, 4.5f);
        spriteColor = colors[typeindex];
        GetComponent<SpriteRenderer>().color = spriteColor;
        GetComponent<TrailRenderer>().startColor = spriteColor;
        GetComponent<TrailRenderer>().endColor = spriteColor;
    }
    public void Initialize(Vector3 mapCenter, int HitType, int num)
    {
        center = mapCenter;
        colortimer = HitType / 10f;
        typeindex = num / 18 % 18;
    }

    void Update()
    {
        timer += Time.deltaTime;
        colortimer += Time.deltaTime;

        if (colortimer >= 1.8f)
        {
            typeindex++;

            if (typeindex >= colors.Length)
            {
                typeindex = 0;
            }

            colortimer = 0.0f;
        }

        ChangeType();

        float t = colortimer / 1.8f;
        spriteColor = Color.Lerp(colors[typeindex], colors[(typeindex + 1) % colors.Length], t);
        GetComponent<SpriteRenderer>().color = spriteColor;
        GetComponent<TrailRenderer>().startColor = spriteColor;
        GetComponent<TrailRenderer>().endColor = spriteColor;

        if (timer < 2.5f)
        {
            moveSpeed = Mathf.Exp(timer * 3f/2.5f);
        }
        else
        {
            moveSpeed = Mathf.Exp(3);
        }
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, type);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
    private void ChangeType()
    {
        switch (typeindex)
        {
            case 0: type = PokemonType.TypeEnum.Normal; break;
            case 1: type = PokemonType.TypeEnum.Fighting; break;
            case 2: type = PokemonType.TypeEnum.Flying; break;
            case 3: type = PokemonType.TypeEnum.Poison; break;
            case 4: type = PokemonType.TypeEnum.Ground; break;
            case 5: type = PokemonType.TypeEnum.Rock; break;
            case 6: type = PokemonType.TypeEnum.Bug; break;
            case 7: type = PokemonType.TypeEnum.Ghost; break;
            case 8: type = PokemonType.TypeEnum.Steel; break;
            case 9: type = PokemonType.TypeEnum.Fire; break;
            case 10: type = PokemonType.TypeEnum.Water; break;
            case 11: type = PokemonType.TypeEnum.Grass; break;
            case 12: type = PokemonType.TypeEnum.Electric; break;
            case 13: type = PokemonType.TypeEnum.Psychic; break;
            case 14: type = PokemonType.TypeEnum.Ice; break;
            case 15: type = PokemonType.TypeEnum.Dragon; break;
            case 16: type = PokemonType.TypeEnum.Dark; break;
            case 17: type = PokemonType.TypeEnum.Fairy; break;
        }
    }
}
