using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalOnixStealthRock : MonoBehaviour
{

    [Header("隐形岩的存在时间")]
    public float Exittime;





    private void Update()
    {
        Exittime -= Time.deltaTime;
        if (Exittime <= 0)
        {
            GetComponent<Animator>().SetTrigger("Over");
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("xxx");
        if (other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(null, other.gameObject, 2, 0, 0, Type.TypeEnum.IgnoreType);
            if (p != null)
            {
                p.KnockOutPoint = 5;
                p.KnockOutDirection = (p.transform.position - transform.position).normalized;
            }
            
        }
    }

    public void DestroyFelf()
    {
        Destroy(gameObject);
    }
}
