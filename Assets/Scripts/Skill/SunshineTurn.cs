using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunshineTurn : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.back * Time.deltaTime*0.5f);
    }

}
