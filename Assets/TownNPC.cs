using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : NPC<PlayerPokemon>
{
    public enum NPCState
    {
        NotJoined,  //δ����С��
        NotinTown,  //����С�򵫲���С��
        Idle,       //վ�Ų���
        Walk,       //��·��
    }
}
