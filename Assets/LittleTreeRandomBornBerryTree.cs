using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTreeRandomBornBerryTree : MonoBehaviour
{

    public RandomBerryTree BerryTree;

    // Start is called before the first frame update
    void Start()
    {

        if (Random.Range(0.0f , 1.0f) > 0.996f) {
            if (transform.parent != null)
            {
                Instantiate(BerryTree, transform.position, Quaternion.identity, transform.parent);
            }
            else
            {
                Instantiate(BerryTree, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

}
