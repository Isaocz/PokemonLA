using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : Skill
{

    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserStartWidth;
    float LeserEndWidth;
    float MaxLength;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;

    ParticleSystem RayPS;


    bool isPSStop;


    GameObject PlayerSpriteParent;

    bool isFly;


    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        transform.rotation = Quaternion.Euler(0,0,90);
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        player.isCanNotMove = true; player.animator.SetFloat("Speed", 0);
        player.isInvincibleAlways = true;
        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position + Vector3.up);
        lineRenderer.SetPosition(1, transform.position + Vector3.up);

        //初始化激光宽度
        LeserStartWidth = lineRenderer.startWidth;
        LeserEndWidth = lineRenderer.endWidth;
        //lineRenderer.startWidth = 0;
        //lineRenderer.endWidth = 0;

        //获取组件
        EndVFX = transform.GetChild(2).gameObject;
        EndVFX.transform.position = transform.position;
        StartVFX = transform.GetChild(1).gameObject;
        StartVFX.transform.position = transform.position;
        MaxLength = 1;

        RayPS = transform.GetChild(1).GetChild(2).GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();


        if (ExistenceTime > 3.0f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 4.5f, 0.0f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
            //PlayerSpriteParent.transform.localScale = new Vector3(PlayerSpriteParent.transform.localScale.x, Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2, 0.8f, 1), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 3.0f && ExistenceTime > 2.95f)
        {
            if (SkillFrom == 2 && player.isCanNotMove) {
                player.isCanNotMove = false;
                if (!isFly && player.gameObject.layer != LayerMask.NameToLayer("PlayerFly"))
                {
                    isFly = true;
                    player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
                } 
            }
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.95f && ExistenceTime > 2.85f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.85f && ExistenceTime > 2.75f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.75f && ExistenceTime > 2.65f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.65f && ExistenceTime > 2.55f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.55f && ExistenceTime > 2.45f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.45f && ExistenceTime > 2.35f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.35f && ExistenceTime > 2.25f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.25f && ExistenceTime > 2.15f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.15f && ExistenceTime > 2.05f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 2.0f, 3.8f, 3.9f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 2.05f && ExistenceTime > 1.75f)
        {
            if (SkillFrom == 2 && !player.isCanNotMove) {
                player.isCanNotMove = true;
                if (isFly && player.gameObject.layer == LayerMask.NameToLayer("PlayerFly"))
                {
                    isFly = false;
                    player.gameObject.layer = LayerMask.NameToLayer("Player");
                }
            }
            player.animator.SetFloat("Speed", 0);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 16, 0.0f, 4.0f), PlayerSpriteParent.transform.localPosition.z);
            //PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 4f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 8f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1.75f && ExistenceTime > 1.7f)
        {  
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 4f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 8f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1.7f && ExistenceTime > 1.5f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime < 1.5f)
        {
            if (player.isCanNotMove) {
                player.isCanNotMove = false;
                transform.parent = null;
            }
        }
        else if (ExistenceTime < 0.4f)
        {
            if (player.isInvincibleAlways)
            {
                player.isInvincibleAlways = false;
            }
        }

        //水柱
        {
            RayTimer += Time.deltaTime;
            StartVFX.transform.position = transform.position;
            rayPosition();
            if (RayTimer >= 1.3f)
            {
                lineRenderer.startWidth -= Time.deltaTime * LeserStartWidth / 0.7f;
                lineRenderer.endWidth -= Time.deltaTime * LeserEndWidth / 0.7f;
                StartVFX.transform.localScale -= Time.deltaTime * new Vector3(1, 1, 1) / 0.7f;
                EndVFX.transform.localScale -= Time.deltaTime * new Vector3(1, 1, 1) / 0.7f;
                if (RayTimer >= 1.55f)
                {

                    transform.GetChild(4).gameObject.SetActive(true);
                }
                if (RayTimer >= 2.05f && !transform.GetChild(3).gameObject.activeSelf)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(false);
                    transform.GetChild(3).gameObject.SetActive(true);
                }
                if (RayTimer >= 3.0f && transform.GetChild(3).gameObject.activeSelf)
                {
                    transform.GetChild(3).gameObject.SetActive(false);
                }
            }
        }
    }



    private void rayPosition()
    {
        MaxLength = Mathf.Clamp(MaxLength + 4.0f * Time.deltaTime, 0, 4);
        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        //否则正常显示
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * MaxLength);
        Vector2 EndPoint = lineRenderer.GetPosition(1);
        EndVFX.transform.position = EndPoint;
        float Length = (EndVFX.transform.position - transform.position).magnitude;


        var RayPSShape = RayPS.shape;
        RayPSShape.radius = Length / 2;
        RayPSShape.position = new Vector3(Length / 2, 0, 0);
        var RayPSEmission = RayPS.emission;
        RayPSEmission.rateOverTime = (int)Length * 75;



    }


    void ResetPlayer()
    {
        if ((player == null && baby == null) || PlayerSpriteParent == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
            PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        }
    }

    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal;
        PlayerSpriteParent.transform.localPosition = player.PlayerLocalPosition;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (Random.Range(0.0f , 1.0f) + ((float)player.LuckPoint / 30.0f) >= 0.8f ) {
                    target.Fear(3.0f , 1.0f);
                }
                if (SkillFrom == 2)
                {
                    target.SpeedChange();
                    target.SpeedRemove01(15.0f * target.OtherStateResistance);
                }
            }


        }
    }








}
