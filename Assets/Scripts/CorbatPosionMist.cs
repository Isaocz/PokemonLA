using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorbatPosionMist : MonoBehaviour
{
    float BlastTimer;
    bool isBlast;
    public Empty ParentEmpty;

    // Update is called once per frame
    void Update()
    {
        BlastTimer += Time.deltaTime;
        if (ParentEmpty != null && !ParentEmpty.isDie) {
            if (!isBlast && BlastTimer >= 5.05f)
            {
                transform.GetChild(0).gameObject.SetActive(false); isBlast = true;
            }
        }
        else
        {
            if (!isBlast) {
                var e1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
                e1.rateOverTime = 0;
                transform.GetChild(1).gameObject.SetActive(false);
                var e2 = transform.GetComponent<ParticleSystem>().main;
                e2.startColor = new Color(0, 0, 0, 0);
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(false);
                var e2 = transform.GetComponent<ParticleSystem>().main;
                e2.simulationSpeed = 10;
            }
        }
    }
    private void OnDestroy()
    {
        ParentEmpty.GetComponent<Corbat>().IsPosionMistExit = false;
    }
}
