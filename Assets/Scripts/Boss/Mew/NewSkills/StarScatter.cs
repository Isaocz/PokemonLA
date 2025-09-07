using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScatter : MewBaseSkill
{
    public GameObject projectileprf;
    public int projectileNum;
    public float spreadAngle = 360f;

    public override IEnumerator CoreLogic()
    {
        // 获取玩家引用
        Transform playerTransform = null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        for (int i = 0; i < 3; i++)
        {
            List<GameObject> ps = new List<GameObject>();
            for (int j = 0; j < projectileNum; j++)
            {
                float angle = j * (spreadAngle / projectileNum) + i * 15;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

                GameObject projectile = Instantiate(projectileprf, transform.position, Quaternion.identity);
                ps.Add(projectile);

                BarrageProjectile msl = projectile.GetComponent<BarrageProjectile>();
                msl.empty = this.empty;
                if (msl != null)
                {
                    msl.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                    msl.SetDirection(direction);
                    msl.SetSpeed(6f, -20f);
                }
            }
            yield return new WaitForSeconds(0.3f);

            for (int k = 0; k < ps.Count; k++)
            {
                BarrageProjectile msl = ps[k].GetComponent<BarrageProjectile>();

                if (msl != null)
                {
                    msl.SetSpeed(0f, 6f);
                }
            }
            ps.Clear();
        }
    }

   
}
