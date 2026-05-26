using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkillKillAll : Skill
{
    /// <summary>
    /// 模式
    /// </summary>
    public enum KillAllMode
    {
        killall,//全杀
        killone,//仅杀一个
    }
    public KillAllMode mode;
    /// <summary>
    /// 仅杀一个模式时 ，攻击序列号
    /// </summary>
    public int KillOneIndex = 0;

    /// <summary>
    /// 杀的次数 ，对应boss的减伤
    /// </summary>
    public int KillCount = 1;


    // Start is called before the first frame update
    void Start()
    {
        GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
        for (int j = 0; j < KillCount; j++)
        {
            switch (mode)
            {
                //全杀
                case KillAllMode.killall:
                    {
                        for (int i = 0; i < EmptyParent.transform.childCount; i++)
                        {
                            Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
                            if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
                        }
                    }
                    break;
                //仅杀一个
                case KillAllMode.killone:
                    {
                        if (KillOneIndex < 0 || KillOneIndex >= EmptyParent.transform.childCount) { KillOneIndex = 0; }
                        Empty e = EmptyParent.transform.GetChild(KillOneIndex).GetComponent<Empty>();
                        if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
                    }
                    break;
            }
        }
        Destroy(gameObject);
    }

}
