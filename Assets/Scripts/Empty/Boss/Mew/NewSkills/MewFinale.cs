using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Replay the actual phase-two skills, followed by a persistent-arena windmill.</summary>
public sealed class MewFinale : MewBaseSkill
{
    private GameObject projectilePrefab;
    private MewBaseSkill activeSkill;
    private readonly List<StardustFantasyBullet> windmillStars = new List<StardustFantasyBullet>();
    private readonly GameObject[] flowerApostles = new GameObject[5];
    private GameObject coreWarning;
    private Material coreMaterial;

    public float Progress { get; private set; }
    public bool TrialCleared { get; private set; }
    public float TotalSeconds { get; private set; }
    public float RemainingSeconds => Mathf.Max(0f, TotalSeconds - elapsedSeconds);
    private float elapsedSeconds;
    private bool clockRunning;
    private const float CenterMoveSeconds = 1.1f;

    public static float GetTrialSeconds(Mew boss)
    {
        return SlotSeconds(boss.MirrorStarLightPref) + SlotSeconds(boss.CelestialClockPref) +
            SlotSeconds(boss.MimicStarsPref) + SlotSeconds(boss.StarWavePref) + 3.2f + 18f + CenterMoveSeconds - 6f;
    }

    private static float SlotSeconds(GameObject prefab)
    {
        if (prefab == null) return 0f;
        if (prefab.TryGetComponent(out MirrorStarLight mirror)) return mirror.FinalTrialSlotSeconds;
        if (prefab.TryGetComponent(out CelestialClock clock)) return clock.FinalTrialSlotSeconds;
        if (prefab.TryGetComponent(out MimicStars mimic)) return mimic.FinalTrialSlotSeconds;
        if (prefab.TryGetComponent(out StarWave wave)) return wave.FinalTrialSlotSeconds;
        return 0f;
    }

    private void Update()
    {
        if (!clockRunning || IsFinished) return;
        elapsedSeconds += Time.deltaTime;
        Progress = TotalSeconds > 0f ? Mathf.Clamp01(elapsedSeconds / TotalSeconds) : 0f;
    }

    public void Configure(GameObject projectile, Mew boss)
    {
        projectilePrefab = projectile;
        TotalSeconds = GetTrialSeconds(boss);
        skillName = "终极试炼 · 星海风车";
    }

    public override IEnumerator CoreLogic()
    {
        Mew boss = empty as Mew;
        if (boss == null || projectilePrefab == null) yield break;
        TotalSeconds = GetTrialSeconds(boss);
        elapsedSeconds = 0f;
        clockRunning = true;
        float nextDeadline = 0f;
        GameObject[] sequence = { boss.MirrorStarLightPref, boss.CelestialClockPref, boss.MimicStarsPref, boss.StarWavePref };
        for (int index = 0; index < sequence.Length; index++)
        {
            nextDeadline += SlotSeconds(sequence[index]) - (index >= 2 ? 3f : 0f);
            if (sequence[index] == null)
            {
                Debug.LogError("Mew final trial is missing phase-two skill " + index, this);
                yield break;
            }
            GameObject obj = Instantiate(sequence[index], ArenaCenter, Quaternion.identity);
            activeSkill = obj.GetComponent<MewBaseSkill>();
            if (activeSkill == null) { Destroy(obj); yield break; }
            if (activeSkill is MirrorStarLight mirror) mirror.ConfigureFinalTrial();
            if (activeSkill is CelestialClock clock) clock.ConfigureFinalTrial();
            if (activeSkill is MimicStars mimic) mimic.ConfigureFinalTrial();
            if (activeSkill is StarWave wave) wave.ConfigureFinalTrial();
            bool completed = false;
            activeSkill.Finished += (skill, reason) => completed = reason == MewSkillFinishReason.Completed;
            activeSkill.Initialize(new MewSkillContext(empty, MewSkillPhase.Phase3, Player,
                obj.transform, ArenaCenter, ArenaRadius, ArenaTransform), true);
            while (activeSkill != null && !activeSkill.IsFinished) yield return null;
            if (!completed) yield break;
            activeSkill = null;
            // Early exits (e.g. vertices reaching their destination) use the remaining slot for dodging residual stars.
            nextDeadline += 0.8f;
            if (index >= 2)
            {
                float immediateDeadline = elapsedSeconds + 0.3f;
                TotalSeconds += immediateDeadline - nextDeadline;
                nextDeadline = immediateDeadline;
            }
            while (elapsedSeconds < nextDeadline) yield return null;
        }
        TotalSeconds = elapsedSeconds + CenterMoveSeconds + 18f;
        yield return boss.MoveToFinaleCenter(CenterMoveSeconds);
        yield return Windmill();
        TrialCleared = true;
        elapsedSeconds = TotalSeconds;
        clockRunning = false;
        Progress = 1f;
    }

    private IEnumerator Windmill()
    {
        // Even an unusually slow frame or edited skill prefab must not shorten the final attack.
        TotalSeconds = elapsedSeconds + 18f;
        coreWarning = new GameObject("Windmill core warning");
        LineRenderer ring = coreWarning.AddComponent<LineRenderer>();
        coreMaterial = new Material(Shader.Find("Sprites/Default"));
        ring.sharedMaterial = coreMaterial;
        ring.positionCount = 96;
        ring.loop = true;
        ring.startWidth = ring.endWidth = 0.16f;
        ring.sortingOrder = 95;
        for (int i = 0; i < 96; i++)
        {
            float angle = i * Mathf.PI * 2f / 96f;
            ring.SetPosition(i, ArenaCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f);
        }
        float nextShot = 2f, nextCoreHit = 0f;
        float flowerEpoch = Time.time;
        var apostleSource = projectilePrefab.GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < flowerApostles.Length; i++)
        {
            var apostle = new GameObject("Final flower apostle " + (i + 1));
            flowerApostles[i] = apostle;
            apostle.transform.SetParent(coreWarning.transform, false);
            var sprite = StarRingReturn.BuildApostleVisual(apostle, apostleSource);
            sprite.color = new Color(0.85f, 0.72f, 1f, 1f);
        }
        // The countdown and the final attack share one clock; the last two seconds are its release.
        float duration = RemainingSeconds;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            for (int i = 0; i < flowerApostles.Length; i++)
            {
                float angle = (i * 72f + (Time.time - flowerEpoch) * StardustFantasyBullet.FlowerRotationSpeed) * Mathf.Deg2Rad;
                flowerApostles[i].transform.position = ArenaCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2.4f;
                float visible = Mathf.Min(Mathf.Clamp01(t / 0.7f), Mathf.Clamp01((duration - t) / 1f));
                flowerApostles[i].transform.localScale = Vector3.one * visible * 0.65f;
            }
            bool damaging = t >= 2f && t < duration - 2f;
            ring.startColor = ring.endColor = new Color(1f, 0.45f, 0.9f, damaging ? 0.95f : 0.35f);
            if (damaging && t >= nextShot)
            {
                windmillStars.RemoveAll(star => star == null);
                // Five paired petal outlines, not independent crossing spirals.
                if (windmillStars.Count + 10 <= 100)
                {
                    for (int petal = 0; petal < 5; petal++)
                    {
                        SpawnFlowerStar(petal * 72f, false, flowerEpoch);
                        SpawnFlowerStar(petal * 72f, true, flowerEpoch);
                    }
                }
                nextShot = t + 0.55f;
            }
            if (damaging && Player != null && Time.time >= nextCoreHit &&
                Vector2.Distance(Player.position, ArenaCenter) < 1.8f)
            {
                nextCoreHit = Time.time + 0.4f;
                Pokemon.PokemonHpChange(empty.gameObject, Player.gameObject, 0, 42f, 0, PokemonType.TypeEnum.Psychic);
                if (((Mew)empty).IsEnding) yield break;
            }
            if (t >= duration - 2f)
            {
                foreach (StardustFantasyBullet star in windmillStars) if (star != null) star.BeginAnimatedDespawn(0.6f);
                windmillStars.Clear();
                ring.startColor = ring.endColor = new Color(1f, 0.45f, 0.9f, (duration - t) * 0.35f);
            }
            yield return null;
        }
    }

    private void SpawnFlowerStar(float petalAngle, bool inward, float epoch)
    {
        Vector3 origin = inward ? (Vector3)ArenaCenter : flowerApostles[Mathf.RoundToInt(petalAngle / 72f) % 5].transform.position;
        GameObject obj = Instantiate(projectilePrefab, origin, Quaternion.identity);
        StardustFantasyBullet bullet = obj.GetComponent<StardustFantasyBullet>();
        if (bullet == null) bullet = obj.AddComponent<StardustFantasyBullet>();
        bullet.InitializeRotatingFlower(empty, ArenaCenter, ArenaRadius - 1f, petalAngle, inward, epoch,
            inward ? new Color(0.58f, 0.86f, 1f) : new Color(1f, 0.64f, 0.88f));
        windmillStars.Add(bullet);
    }
    private void ClearOwnedProjectiles()
    {
        foreach (BarrageProjectile projectile in FindObjectsOfType<BarrageProjectile>())
            if (projectile != null && projectile.empty == empty)
            {
                foreach (Collider2D hitbox in projectile.GetComponentsInChildren<Collider2D>()) hitbox.enabled = false;
                projectile.Despawn();
            }
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        clockRunning = false;
        if (activeSkill != null) activeSkill.Cancel(true);
        activeSkill = null;
        if (!(empty is Mew boss) || !boss.IsEnding) ClearOwnedProjectiles();
        windmillStars.Clear();
        if (coreWarning != null) Destroy(coreWarning);
        if (coreMaterial != null) Destroy(coreMaterial);
    }
}
