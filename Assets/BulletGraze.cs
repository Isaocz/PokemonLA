using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGraze : MonoBehaviour
{
    private Collider2D playerCollider;
    private Vector2 playerColliderRange;
    private PlayerControler player;
    private int playerHP;
    private float timer;
    public GameObject grazeEffect;
    public float DamageImprovement;

    private void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        playerCollider = player.GetComponent<Collider2D>();
        DamageImprovement = 1f;
        if (playerCollider != null)
        {
            float playerWidth = playerCollider.bounds.size.x;
            float playerHeight = playerCollider.bounds.size.y;
            playerColliderRange =new Vector3(playerWidth, playerHeight);
        }
    }
    private void Update()
    {
        if(timer > 0)
        {
            DamageImprovement = 1f + 0.2f * timer / 10f;
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectel"))
        {
            playerHP = player.Hp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectel"))
        {
            if (player.Hp == playerHP)
            {
                OnGraze();
            }
            else
            {
                timer = 0f;
            }
        }
    }


    private void OnGraze()
    {
        GameObject GrazeEffect = Instantiate(grazeEffect, transform.position, Quaternion.identity);
        Destroy(GrazeEffect, 0.35f);
        timer = 10f;
    }
}
