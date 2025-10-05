using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternaBeamRotate : MonoBehaviour
{
    public float rotationSpeed;
    public float timer;
    private float currentSpeed = 0f;
    private float time = 0f;
    private PlayerControler player;
    private new Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        time += Time.deltaTime;

        if (collider != null)
        {
            Vector3 closestPoint = collider.ClosestPoint(player.transform.position);
            float distance = Vector3.Distance(closestPoint, player.transform.position);
            float newRotationSpeed = rotationSpeed;

            if (distance < 5f)
            {
                newRotationSpeed = 20f + (distance / 5f) * 30f;
                currentSpeed = newRotationSpeed;
            }

            if (time < timer)
            {
                currentSpeed = newRotationSpeed * time / timer;
            }
        }

        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }
}
