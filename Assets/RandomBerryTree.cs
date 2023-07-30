using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBerryTree : MonoBehaviour
{

    public List<BerryTree> BerryList = new List<BerryTree> { };

    // Start is called before the first frame update
    void Start()
    {
        int TreeIndex = Random.Range(0, BerryList.Count);
        if(transform.parent != null)
        {
            Instantiate(BerryList[TreeIndex], transform.position, Quaternion.identity, transform.parent);
        }
        else
        {
            Instantiate(BerryList[TreeIndex], transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }

}
