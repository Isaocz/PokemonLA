using System.Collections;
using UnityEngine;

/// <summary>
/// 技能3：旋转星光。
/// 发射一颗缓慢移动且自转的主星；主星按固定间隔散射星光，
/// 每轮散射相对上一轮偏转固定角度，而不是重新随机。
/// </summary>
public class StampStar : MewBaseSkill
{
    [Header("主星")]
    public GameObject projectilePref;
    public float projectileSpeed = 3f;
    public float mainStarSpinSpeed = 60f;

    [Header("散射星光")]
    public GameObject starsPref;
    public float starSpeed;
    public float starAcceratation = 1f;
    [Min(1)] public int starsCount = 4;
    [Min(1)] public int burstCount = 5;
    [Min(0.05f)] public float releaseInterval = 1.5f;
    public float rotationPerBurst = 15f;
    public bool randomizeFirstBurst = true;

    private GameObject stamp;

    public override IEnumerator CoreLogic()
    {
        if (projectilePref == null || starsPref == null)
        {
            Debug.LogError(
                name + "：StampStar 必须设置 projectilePref 和 starsPref。",
                this);
            yield break;
        }

        Transform target = GetPlayerTransform();
        if (target == null)
        {
            Debug.LogWarning(name + "：StampStar 找不到玩家。", this);
            yield break;
        }

        Vector2 direction =
            ((Vector2)target.position - (Vector2)SkillOrigin).normalized;

        stamp = Instantiate(
            projectilePref,
            SkillOrigin,
            Quaternion.identity);
        RegisterTemporaryObject(stamp);

        BarrageProjectile mainProjectile =
            stamp.GetComponent<BarrageProjectile>();

        if (mainProjectile == null)
        {
            Debug.LogError(
                projectilePref.name + " 缺少 BarrageProjectile。",
                stamp);
            Destroy(stamp);
            UnregisterTemporaryObject(stamp);
            yield break;
        }

        mainProjectile.empty = empty;
        mainProjectile.IsSpin = true;
        mainProjectile.SpinSpeed = mainStarSpinSpeed;
        mainProjectile.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
        mainProjectile.SetDirection(direction);
        mainProjectile.SetSpeed(projectileSpeed);

        int currentStarCount = IsPhase2OrLater
            ? Mathf.Max(1, starsCount + 2)
            : Mathf.Max(1, starsCount);

        float burstRotation = randomizeFirstBurst
            ? Random.Range(0f, 360f)
            : 0f;

        int totalBursts = Mathf.Max(1, burstCount);
        for (int burstIndex = 0; burstIndex < totalBursts; burstIndex++)
        {
            yield return new WaitForSeconds(releaseInterval);

            if (stamp == null || mainProjectile == null || mainProjectile.FadeMode != 0)
            {
                break;
            }

            SpawnBurst(currentStarCount, burstRotation);
            burstRotation += rotationPerBurst;
        }

        if (mainProjectile != null)
        {
            mainProjectile.StopMovement();
            mainProjectile.FadeMode = 1;
        }

        if (stamp != null)
        {
            UnregisterTemporaryObject(stamp);
            Destroy(stamp, 0.5f);
        }
    }

    private void SpawnBurst(int count, float rotation)
    {
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = rotation + i * angleStep;
            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            // 移动方向由 BarrageProjectile.direction 控制。
            // 保持 Prefab 原始旋转，避免贴图/子物体被额外旋转。
            GameObject starObject = Instantiate(
                starsPref,
                stamp.transform.position,
                Quaternion.identity);

            BarrageProjectile star =
                starObject.GetComponent<BarrageProjectile>();

            if (star == null)
            {
                Debug.LogError(
                    starsPref.name + " 缺少 BarrageProjectile。",
                    starObject);
                Destroy(starObject);
                continue;
            }

            star.empty = empty;
            star.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
            star.SetDirection(direction);
            star.SetSpeed(starSpeed, starAcceratation);
        }
    }

    protected override int GetExecutionCount()
    {
        // 本技能的轮次已经由脚本内部管理，避免 Prefab 上旧 repeat 数值重复整套技能。
        return 1;
    }
}
