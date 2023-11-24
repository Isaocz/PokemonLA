using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorbatPosionMist : MonoBehaviour
{
    float BlastTimer;
    bool isBlast;

    // Update is called once per frame
    void Update()
    {
        BlastTimer += Time.deltaTime;
        if (!isBlast && BlastTimer >= 5.05f) 
        {
            transform.GetChild(0).gameObject.SetActive(false); isBlast = true;
        }
    }
}
