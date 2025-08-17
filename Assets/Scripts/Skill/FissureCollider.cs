using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureCollider : MonoBehaviour
{
    public Fissure fissure;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if(e != null)
            {
                int EMaxHp = 0;
                if ((e.EmptyBossLevel == Empty.emptyBossLevel.Boss || e.EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
                {
                    
                    EMaxHp = (int)((e.Emptylevel + 10 + (int)(((float)e.Emptylevel * e.HpEmptyPoint * 2) / 100.0f)));
                }
                else
                {
                    EMaxHp = e.EmptyHp;
                }
                
                Pokemon.PokemonHpChange(null ,e.gameObject , EMaxHp, 0 , 0 , PokemonType.TypeEnum.IgnoreType );
            }
        }
        if (fissure.SkillFrom == 2 && other.tag == "Enviroment")
        {
            SoftMud sm = other.GetComponent<SoftMud>();
            if(sm != null)
            {
                Instantiate(fissure, transform.position , Quaternion.identity );
                sm.isBeUsed = true;
            }
        }
    }
}
