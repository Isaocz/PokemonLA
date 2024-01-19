using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinecoBabySprite : MonoBehaviour
{
    PinecoBaby ParentPinecon;
    // Start is called before the first frame update
    void Start()
    {
        ParentPinecon = transform.parent.GetComponent<PinecoBaby>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentPinecon.NowState == PinecoBaby.State.Move ) {
            if (other.tag == "Room")
            {
                ParentPinecon.Return();
            }
            if (other.tag == "Empty")
            {
                ParentPinecon.Blast();
            }
        }
    }
}
