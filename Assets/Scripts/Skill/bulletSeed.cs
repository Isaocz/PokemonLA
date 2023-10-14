using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletSeed : GrassSkill
{
    int Count;
    public BulletSeedShoot Seed;

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
        for (int i = 0; i < counts; i++)
        {
            BulletSeedShoot Seeds = Instantiate(Seed, transform.position, transform.rotation);
            Seeds.ParentBS = this;
            Seeds.player = this.player;
            yield return new WaitForSeconds(0.075f);
        }
    }

}
