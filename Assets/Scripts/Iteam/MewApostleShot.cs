using UnityEngine;

public sealed class MewApostleShot : MonoBehaviour
{
    public const float Power = 15f;
    private PlayerControler owner;
    private Empty target;
    private Vector2 direction;
    private float age, retargetAt;
    private SpriteRenderer visual;
    private static Material starMaterial;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[32];

    public void Launch(PlayerControler player, Empty enemy, Vector3 position, Sprite sprite)
    {
        owner = player; target = enemy; age = 0f; retargetAt = 0f;
        transform.position = position; transform.localScale = new Vector3(1.5f, 1.5f, 1f); // Same as Mew/Skills/StarLight.prefab.
        direction = ((Vector2)enemy.transform.position - (Vector2)position).normalized;
        if (visual == null) visual = gameObject.AddComponent<SpriteRenderer>();
        // RewardStar contains the same bring_4 sprite and PPU as the boss star.
        if (starMaterial == null) starMaterial = Resources.Load<Material>("MewVisuals/RewardStarMaterial");
        if (starMaterial != null) visual.sharedMaterial = starMaterial;
        visual.sprite = sprite; visual.color = Color.white; visual.sortingLayerID = 0; visual.sortingOrder = 12; // Below inventory UI (19/20).
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Time.deltaTime <= 0f) return; // No homing or hit callbacks during rescue/pause.
        age += Time.deltaTime;
        if (owner == null || owner.IsDead || !owner.gameObject.activeInHierarchy || age > 3f
            || Vector2.Distance(transform.position, owner.transform.position) > 18f)
        { gameObject.SetActive(false); return; }
        if (!MewRewardPassives.ValidTarget(target) && age >= retargetAt)
        { target = MewRewardPassives.FindTarget(transform.position, direction, 6f); retargetAt = age + 0.2f; }
        if (MewRewardPassives.ValidTarget(target))
        {
            Vector2 desired = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            direction = Vector3.RotateTowards(direction, desired, 7f * Time.deltaTime, 0f);
        }
        float distance = 9f * Time.deltaTime;
        int count;
        bool savedTriggers = Physics2D.queriesHitTriggers;
        try
        {
            Physics2D.queriesHitTriggers = true;
            count = Physics2D.CircleCast(transform.position, 0.12f, direction, MewRewardPassives.QueryFilter, hits, distance);
        }
        finally { Physics2D.queriesHitTriggers = savedTriggers; }
        // Query is non-allocating; choose the first blocking surface/enemy by distance, not array order.
        Empty enemyHit = null; float nearest = float.MaxValue; bool blocked = false;
        for (int i = 0; i < count; i++)
        {
            var collider = hits[i].collider;
            if (collider == null || collider.GetComponentInParent<PlayerControler>() != null) continue;
            var enemy = collider.GetComponentInParent<Empty>();
            // Trigger volumes can mark room entry/interaction areas; only solid scenery blocks shots.
            bool wall = !collider.isTrigger && (collider.CompareTag("Room") || collider.CompareTag("Enviroment"));
            if ((MewRewardPassives.ValidTarget(enemy) || wall) && hits[i].distance < nearest)
            { nearest = hits[i].distance; enemyHit = enemy; blocked = true; }
        }
        if (blocked)
        {
            gameObject.SetActive(false); // Disable before damage callbacks to avoid duplicate hits.
            if (MewRewardPassives.ValidTarget(enemyHit))
                Pokemon.PokemonHpChange(owner.gameObject, enemyHit.gameObject, 0, Power, 0, PokemonType.TypeEnum.Psychic);
            return;
        }
        transform.position += (Vector3)(direction * distance);
        transform.Rotate(0f, 0f, Time.deltaTime * 180f);
    }
}
