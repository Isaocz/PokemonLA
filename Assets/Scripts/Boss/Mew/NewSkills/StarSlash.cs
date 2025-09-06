using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSlash : MewBaseSkill
{
    public GameObject ProjectilePref;
    public float projectileSpeed = 3f;
    public float projectileAcceration = 5f;

    public override IEnumerator CoreLogic()
    {

        GameObject starSlash = Instantiate(ProjectilePref, transform.position, Quaternion.identity);
        BarrageProjectile bp = starSlash.GetComponent<BarrageProjectile>();
        bp.empty = this.empty;
        if (bp != null) 
        {
            bp.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
            bp.SetDirection(GetPlayerDirection());
            bp.SetSpeed(projectileSpeed, projectileAcceration);
        }
        yield return null;
    }
}
