using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsMew : Projectile
{
    private float moveSpeed;
    private float timer;
    private Vector3 center;
    private Color spriteColor;
    private int typeindex;
    private Type.TypeEnum type;

    void Start()
    {
        Vector3 direction = (center - transform.position).normalized;
        transform.right = direction;
        Destroy(gameObject, 5f);
        switch (typeindex)
        {
            case 0:type = Type.TypeEnum.Normal;spriteColor = new Color(0.7176471f, 0.6901961f, 0.6666667f, 0.5882353f); break;
            case 1:type = Type.TypeEnum.Fighting;spriteColor = new Color(0.4433962f, 0.2580442f, 0.08993414f, 0.7803922f); break;
            case 2:type = Type.TypeEnum.Flying;spriteColor = new Color(0.24644f, 0.4380967f, 0.735849f, 0.5882353f); break;
            case 3:type = Type.TypeEnum.Poison;spriteColor = new Color(0.5660378f, 0.08810966f, 0.4045756f, 0.5882353f); break;
            case 4:type = Type.TypeEnum.Ground;spriteColor = new Color(0.764151f, 0.6169877f, 0.2703364f, 0.7058824f); break;
            case 5:type = Type.TypeEnum.Rock;spriteColor = new Color(0.7075472f, 0.5511783f, 0.2636614f, 0.7372549f); break;
            case 6:type = Type.TypeEnum.Bug;spriteColor = new Color(0.7019608f, 0.7372549f, 0.2705882f, 0.5882353f); break;
            case 7:type = Type.TypeEnum.Ghost;spriteColor = new Color(0.2357066f, 0.1169455f, 0.3396226f, 0.627451f); break;
            case 8:type = Type.TypeEnum.Steel;spriteColor = new Color(0.5f, 0.5f, 0.5f, 0.5882353f);break;
            case 9:type = Type.TypeEnum.Fire;spriteColor = new Color(1, 0.2308041f, 0.08962262f, 0.6862745f);break;
            case 10:type = Type.TypeEnum.Water;spriteColor = new Color(0.2705882f, 0.6196079f, 0.9803922f, 0.5882353f);break;
            case 11:type = Type.TypeEnum.Grass;spriteColor = new Color(0.4823529f, 0.7137255f, 0.1960784f, 0.5882353f);break;
            case 12:type = Type.TypeEnum.Electric;spriteColor = new Color(0.9921569f, 0.9372549f, 0.1529412f, 0.5882353f);break;
            case 13:type = Type.TypeEnum.Psychic;spriteColor = new Color(0.8301887f, 0.1683873f, 0.4182985f, 0.5882353f);break;
            case 14:type = Type.TypeEnum.Ice;spriteColor = new Color(0.509804f, 0.7294118f, 1, 0.5882353f);break;
            case 15:type = Type.TypeEnum.Dragon;spriteColor = new Color(0, 0.04001015f, 0.5137255f, 0.682353f);break;
            case 16:type = Type.TypeEnum.Dark;spriteColor = new Color(0.4056604f, 0.3041446f, 0.2851104f, 0.8313726f);break;
            case 17:type = Type.TypeEnum.Fairy;spriteColor = new Color(0.764151f, 0.4649787f, 0.6818696f, 0.5882353f);break;
        }
        GetComponent<SpriteRenderer>().color = spriteColor;
        GetComponent<TrailRenderer>().startColor = spriteColor;
        GetComponent<TrailRenderer>().endColor = spriteColor;
    }
    public void Initialize(Vector3 mapCenter, int HitType)
    {
        center = mapCenter;
        typeindex = HitType;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 2.5f)
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

}
