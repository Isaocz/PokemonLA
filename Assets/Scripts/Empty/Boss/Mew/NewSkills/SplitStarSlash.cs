using UnityEngine;

/// <summary>
/// 星之刃的分裂行为：碰到玩家或 Blocktags 中的障碍物后，生成8方向星光。
/// </summary>
public class SplitStarSlash : BarrageProjectile
{
    public float SplitAirSlashSpeed = 10f;
    public GameObject SplitStarSlashPref;
    [Min(1)] public int SplitStarSlashNum = 8;
    public bool randomizeSplitRotation;

    private bool hasSplit;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSplit || collision == null)
        {
            return;
        }

        if (IsBlockedBy(collision))
        {
            SplitSlash();
            return;
        }

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (empty != null)
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(
                empty.gameObject,
                collision.gameObject,
                0,
                SpDmage,
                0,
                ProType);

            if (playerControler != null)
            {
                playerControler.KnockOutPoint = knockPoint > 0f ? knockPoint : 2.5f;
                playerControler.KnockOutDirection =
                    (playerControler.transform.position - transform.position).normalized;
            }
        }

        SplitSlash();
    }

    private bool IsBlockedBy(Collider2D collision)
    {
        if (collision.CompareTag("Room") || collision.CompareTag("Enviroment"))
        {
            return true;
        }

        if (Blocktags == null)
        {
            return false;
        }

        for (int i = 0; i < Blocktags.Length; i++)
        {
            string blockTag = Blocktags[i];
            if (!string.IsNullOrEmpty(blockTag) && collision.CompareTag(blockTag))
            {
                return true;
            }
        }

        return false;
    }

    private void SplitSlash()
    {
        if (hasSplit)
        {
            return;
        }

        hasSplit = true;
        StopMovement();

        if (SplitStarSlashPref != null)
        {
            int splitCount = Mathf.Max(1, SplitStarSlashNum);
            float baseAngle = randomizeSplitRotation
                ? Random.Range(0f, 360f)
                : 0f;

            for (int i = 0; i < splitCount; i++)
            {
                float angle = baseAngle + i * (360f / splitCount);
                Vector2 direction =
                    Quaternion.Euler(0f, 0f, angle) * Vector2.right;

                GameObject splitObject = Instantiate(
                    SplitStarSlashPref,
                    transform.position,
                    Quaternion.identity);

                BarrageProjectile splitProjectile =
                    splitObject.GetComponent<BarrageProjectile>();

                if (splitProjectile == null)
                {
                    Debug.LogError(
                        SplitStarSlashPref.name + " 缺少 BarrageProjectile。",
                        splitObject);
                    Destroy(splitObject);
                    continue;
                }

                splitProjectile.empty = empty;
                splitProjectile.SetBehavior(projectileBehavior.Straight);
                splitProjectile.SetDirection(direction);
                splitProjectile.SetSpeed(SplitAirSlashSpeed);
            }
        }

        Destroy(gameObject);
    }
}
