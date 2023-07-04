using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubstituteSkill : Skill
{
    public Substitute SubstitutePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp * 0.1f, 0, 0, Type.TypeEnum.IgnoreType) ;
        Substitute Obj = Instantiate(SubstitutePrefabs , transform.position , Quaternion.Euler(Vector3.zero) );
        Obj.SetSubstitute(player.maxHp/2 ,player);
        if (SkillFrom == 2) { Obj.isDieBoom = true; }
    }
    private void Update()
    {
        StartExistenceTimer();
    }
}
