using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swift2 : Skill
{
    [Header("高速星星技能选项")]
    public GameObject Star;//星星子弹预制体
    public int starNumber;//未强化时的数量
    public int updatedStarNumber;//强化技能星星数量
    public float HitRadius;//未升级时攻击范围
    public float UpdatedHitRadius;//升级后攻击范围
    public float ShootTime;//攻击间隔时间

    [Header("高速星星子弹选项")]
    public float starSurrundSpeed;//星星绕转速度
    public float starRotateSpeed;//星星自转速度;
    public float starSpeed;//星星攻击速度
    public float starRedius;//星星绕玩家的半径

    private int surplusStar;//剩余星星数量
    private float closestDistance = Mathf.Infinity;//与敌人的距离
    private GameObject target;
    private List<GameObject> detectedEnemies = new List<GameObject>();
    private List<GameObject> stars = new List<GameObject>();
    private float timer;//计时器

    private void Awake()
    {
        if (SkillFrom != 2)
        {
            surplusStar = starNumber;
        }
        else
        {
            surplusStar = updatedStarNumber;
        }
        float angleIncrement = 360f / surplusStar;
        for(int i = 0; i < surplusStar; i++)
        {
            GameObject star = Instantiate(Star, transform.position, Quaternion.identity);
            SwiftStar ss = star.GetComponent<SwiftStar>();
            ss.index = i;
            ss.angleIncrement = angleIncrement;
            ss.rotateSpeed = starRotateSpeed;
            ss.surrundSpeed = starSurrundSpeed;
            ss.Speed = starSpeed;
            ss.center = transform;
            ss.radius = starRedius;

            ss.player = player;
            ss.SpDamage = SpDamage;
            stars.Add(star);
        }
        surplusStar = surplusStar - 1;
    }
    private void Update()
    {
        StartExistenceTimer();
        timer += Time.deltaTime;
        if (surplusStar < 0)
        {
            Destroy(this.gameObject);
        }

        else if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enviroment", "Room"));

            if (hit)
            {
                Debug.Log("Current target is blocked. Searching for new target...");
                target = null;
                closestDistance = Mathf.Infinity;
            }

            if (timer > ShootTime)
            {
                timer = 0;
                SwiftStar sss = stars[surplusStar].GetComponent<SwiftStar>();
                if (sss != null)
                {
                    sss.attack = true;
                    sss.target = target;
                }
                surplusStar--;
            }
        }
        

        if (target == null)
        {
            SearchForNewTarget();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            if (!detectedEnemies.Contains(collision.gameObject))
            {
                detectedEnemies.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            detectedEnemies.Remove(collision.gameObject);

            // 如果当前 target 离开触发器，重新搜索目标
            if (target == collision.gameObject)
            {
                target = null;
                closestDistance = Mathf.Infinity;
                Debug.Log("Target left the trigger. Searching for new target...");
                SearchForNewTarget();
            }
        }
    }

    private void SearchForNewTarget()
    {
        foreach (GameObject enemy in detectedEnemies)
        {
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enviroment", "Room"));

                if (!hit && distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy;
                    Debug.Log("New target set: " + enemy.name + " at distance " + distance);
                }
            }
        }
    }

}
