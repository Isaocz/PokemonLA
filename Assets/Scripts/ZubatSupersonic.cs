using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZubatSupersonic : MonoBehaviour
{
    public Empty ParentZubat;



    private void Start()
    {
        if (ParentZubat.isEmptyInfatuationDone)
        {
            var C = GetComponent<ParticleSystem>().collision;
            C.collidesWith = LayerMask.GetMask("Empty", "EmptyJump", "Room");
        }
    }


    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other)
    {
        
        if (!ParentZubat.isEmptyInfatuationDone && other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            if (p != null)
            {
                p.ConfusionFloatPlus(0.5f);
            }
        }
        if (ParentZubat.isEmptyInfatuationDone && other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                e.EmptyConfusion(7.5f, 1);
            }
        }
    }
}
