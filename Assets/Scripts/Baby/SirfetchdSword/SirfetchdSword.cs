using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirfetchdSword : SwordBaby
{

    public float CD;
    float CDTimer;

    // Start is called before the first frame update
    void Start()
    {
        SwordBabyStart();
    }

    // Update is called once per frame
    void Update()
    {
        SwordBabyUpdate();
        if (CDTimer > 0)
        {
            CDTimer -= Time.deltaTime;
            if (CDTimer <= 0) { CDTimer = 0; }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CDTimer == 0)
        {
            if (other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e != null)
                {
                    CDTimer = CD;
                    Pokemon.PokemonHpChange( player.gameObject , e.gameObject , 15 , 0 , 0 , PokemonType.TypeEnum.Grass );
                }
            }
        }
    }

}
