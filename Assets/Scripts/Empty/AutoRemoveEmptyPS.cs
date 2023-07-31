using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemoveEmptyPS : MonoBehaviour
{

    ParticleSystem ps;
    Empty ParentEmpty;
    
    // Start is called before the first frame update
    void Start()
    {
        ParentEmpty = transform.parent.GetComponent<Empty>();
        ps = GetComponent<ParticleSystem>();
        ps.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.isPaused && !ParentEmpty.isBorn)
        {
            ps.Play();
        }

        if (ps.isStopped)
        {
            Destroy(gameObject);
        }
        
    }
}
