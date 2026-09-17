using System.Collections;
using UnityEngine;

/// <summary>Three telegraphed dashes with star emissions measured along actual travelled distance.</summary>
public class StarCharge : MewBaseSkill
{
    [SerializeField] private GameObject dashArrowPrefab;
    public GameObject MeleePref;
    public GameObject BarragePref;
    public GameObject LingeringNotePrefab;
    [Min(1)] public int dashCount = 3;
    [Min(0f)] public float StartCharge = 0.8f;
    [Min(0.35f)] public float followupWarningTime = 0.55f;
    [Min(0.05f)] public float ChargeTime = 1f;
    [Min(0f)] public float dashInterval = 0.35f;
    public Vector3 TransformOffset = new Vector3(0f, 0.5f, 0f);
    public MeleeProjectile.MeleeWay movementCurve = MeleeProjectile.MeleeWay.Sine;
    [HideInInspector] public float BarrageInterval = 0.1f; // Legacy prefab value; emission now uses distance.
    [Min(0.25f)] public float barrageSpacing = 1.5f;
    public float barrageStartSpeed = 1f;
    public float barrageAcceleration = 4f;
    [Min(0f)] public float lingeringNoteDuration = 2.5f;

    public override IEnumerator CoreLogic()
    {
        if (MeleePref == null || BarragePref == null)
        {
            Debug.LogError(name + ": missing dash or star prefab.", this);
            yield break;
        }
        for (int dash = 0; dash < Mathf.Max(1, dashCount); dash++)
        {
            Transform target = GetPlayerTransform();
            if (target == null || empty == null) yield break;
            GameObject obj = Instantiate(MeleePref, empty.transform.position + TransformOffset, Quaternion.identity);
            RegisterTemporaryObject(obj);
            MeleeProjectile melee = obj.GetComponent<MeleeProjectile>();
            if (melee == null) { Destroy(obj); yield break; }
            melee.empty = empty;
            melee.TransformOffset = TransformOffset;
            melee.DamageEnabled = false;
            melee.SetBehavior(MeleeProjectile.projectileBehavior.None);
            MewDashPreview preview = MewDashPreview.Create(dashArrowPrefab);
            RegisterTemporaryObject(preview.gameObject);
            float warning = Mathf.Max(0.35f, dash == 0 ? Mathf.Max(0.55f, StartCharge) : followupWarningTime);
            Vector2 end = melee.PrepareDash(target);
            for (float t = 0f; t < warning; t += Time.deltaTime)
            {
                if (empty == null || target == null || melee == null) yield break;
                if (t < warning - 0.25f) end = melee.PrepareDash(target);
                preview.Show(empty.transform.position + TransformOffset, end,
                    Mathf.Clamp01(t / (warning * 0.65f)), false, Mathf.Max(0.5f, melee.HitRadius * 2f));
                yield return null;
            }
            melee.SetTime(ChargeTime);
            melee.SetBehavior(MeleeProjectile.projectileBehavior.TargetStraight, movementCurve);
            melee.DamageEnabled = true;
            Vector2 previous = empty.transform.position + TransformOffset;
            Vector2 direction = (end - previous).normalized;
            var spacing = new MewDashSpacing(barrageSpacing);
            SpawnDashBarrage(previous, direction);
            float elapsed = 0f;
            while (melee != null && !melee.IsMovementFinished && elapsed < ChargeTime + 0.5f)
            {
                Vector2 current = empty.transform.position + TransformOffset;
                EmitSegment(previous, current, direction, ref spacing);
                previous = current;
                preview.Show(current, end, 1f, true, Mathf.Max(0.5f, melee.HitRadius * 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (empty == null) yield break;
            // Include the final movement sample regardless of Update/coroutine ordering.
            EmitSegment(previous, empty.transform.position + TransformOffset, direction, ref spacing);
            SpawnLingeringNote(empty.transform.position);
            UnregisterTemporaryObject(preview.gameObject);
            Destroy(preview.gameObject);
            UnregisterTemporaryObject(obj);
            if (obj != null) Destroy(obj);
            if (dash < dashCount - 1 && dashInterval > 0f) yield return new WaitForSeconds(dashInterval);
        }
    }

    private void EmitSegment(Vector2 from, Vector2 to, Vector2 direction, ref MewDashSpacing spacing)
    {
        Vector2 delta = to - from;
        float length = delta.magnitude;
        if (length < 0.00001f) return;
        Vector2 along = delta / length;
        float travelled = 0f;
        while (spacing.TryTake(length, ref travelled, out float offset))
        {
            SpawnDashBarrage(from + along * offset, direction);
        }
    }

    private void SpawnDashBarrage(Vector2 position, Vector2 direction)
    {
        if (empty == null || (empty is Mew boss && boss.IsEnding)) return;
        if (!IsPhase2OrLater) SpawnSingle(position, direction);
        else
        {
            Vector2 side = new Vector2(-direction.y, direction.x);
            SpawnSingle(position, side);
            SpawnSingle(position, -side);
        }
    }

    private void SpawnSingle(Vector2 position, Vector2 direction)
    {
        GameObject obj = MewStarPool.Spawn(BarragePref, position,
            Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg), empty);
        BarrageProjectile bullet = obj.GetComponent<BarrageProjectile>();
        if (bullet == null) { Destroy(obj); return; }
        bullet.empty = empty;
        bullet.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
        bullet.SetDirection(direction);
        bullet.SetSpeed(barrageStartSpeed, barrageAcceleration);
    }

    private void SpawnLingeringNote(Vector3 position)
    {
        if (LingeringNotePrefab == null) return;
        GameObject obj = Instantiate(LingeringNotePrefab, position, Quaternion.identity);
        if (obj.TryGetComponent(out Projectile projectile)) projectile.empty = empty;
        Destroy(obj, Mathf.Max(0.05f, lingeringNoteDuration));
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason) { CleanupTemporaryObjects(); }
    protected override int GetExecutionCount() { return 1; }
}
