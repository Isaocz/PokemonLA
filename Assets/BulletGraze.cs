using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGraze : MonoBehaviour
{
    private PlayerControler player;
    private int playerHP;
    private float timer;
    public GameObject grazeEffect;
    public float DamageImprovement;
    public AudioClip Graze;
    private AudioSource audioSource;

    private List<GameObject> projectelList = new List<GameObject>();// 用于记录进入触发器的Projectel

    private void Start()
    {
        DamageImprovement = 1f;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(timer > 0)
        {
            DamageImprovement = 1f + 0.25f * timer / 10f;
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectel") && !projectelList.Contains(collision.gameObject))
        {
            player = FindObjectOfType<PlayerControler>();
            if (player != null)
            {
                playerHP = player.Hp;

                projectelList.Add(collision.gameObject);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectel"))
        {
            player = FindObjectOfType<PlayerControler>();

            projectelList.Remove(collision.gameObject);

            if (player != null && player.Hp == playerHP)
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

        if (Graze != null)
        {
            audioSource.PlayOneShot(Graze);
        }
    }
}
