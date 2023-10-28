using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPowerCollider : MonoBehaviour
{
    Skill ParentEarthPower;
    List<Empty> Empties = new List<Empty> { };

    private void Start()
    {
        ParentEarthPower = transform.parent.parent.GetComponent<Skill>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (ParentEarthPower.SkillFrom == 2 && e.isBlindDone)
            {
                ParentEarthPower.SpDamage = 120;
            }
            if (e != null)
            {
                ParentEarthPower.HitAndKo(e);
                if (!Empties.Contains(e)) {
                    Empties.Add(e);
                    if (Random.Range(0.0f, 1.0f) + ((float)ParentEarthPower.player.LuckPoint / 30) > 0.9f )
                    {
                        e.SpDChange(-1, 0.0f);
                    }
                }
            }
        }
    }

}
