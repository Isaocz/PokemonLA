using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglypuffSing : MonoBehaviour
{
    // Start is called before the first frame update

    float MaxRadius;
    float SingTimer;
    CircleCollider2D SingRange;
    Empty ParentEmpty;

    private void Start()
    {
        MaxRadius = 4.3f;
        SingRange = GetComponent<CircleCollider2D>();
        ParentEmpty = transform.parent.GetComponent<Empty>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<PlayerControler>() != null) {
                other.GetComponent<PlayerControler>().SleepFloatPlus(0.2f);
            }
        }   
        if(ParentEmpty.isEmptyInfatuationDone && (other.tag == "Empty") && other.gameObject != ParentEmpty.gameObject){
            other.GetComponent<Empty>().EmptySleepDone(0.1f, 10 ,1);
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
        SingTimer += Time.deltaTime;
        if (SingRange.radius <= MaxRadius)
        {
            SingRange.radius += Time.deltaTime;
        }
        if (SingTimer >= 5.4f)
        {
            SingRange.radius -= Time.deltaTime * 1.8f;
        }
    }

    public void SingOver()
    {
        GetComponent<CircleCollider2D>().radius -= 1.0f;
        MaxRadius -= 1.0f;
    }
}
