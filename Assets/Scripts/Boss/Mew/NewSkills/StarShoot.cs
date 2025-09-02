using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShoot : MewBaseSkill
{
    public GameObject projectilePref;
    public int repeatSkill;
    public float repeatInterval = 0.3f;
    public int projectileNum;
    public float spreadAngle = 60f;
    public float projectileSpeed = 8f;

    public override IEnumerator CoreLogic()
    {
        // 获取玩家引用
        Transform playerTransform = null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        for (int i = 0; i < repeatSkill; i++) 
        {
            Vector2 baseDirection = (playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            for (int j = 0; j < projectileNum; j++) 
            {
                float angleOffset = (j - (projectileNum - 1) / 2f) * (spreadAngle / projectileNum);

                // 应用角度偏移并转换为方向向量
                float currentAngle = baseAngle + angleOffset;
                Vector2 direction = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                ).normalized;

                GameObject Projectile = Instantiate(projectilePref, transform.position, Quaternion.identity);
                BarrageProjectile msl = Projectile.GetComponent<BarrageProjectile>();
                msl.empty = this.empty;

                if (msl != null)
                {
                    msl.SetBehavior(BarrageProjectile.projectileBehavior.CloseTarget);
                    msl.SetDirection(direction);
                    msl.SetSpeed(projectileSpeed);
                    msl.SetTarget(playerTransform, 2f, 4f, 2f);
                }
            }

            yield return new WaitForSeconds(repeatInterval);
        }

    }
}
