using System.Collections;
using UnityEngine;

/// <summary>
/// 技能2：星光漫射。
/// 一阶段向玩家连续发射3轮、每轮3颗弱追踪星光；中央弹始终朝向玩家。
/// 二阶段每轮增加为5颗。
/// </summary>
public class StarShoot : MewBaseSkill
{
    [Header("弹幕")]
    public GameObject projectilePref;
    [Min(1)] public int repeatSkill = 3;
    [Min(0f)] public float repeatInterval = 0.3f;
    [Min(1)] public int projectileNum = 3;
    [Min(0f)] public float spreadAngle = 40f;
    public float projectileSpeed = 8f;

    [Header("弱追踪")]
    [Min(0f)] public float targetStrength = 1.25f;
    [Min(0f)] public float targetStartDistance = 7f;
    [Min(0f)] public float targetStopDistance = 2f;

    public override IEnumerator CoreLogic()
    {
        if (projectilePref == null)
        {
            Debug.LogError(name + "：StarShoot 未设置 projectilePref。", this);
            yield break;
        }

        int volleyCount = Mathf.Max(1, repeatSkill);
        int shotCount = IsPhase2OrLater
            ? Mathf.Max(1, projectileNum + 2)
            : Mathf.Max(1, projectileNum);

        for (int volleyIndex = 0; volleyIndex < volleyCount; volleyIndex++)
        {
            Transform target = GetPlayerTransform();
            if (target == null)
            {
                Debug.LogWarning(name + "：StarShoot 找不到玩家，终止技能。", this);
                yield break;
            }

            Vector2 baseDirection =
                ((Vector2)target.position - (Vector2)SkillOrigin).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            for (int shotIndex = 0; shotIndex < shotCount; shotIndex++)
            {
                float normalizedOffset = shotCount <= 1
                    ? 0f
                    : shotIndex / (float)(shotCount - 1) - 0.5f;

                float currentAngle = baseAngle + normalizedOffset * spreadAngle;
                Vector2 direction =
                    Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right;

                GameObject projectileObject = Instantiate(
                    projectilePref,
                    SkillOrigin,
                    Quaternion.Euler(0f, 0f, currentAngle));

                BarrageProjectile projectile =
                    projectileObject.GetComponent<BarrageProjectile>();

                if (projectile == null)
                {
                    Debug.LogError(
                        projectilePref.name + " 缺少 BarrageProjectile。",
                        projectileObject);
                    Destroy(projectileObject);
                    continue;
                }

                projectile.empty = empty;
                projectile.SetBehavior(BarrageProjectile.projectileBehavior.CloseTarget);
                projectile.SetDirection(direction);
                projectile.SetSpeed(projectileSpeed);
                projectile.SetTarget(
                    target,
                    targetStrength,
                    targetStartDistance,
                    targetStopDistance);
            }

            if (volleyIndex < volleyCount - 1 && repeatInterval > 0f)
            {
                yield return new WaitForSeconds(repeatInterval);
            }
        }
    }

    protected override int GetExecutionCount()
    {
        // 本技能的轮次已经由脚本内部管理，避免 Prefab 上旧 repeat 数值重复整套技能。
        return 1;
    }
}
