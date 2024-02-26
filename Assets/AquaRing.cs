using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaRing : Skill
{
    List<Empty> SpeedDownList = new List<Empty> { };
    float Timer = 1.6f;

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        Timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject == player.gameObject)
        {
            if (Timer >= 1.6f)
            {
                Pokemon.PokemonHpChange( null , other.gameObject , 0 , 0 , (int)(other.gameObject.GetComponent<PlayerControler>().maxHp / 16) , Type.TypeEnum.IgnoreType );
                Timer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                target.SpeedChange();
                SpeedDownList.Add(target);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null && SpeedDownList.Contains(target))
            {
                target.SpeedRemove01(0);
            }
        }
    }
}
