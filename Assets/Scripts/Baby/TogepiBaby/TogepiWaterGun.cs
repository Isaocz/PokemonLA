using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogepiWaterGun : Projectile
{
    bool isDestory;
    bool isDmageDonw;
    bool isDestroyPSPlay;
    public ParticleSystem DestroyPS;


    // Start is called before the first frame update
    private void Awake()
    {
        AwakeProjectile();
    }



    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryByRange(10);
        if (isDestory)
        {
            if (!isDestroyPSPlay && DestroyPS)
            {
                isDestroyPSPlay = true;
                DestroyPS.gameObject.SetActive(true);
                DestroyPS.transform.parent = null;
            }
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Empty"))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            if (other.GetComponent<Empty>() != null)
            {
                if (!isDmageDonw)
                {
                    isDmageDonw = true;
                    //Debug.Log(transform.gameObject);\
                    Empty target = other.GetComponent<Empty>();
                    Pokemon.PokemonHpChange(Baby.gameObject, target.gameObject, Dmage, SpDmage, 0, ProType);
                    //target.EmptyHpChange(Mathf.Clamp( (int)((float)(10 * (2 * BabyLevel + 10) *30) / (float)(250 * target.DefAbilityPoint  + 2)), 1 , 100000), 0, 12);

                }
            }
        }
    }
}
