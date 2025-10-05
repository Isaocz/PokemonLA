using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitStarSlash : BarrageProjectile
{
    public float SplitAirSlashSpeed = 10f;
    public GameObject SplitStarSlashPref;
    public int SplitStarSlashNum = 8;

    private bool hasSplit = false;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSplit) return;

        if (Blocktags != null)
        {
            foreach (var blocktag in Blocktags)
            {
                if (collision.tag == blocktag)
                {
                    moveBehavior = projectileBehavior.Idle;
                    SplitSlash();
                    hasSplit = true;
                    Destroy(this.gameObject);
                    return;
                }
            }
        }

        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, ProType);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            SplitSlash();
            hasSplit = true;
            Destroy(this.gameObject);
            return;
        }
    }

    private void SplitSlash()
    {
        if (hasSplit || SplitStarSlashPref == null) return;

        if (SplitStarSlashPref != null)
        {
            float angle = Random.Range(0f, 360f);
            for (int i = 0; i < SplitStarSlashNum; i++)
            {
                float splitangle = angle + i * (360f / SplitStarSlashNum);
                Vector2 direction2 = Quaternion.Euler(0, 0, splitangle) * Vector2.right;
                GameObject splitAirSlash = Instantiate(SplitStarSlashPref, transform.position, Quaternion.identity);
                BarrageProjectile bp = splitAirSlash.GetComponent<BarrageProjectile>();
                bp.empty = this.empty;
                if (bp != null)
                {
                    bp.SetBehavior(projectileBehavior.Straight);
                    bp.SetDirection(direction2);
                    bp.SetSpeed(SplitAirSlashSpeed);
                }
            }
        }
    }
}
