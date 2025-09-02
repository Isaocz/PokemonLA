using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampStar : MewBaseSkill
{
    public GameObject projectilePref;
    public GameObject starsPref;
    public float projectileSpeed = 3f;
    public float starSpeed = 0f;
    public float starAcceratation = 1f;
    public int starsCount = 4;
    public float releaseInterval = 1.5f;

    private GameObject Stamp;

    public override IEnumerator CoreLogic()
    {
        Transform playerTransform = null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        Stamp = Instantiate(projectilePref, transform.position, Quaternion.identity);
        BarrageProjectile bp = Stamp.GetComponent<BarrageProjectile>();
        bp.empty = this.empty;

        if (!empty)
        {
            bp.empty = this.empty;
        }
        

        if (bp != null)
        {
            bp.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
            bp.SetDirection(direction);
            bp.SetSpeed(projectileSpeed);
        }

        yield return StartCoroutine(SpawnStars());
    }

    IEnumerator SpawnStars()
    {
        while (Stamp != null)
        {
            float randomAngle = Random.Range(0f, 360f);
            for (int i = 0; i < starsCount; i++)
            {
                float angle = i * 360 / starsCount + randomAngle;
                Vector2 direction2 = Quaternion.Euler(0, 0, angle) * Vector2.right;
                GameObject stars = Instantiate(starsPref, Stamp.transform.position, Quaternion.identity);
                BarrageProjectile bp2 = stars.GetComponent<BarrageProjectile>();
                bp2.empty = this.empty;
                if (bp2 != null)
                {
                    bp2.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                    bp2.SetDirection(direction2);
                    bp2.SetSpeed(starSpeed, starAcceratation);
                }
            }
            yield return new WaitForSeconds(releaseInterval);
        }
    }

}
