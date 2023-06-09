using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxBerry : MonoBehaviour
{
    public bool TurnToU;
    public bool TurnToD;
    public bool TurnToL;
    public bool TurnToR;
    public int BerryIndex;
    public bool isCanBeEat;

    Vector2Int director;
    Rigidbody2D BerryRigidbody2D;
    Collider2D BerryCollider2D;
    Animator animator;
    public bool isTurn;


    // Start is called before the first frame update
    void Start()
    {
        director = new Vector2Int( Random.Range(0.0f,1.0f)>0.5f?-1:1 , Random.Range(0.0f, 1.0f) > 0.5f ? -1 : 1);
        BerryRigidbody2D = GetComponent<Rigidbody2D>();
        BerryCollider2D = GetComponent<Collider2D>();
        Invoke("CallCanBeEat", 3);
    }

    private void FixedUpdate()
    {
        Vector2 postion = BerryRigidbody2D.position;
        postion.x = postion.x + 3 * director.x * Time.deltaTime;
        postion.y = postion.y + 3 * director.y * Time.deltaTime;
        BerryRigidbody2D.position = postion;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckTurn();
    }

    void CheckTurn()
    {
        if (!isTurn)
        {
            if (TurnToD)
            {
                director.y *= -1; isTurn = true; Invoke("CallisTurnFalse", 0.02f);
            }
            if (TurnToU)
            {
                director.y *= -1; isTurn = true; Invoke("CallisTurnFalse", 0.02f);
            }
            if (TurnToL)
            {
                director.x *= -1; isTurn = true; Invoke("CallisTurnFalse", 0.02f);
            }
            if (TurnToR)
            {
                director.x *= -1; isTurn = true; Invoke("CallisTurnFalse", 0.02f);
            }
        }
    }

    void CallCanBeEat()
    {
        isCanBeEat = true;
    }

    void CallisTurnFalse()
    {
        isTurn = false;
    }

    public void DestoryBerry()
    {
        Destroy(gameObject);
    }

    public void BerryDestoryAnimator()
    {
        if (animator != null) {
            animator.SetTrigger("Destory");
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
            playerControler.ChangeHp(-2, 0, 19);
            playerControler.KnockOutPoint = 3;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
        }else if(other.transform.tag == ("Empty"))
        {
            Snorlax ParentSnorlax = other.gameObject.GetComponent<Snorlax>();
            if (ParentSnorlax != null && isCanBeEat) {
                ParentSnorlax.EatBerry(BerryIndex);
                ParentSnorlax.DestoryEvent -= DestoryBerry;
                Destroy(gameObject);
            }
        }
    }
}
