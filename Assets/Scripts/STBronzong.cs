using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STBronzong : Projectile
{
    bool isHitDone;

    void Start()
    {
        Invoke("SetPSActive", 0.2f);
        Invoke("DestroySelf", 0.8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isHitDone = true;
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Ground);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 9f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }

        }
    }

    void SetPSActive()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void DestroySelf()
    {
        Bronzong ParentBronzong = empty.GetComponent<Bronzong>();
        if (!isHitDone && ParentBronzong.STAtkUpCount < 3) { empty.AtkChange(1, 0.0f); ParentBronzong.STAtkUpCount++; }
        if (isHitDone && ParentBronzong.STAtkUpCount > 0) { empty.AtkChange(-ParentBronzong.STAtkUpCount, 0.0f); ParentBronzong.STAtkUpCount = 0; }
        Destroy(gameObject);
    }
}
