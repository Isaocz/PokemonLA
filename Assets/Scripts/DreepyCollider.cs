using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreepyCollider : MonoBehaviour
{
    Dreepy ParentDreepy;
    // Start is called before the first frame update
    void Start()
    {
        ParentDreepy = transform.parent.GetComponent<Dreepy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentDreepy.NowState == Dreepy.State.Move)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null)
                {
                    target.EmptyKnockOut(5);
                    Pokemon.PokemonHpChange(ParentDreepy.gameObject, target.gameObject, 50, 0, 0, Type.TypeEnum.Dragon);
                }
            }
            if (ParentDreepy.NowState == Dreepy.State.Move)
            {
                if (other.tag == "Room")
                {
                    ParentDreepy.Return();
                }
            }
        }
    }
}
