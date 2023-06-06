using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInWaterTrigger : MonoBehaviour
{

    public GameObject RIWAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PushStone>() != null)
        {
            Destroy(transform.parent.GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Instantiate(RIWAnimation, transform.parent.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }
    }
}
