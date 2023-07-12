using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapinchSandTomb : MonoBehaviour
{
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public float bornTime = 1;
    public float bornScale = 1;
    public float dragForce = 200F;

    private float prePS1Radius;
    private float prePS2Radius;

    // Start is called before the first frame update
    void Start()
    {
        prePS1Radius = ps1.shape.radius;
        prePS2Radius = ps2.shape.radius;

        transform.localScale = new Vector3(0,0,0);
        ParticleSystem.ShapeModule shape1 = ps1.shape;
        ParticleSystem.ShapeModule shape2 = ps2.shape;
        shape1.radius = 0;
        shape2.radius = 0;

        transform.DOScale(new Vector3(bornScale, bornScale, bornScale), bornTime).SetEase(Ease.OutQuad);
        DOTween.To(() => shape1.radius, x => shape1.radius = x, prePS1Radius * bornScale, bornTime).SetEase(Ease.OutQuad);
        DOTween.To(() => shape2.radius, x => shape2.radius = x, prePS2Radius * bornScale, bornTime).SetEase(Ease.OutQuad);
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
    }
}
