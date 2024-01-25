using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBronzong : MonoBehaviour
{
    public Vector2 BlockDir;

    float Timer;
    float ExitTime = 30;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer >= ExitTime)
        {
            BreakBlock();
        }
        else
        {
            Timer += Time.deltaTime;
        }
    }

    public void BreakBlock()
    {
        Timer = ExitTime;
        animator.SetTrigger("Break");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Timer += 14f * Time.deltaTime;
        }
    }
}
