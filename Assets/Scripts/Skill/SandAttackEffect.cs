using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandAttackEffect : MonoBehaviour
{
    List<Empty> Empties = new List<Empty>();
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {

            Empty target = other.GetComponent<Empty>();
            gameObject.transform.parent.GetComponent<SandAttack>();
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.Blind(2, 1);
            }
        }
    }
}
