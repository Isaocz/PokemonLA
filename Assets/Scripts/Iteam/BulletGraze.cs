/* Previous implementation retained for review; replaced below.
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

    public static BulletGraze instance;

    private List<GameObject> projectelList = new List<GameObject>();// 用于记录进入触发器的Projectel

    private void Awake()
    {
        instance = this;
    }
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

*/
using System.Collections.Generic;
using UnityEngine;

// Batched feedback; gameplay reward still refreshes on every valid graze.
public class BulletGraze : MonoBehaviour
{
    public GameObject grazeEffect;
    public float DamageImprovement = 1f;
    public AudioClip Graze;
    public static BulletGraze instance;
    private PlayerControler player;
    private readonly Dictionary<Collider2D, int> enteredHp = new Dictionary<Collider2D, int>();
    private readonly List<Collider2D> stale = new List<Collider2D>();
    private float timer, nextFeedback, effectUntil, nextCleanup;
    private GameObject visual;
    private Animator visualAnimator;
    private void Awake() { instance = this; }
    private void Start()
    {
        player = GetComponentInParent<PlayerControler>();
        if (player == null) player = FindObjectOfType<PlayerControler>();
        // Allocate once, not once per projectile exit.
        if (grazeEffect != null)
        {
            visual = Instantiate(grazeEffect, transform.position, Quaternion.identity);
            visualAnimator = visual.GetComponent<Animator>();
            visual.SetActive(false);
        }
    }
    private void Update()
    {
        timer = Mathf.Max(0f, timer - Time.deltaTime);
        DamageImprovement = 1f + 0.25f * timer / 10f;
        if (visual != null && visual.activeSelf && Time.time >= effectUntil) visual.SetActive(false);
        if (Time.time < nextCleanup) return;
        nextCleanup = Time.time + 0.5f;
        if (player == null) player = FindObjectOfType<PlayerControler>();
        stale.Clear();
        foreach (var pair in enteredHp)
            if (pair.Key == null || !pair.Key.enabled || !pair.Key.gameObject.activeInHierarchy) stale.Add(pair.Key);
        foreach (var key in stale) enteredHp.Remove(key);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null && other.CompareTag("Projectel") && !enteredHp.ContainsKey(other))
            enteredHp.Add(other, player.Hp);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!enteredHp.TryGetValue(other, out int hp)) return;
        enteredHp.Remove(other);
        // Despawn/pooling is not a successful dodge.
        if (player == null || !other.enabled || !other.gameObject.activeInHierarchy) return;
        if (player.Hp != hp) { timer = 0f; DamageImprovement = 1f; return; }
        timer = 10f;
        DamageImprovement = 1.25f;
        if (Time.unscaledTime < nextFeedback) return;
        nextFeedback = Time.unscaledTime + 0.12f;
        if (visual != null)
        {
            visual.transform.position = transform.position;
            visual.SetActive(true);
            if (visualAnimator != null) { visualAnimator.Rebind(); visualAnimator.Update(0f); }
            effectUntil = Time.time + 0.35f;
        }
        // Previous per-event audioSource.PlayOneShot(Graze) stacked many voices.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXGrouped(Graze, transform.position, 0.12f, true);
    }
    private void OnDisable()
    {
        enteredHp.Clear(); timer = 0f; DamageImprovement = 1f;
        if (visual != null) visual.SetActive(false);
    }
    private void OnDestroy()
    {
        if (visual != null) Destroy(visual);
        if (instance == this) instance = null;
    }
}