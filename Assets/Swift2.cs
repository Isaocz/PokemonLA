using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swift2 : Skill
{
    [Header("�������Ǽ���ѡ��")]
    public GameObject Star;//�����ӵ�Ԥ����
    public int starNumber;//δǿ��ʱ������
    public int updatedStarNumber;//ǿ��������������
    public float HitRadius;//δ����ʱ������Χ
    public float UpdatedHitRadius;//�����󹥻���Χ
    public float ShootTime;//�������ʱ��

    [Header("���������ӵ�ѡ��")]
    public float starSurrundSpeed;//������ת�ٶ�
    public float starRotateSpeed;//������ת�ٶ�;
    public float starSpeed;//���ǹ����ٶ�
    public float starRedius;//��������ҵİ뾶

    private int surplusStar;//ʣ����������
    private float closestDistance = Mathf.Infinity;//����˵ľ���
    private GameObject target;
    private List<GameObject> detectedEnemies = new List<GameObject>();
    private List<GameObject> stars = new List<GameObject>();
    private float timer;//��ʱ��

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

            // �����ǰ target �뿪����������������Ŀ��
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
