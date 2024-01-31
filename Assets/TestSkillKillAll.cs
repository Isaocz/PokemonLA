using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkillKillAll : Skill
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
        for (int i = 0; i < EmptyParent.transform.childCount; i++)
        {
            Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
            if (e != null) { Pokemon.PokemonHpChange(null,e.gameObject,e.maxHP,0,0,Type.TypeEnum.IgnoreType); }
        }
        Destroy(gameObject);
    }

}
