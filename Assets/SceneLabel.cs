using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLabel : MonoBehaviour
{
    public float Duration;
    void Start()
    {
        Destroy(gameObject, Duration);
    }

}
