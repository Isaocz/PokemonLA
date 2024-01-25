using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetScentMawile : MonoBehaviour
{
    ParticleSystem PS1;
    float Timer ;
    public Empty ParentEmpty;

    // Start is called before the first frame update
    void Start()
    {
        PS1 = transform.GetChild(0).GetComponent<ParticleSystem>();
        var PS1Main = PS1.main;
        PS1Main.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 6.0f) { Destroy(gameObject); }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            if (p != null)
            {
                p.SpeedChange();
                p.SpeedRemove01(3.0f);
            }
        }
        else if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                e.SpeedChange();
                e.SpeedRemove01(3.0f);
            }
        }
    }

}
