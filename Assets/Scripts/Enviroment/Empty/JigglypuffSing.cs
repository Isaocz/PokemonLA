using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglypuffSing : MonoBehaviour
{
    // Start is called before the first frame update

    float MaxRadius;

    private void Start()
    {
        MaxRadius = 4.6f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerControler>().SleepFloatPlus(0.2f);
        }   
    }


    void Update()
    {

        if (transform.GetChild(0).GetComponent<ParticleSystem>().isStopped && transform.GetChild(1).GetComponent<ParticleSystem>().isStopped)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(GetComponent<CircleCollider2D>().radius <= MaxRadius)
        {
            GetComponent<CircleCollider2D>().radius += Time.deltaTime;
        }
    }

    public void SingOver()
    {
        GetComponent<CircleCollider2D>().radius -= 1.0f;
        MaxRadius -= 1.0f;
    }
}
