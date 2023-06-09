using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLeavePSStop : MonoBehaviour
{
    ParticleSystem ThisPS;

    private void Start()
    {
        ThisPS = this.GetComponent<ParticleSystem>();
    }
    private void OnBecameInvisible()
    {
        ThisPS.Pause();
    }

    private void OnBecameVisible()
    {
        ThisPS.Play();
    }
}
