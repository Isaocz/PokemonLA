using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSeedShoot : Skill
{
    Vector3 StartPostion;
    public float moveSpeed;
    public Vector3 moveDirection;
    public bulletSeed ParentBS;
    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward) * Vector2.right;
        transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
        StartPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if ((StartPostion - transform.position).magnitude > MaxRange)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.DetachChildren();
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger) {
            if (collision.CompareTag("Empty"))
            {
                Empty target = collision.GetComponent<Empty>();
                if (target != null)
                {
                    HitAndKo(target);
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.DetachChildren();
                    if (ParentBS.SkillFrom == 2) { 
                        ParentBS.BornAGrass(transform.position);

                        if (Random.Range(0.0f , 1.0f)>0.6f && ParentBS.player.isInSuperGrassyTerrain)
                        {
                            ParentBS.BornAGrass(player.transform.position + Vector3.up + Vector3.right);
                        }
                    }
                    Destroy(gameObject);
                    
                }
            }
            if (collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.DetachChildren();
                Destroy(gameObject);

            }
        }
    }
}
