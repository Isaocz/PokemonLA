using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglypuffsMicrophoneSing : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                e.EmptySleepDone(0.5f , 5 , 1);
            }
        }
    }
}
