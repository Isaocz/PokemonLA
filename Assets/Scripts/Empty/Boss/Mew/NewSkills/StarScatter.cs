using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能1：神圣新星。
/// 一阶段释放3圈、每圈12颗；弹幕先减速，再重新加速。
/// 二阶段预留为5圈且二次加速度更高。
/// </summary>
public class StarScatter : MewBaseSkill
{
    [Header("弹幕")]
    public GameObject projectileprf;
    [Min(1)] public int projectileNum = 12;
    public float spreadAngle = 360f;

    [Header("圈数")]
    [Min(1)] public int phase1RingCount = 3;
    [Min(1)] public int phase2RingCount = 5;
    [Min(0f)] public float ringInterval = 0.25f;
    public float rotationPerRing = 15f;

    [Header("变速")]
    public float initialSpeed = 6f;
    public float deceleration = -10f;
    [Min(0.05f)] public float decelerationDuration = 0.45f;
    public float minimumSpeed = 0.75f;
    public float phase1Acceleration = 6f;
    public float phase2Acceleration = 9f;

    public override IEnumerator CoreLogic()
    {
        if (projectileprf == null)
        {
            Debug.LogError(name + "：StarScatter 未设置 projectileprf。", this);
            yield break;
        }

        int ringCount = IsPhase2OrLater ? phase2RingCount : phase1RingCount;
        int bulletCount = Mathf.Max(1, projectileNum);
        float acceleration = IsPhase2OrLater
            ? phase2Acceleration
            : phase1Acceleration;

        for (int ringIndex = 0; ringIndex < ringCount; ringIndex++)
        {
            List<BarrageProjectile> ringProjectiles =
                new List<BarrageProjectile>(bulletCount);

            float ringRotation = ringIndex * rotationPerRing;
            float angleStep = spreadAngle / bulletCount;

            for (int bulletIndex = 0; bulletIndex < bulletCount; bulletIndex++)
            {
                float angle = ringRotation + bulletIndex * angleStep;
                Vector2 direction =
                    Quaternion.Euler(0f, 0f, angle) * Vector2.right;

                GameObject projectileObject = Instantiate(
                    projectileprf,
                    SkillOrigin,
                    Quaternion.Euler(0f, 0f, angle));

                BarrageProjectile projectile =
                    projectileObject.GetComponent<BarrageProjectile>();

                if (projectile == null)
                {
                    Debug.LogError(
                        projectileprf.name + " 缺少 BarrageProjectile。",
                        projectileObject);
                    Destroy(projectileObject);
                    continue;
                }

                projectile.empty = empty;
                projectile.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                projectile.SetDirection(direction);
                projectile.SetSpeed(initialSpeed, deceleration);
                ringProjectiles.Add(projectile);
            }

            yield return new WaitForSeconds(decelerationDuration);

            for (int i = 0; i < ringProjectiles.Count; i++)
            {
                BarrageProjectile projectile = ringProjectiles[i];
                if (projectile != null && projectile.FadeMode == 0)
                {
                    projectile.SetSpeed(minimumSpeed, acceleration);
                }
            }

            if (ringIndex < ringCount - 1 && ringInterval > 0f)
            {
                yield return new WaitForSeconds(ringInterval);
            }
        }
    }

    protected override int GetExecutionCount()
    {
        // 本技能的轮次已经由脚本内部管理，避免 Prefab 上旧 repeat 数值重复整套技能。
        return 1;
    }
}
