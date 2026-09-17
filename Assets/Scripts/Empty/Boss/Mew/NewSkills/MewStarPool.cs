using System.Collections.Generic;
using UnityEngine;

/// <summary>Encounter-local pool for straight and mimic stars; reset optional formation motion before every reuse.</summary>
public sealed class MewStarPool : MonoBehaviour
{
    private sealed class Entry
    {
        public BarrageProjectile Bullet;
        public BarrageProjectile Template;
        public GameObject Prefab;
        public SpriteRenderer[] Sprites;
        public Color[] Colors;
        public Collider2D[] Colliders;
        public bool[] ColliderStates;
        public MonoBehaviour[] Behaviours;
        public bool[] BehaviourStates;
        public ParticleSystem[] Particles;
        public TrailRenderer[] Trails;
        public MimicStarBullet Formation;
        public bool Available;
    }

    private readonly Dictionary<GameObject, Stack<Entry>> available = new Dictionary<GameObject, Stack<Entry>>();
    private readonly Dictionary<BarrageProjectile, Entry> entries = new Dictionary<BarrageProjectile, Entry>();

    public static System.Collections.IEnumerator Prewarm(Empty owner, GameObject prefab, int count)
    {
        if (prefab == null) yield break;
        var warming = new List<BarrageProjectile>(count);
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Spawn(prefab, owner.transform.position, Quaternion.identity, owner);
            BarrageProjectile bullet = obj.GetComponent<BarrageProjectile>();
            if (bullet == null) { Destroy(obj); break; }
            obj.SetActive(false);
            warming.Add(bullet);
            if (i % 4 == 3) yield return null;
        }
        foreach (BarrageProjectile bullet in warming) if (bullet != null) bullet.Despawn();
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Empty owner)
    {
        MewStarPool pool = owner.GetComponent<MewStarPool>();
        if (pool == null) pool = owner.gameObject.AddComponent<MewStarPool>();
        GameObject obj = pool.Rent(prefab, position, rotation);
        if (obj.TryGetComponent(out BarrageProjectile scaledBullet)) scaledBullet.ApplyEvolutionScale(owner);
        if (owner is Mew boss && boss.currentPhase == 3 && obj.TryGetComponent(out BarrageProjectile bullet))
            bullet.SetArenaCull(boss.BattleCenter, boss.ArenaRadius + 4f);
        return obj;
    }

    private GameObject Rent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!available.TryGetValue(prefab, out Stack<Entry> stack))
            available.Add(prefab, stack = new Stack<Entry>());
        Entry entry = null;
        while (stack.Count > 0 && entry == null)
        {
            Entry candidate = stack.Pop();
            if (candidate.Bullet != null) entry = candidate;
        }
        if (entry == null)
        {
            GameObject obj = Instantiate(prefab, position, rotation);
            BarrageProjectile bullet = obj.GetComponent<BarrageProjectile>();
            if (bullet == null) return obj;
            // This component is present on the common prefab but is unused by straight stars.
            StarRingReturnBullet orbit = obj.GetComponent<StarRingReturnBullet>();
            if (orbit != null) orbit.enabled = false;
            MimicStarBullet formation = obj.GetComponent<MimicStarBullet>();
            if (formation == null) formation = obj.AddComponent<MimicStarBullet>();
            formation.enabled = false;
            entry = new Entry { Bullet = bullet, Template = prefab.GetComponent<BarrageProjectile>(), Prefab = prefab,
                Formation = formation,
                Sprites = obj.GetComponentsInChildren<SpriteRenderer>(true),
                Colliders = obj.GetComponentsInChildren<Collider2D>(true),
                Behaviours = obj.GetComponentsInChildren<MonoBehaviour>(true),
                Particles = obj.GetComponentsInChildren<ParticleSystem>(true),
                Trails = obj.GetComponentsInChildren<TrailRenderer>(true) };
            entry.Colors = new Color[entry.Sprites.Length];
            for (int i = 0; i < entry.Colors.Length; i++) entry.Colors[i] = entry.Sprites[i].color;
            entry.ColliderStates = new bool[entry.Colliders.Length];
            for (int i = 0; i < entry.Colliders.Length; i++) entry.ColliderStates[i] = entry.Colliders[i].enabled;
            entry.BehaviourStates = new bool[entry.Behaviours.Length];
            for (int i = 0; i < entry.Behaviours.Length; i++) entry.BehaviourStates[i] = entry.Behaviours[i].enabled;
            entries.Add(bullet, entry);
            bullet.ReturnToPool = Return;
        }
        entry.Available = false;
        BarrageProjectile b = entry.Bullet;
        BarrageProjectile defaults = entry.Template;
        b.transform.SetParent(null, false);
        b.transform.SetPositionAndRotation(position, rotation);
        b.transform.localScale = entry.Prefab.transform.localScale;
        b.ExistTime = defaults.ExistTime;
        b.FadeMode = 0;
        b.IsSpin = defaults.IsSpin;
        b.SpinSpeed = defaults.SpinSpeed;
        b.Target = null;
        b.isTargeting = false;
        b.ResetLifetime();
        entry.Formation.ResetForReuse();
        b.StopMovement();
        if (b.rigidbody2D != null) b.rigidbody2D.simulated = true;
        for (int i = 0; i < entry.Sprites.Length; i++) entry.Sprites[i].color = entry.Colors[i];
        for (int i = 0; i < entry.Colliders.Length; i++) entry.Colliders[i].enabled = entry.ColliderStates[i];
        for (int i = 0; i < entry.Behaviours.Length; i++) entry.Behaviours[i].enabled = entry.BehaviourStates[i];
        entry.Formation.enabled = false;
        foreach (ParticleSystem particles in entry.Particles) particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        foreach (TrailRenderer trail in entry.Trails) trail.Clear();
        StarParticalController effect = b.GetComponent<StarParticalController>();
        b.gameObject.SetActive(true);
        // Activation can restart playOnAwake effects. Clear after activation as well as on return.
        if (effect != null) effect.ResetForReuse();
        return b.gameObject;
    }

    private void Return(BarrageProjectile bullet)
    {
        if (!entries.TryGetValue(bullet, out Entry entry) || entry.Available) return;
        entry.Available = true;
        foreach (ParticleSystem particles in entry.Particles) particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        bullet.StopMovement();
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform, false);
        available[entry.Prefab].Push(entry);
    }

    private void OnDestroy()
    {
        foreach (Entry entry in entries.Values)
            if (entry.Bullet != null) Destroy(entry.Bullet.gameObject);
        entries.Clear();
        available.Clear();
    }
}
