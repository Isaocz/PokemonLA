using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderSnow02DestorySelf : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        gameObject.transform.localScale = new Vector3(Random.Range(0.5f,1.0f) , Random.Range(0.5f,1.0f) , 0);
        animator.SetFloat("Blend", Random.Range(0.0f, 1.0f));
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
