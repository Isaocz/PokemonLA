using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GressBF : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("LookX" , ((Random.Range(0.0f , 1.0f) > 0.5f) ? -1 : 1));
        transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.8f, 1.5f);
        switch (Random.Range(0 , 6))
        {
            case 0:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
                break;
            case 1:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.990566f, 0.6494749f, 0.7781893f, 0.7f);
                break;
            case 2:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.6367924f, 0.6628549f, 1, 0.7f);
                break;
            case 3:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.9927715f, 1, 0.6352941f, 0.7f);
                break;
            case 4:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.5401388f, 0.9622642f, 0.6250517f, 0.7f);
                break;
            case 5:
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8867924f, 0.5052213f, 0.4308473f, 0.7f);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("Fly");
            if (other.transform.position.x > transform.position.x) {
                animator.SetFloat("LookX", -1);
            }
            else
            {
                animator.SetFloat("LookX", 1);
            }
        }
    }

    void DestorySelf()
    {
        Destroy(gameObject);
    }

}
