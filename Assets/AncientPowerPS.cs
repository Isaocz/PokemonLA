using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientPowerPS : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    bool isAbllityUODone;
    bool isSubPSBorn;
    AncientPower ParentAC;

    private void Start()
    {
        ParentAC = transform.parent.GetComponent<AncientPower>();
    }



    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (!isSubPSBorn)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isSubPSBorn = true;
            //GetComponent<ParticleSystem>().TriggerSubEmitter(0);
        }
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            ParentAC.HitAndKo(target);

            if (ParentAC.SkillFrom == 2 && target.EmptyHp<=0 && !isAbllityUODone) {
                Debug.Log(isAbllityUODone);
                isAbllityUODone = true;
                if (ParentAC.player.playerData.AtkBounsJustOneRoom <= 8) ParentAC.player.playerData.AtkBounsJustOneRoom += 1;
                if (ParentAC.player.playerData.SpABounsJustOneRoom <= 8) ParentAC.player.playerData.SpABounsJustOneRoom += 1;
                if (ParentAC.player.playerData.DefBounsJustOneRoom <= 8) ParentAC.player.playerData.DefBounsJustOneRoom += 1;
                if (ParentAC.player.playerData.SpDBounsJustOneRoom <= 8) ParentAC.player.playerData.SpDBounsJustOneRoom += 1;
                if (ParentAC.player.playerData.SpeBounsJustOneRoom <= 8) ParentAC.player.playerData.SpeBounsJustOneRoom += 1;
                ParentAC.player.ReFreshAbllityPoint();
            }

            ParentAC.isParticleCollider = true;
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
            }
            if (!isAbllityUODone)
            {
                isAbllityUODone = true;
                if (Random.Range(0.0f, 1.0f) + (float)ParentAC.player.LuckPoint/30 >= 0.9f)
                {
                    if(ParentAC.player.playerData.AtkBounsJustOneRoom <= 8) ParentAC.player.playerData.AtkBounsJustOneRoom += 1;
                    if (ParentAC.player.playerData.SpABounsJustOneRoom <= 8) ParentAC.player.playerData.SpABounsJustOneRoom += 1;
                    if (ParentAC.player.playerData.DefBounsJustOneRoom <= 8) ParentAC.player.playerData.DefBounsJustOneRoom += 1;
                    if (ParentAC.player.playerData.SpDBounsJustOneRoom <= 8) ParentAC.player.playerData.SpDBounsJustOneRoom += 1;
                    if (ParentAC.player.playerData.SpeBounsJustOneRoom <= 8) ParentAC.player.playerData.SpeBounsJustOneRoom += 1;
                    ParentAC.player.ReFreshAbllityPoint();
                }
            }
            
        }
    }
}
