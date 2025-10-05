using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCharge : MewBaseSkill
{
    public GameObject MeleePref;
    public GameObject BarragePref;
    public float StartCharge = 0.5f;
    public float ChargeTime = 1f;
    public Vector3 TransformOffset = new Vector3(0, 0.5f, 0);
    public float BarrageInterval = 0.1f;

    private bool isCharge;
    private Vector2 direction;

    public override IEnumerator CoreLogic()
    {
        GameObject sc = Instantiate(MeleePref, empty.transform.position + TransformOffset, Quaternion.identity);
        MeleeProjectile mp = sc.GetComponent<MeleeProjectile>();
        if (mp != null)
        {
            mp.empty = this.empty;
            mp.SetBehavior(MeleeProjectile.projectileBehavior.None);
        }

        if(StartCharge > 0f)
        {
            yield return new WaitForSeconds(StartCharge);
            direction = (GetPlayerTransform().position - empty.transform.position).normalized;
        }

        if (mp != null)
        {
            mp.empty = this.empty;
            mp.SetBehavior(MeleeProjectile.projectileBehavior.TargetStraight, MeleeProjectile.MeleeWay.Sine);
            mp.SetTime(ChargeTime);
            mp.SetTarget(GetPlayerTransform());

            isCharge = true;
            StartCoroutine(createBarrage());
        }
        yield return StartCoroutine(SkillRepeat());
        isCharge = false;
    }

    private IEnumerator createBarrage()
    {
        while (BarragePref != null && isCharge)
        {
            yield return new WaitForSeconds(BarrageInterval);
            GameObject barrage = Instantiate(BarragePref, empty.transform.position + TransformOffset, Quaternion.identity);
            BarrageProjectile bp = barrage.GetComponent<BarrageProjectile>();
            if (bp != null)
            {
                bp.empty = this.empty;
                bp.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                if(direction != null)
                {
                    bp.SetDirection(direction);
                }
                bp.SetSpeed(1f, 4f);
            }
        }
    }
}
