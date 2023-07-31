using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTombRockFall : MonoBehaviour
{
    public SkillBreakabelStone Stone;
    RockTomb ParentRockTomb;

    private void Start()
    {
        ParentRockTomb = transform.parent.GetComponent<RockTomb>();
    }

    public void RockFallOver()
    {
        Instantiate(Stone, transform.position, Quaternion.identity);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                ParentRockTomb.HitAndKo(target);
            }
        }
    }

}
