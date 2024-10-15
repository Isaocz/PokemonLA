using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastEmptyFire : Projectile
{
    // Start is called before the first frame update


    private void Awake()
    {
        AwakeProjectile();
        Invoke("CallDestory", 10);

    }

    bool isDestory;
    bool isCanNotMove;
    int DestoryTimer;
    float BornTimer;

    private void Update()
    {
        if (BornTimer < 4.0f) {
            BornTimer += Time.deltaTime;
            if (gameObject.transform.localScale.x < 2) {
                gameObject.transform.localScale += new Vector3(Time.deltaTime * 0.25f, Time.deltaTime * 0.25f, Time.deltaTime * 0.25f);
            }
        }
        if (!isCanNotMove) {
            MoveNotForce();
        }
    }

    private void FixedUpdate()
    {
        if (isDestory)
        {
            DestoryTimer++;
            if (DestoryTimer >= 6)
            {
                gameObject.transform.localScale -= new Vector3(0.08f, 0.08f, 0.08f);
                if (gameObject.transform.localScale.x <= 0.05)
                {
                    Destroy(gameObject);
                }
                DestoryTimer = 0;
            }
        }
    }

    //使用动画回调的开始摧毁鬼火的回调函数
    void CallDestory()
    {
        isDestory = true;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enviroment" || other.tag == "Player" || other.tag == "Room")
        {
            isCanNotMove = true;
            if (other.tag == "Player")
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage , 0, PokemonType.TypeEnum.Fire); }
                else { Pokemon.PokemonHpChange(null, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Fire); }
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            gameObject.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
            if (gameObject.transform.localScale.x <= 0.3)
            {
                Destroy(gameObject);
            }
        }
    }
}
