using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadersCrestAtrackCircle : MonoBehaviour
{
    float Timer;


    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 2.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null && !e.isEmptyInfatuationDone)
            {
                e.EmptyInfatuation(18, 10);
            }
        }
    }

}
