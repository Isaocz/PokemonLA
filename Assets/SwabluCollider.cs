using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwabluCollider : MonoBehaviour
{
    SwabluBaby ParentSwablu;
    // Start is called before the first frame update
    void Start()
    {
        ParentSwablu = transform.parent.GetComponent<SwabluBaby>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentSwablu.NowState == SwabluBaby.State.Move || ParentSwablu.NowState == SwabluBaby.State.Return)
        {
            if(other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null) {
                    target.EmptyKnockOut(5);
                    Pokemon.PokemonHpChange(ParentSwablu.gameObject, target.gameObject, 40, 0, 0, Type.TypeEnum.Flying);
                }
            }
            if (ParentSwablu.NowState == SwabluBaby.State.Move)
            {
                if (other.tag == "Room" || other.tag == "Empty")
                {
                    ParentSwablu.Return();
                }
            }
        }
    }
}
