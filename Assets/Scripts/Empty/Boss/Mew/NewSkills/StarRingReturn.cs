using System.Collections;
using UnityEngine;

/// <summary>Legacy component/file identity retained for prefab references. Skill six now summons star apostles.</summary>
public class StarRingReturn : MewBaseSkill
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Min(0.3f)] private float summonTime = 0.65f;
    [SerializeField, Min(0.5f)] private float firingDuration = 4f;
    [SerializeField, Min(0.06f)] private float firingInterval = 0.16f;
    [SerializeField, Min(1f)] private float starSpeed = 12f;
    [SerializeField, Range(0f, 15f)] private float phaseTwoSpread = 11f;
    [SerializeField] private float orbitSpeed = 48f;
    private GameObject[] apostles;

    private void UpdateOrbit(int index, int count, float elapsed, int walls)
    {
        if (empty == null || apostles[index] == null) return;
        float angle = (count == 2 ? 0f : 45f) + index * 360f / count + elapsed * orbitSpeed;
        Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
        Vector3 origin = empty.transform.position + Vector3.up * 0.6f;
        var hit = Physics2D.Raycast(origin, direction, 2.6f, walls);
        float radius = hit.collider != null ? Mathf.Max(0f, hit.distance - 0.6f) : 2.6f;
        apostles[index].transform.position = origin + (Vector3)direction * radius;
    }

    protected override void OnContextInitialized(MewSkillContext context) { skillName = "星之使徒"; }

    public override IEnumerator CoreLogic()
    {
        if (empty == null || projectilePrefab == null) yield break;
        int count = IsPhase2OrLater ? 3 : 2;
        apostles = new GameObject[count];
        var sprites = new SpriteRenderer[count];
        var positions = new Vector3[count];
        var nextShot = new float[count];
        SpriteRenderer source = projectilePrefab.GetComponentInChildren<SpriteRenderer>();
        int walls = LayerMask.GetMask("Room", "Enviroment", "DefaultEnvirmont");
        for (int i = 0; i < count; i++)
        {
            float angle = count == 2 ? i * 180f : 45f + i * 360f / count;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
            Vector3 origin = empty.transform.position + Vector3.up * 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, 2.6f, walls);
            float radius = hit.collider != null ? Mathf.Max(0f, hit.distance - 0.6f) : 2.6f;
            positions[i] = origin + (Vector3)direction * radius;
            var obj = new GameObject("Mew star apostle " + (i + 1));
            apostles[i] = obj;
            obj.transform.position = positions[i];
            RegisterTemporaryObject(obj);
            sprites[i] = BuildApostleVisual(obj, source);
            obj.transform.localScale = Vector3.zero;
            nextShot[i] = i * 0.035f;
        }
        for (float t = 0f; t < summonTime; t += Time.deltaTime)
        {
            float a = Mathf.SmoothStep(0f, 1f, t / summonTime);
            for (int i = 0; i < count; i++)
            {
                UpdateOrbit(i, count, t, walls);
                apostles[i].transform.localScale = Vector3.one * a;
                sprites[i].color = new Color(0.85f, 0.72f, 1f, a);
            }
            yield return null;
        }
        for (float t = 0f; t < firingDuration; t += Time.deltaTime)
        {
            Transform player = GetPlayerTransform();
            if (player == null || empty == null || (empty is Mew boss && boss.IsEnding)) yield break;
            for (int i = 0; i < count; i++)
            {
                apostles[i].transform.localScale = Vector3.one;
                sprites[i].color = new Color(0.85f, 0.72f, 1f, 1f);
                UpdateOrbit(i, count, summonTime + t, walls);
                sprites[i].transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(t * 2f + i) * 12f);
                if (t < nextShot[i]) continue;
                Vector2 direction = ((Vector2)player.position - (Vector2)apostles[i].transform.position).normalized;
                if (direction.sqrMagnitude < 0.001f) direction = Vector2.up;
                if (IsPhase2OrLater)
                {
                    float deviation = Random.Range(Mathf.Min(1f, phaseTwoSpread), phaseTwoSpread) * (Random.value < 0.5f ? -1f : 1f);
                    direction = Quaternion.Euler(0f, 0f, deviation) * direction;
                }
                var obj = MewStarPool.Spawn(projectilePrefab, apostles[i].transform.position,
                    Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg), empty);
                if (obj.TryGetComponent(out BarrageProjectile bullet))
                {
                    bullet.empty = empty;
                    bullet.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
                    bullet.SetDirection(direction);
                    bullet.SetSpeed(starSpeed);
                }
                else Destroy(obj);
                nextShot[i] = t + Mathf.Max(0.06f, firingInterval);
            }
            yield return null;
        }
        for (float t = 0f; t < 0.45f; t += Time.deltaTime)
        {
            for (int i = 0; i < count; i++)
            {
                UpdateOrbit(i, count, summonTime + firingDuration + t, walls);
                apostles[i].transform.localScale = Vector3.one * (1f - t / 0.45f);
            }
            yield return null;
        }
        CleanupTemporaryObjects();
    }

    public static SpriteRenderer BuildApostleVisual(GameObject obj, SpriteRenderer source)
    {
        SpriteRenderer sprite;
            var halo = MewSigilVisual.Create(obj.transform, obj.transform.position, 0.9f, MewSigilVisual.Style.Mirror, 84);
            var body = new GameObject("Apostle star");
            body.transform.SetParent(obj.transform, false);
            body.transform.localScale = Vector3.one * 1.7f;
            sprite = body.AddComponent<SpriteRenderer>();
            sprite.sprite = source != null ? source.sprite : null;
            sprite.sortingOrder = 89;
            sprite.color = new Color(0.85f, 0.72f, 1f, 0f);
            for (int side = -1; side <= 1; side += 2)
            {
                var wing = new GameObject("Light wing").AddComponent<LineRenderer>();
                wing.transform.SetParent(obj.transform, false);
                wing.sharedMaterial = halo.GetComponentInChildren<LineRenderer>().sharedMaterial;
                wing.useWorldSpace = false;
                wing.positionCount = 9;
                wing.numCornerVertices = 3;
                wing.numCapVertices = 3;
                wing.SetPosition(0, new Vector3(side * 0.2f, 0f));
                wing.SetPosition(1, new Vector3(side * 0.75f, 0.45f));
                wing.SetPosition(2, new Vector3(side * 1.1f, 0.25f));
                wing.SetPosition(3, new Vector3(side * 1.35f, 0.65f));
                wing.SetPosition(4, new Vector3(side * 1.08f, -0.05f));
                wing.SetPosition(5, new Vector3(side * 0.85f, 0.13f));
                wing.SetPosition(6, new Vector3(side * 0.72f, -0.3f));
                wing.SetPosition(7, new Vector3(side * 0.48f, -0.14f));
                wing.SetPosition(8, new Vector3(side * 0.22f, -0.38f));
                wing.startWidth = 0.12f; wing.endWidth = 0.025f;
                wing.startColor = new Color(0.8f, 0.65f, 1f, 0.6f);
                wing.endColor = new Color(0.5f, 0.95f, 1f, 0.2f);
                wing.sortingOrder = 87;
            }
            var core = new GameObject("Pearl star core").AddComponent<SpriteRenderer>();
            core.transform.SetParent(body.transform, false);
            core.transform.localScale = Vector3.one * 0.45f;
            core.sprite = sprite.sprite;
            core.color = new Color(1f, 0.95f, 1f, 0.9f);
            core.sortingOrder = 90;
        return sprite;
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason) { CleanupTemporaryObjects(); }
    protected override int GetExecutionCount() { return 1; }
}
