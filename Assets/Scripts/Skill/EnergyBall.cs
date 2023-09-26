using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Skill
{
    public float initialSpeed; 
    private float moveSpeed;
    private Empty enemy;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed -= 2 * Time.deltaTime;
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if(moveSpeed <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (enemy != target)
                {
                    enemy = target;//¼ÇÂ¼target£¬·ÀÖ¹±»ÖØ¸´¼ì²â½µ·À
                    if (Random.Range(0f, 1f) + (float)player.LuckPoint / 30 > 0.9f)
                        target.LevelChange(-1, "SpD");
                }
            }
        }
    }
}
