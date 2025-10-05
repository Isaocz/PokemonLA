using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GressPlayerINOUT : MonoBehaviour
{

    Animator animator;
    public GameObject RareCandy;
    public GameObject CCG;
    public GameObject StartDust;
    public GameObject SharpStone;
    bool isVisit;
    PlayerControler player;
    public bool isDie;

    bool BanUp;
    bool BanDown;
    bool BanRight;
    bool BanLeft;



    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

    }

    void LunchNewItem(GameObject LunchGameObject)
    {
        CheckBan();
        IteamPickUp NewItem = Instantiate(LunchGameObject, gameObject.transform.position+Vector3.down* 0.4f, Quaternion.identity,transform).GetComponent<IteamPickUp>();
        NewItem.isLunch = true;
        NewItem.BanLunchUp = BanUp;
        NewItem.BanLunchDown = BanDown;
        NewItem.BanLunchRight = BanRight;
        NewItem.BanLunchLeft = BanLeft;
        Debug.Log(BanUp + "+" + BanDown + "+" + BanRight + "+" + BanLeft);

    }

    void CreatNewItem()
    {
        if (!isVisit)
        {
            isVisit = true;
            if (Random.Range(0.000f, 1.000f) < (0.1f+(((float)player.LuckPoint))/60) )
            {
                float JudgeF = Random.Range(0.0f, 1.0f);
                if (JudgeF <= 1.0f && JudgeF > 0.82f) {
                    LunchNewItem(RareCandy); }
                else if (JudgeF <= 0.82f && JudgeF > 0.64f) {
                    LunchNewItem(StartDust); }
                else if (JudgeF <= 0.64f && JudgeF >= 0.46f) { 
                    LunchNewItem(CCG); }
                else if (JudgeF <= 0.46f && JudgeF >= 0.4f) {
                    LunchNewItem(SharpStone); }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player") {
            if (other.GetComponent<PlayerControler>() != null && other.gameObject.layer != LayerMask.NameToLayer("PlayerFly") && other.gameObject.layer != LayerMask.NameToLayer("PlayerJump") ) {
                animator.SetTrigger("PlayerIn");
                player = other.GetComponent<PlayerControler>();
                CreatNewItem();
                other.GetComponent<PlayerControler>().InGressCount.Add(gameObject);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            animator.SetTrigger("PlayerOut");
            other.GetComponent<PlayerControler>().InGressCount.Remove(gameObject);
        }
    }


    public void GrassDie()
    {
        isDie = true;
        animator.SetTrigger("Die");
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


    void CheckBan()
    {
        RaycastHit2D CheckRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.1f), Vector2.right, 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        RaycastHit2D CheckLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.1f), Vector2.left, 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        RaycastHit2D CheckUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.1f), Vector2.up, 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        RaycastHit2D CheckDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.1f), Vector2.down, 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        if (CheckRight.collider != null && CheckRight.collider.transform != transform.parent) { BanRight = true; }
        if (CheckLeft.collider != null && CheckLeft.collider.transform != transform.parent) { BanLeft = true;  }
        if (CheckUp.collider != null && CheckUp.collider.transform != transform.parent) { BanUp = true; }
        if (CheckDown.collider != null && CheckDown.collider.transform != transform.parent) { BanDown = true; }
        
    }
}
