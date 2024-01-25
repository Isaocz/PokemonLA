using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixRockTomb : MonoBehaviour
{
    public OnixRockTombRock Stone;
    public Empty ParentOnix;
    public bool isStealthRock;




    public void RockFallOver()
    {
        Instantiate(Stone, transform.position, Quaternion.identity , transform.parent).isStealthRock = isStealthRock;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        Invoke("DestroySelf", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentOnix.gameObject, other.gameObject, 60, 0, 0, Type.TypeEnum.Rock);
            if (p != null)
            {
                p.KnockOutPoint = ParentOnix.Knock;
                p.KnockOutDirection = (p.transform.position - transform.position).normalized;
            }
        }
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
