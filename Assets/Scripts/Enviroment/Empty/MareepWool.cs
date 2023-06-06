using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MareepWool : Projectile
{

    public Sprite Wool1;
    public Sprite Wool2;
    public Sprite Wool3;
    bool isDestory;

    private void Awake()
    {
        AwakeProjectile();
    }

    private void FixedUpdate()
    {
        DestoryProjectile(5);
        if (isDestory)
        {
            spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 0.2f * Time.deltaTime);
            if (spriteRenderer.material.color.a <= 0.09f)
            {
                Destroy(gameObject);
            }
        }
    }

    void DestoryWool()
    {
        isDestory = true;
    }

    private void Start()
    {
        switch (Random.Range(1, 4))
        {
            case 1:
                spriteRenderer.sprite = Wool1;
                break;
            case 2:
                spriteRenderer.sprite = Wool2;
                break;
            case 3:
                spriteRenderer.sprite = Wool3;
                break;
        }
        Invoke("DestoryWool" , 13);
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Player")
        {
            isDestory = true;
            PlayerControler playerControler = other.transform.GetComponent<PlayerControler>();
            playerControler.ChangeHp(0, -(10 * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 13);
            playerControler.KnockOutPoint = 5;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            playerControler.ParalysisFloatPlus(0.2f);
        }
        
    }
}
