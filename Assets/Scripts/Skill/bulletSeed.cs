using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletSeed : Skill
{
    int Count;
    public GameObject Seed;
    public GameObject SeedAdvanced;

    void Start()
    {
        Count = Count2_5();
        StartCoroutine(ShootSeed(Count));
    }

    void Update()
    {
        StartExistenceTimer();
    }

    IEnumerator ShootSeed(int counts)
    {
        if (SkillFrom != 2)
        {
            for (int i = 0; i < counts; i++)
            {
                GameObject Seeds = Instantiate(Seed, transform.position, Quaternion.identity);
                Seeds.GetComponent<BulletSeedShoot>().player = this.player;
                Seeds.GetComponent<BulletSeedShoot>().moveDirection = transform.right;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < counts; i++)
            {
                GameObject Seeds2 = Instantiate(SeedAdvanced, transform.position, Quaternion.identity);
                Seeds2.GetComponent<BulletSeedShoot>().player = this.player;
                Seeds2.GetComponent<BulletSeedShoot>().moveDirection = transform.right;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

}
