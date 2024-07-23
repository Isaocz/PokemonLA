using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impoisonEffect : MonoBehaviour
{
    public float Duration;
    private float timer;
    private Transform impoison;
    private SpriteRenderer sp;
    public PlayerControler player;

    void Start()
    {
        impoison = transform.GetChild(0);
        sp = impoison.GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerControler>();
        timer = 0f;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        transform.position = player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0];
        if (timer > 0 && timer < 0.5f)
        {
            sp.color = new Color(1f, 1f, 1f, 0.2f + 0.5f * timer / 0.5f);
            impoison.localScale = new Vector3(1.15f - 0.75f * timer / 0.5f, 1.15f - 0.75f * timer / 0.5f, 1f);
        }
        else
        {
            sp.color = new Color(1f, 1f, 1f, 0.7f);
            impoison.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
    }
}
