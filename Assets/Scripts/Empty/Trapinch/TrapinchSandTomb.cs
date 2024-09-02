using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TrapinchSandTomb : MonoBehaviour
{
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    // 初始化时的scale
    public float startScale = 0.1f;
    public float bornTime = 1;
    // 第一阶的scale
    public float bornScale = 1;
    public float growTime = 1;
    // 第二阶的scale
    public float growScale = 1.5f;
    public float lifeTime = 10;
    public float deadTime = 2;
    public float hitDuration = 1;
    public float dragForceDefault = 650F;
    public float dragForceSubsititue = 6000F;
    // 发生吸力的最小距离，防止大颚蚁自己被推开
    public float addForceDis = 1f;

    private enum LIFE
    {
        BORN,
        LITTLE,
        GROW,
        BIG,
        DEAD,
    }
    private string tweenName = "scale";

    private LIFE life;
    private float prePS1Radius;
    private float prePS2Radius;
    private ParticleSystem.ShapeModule shape1;
    private ParticleSystem.ShapeModule shape2;
    private Trapinch trapinch;
    private Tween action1;
    private Tween action2;
    private Tween action3;
    private Dictionary<GameObject, float> hitMap = new Dictionary<GameObject, float>();
    private bool isGrowUp = false;
    public bool IsGrowUp
    {
        get { return isGrowUp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        prePS1Radius = ps1.shape.radius;
        prePS2Radius = ps2.shape.radius;
        shape1 = ps1.shape;
        shape2 = ps2.shape;

        transform.localScale = new Vector3(startScale, startScale, startScale);
        shape1.radius = prePS1Radius * startScale;
        shape2.radius = prePS2Radius * startScale;

        // born
        life = LIFE.BORN;
        scaleTo(bornScale, bornTime, ()=>
        {
            life = LIFE.LITTLE;
        });

        // dead
        Timer.Start(this, lifeTime - deadTime, () =>
        {
            tryDead();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (trapinch)
        {
            if (trapinch.isSilence || trapinch.isSleepDone)
            {
                tryDead();
            }
        }
        else
        {
            tryDead();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Vector3 direction = Vector3.Normalize(transform.position - other.transform.position);
        bool isOwnerInInfatuation = trapinch && trapinch.isEmptyInfatuationDone;
        if (other.transform.tag == "Player")
        {
            if (isOwnerInInfatuation)
            {
                // 魅惑时不对player产生作用
                return;
            }
            if((other.gameObject.layer != LayerMask.NameToLayer("PlayerFly") && other.gameObject.layer != LayerMask.NameToLayer("PlayerJump")) && Vector3.Distance(transform.position, other.transform.position) > addForceDis)
            {

                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    other.attachedRigidbody.AddForce(dragForceDefault * direction);
                }
                else
                {
                    //替身
                    other.attachedRigidbody.AddForce(dragForceSubsititue * direction);
                }
            }
        }
        else if (other.transform.tag == "Empty")
        {
            if (!isOwnerInInfatuation)
            {
                // 仅在魅惑时对 enemy 产生作用
                return;
            }
            if (Vector3.Distance(transform.position, other.transform.position) > addForceDis)
            {
                Rigidbody2D rigidbody2D = other.attachedRigidbody;
                if (rigidbody2D)
                {
                    other.attachedRigidbody.AddForce(dragForceDefault * direction);
                }
            }
        }

        //造成伤害
        //TODO: 处理 trapinch 死亡
        //GameObject target = null;
        //if (other.transform.tag == ("Player"))
        //{
        //    target = other.gameObject;
        //}
        //if (trapinch.isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        //{
        //    target = other.gameObject;
        //}
        //if (target)
        //{
        //    float lastHitTime;
        //    hitMap.TryGetValue(target, out lastHitTime);
        //    if (Time.time - lastHitTime > hitDuration)
        //    {
        //        Pokemon.PokemonHpChange(trapinch.gameObject, other.gameObject, 5, 0, 0, Type.TypeEnum.Ground);
        //        hitMap[target] = Time.time;
        //        //主角覆盖击退值
        //        PlayerControler playerControler = target.GetComponent<PlayerControler>();
        //        if (playerControler != null)
        //        {
        //            playerControler.KnockOutPoint = 0;
        //        }
        //    }
        //}
    }

    private void OnDestroy()
    {
        // 对象销毁时，DOTween 不会及时停止
        stopTween(action1);
        stopTween(action2);
        stopTween(action3);
    }

    public void SetOwner(Trapinch t)
    {
        trapinch = t;   
    }

    public void GrowUp()
    {
        if (!IsCanGrowUp())
        {
            return;
        }
        isGrowUp = true;
        life = LIFE.GROW;
        scaleTo(growScale, growTime, () =>
        {
            life = LIFE.BIG;
        });
    }

    public bool IsCanGrowUp()
    {
        return life == LIFE.LITTLE;
    }

    public void Cancel()
    {
        tryDead();
    }

    private void tryDead()
    {
        if (life == LIFE.DEAD)
        {
            return;
        }
        life = LIFE.DEAD;
        scaleTo(0, deadTime, () =>
        {
            Destroy(this.gameObject);
        });
    }

    private void stopTween(Tween action)
    {
        if (action != null && action.IsActive() && action.IsPlaying())
        {
            action.Kill();
        }
    }

    private void scaleTo(float scale, float second, Action callback = null)
    {
        stopTween(action1);
        stopTween(action2);
        stopTween(action3);
        //DOTween.Kill(tweenName);
        if (callback == null)
        {
            callback = () => { };
        }
        action1 = DOTween.To(() => shape1.radius, x => shape1.radius = x, prePS1Radius * scale, second).SetEase(Ease.OutQuad).SetId<Tween>(tweenName);
        action2 = DOTween.To(() => shape2.radius, x => shape2.radius = x, prePS2Radius * scale, second).SetEase(Ease.OutQuad).SetId<Tween>(tweenName);
        action3 = transform.DOScale(new Vector3(scale, scale, scale), second).SetEase(Ease.OutQuad).SetId<Tween>(tweenName).OnComplete(()=>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }
}
