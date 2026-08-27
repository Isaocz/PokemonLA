using System.Collections;
using UnityEngine;

/// <summary>
/// 技能4：星之冲刺。
/// 每次冲刺前以 MeleePref 显示环绕音符预警，随后锁定玩家位置冲刺；
/// 冲刺中持续释放沿冲刺方向缓慢加速的星光，共冲刺3次。
/// </summary>
public class StarCharge : MewBaseSkill
{
    [Header("冲刺对象与弹幕")]
    public GameObject MeleePref;
    public GameObject BarragePref;
    public GameObject LingeringNotePrefab;

    [Header("冲刺")]
    [Min(1)] public int dashCount = 3;
    [Tooltip("星之冲刺专用预警：只在第一次冲刺前等待。它属于 CoreLogic 内部，不要再叠加 skillStartup。")]
    [Min(0f)] public float StartCharge = 0.5f;
    [Tooltip("每一段冲刺本身持续的时间。")]
    [Min(0.05f)] public float ChargeTime = 1f;
    [Tooltip("三段冲刺之间的间隔，不是技能后摇。")]
    [Min(0f)] public float dashInterval = 0.35f;
    public Vector3 TransformOffset = new Vector3(0f, 0.5f, 0f);
    public MeleeProjectile.MeleeWay movementCurve = MeleeProjectile.MeleeWay.Sine;

    [Header("冲刺弹幕")]
    [Min(0.02f)] public float BarrageInterval = 0.1f;
    public float barrageStartSpeed = 1f;
    public float barrageAcceleration = 4f;

    [Header("滞留音符圈")]
    [Min(0f)] public float lingeringNoteDuration = 2.5f;


    public override IEnumerator CoreLogic()
    {
        if (MeleePref == null)
        {
            Debug.LogError(name + "：StarCharge 未设置 MeleePref。", this);
            yield break;
        }

        if (BarragePref == null)
        {
            Debug.LogError(name + "：StarCharge 未设置 BarragePref。", this);
            yield break;
        }

        int totalDashes = Mathf.Max(1, dashCount);

        for (int dashIndex = 0; dashIndex < totalDashes; dashIndex++)
        {
            Transform target = GetPlayerTransform();
            if (target == null || empty == null)
            {
                yield break;
            }

            GameObject dashObject = Instantiate(
                MeleePref,
                empty.transform.position + TransformOffset,
                Quaternion.identity);
            RegisterTemporaryObject(dashObject);

            MeleeProjectile melee = dashObject.GetComponent<MeleeProjectile>();
            if (melee == null)
            {
                Debug.LogError(
                    MeleePref.name + " 缺少 MeleeProjectile。",
                    dashObject);
                Destroy(dashObject);
                UnregisterTemporaryObject(dashObject);
                yield break;
            }

            melee.empty = empty;
            melee.TransformOffset = TransformOffset;
            melee.SetBehavior(MeleeProjectile.projectileBehavior.None);

            // 文档描述是“音符环绕后连续冲刺 3 次”，前摇只在第一段冲刺前执行一次。
            if (dashIndex == 0 && StartCharge > 0f)
            {
                yield return new WaitForSeconds(StartCharge);
            }

            Vector2 dashDirection =
                ((Vector2)target.position - (Vector2)empty.transform.position).normalized;

            melee.ResetAttack();
            melee.SetTarget(target);
            melee.SetTime(ChargeTime);
            melee.SetBehavior(
                MeleeProjectile.projectileBehavior.TargetStraight,
                movementCurve);

            Coroutine emitter = StartCoroutine(
                EmitBarrageDuringDash(melee, dashDirection));

            float timeout = ChargeTime + 0.5f;
            float elapsed = 0f;
            while (melee != null && !melee.IsMovementFinished && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (emitter != null)
            {
                StopCoroutine(emitter);
            }

            Vector3 dashEndPosition = empty.transform.position;
            SpawnLingeringNote(dashEndPosition);

            if (dashObject != null)
            {
                UnregisterTemporaryObject(dashObject);
                Destroy(dashObject);
            }

            if (dashIndex < totalDashes - 1 && dashInterval > 0f)
            {
                yield return new WaitForSeconds(dashInterval);
            }
        }
    }

    private IEnumerator EmitBarrageDuringDash(
        MeleeProjectile melee,
        Vector2 dashDirection)
    {
        while (melee != null && !melee.IsMovementFinished)
        {
            SpawnDashBarrage(dashDirection);
            yield return new WaitForSeconds(BarrageInterval);
        }
    }

    private void SpawnDashBarrage(Vector2 dashDirection)
    {
        if (empty == null)
        {
            return;
        }

        if (!IsPhase2OrLater)
        {
            SpawnSingleBarrage(dashDirection);
            return;
        }

        // 文档中的二阶段强化：每次生成2颗，分别向冲刺方向的两侧移动。
        Vector2 perpendicular = new Vector2(-dashDirection.y, dashDirection.x);
        SpawnSingleBarrage(perpendicular);
        SpawnSingleBarrage(-perpendicular);
    }

    private void SpawnSingleBarrage(Vector2 direction)
    {
        Vector3 spawnPosition = empty.transform.position + TransformOffset;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject barrageObject = Instantiate(
            BarragePref,
            spawnPosition,
            Quaternion.Euler(0f, 0f, angle));

        BarrageProjectile barrage =
            barrageObject.GetComponent<BarrageProjectile>();

        if (barrage == null)
        {
            Debug.LogError(
                BarragePref.name + " 缺少 BarrageProjectile。",
                barrageObject);
            Destroy(barrageObject);
            return;
        }

        barrage.empty = empty;
        barrage.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
        barrage.SetDirection(direction);
        barrage.SetSpeed(barrageStartSpeed, barrageAcceleration);
    }

    private void SpawnLingeringNote(Vector3 position)
    {
        if (LingeringNotePrefab == null)
        {
            return;
        }

        GameObject noteCircle = Instantiate(
            LingeringNotePrefab,
            position,
            Quaternion.identity);

        Projectile projectile = noteCircle.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.empty = empty;
        }

        Destroy(noteCircle, Mathf.Max(0.05f, lingeringNoteDuration));
    }


    protected override int GetExecutionCount()
    {
        // 本技能的轮次已经由脚本内部管理，避免 Prefab 上旧 repeat 数值重复整套技能。
        return 1;
    }
}
