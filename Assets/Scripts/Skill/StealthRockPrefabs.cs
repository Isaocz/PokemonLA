using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRockPrefabs : MonoBehaviour
{

    [Header("多段伤害的间隔时间")]
    public float HitCD;

    [Header("隐形岩的存在时间")]
    public float Exittime;

    struct EmptyList
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
    List<EmptyList> TargetList = new List<EmptyList> { };



    private void Update()
    {
        Exittime -= Time.deltaTime;
        if(Exittime <= 0)
        {
            GetComponent<Animator>().SetTrigger("Over");
        }
        for (int i = 0; i < TargetList.Count; i++)
        {
            EmptyList CDCell = TargetList[i];
            if (CDCell.isMultipleDamageColdDown)
            {
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= HitCD) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("xxx");
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();


            if (target != null) {
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
                    Pokemon.PokemonHpChange(null, TCEell.Target.gameObject, Mathf.Clamp((((float)TCEell.Target.maxHP) / 20) , 1, (TCEell.Target.EmptyBossLevel == Empty.emptyBossLevel.Boss || TCEell.Target.EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? 2 : 3), 0 , 0 , PokemonType.TypeEnum.IgnoreType ) ;
                    
                    TCEell.isMultipleDamageColdDown = true;
                    TargetList[ListIndex] = TCEell;
                }
            }
        }
    }

    public void DestroyFelf()
    {
        Destroy(gameObject);
    }

}
