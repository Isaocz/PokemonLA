using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticalController : MonoBehaviour
{
    private ParticleSystem ps;
    private BarrageProjectile bp;
    public GameObject Child;
    private bool IsStart = false;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        bp = GetComponent<BarrageProjectile>();
        ps.Stop();
    }

    void Update()
    {
        if (ps != null) 
        { 
            if((bp.moveBehavior == BarrageProjectile.projectileBehavior.Target || bp.moveBehavior == BarrageProjectile.projectileBehavior.CloseTarget) && !bp.isTargeting && !IsStart)
            {
                ps.Play();
                IsStart = true;
            }else if (bp.isTargeting && IsStart)
            {
                ps.Stop();
                IsStart= false;
            }
        }

        if (Child && bp.FadeMode == 2)
        {
            Child.gameObject.SetActive(true);
        }
    }
}
