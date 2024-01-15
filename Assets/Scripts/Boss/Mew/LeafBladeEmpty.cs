using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBladeEmpty : Projectile
{
    public float moveSpeed = 10f;
    public float radius;
    public GameObject Trail4;
    private Transform target; //目标
    private int phase;
    private int mode;

    private bool isInitialized;

    /// <summary>
    /// 初始化叶刃
    /// </summary>
    /// <param name="Target">玩家（目标位置）</param>
    /// <param name="currentPhase">梦幻的当前阶段</param>
    /// <param name="blademode">叶刃发射的方向模式</param>
    public void Initialize(Transform Target, int currentPhase, int blademode)
    {
        target = Target;
        phase = currentPhase;
        mode = blademode;
    }
    void OnEnable()
    {
        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            Vector3 direction = target.position - transform.position;
            Vector3 perpendicularDirection;
            Vector3 targetPosition;
            Vector3 moveDirection;
            switch (mode)
            {
                case 2:
                    perpendicularDirection = new Vector3(-direction.y, direction.x).normalized * radius;
                    targetPosition = target.position + perpendicularDirection;
                    moveDirection = (targetPosition - transform.position).normalized;
                    break;
                case 1:
                    perpendicularDirection = new Vector3(-direction.y, direction.x).normalized * radius;
                    targetPosition = target.position - perpendicularDirection;
                    moveDirection = (targetPosition - transform.position).normalized;
                    break;
                case 0:
                default:
                    moveDirection = direction;
                    break;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject, 5f);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            if (phase == 3)
            {
                moveSpeed = 20f;
                GameObject trail4 = Instantiate(Trail4, transform.position, Quaternion.Euler(0f, 0f, angle));
                Destroy(trail4, 0.5f);
            }
        }


        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, Type.TypeEnum.Grass);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
