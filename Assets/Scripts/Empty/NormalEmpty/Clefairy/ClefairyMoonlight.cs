using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClefairyMoonlight : MonoBehaviour
{
    public Empty ParentEmpty;

    List<Empty> EList = new List<Empty> { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject)
        {
            if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && !EList.Contains(e))
                {
                    EList.Add(e);
                    Pokemon.PokemonHpChange(null , e.gameObject , 0 , 0 , e.maxHP / 5 , PokemonType.TypeEnum.No , false);
                }
            }
        }
    }
}
