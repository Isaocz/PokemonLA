using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternaBeamRotate : MonoBehaviour
{
    public float rotationSpeed;
    public float timer;
    private float currentSpeed = 0f;
    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        time += Time.deltaTime;
        if (currentSpeed < rotationSpeed)
        {
            currentSpeed = rotationSpeed * time / timer;
        }
        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }
}
