using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : NPC<PlayerPokemon>
{
    public enum NPCState
    {
        NotJoined,  //未加入小镇
        NotinTown,  //加入小镇但不在小镇
        Idle,       //站着不动
        Walk,       //走路中
    }
}
