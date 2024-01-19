using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilceryCuteCharm : MonoBehaviour
{
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
    void Start()
    {
        
    }

    private void Update()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            EmptyList CDCell = TargetList[i];
            if (CDCell.isMultipleDamageColdDown)
            {
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= 0.8f) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }

    public void Attrack(Empty target)
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
            target.EmptyInfatuation(30,0.35f);
            TCEell.isMultipleDamageColdDown = true;
            TargetList[ListIndex] = TCEell; ;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty" && other.GetComponent<Empty>())
        {
            Attrack(other.GetComponent<Empty>());
        }   
    }

}
