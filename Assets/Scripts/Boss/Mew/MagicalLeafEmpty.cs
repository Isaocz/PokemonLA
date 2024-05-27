using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalLeafEmpty : Projectile
{
    public GameObject Reticle;
    GameObject reticle;
    private Transform target; // 跟随的目标
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 middlePosition;
    private Vector3 playerPosition;
    private Vector3 direction;
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    public float moveSpeed;
    private float percent = 0;
    private float percentSpeed;
    private bool changeDirection;
    private float setTime;

    public float bezierRadio;
    public int changeStages;
    public float changetime = 1f;

    private void OnEnable()
    {   //初始化
        target = FindObjectOfType<PlayerControler>().transform;
        reticle = Instantiate(Reticle);
        reticle.GetComponent<SpriteRenderer>().color = Color.green;
        reticle.GetComponent<Animator>().Play("emphasizeReticle");
        playerPosition = target.position;
        changeStages = 2;
        changeDirection = false;
        setTime = changetime;
        percent = 0;
        UpdatePositions();
    }

    private void Update()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        Vector3 Predictdirection = (target.position - playerPosition).normalized;
        float playerSpeed = (target.position - playerPosition).magnitude;
        playerPosition = target.position;
        targetPosition += Predictdirection * playerSpeed * 0.5f;

        if (percent < 1 && !changeDirection)
        {
            reticle.transform.position = targetPosition;
            
            //对象走贝塞尔曲线
            lastPosition = transform.position;
            percent += percentSpeed * Time.deltaTime;
            transform.position = MathUtils.Bezier(percent, startPosition, middlePosition, targetPosition);
            currentPosition = transform.position;
            direction = (currentPosition - lastPosition).normalized;
            if (percent >= 1)
            {
                changeDirection = true;
            }
        }
        else if (changeDirection)
        {
            //对象移动到预测点上的逻辑
            changetime -= Time.deltaTime;
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
            if (changetime < 0f)
            {
                if(changeStages <= 0)
                {
                    ObjectPoolManager.ReturnObjectToPool(gameObject);
                    Destroy(reticle);
                }
                changeStages--;
                changeDirection = false;
                changetime = setTime;
            }
        }
        else if (percent > 1 && !changeDirection)
        {
            //进行重复
            percent = 0;
            startPosition = transform.position;
            targetPosition = target.position;
            middlePosition = GetMiddlePosition(startPosition, direction, moveSpeed);
            percentSpeed = moveSpeed / (targetPosition - startPosition).magnitude;
            reticle.GetComponent<Animator>().Play("emphasizeReticle");
        }
    }

    private void UpdatePositions()
    {
        startPosition = transform.position;
        targetPosition = target.position;
        middlePosition = MathUtils.BezierGetMiddle(startPosition, targetPosition);
        percentSpeed = moveSpeed / (targetPosition - startPosition).magnitude;
    }

    private Vector2 GetMiddlePosition(Vector2 pos, Vector2 diretion, float v)
    {
        return pos + bezierRadio * diretion * v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Grass);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject); // 在碰撞后销毁魔法叶子对象
            Destroy(reticle);
        }
    }
}
