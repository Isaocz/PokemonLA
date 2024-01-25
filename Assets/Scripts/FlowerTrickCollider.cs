using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTrickCollider : MonoBehaviour
{
    FlowerTrick ParentFlowerTrick;
    // Start is called before the first frame update
    void Start()
    {
        ParentFlowerTrick = transform.parent.GetComponent<FlowerTrick>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentFlowerTrick.NowState == FlowerTrick.State.Move)
        {
            if (other.tag == "Room")
            {
                ParentFlowerTrick.Return();
            }
            if (other.tag == "Empty")
            {
                ParentFlowerTrick.Blast();
            }
        }
    }
}
