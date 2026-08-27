using System.Collections;
using UnityEngine;

/// <summary>
/// 技能5：星之刃。
/// 一阶段连续发射3枚逐渐加速的星之刃；命中玩家或障碍物后由 SplitStarSlash
/// 分裂成8方向星光。二阶段数量+2且发射间隔缩短。
/// </summary>
public class StarSlash : MewBaseSkill
{
    [Header("星之刃")]
    public GameObject ProjectilePref;
    public float projectileSpeed = 3f;
    public float projectileAcceration = 5f;

    [Header("连射")]
    [Min(1)] public int phase1ShotCount = 3;
    [Min(0f)] public float phase1ShotInterval = 0.45f;
    [Min(0f)] public float phase2ShotInterval = 0.28f;

    [Header("视觉旋转")]
    [Tooltip("星之刃根对象的自转速度，单位为度/秒。原 Prefab 的 3 度/帧约等于 180 度/秒。")]
    [Min(0f)] public float slashSpinSpeed = 180f;

    public override IEnumerator CoreLogic()
    {
        if (ProjectilePref == null)
        {
            Debug.LogError(name + "：StarSlash 未设置 ProjectilePref。", this);
            yield break;
        }

        int shotCount = IsPhase2OrLater
            ? Mathf.Max(1, phase1ShotCount + 2)
            : Mathf.Max(1, phase1ShotCount);

        float shotInterval = IsPhase2OrLater
            ? phase2ShotInterval
            : phase1ShotInterval;

        for (int shotIndex = 0; shotIndex < shotCount; shotIndex++)
        {
            Transform target = GetPlayerTransform();
            if (target == null)
            {
                yield break;
            }

            Vector2 direction =
                ((Vector2)target.position - (Vector2)SkillOrigin).normalized;
            // 原 Prefab 的贴图和子物体按 Quaternion.identity 制作。
            // 移动方向与贴图旋转分离，避免生成瞬间被方向角扭转。
            GameObject slashObject = Instantiate(
                ProjectilePref,
                SkillOrigin,
                Quaternion.identity);

            // AirSlashEmpty 1 由根刃体 + 子 StarLight/Trail 组成。
            // 两层必须使用同一个局部中心，否则根对象旋转时会出现图中那种错位。
            PrepareCompositeVisual(slashObject);

            BarrageProjectile projectile =
                slashObject.GetComponent<BarrageProjectile>();

            if (projectile == null)
            {
                Debug.LogError(
                    ProjectilePref.name + " 缺少 BarrageProjectile。",
                    slashObject);
                Destroy(slashObject);
            }
            else
            {
                projectile.empty = empty;
                projectile.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                projectile.SetDirection(direction);
                projectile.SetSpeed(projectileSpeed, projectileAcceration);
                projectile.IsSpin = slashSpinSpeed > 0f;
                projectile.SpinSpeed = slashSpinSpeed;

                SplitStarSlash splitter = slashObject.GetComponent<SplitStarSlash>();
                if (splitter != null)
                {
                    splitter.SplitStarSlashNum = 8;
                }
            }

            if (shotIndex < shotCount - 1 && shotInterval > 0f)
            {
                yield return new WaitForSeconds(shotInterval);
            }
        }
    }


    private static void PrepareCompositeVisual(GameObject slashObject)
    {
        if (slashObject == null)
        {
            return;
        }

        Transform root = slashObject.transform;

        Vector2 visualCenter = Vector2.zero;
        BoxCollider2D rootBox = slashObject.GetComponent<BoxCollider2D>();
        if (rootBox != null)
        {
            visualCenter = rootBox.offset;
        }
        else
        {
            Collider2D rootCollider = slashObject.GetComponent<Collider2D>();
            if (rootCollider != null)
            {
                visualCenter = rootCollider.offset;
            }
        }

        BarrageProjectile[] nestedProjectiles =
            slashObject.GetComponentsInChildren<BarrageProjectile>(true);

        for (int i = 0; i < nestedProjectiles.Length; i++)
        {
            BarrageProjectile nested = nestedProjectiles[i];

            if (nested == null || nested.gameObject == slashObject)
            {
                continue;
            }

            // 子 StarLight 在这个 Prefab 中只是贴图与尾迹，
            // 不应再拥有第二套移动和命中逻辑。
            nested.enabled = false;

            Rigidbody2D nestedBody = nested.GetComponent<Rigidbody2D>();
            if (nestedBody != null)
            {
                nestedBody.velocity = Vector2.zero;
                nestedBody.angularVelocity = 0f;
                nestedBody.simulated = false;
            }

            Collider2D[] nestedColliders =
                nested.GetComponents<Collider2D>();

            for (int j = 0; j < nestedColliders.Length; j++)
            {
                if (nestedColliders[j] != null)
                {
                    nestedColliders[j].enabled = false;
                }
            }
        }

        Transform starLight = root.Find("StarLight");
        if (starLight == null)
        {
            return;
        }

        float originalZ = starLight.localPosition.z;
        starLight.localPosition =
            new Vector3(visualCenter.x, visualCenter.y, originalZ);
        starLight.localRotation = Quaternion.identity;

        TrailRenderer[] trails =
            starLight.GetComponentsInChildren<TrailRenderer>(true);

        for (int i = 0; i < trails.Length; i++)
        {
            if (trails[i] == null)
            {
                continue;
            }

            trails[i].Clear();
            trails[i].emitting = true;
        }
    }



    protected override int GetExecutionCount()
    {
        // 本技能的轮次已经由脚本内部管理，避免 Prefab 上旧 repeat 数值重复整套技能。
        return 1;
    }
}
