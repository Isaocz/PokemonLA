using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWisp : Projectile
{

    private void Awake()
    {
        AwakeProjectile();
        Invoke("CallDestory" , 10);
    }

    bool isDestory;
    int DestoryTimer;
    private void FixedUpdate()
    {
        if (isDestory)
        {
            DestoryTimer++;
            if(DestoryTimer >= 12)
            {
                gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
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
        if(other.tag == "Enviroment" || other.tag == "Player" || other.tag == "Room" || (empty.isEmptyInfatuationDone && other.tag == "Empty"))
        {
            if(other.tag == "Player")
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(null , other.gameObject , 0 , 1 , 0 , Type.TypeEnum.IgnoreType);
                if (playerControler != null) {
                    playerControler.BurnFloatPlus(0.4f);
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if(other.tag == "Empty" && other.gameObject != empty.gameObject)
            {
                Empty e = other.GetComponent<Empty>();
                e.EmptyBurnDone(0.4f , 10f , 1);
                Pokemon.PokemonHpChange(null, e.gameObject, 0, 1, 0, Type.TypeEnum.IgnoreType);
                
            }
            gameObject.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
            if(gameObject.transform.localScale.x <= 0.3)
            {
                Destroy(gameObject);
            }
        }
    }
}
