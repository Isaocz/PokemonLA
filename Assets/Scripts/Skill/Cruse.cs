using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruse : Skill
{
    public GameObject CurseAnimation;
    List<Empty> CurseList = new List<Empty> { };
    bool isPlayerGhost;

    // Start is called before the first frame update
    void Start()
    {
        if (player.PlayerType01 == 8 || player.PlayerType02 == 8 || player.PlayerTeraTypeJOR == 8 || player.PlayerTeraType == 8)
        {
            isPlayerGhost = true;
        }
        if (isPlayerGhost)
        {
            if(player.Hp > player.maxHp / 2)
            {
                Pokemon.PokemonHpChange(null , player.gameObject, player.maxHp/2 , 0 , 0 , PokemonType.TypeEnum.IgnoreType);
            }
            else
            {
                if (player.Hp > 1) { Pokemon.PokemonHpChange(null, player.gameObject, player.Hp-1, 0, 0, PokemonType.TypeEnum.IgnoreType); }
            }
            Instantiate(CurseAnimation, player.transform.position + Vector3.right * 0.2f + Vector3.up * 0.6f, Quaternion.identity, player.transform);
            if (SkillFrom != 2) { player.isCanNotMove = true; }
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
            player.playerData.MoveSpeBounsJustOneRoom -= 2;
            player.playerData.AtkBounsJustOneRoom ++;
            player.playerData.DefBounsJustOneRoom ++;
            player.ReFreshAbllityPoint();
            if (SkillFrom == 2)
            {
                player.playerData.MoveSpeBounsJustOneRoom --;
                player.playerData.SpABounsJustOneRoom++;
                player.playerData.SpDBounsJustOneRoom++;
                player.playerData.SpeBounsJustOneRoom++;
                player.ReFreshAbllityPoint();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerGhost) {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null && !CurseList.Contains(target))
                {
                    CurseList.Add(target);
                    target.EmptyCurse(0, 1);
                    Instantiate(CurseAnimation, target.transform.position + Vector3.right * 0.2f + Vector3.up * 0.6f, Quaternion.identity, target.transform);

                }
            }
        }
    }

    private void OnDestroy()
    {
        if (isPlayerGhost)
        {
            if (player.isCanNotMove)
            {
                player.isCanNotMove = false;
            }
        }
    }
}
