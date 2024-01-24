using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float Damage;

    protected struct EmptyList
    {

        public EmptyList(Empty target, bool v1, float v2) : this()
        {
            Target = target;
            isMultipleDamageColdDown = v1;
            MultipleDamageColdDownTimer = v2;
        }

        public Empty Target;
        public bool isMultipleDamageColdDown { get; set; }
        public float MultipleDamageColdDownTimer { get; set; }

    }
    protected List<EmptyList> TargetList = new List<EmptyList> { };


    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            //playerControler.ChangeHp(-Damage, 0, 19);
            
            if (playerControler != null && !playerControler.playerData.IsPassiveGetList[13] && !playerControler.isRapidSpin)
            {
                Pokemon.PokemonHpChange(null, other.gameObject, Damage, 0, 0, Type.TypeEnum.IgnoreType);
                playerControler.KnockOutPoint = 1f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
        else if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                SpikeHIt(target);
            }
        }
    }




    private void Update()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            EmptyList CDCell = TargetList[i];
            if (CDCell.isMultipleDamageColdDown)
            {
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= 0.75f) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }

    public void SpikeHIt(Empty target)
    {
        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        bool isTargetExitInList = false;
        if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
        for (int i = 0; i < TargetList.Count; i++)
        {
            if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break; }
        }
        if (!isTargetExitInList)
        {
            TargetList.Add(TCEell);
        }


        if (!TCEell.isMultipleDamageColdDown)
        {
            Pokemon.PokemonHpChange(null, target.gameObject, 10, 0, 0, Type.TypeEnum.IgnoreType);
            TCEell.isMultipleDamageColdDown = true;
            TargetList[ListIndex] = TCEell;
        }

    }


}
