using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientPower : Skill
{
    Vector2 direction;
    public bool isParticleCollider;
    ParticleSystem ACPS;

    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        ACPS = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();

        if (!isParticleCollider && ExistenceTime <= 1.7f)
        {
            var CollisionModule = ACPS.collision;
            CollisionModule.enabled = true;
            Vector3 postion = transform.position;
            postion.x += direction.x * 9.5f * Time.deltaTime;
            postion.y += direction.y * 9.5f * Time.deltaTime;
            transform.position = postion;
        }
    }

}
