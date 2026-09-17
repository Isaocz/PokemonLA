using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>056: two skill-triggered apostles. 057: one-use lethal damage protection.</summary>
public sealed class MewRewardPassives : MonoBehaviour
{
    public const int ApostleId = 56, SanctuaryId = 57;
    private PlayerControler owner;
    private readonly Transform[] apostles = new Transform[2];
    private readonly List<MewApostleShot> shots = new List<MewApostleShot>(24);
    private static readonly Collider2D[] targets = new Collider2D[256];
    // Keep dense barrages and player hitboxes out of enemy/wall queries.
    internal static readonly int QueryMask = ~LayerMask.GetMask("Projectel", "PlayerProjectel",
        "Player", "PlayerFly", "PlayerJump", "PlayerSurround", "UI", "Grass");
    // Enemy hurtboxes are triggers. The 2022.3 project also gates explicit filters through
    // queriesHitTriggers; query synchronously and restore that switch before returning.
    internal static ContactFilter2D QueryFilter => new ContactFilter2D
        { useTriggers = true, useLayerMask = true, layerMask = QueryMask };
    private static Sprite star;
    private const float OrbitRadius = 1.85f, OrbitCenterHeight = 0.45f;
    private float angle;
    private int lastCastFrame = -1;
    private MewMercyEffect sanctuaryEffect;

    public static MewRewardPassives Ensure(PlayerControler player)
    {
        var effect = player.GetComponent<MewRewardPassives>();
        if (effect == null) effect = player.gameObject.AddComponent<MewRewardPassives>();
        effect.owner = player;
        return effect;
    }

    public static bool Owns(PlayerControler player, int id) => player != null && player.playerData != null
        && player.playerData.IsPassiveGetList != null && player.playerData.IsPassiveGetList.Length > id
        && player.playerData.IsPassiveGetList[id];

    private void Update()
    {
        if (owner == null) owner = GetComponent<PlayerControler>();
        bool visible = Owns(owner, ApostleId) && !owner.IsDead;
        if (visible && apostles[0] == null) BuildApostles();
        angle += Time.deltaTime * 70f;
        for (int i = 0; i < 2; i++) if (apostles[i] != null)
        {
            apostles[i].gameObject.SetActive(visible);
            float phase = (angle + i * 180f) * Mathf.Deg2Rad;
            // World-space circle stays round even when the player has non-uniform scale.
            apostles[i].position = transform.position + Vector3.up * OrbitCenterHeight
                + new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), 0f) * OrbitRadius;
        }
    }

    private void BuildApostles()
    {
        if (star == null)
        {
            var frames = Resources.LoadAll<Sprite>("MewVisuals/RewardStar");
            if (frames.Length > 0) star = frames[0];
            foreach (var frame in frames) if (frame.name.EndsWith("_4")) star = frame;
        }
        var sourceObject = new GameObject("Apostle sprite source");
        var source = sourceObject.AddComponent<SpriteRenderer>(); source.sprite = star; source.enabled = false;
        for (int i = 0; i < 2; i++)
        {
            var obj = new GameObject("Mew apostle " + (i + 1));
            obj.transform.SetParent(transform, false);
            obj.transform.localScale = Vector3.one * 0.45f;
            // Keep the boss visual's internal ordering, but render the entire companion
            // below the camera-space inventory canvas (orders 19/20).
            var group = obj.AddComponent<UnityEngine.Rendering.SortingGroup>();
            group.sortingLayerID = 0;
            group.sortingOrder = 11;
            var body = StarRingReturn.BuildApostleVisual(obj, source);
            body.color = new Color(0.9f, 0.75f, 1f, 0.9f);
            apostles[i] = obj.transform;
            float phase = (angle + i * 180f) * Mathf.Deg2Rad;
            apostles[i].position = transform.position + Vector3.up * OrbitCenterHeight
                + new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), 0f) * OrbitRadius;
        }
        Destroy(sourceObject);
    }

    // Called only after a real player skill instance is created, never by subskills or these shots.
    public static void OnSkillReleased(PlayerControler player, Vector2 direction)
    {
        if (!Owns(player, ApostleId) || player.IsDead || player.IsMewSanctuaryRescuing) return;
        var effect = Ensure(player);
        if (effect.lastCastFrame == Time.frameCount) return;
        effect.lastCastFrame = Time.frameCount;
        if (effect.apostles[0] == null) effect.BuildApostles();
        effect.shots.RemoveAll(shot => shot == null); // Scene unloading can destroy detached pooled shots.
        Empty target = FindTarget(player.transform.position, direction, 12f);
        if (target == null) return;
        for (int i = 0; i < 2; i++)
        {
            MewApostleShot shot = null;
            for (int j = 0; j < effect.shots.Count; j++)
                if (effect.shots[j] != null && !effect.shots[j].gameObject.activeSelf) { shot = effect.shots[j]; break; }
            if (shot == null && effect.shots.Count < 24)
            {
                shot = new GameObject("Friendly apostle starlight").AddComponent<MewApostleShot>();
                effect.shots.Add(shot);
            }
            if (shot != null) shot.Launch(player, target, effect.apostles[i].position, star);
        }
    }

    internal static bool ValidTarget(Empty target) => target != null && target.isActiveAndEnabled
        && !target.isBorn && !target.isDie && target.EmptyHp > 0 && !target.GetTotalInvicible;

    internal static Empty FindTarget(Vector2 origin, Vector2 forward, float range)
    {
        int count;
        bool savedTriggers = Physics2D.queriesHitTriggers;
        try
        {
            Physics2D.queriesHitTriggers = true;
            count = Physics2D.OverlapCircle(origin, range, QueryFilter, targets);
        }
        finally { Physics2D.queriesHitTriggers = savedTriggers; }
        Empty best = null; float bestScore = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            var candidate = targets[i].GetComponentInParent<Empty>();
            if (!ValidTarget(candidate)) continue;
            Vector2 offset = (Vector2)candidate.transform.position - origin;
            float score = offset.sqrMagnitude * (1f + (1f - Vector2.Dot(forward.normalized, offset.normalized)) * 0.8f);
            if (score < bestScore) { best = candidate; bestScore = score; }
        }
        System.Array.Clear(targets, 0, count);
        return best;
    }

    public void PlaySanctuary()
    {
        // Start the rescue before optional inventory/toast UI, so it cannot be skipped by UI state.
        owner.BeginMewSanctuaryRescue();
        StartCoroutine(SanctuaryVisual());
        if (AudioManager.Instance != null && AudioManager.Instance.CommonBasicSFXPlayer != null)
            AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonBasicSFXList.HealUp, transform.position);
        var list = FindObjectOfType<PassiveList>();
        if (owner.spaceitemUseUI != null && list != null && list.SpritesList.Length > SanctuaryId)
            owner.spaceitemUseUI.UIAnimationStart(list.SpritesList[SanctuaryId]);
        if (UIBagDataPanel.InBagData != null) UIBagDataPanel.InBagData.RefreshConsumedPassives();
    }

    private IEnumerator SanctuaryVisual()
    {
        var effect = gameObject.AddComponent<MewMercyEffect>();
        sanctuaryEffect = effect;
        try
        {
            // Full boss rescue runs on unscaled time; the ward wakes the player in about 2.6 seconds.
            yield return effect.Play(transform, true, ClearEnemyProjectiles, 1.8f);
        }
        finally
        {
            if (effect != null) { effect.enabled = false; Destroy(effect); }
            sanctuaryEffect = null;
            if (owner != null) owner.EndMewSanctuaryRescue();
        }
    }

    public static void ClearEnemyProjectiles()
    {
        foreach (var projectile in GameObject.FindGameObjectsWithTag("Projectel"))
        {
            // The project's Projectel tag is hostile; friendly apostle stars never use it.
            var barrage = projectile.GetComponent<BarrageProjectile>();
            if (barrage != null) barrage.Despawn();
            else { projectile.SetActive(false); Destroy(projectile); }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (sanctuaryEffect != null) { sanctuaryEffect.enabled = false; Destroy(sanctuaryEffect); sanctuaryEffect = null; }
        if (owner != null) owner.EndMewSanctuaryRescue();
        foreach (var shot in shots) if (shot != null) shot.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        foreach (var shot in shots) if (shot != null) Destroy(shot.gameObject);
    }
}
