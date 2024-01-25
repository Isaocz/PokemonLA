using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSubPS : MonoBehaviour
{

    ParticleSystem PS;
    float SubTimer;
    int isSubLunched;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        SubTimer += Time.deltaTime;
        if (SubTimer >= 0.74f && isSubLunched <= 3)
        {
            isSubLunched ++;
            PS.TriggerSubEmitter(0);
            PS.TriggerSubEmitter(1);
        }
    }

}
