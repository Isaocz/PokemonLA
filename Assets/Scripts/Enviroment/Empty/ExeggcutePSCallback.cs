using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExeggcutePSCallback : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        Debug.Log(other);
        int i = 0;

        while (i < numCollisionEvents)
        {
            Debug.Log(i);
            Debug.Log(collisionEvents[i].colliderComponent.gameObject);
            //if (rb)
            //{
            //    //Vector3 pos = collisionEvents[i].intersection;
            //    //Vector3 force = collisionEvents[i].velocity * 10;
            //    //rb.AddForce(force);
            //}
            i++;
        }
    }

    void OnParticleTrigger()
    {
        //Debug.Log("111111");
    }
}

