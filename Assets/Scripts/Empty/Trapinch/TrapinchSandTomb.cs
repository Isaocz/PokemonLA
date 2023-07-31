using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TrapinchSandTomb : MonoBehaviour
{
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public float bornTime = 1;
    public float bornScale = 1;
    public float growTime = 1;
    public float growScale = 1.5f;
    public float lifeTime = 10;
    public float deadTime = 2;
    public float hitDuration = 1;
    public float dragForce = 200F;

    private float prePS1Radius;
    private float prePS2Radius;
    private ParticleSystem.ShapeModule shape1;
    private ParticleSystem.ShapeModule shape2;
    private Trapinch trapinch;
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

        transform.localScale = new Vector3(0,0,0);
        shape1.radius = 0;
        shape2.radius = 0;

        // born
        scaleTo(bornScale, bornTime);

        // dead
        Timer.Start(this, lifeTime - deadTime, () =>
        {
            scaleTo(0, deadTime, ()=>
            {
                Destroy(this.gameObject);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            if(Vector3.Distance(transform.position, other.transform.position) > 0.5)
            {
                other.attachedRigidbody.AddForce(dragForce * Vector3.Normalize(transform.position - other.transform.position));
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

    public void SetOwner(Trapinch t)
    {
        trapinch = t;   
    }

    public void GrowUp()
    {
        isGrowUp = true;
        scaleTo(growScale, growTime);
    }

    private void scaleTo(float scale, float second, Action callback = null)
    {
        if (callback == null)
        {
            callback = () => { };
        }
        DOTween.To(() => shape1.radius, x => shape1.radius = x, prePS1Radius * scale, second).SetEase(Ease.OutQuad);
        DOTween.To(() => shape2.radius, x => shape2.radius = x, prePS2Radius * scale, second).SetEase(Ease.OutQuad);
        transform.DOScale(new Vector3(scale, scale, scale), second).SetEase(Ease.OutQuad).OnComplete(()=>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }
}
