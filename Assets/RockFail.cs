using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFail : MonoBehaviour
{
    public SkillBreakabelStone Stone;


    public void RockFallOver()
    {
        Instantiate(Stone, transform.position, Quaternion.identity);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

}
