using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraEffect : MonoBehaviour
{

    public List<ParticleSystem> PSList;

    private void Start()
    {
        //Timer.Start(this , 20.0f , () => { GetComponent<Animator>().SetTrigger("Over"); });
    }

    void AuroraOver()
    {
        foreach (ParticleSystem ps in PSList)
        {
            var Emission1 = ps.emission;
            var Main1 = ps.main;
            Emission1.enabled = false;
            Main1.loop = false;
            ps.transform.parent = null;
        }
    }

    public void AuroraOverStart()
    {
        GetComponent<Animator>().SetTrigger("Over");
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
